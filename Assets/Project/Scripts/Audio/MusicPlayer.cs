using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TroUble.Data;
using TroUble.Events;
using TroUble.Player;

namespace TroUble.Audio {
    /// <summary>
    /// Adaptive music. Menu theme in the main menu; in gameplay the stage's Calm, Tension and Chase loops play in sync,
    /// and Tension and Chase fade in as a parent gets close, suspicious, or spots the player.
    /// Spawns itself from Resources/MusicPlayer on Play, so it is not placed in any scene. See AUDIO.md.
    /// </summary>
    public class MusicPlayer : MonoBehaviour {
        [System.Serializable]
        public class StageMusic { public AudioClip Calm, Tension, Chase; }

        [SerializeField] private AudioMixerGroup output;
        [SerializeField, Range( 0f, 1f )] private float volume = 0.7f;
        [SerializeField] private string menuScene = "MainMenu";

        [Header( "Loops" )]
        [SerializeField] private AudioClip menu;
        [SerializeField] private AudioClip reportCard;
        [Tooltip( "0 Childhood, 1 Early Teen, 2 Late Teen" )]
        [SerializeField] private StageMusic[] stages = new StageMusic[3];

        [Header( "Stings" )]
        [SerializeField] private AudioClip noticed;
        [SerializeField] private AudioClip spotted;
        [SerializeField] private AudioClip caught;
        [SerializeField] private AudioClip victory;

        [Header( "Listens to" )]
        [SerializeField] private PlayerAnchor player;
        [SerializeField] private DetectionChannel playerDetected;
        [SerializeField] private CatchChannel playerCaught;
        [SerializeField] private VoidEventChannel demoWon;
        [SerializeField] private StageChannel stageStarted;

        [Header( "Tuning" )]
        [SerializeField] private float nearRadius = 3.5f;   // a parent this close = half tension
        [SerializeField] private float farRadius = 9f;
        [SerializeField] private float chaseHold = 4f;
        [SerializeField] private float hiddenCutoff = 900f;

        private static MusicPlayer instance;
        private readonly AudioSource[] loops = new AudioSource[3];
        private readonly AudioLowPassFilter[] filters = new AudioLowPassFilter[3];
        private readonly Dictionary<Transform, DetectionLevel> watchers = new();
        private readonly Dictionary<Transform, float> awareness = new();
        private readonly List<Transform> gone = new();
        private AudioSource stings;
        private AudioClip[] playing = new AudioClip[3];
        private AudioClip[] next;
        private int stage;
        private bool gameplay, won;
        private float fade, tension, chase, chaseUntil, duckUntil;

        [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.SubsystemRegistration )]
        private static void ResetStatic() { instance = null; }

        [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.AfterSceneLoad )]
        private static void Spawn() {
            var prefab = Resources.Load<MusicPlayer>( "MusicPlayer" );
            if ( instance == null && prefab != null ) { Instantiate( prefab ).name = "MusicPlayer"; }
        }

        private void Awake() {
            if ( instance != null ) { Destroy( gameObject ); return; }
            instance = this;
            DontDestroyOnLoad( gameObject );
            for ( int i = 0; i < 3; i++ ) {
                var child = new GameObject( "Layer " + i );
                child.transform.SetParent( transform );
                loops[i] = NewSource( child, true );
                filters[i] = child.AddComponent<AudioLowPassFilter>();
            }
            stings = NewSource( gameObject, false );
        }

        private AudioSource NewSource( GameObject host, bool loop ) {
            var source = host.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = output;
            source.playOnAwake = false;
            source.loop = loop;
            source.spatialBlend = 0f;
            return source;
        }

        private void OnEnable() {
            if ( instance != this ) { return; }
            SceneManager.sceneLoaded += OnSceneLoaded;
            if ( playerDetected != null ) { playerDetected.Subscribe( OnDetected ); }
            if ( playerCaught != null ) { playerCaught.Subscribe( OnCaught ); }
            if ( demoWon != null ) { demoWon.Subscribe( OnWon ); }
            if ( stageStarted != null ) { stageStarted.Subscribe( OnStage ); }
            OnSceneLoaded( SceneManager.GetActiveScene(), LoadSceneMode.Single );
        }

        private void OnDisable() {
            if ( instance != this ) { return; }
            SceneManager.sceneLoaded -= OnSceneLoaded;
            if ( playerDetected != null ) { playerDetected.Unsubscribe( OnDetected ); }
            if ( playerCaught != null ) { playerCaught.Unsubscribe( OnCaught ); }
            if ( demoWon != null ) { demoWon.Unsubscribe( OnWon ); }
            if ( stageStarted != null ) { stageStarted.Unsubscribe( OnStage ); }
        }

        // ------------------------------------------------------------ public

        /// <summary>Calm report-card music (call this between stages).</summary>
        public static void PlayReportCard() { if ( instance != null ) { instance.gameplay = false; instance.Queue( instance.reportCard, null, null ); } }

        public static void PlayStageMusic( int stageIndex ) { if ( instance != null ) { instance.stage = stageIndex; instance.PlayGameplay(); } }

        // ------------------------------------------------------------ events

        private void OnSceneLoaded( Scene scene, LoadSceneMode mode ) {
            if ( mode != LoadSceneMode.Single ) { return; }
            watchers.Clear();
            awareness.Clear();
            won = false;
            chaseUntil = duckUntil = 0f;
            if ( scene.name == menuScene ) { gameplay = false; Queue( menu, null, null ); } else { PlayGameplay(); }
        }

        private void OnStage( StageData data ) { PlayStageMusic( data == null ? 0 : data.MinAge >= 15 ? 2 : data.MinAge >= 12 ? 1 : 0 ); }

        private void OnDetected( DetectionEvent e ) {
            if ( e.Watcher == null ) { return; }
            watchers.TryGetValue( e.Watcher, out DetectionLevel before );
            watchers[e.Watcher] = e.Level;
            awareness[e.Watcher] = e.Awareness;
            if ( !gameplay || won ) { return; }
            if ( e.Level == DetectionLevel.Spotted && before != DetectionLevel.Spotted ) {
                if ( spotted != null ) { stings.PlayOneShot( spotted ); }
                chaseUntil = Time.unscaledTime + chaseHold;
            } else if ( e.Level == DetectionLevel.Suspicious && before == DetectionLevel.None ) {
                if ( noticed != null ) { stings.PlayOneShot( noticed ); }
            }
        }

        private void OnCaught( CatchReport report ) {
            if ( caught != null ) { stings.PlayOneShot( caught ); }
            duckUntil = Time.unscaledTime + 3f;
            chaseUntil = 0f;
        }

        private void OnWon() {
            won = true;
            if ( victory != null ) { stings.PlayOneShot( victory ); }
        }

        // ------------------------------------------------------------ mixing

        private void PlayGameplay() {
            gameplay = true;
            StageMusic music = stages[Mathf.Clamp( stage, 0, stages.Length - 1 )];
            if ( music != null ) { Queue( music.Calm, music.Tension, music.Chase ); }
        }

        private void Queue( AudioClip calm, AudioClip tense, AudioClip chased ) {
            if ( calm == playing[0] && tense == playing[1] ) { next = null; return; }
            next = new[] { calm, tense, chased };
        }

        private void Update() {
            float dt = Time.unscaledDeltaTime;
            float now = Time.unscaledTime;

            // Fade out, swap, fade back in. All layers start on the same sample so they stay in time.
            if ( next != null ) {
                fade = Mathf.MoveTowards( fade, 0f, dt / 0.8f );
                if ( fade <= 0f ) {
                    double start = AudioSettings.dspTime + 0.1;
                    for ( int i = 0; i < 3; i++ ) {
                        loops[i].Stop();
                        loops[i].clip = next[i];
                        if ( next[i] != null ) { loops[i].PlayScheduled( start ); }
                    }
                    playing = next;
                    next = null;
                }
            } else {
                fade = Mathf.MoveTowards( fade, 1f, dt / 1.5f );
            }

            // Tension: how close the nearest parent is, or how aware the most suspicious one is.
            float target = 0f, maxAware = 0f;
            bool havePlayer = player != null && player.IsSet;
            gone.Clear();
            foreach ( var pair in awareness ) {
                if ( pair.Key == null ) { gone.Add( pair.Key ); continue; }
                maxAware = Mathf.Max( maxAware, pair.Value );
                if ( !havePlayer ) { continue; }
                Vector3 offset = pair.Key.position - player.Controller.transform.position;
                if ( Mathf.Abs( offset.y ) > 2.2f ) { continue; }   // other floor
                offset.y = 0f;
                float near = 1f - Mathf.InverseLerp( nearRadius, farRadius, offset.magnitude );
                target = Mathf.Max( target, 0.5f * near * near );
            }
            foreach ( Transform t in gone ) { awareness.Remove( t ); watchers.Remove( t ); }
            if ( maxAware > 0f ) { target = Mathf.Max( target, Mathf.Lerp( 0.55f, 1f, maxAware ) ); }

            bool active = gameplay && !won && now >= duckUntil;
            if ( !active ) { target = 0f; }
            tension = Mathf.MoveTowards( tension, target, ( target > tension ? 1.5f : 0.25f ) * dt );
            bool chasing = active && ( now < chaseUntil || maxAware >= 0.75f );
            chase = Mathf.MoveTowards( chase, chasing ? 1f : 0f, ( chasing ? 3f : 0.4f ) * dt );

            float t01 = Mathf.SmoothStep( 0f, 1f, Mathf.InverseLerp( 0.1f, 0.65f, tension ) );
            float duck = now < duckUntil ? 0.15f : 1f;
            float master = volume * fade * duck;
            loops[0].volume = master * ( 1f - 0.2f * t01 - 0.35f * chase );
            loops[1].volume = master * t01 * ( 1f - 0.35f * chase );
            loops[2].volume = master * chase;

            // Muffled while hiding in a closet.
            bool hidden = gameplay && havePlayer && player.Interactor != null && player.Interactor.IsHidden;
            foreach ( AudioLowPassFilter f in filters ) {
                f.enabled = hidden;
                f.cutoffFrequency = hiddenCutoff;
            }
        }
    }
}
