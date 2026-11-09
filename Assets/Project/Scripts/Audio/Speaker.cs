using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TroUble.Data;
using TroUble.Events;

namespace TroUble.Audio {
    /// <summary>
    /// Gives a character a voice: picks a line from its BarkSet, types it into a speech bubble over its head, and
    /// mumbles gibberish syllables from its VoiceProfile while the letters appear.
    ///
    /// Parents react on their own: put this on the same object as their Perception and they talk as suspicion
    /// rises (first name, then first + middle, then the full name) and when the player is caught. Anything else
    /// (distractions, chores, the brother menu) calls Say( BarkTrigger.X ) directly.
    /// </summary>
    public class Speaker : MonoBehaviour {
        // The player's name for {first} {middle} {last} in lines. Set these from a name-entry screen later.
        public static string FirstName = "Sam";
        public static string MiddleName = "James";
        public static string LastName = "Carter";

        private static readonly List<Speaker> all = new();

        [SerializeField] private BarkSet barks;
        [SerializeField] private VoiceProfile voice;

        [Header( "Bubble" )]
        [Tooltip( "Where the bubble sits. Leave empty to float it headHeight metres above this object." )]
        [SerializeField] private Transform bubbleAnchor;
        [SerializeField] private float headHeight = 2.2f;
        [Tooltip( "Optional. Empty = TextMesh Pro's default font. Bangers fits the menus." )]
        [SerializeField] private TMP_FontAsset font;
        [Tooltip( "Walls, doors and furniture (Perception's Obstacle Mask works). The bubble hides when one is between the camera and the head." )]
        [SerializeField] private LayerMask bubbleBlockers;
        [SerializeField] private float maxShowDistance = 12f;
        [Tooltip( "Seconds the finished line stays up, plus a little more for long lines." )]
        [SerializeField] private float holdTime = 1.4f;

        [Header( "Reacting on their own (parents)" )]
        [Tooltip( "Which Perception's DetectionEvents are about this character. Empty = this object." )]
        [SerializeField] private Transform watcher;
        [SerializeField] private DetectionChannel playerDetected;
        [SerializeField] private CatchChannel playerCaught;
        [SerializeField, Range( 0f, 1f )] private float halfThreshold = 0.5f;
        [SerializeField, Range( 0f, 1f )] private float highThreshold = 0.85f;
        [Tooltip( "Minimum seconds between ordinary barks, so nobody chatters nonstop." )]
        [SerializeField] private float cooldown = 3f;

        private SpeechBubble bubble;
        private AudioSource mouth;

        private string line;
        private int shown;
        private float letterTimer;
        private float holdTimer;
        private int lettersSinceSyllable;
        private bool realtime;
        private float lastSpoke = -99f;
        private int suspicionStep;    // 0 none, 1 suspicious, 2 half, 3 high

        public bool IsSpeaking => line != null;

        [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.SubsystemRegistration )]
        private static void ResetStatics() { all.Clear(); }

        private void Awake() {
            if ( watcher == null ) { watcher = transform; }
            Transform anchor = bubbleAnchor;
            if ( anchor == null ) {
                anchor = new GameObject( "BubbleAnchor" ).transform;
                anchor.SetParent( transform, false );
                anchor.localPosition = Vector3.up * headHeight;
            }
            bubble = SpeechBubble.Create( transform, anchor, font, bubbleBlockers, maxShowDistance );

            // Its own 3D source at the head, so you can hear where a parent is through walls and floors.
            mouth = anchor.gameObject.AddComponent<AudioSource>();
            mouth.playOnAwake = false;
            mouth.spatialBlend = 1f;
            mouth.rolloffMode = AudioRolloffMode.Linear;
            mouth.minDistance = 1f;
            mouth.maxDistance = 14f;
            mouth.dopplerLevel = 0f;
            if ( voice != null ) { mouth.outputAudioMixerGroup = voice.MixerGroup; }
        }

        private void OnEnable() {
            all.Add( this );
            if ( playerDetected != null ) { playerDetected.Subscribe( OnDetected ); }
            if ( playerCaught != null ) { playerCaught.Subscribe( OnCaught ); }
        }

        private void OnDisable() {
            all.Remove( this );
            if ( playerDetected != null ) { playerDetected.Unsubscribe( OnDetected ); }
            if ( playerCaught != null ) { playerCaught.Unsubscribe( OnCaught ); }
            StopTalking();
        }

        /// <summary>
        /// Say a random line for the trigger. Ordinary barks wait out the cooldown and never cut off a line;
        /// interrupt = true talks over whatever is being said. Returns false if nothing was said.
        /// realtime = keep typing while the game is stopped (time scale 0), e.g. during the caught banner.
        /// </summary>
        public bool Say( BarkTrigger trigger, bool interrupt = false, bool realtime = false ) {
            if ( barks == null || !barks.Has( trigger ) ) { return false; }
            if ( !interrupt && ( IsSpeaking || Time.unscaledTime - lastSpoke < cooldown ) ) { return false; }
            SayLine( barks.Pick( trigger ), realtime );
            return true;
        }

        /// <summary>Say exact text, for one-off story moments that aren't in the BarkSet.</summary>
        public void SayLine( string text, bool realtime = false ) {
            if ( string.IsNullOrEmpty( text ) ) { return; }
            line = FillName( text );
            shown = 0;
            letterTimer = 0f;
            lettersSinceSyllable = 0;
            this.realtime = realtime;
            lastSpoke = Time.unscaledTime;
            bubble.Show( line );
        }

        public void StopTalking() {
            line = null;
            if ( bubble != null ) { bubble.Hide(); }
        }

        private void Update() {
            if ( line == null ) { return; }
            float dt = realtime ? Time.unscaledDeltaTime : Time.deltaTime;

            if ( shown < line.Length ) {
                letterTimer -= dt;
                while ( letterTimer <= 0f && shown < line.Length ) {
                    char c = line[shown];
                    shown++;
                    Mumble( c );
                    letterTimer += Delay( c );
                }
                bubble.SetVisibleCharacters( shown );
                if ( shown >= line.Length ) { holdTimer = holdTime + line.Length * 0.02f; }
                return;
            }

            holdTimer -= dt;
            if ( holdTimer <= 0f ) { StopTalking(); }
        }

        // One syllable every few letters; nothing on spaces or punctuation.
        private void Mumble( char c ) {
            if ( voice == null || !voice.HasSyllables || !char.IsLetterOrDigit( c ) ) { return; }
            if ( lettersSinceSyllable++ % voice.LettersPerSyllable != 0 ) { return; }

            float pitch = voice.BasePitch * ( 1f + Random.Range( -voice.PitchJitter, voice.PitchJitter ) );
            float volume = voice.Volume;
            bool nearEnd = shown >= line.Length - voice.LettersPerSyllable;
            string trimmed = line.TrimEnd();
            if ( nearEnd && trimmed.EndsWith( "?" ) ) { pitch += voice.QuestionLift; }
            if ( trimmed.EndsWith( "!" ) ) { volume = Mathf.Min( 1f, volume * 1.25f ); }

            mouth.pitch = pitch;
            mouth.PlayOneShot( voice.RandomSyllable(), volume );
        }

        private float Delay( char c ) {
            float perLetter = 1f / ( voice != null ? voice.LettersPerSecond : 30f );
            bool pause = c == ',' || c == '.' || c == '!' || c == '?';
            return perLetter + ( pause && voice != null ? voice.PunctuationPause : 0f );
        }

        // Suspicion rising walks up the full-name meter: Suspicious, then SuspicionHalf, then SuspicionHigh.
        // It only goes back to the start once she has forgotten about the player entirely.
        private void OnDetected( DetectionEvent e ) {
            if ( e.Watcher != watcher ) { return; }

            int step = e.Awareness >= highThreshold ? 3 : e.Awareness >= halfThreshold ? 2 : e.Awareness > 0f ? 1 : 0;
            if ( step == 0 ) { suspicionStep = 0; return; }
            if ( step <= suspicionStep ) { return; }
            suspicionStep = step;

            BarkTrigger trigger = step == 3 ? BarkTrigger.SuspicionHigh : step == 2 ? BarkTrigger.SuspicionHalf : BarkTrigger.Suspicious;
            // Climbing the meter always talks over the previous step.
            Say( trigger, interrupt: step > 1 );
        }

        // The caught report doesn't say who caught them, so the closest speaker with a line for it answers.
        private void OnCaught( CatchReport report ) {
            BarkTrigger trigger = TriggerFor( report.Reason );
            Speaker closest = null;
            float best = float.MaxValue;
            foreach ( Speaker s in all ) {
                if ( s.playerCaught == null || s.barks == null || !s.barks.Has( trigger ) ) { continue; }
                float d = ( s.transform.position - report.Where ).sqrMagnitude;
                if ( d < best ) { best = d; closest = s; }
            }
            if ( closest != this ) { return; }
            // The game stops while the caught banner shows, so this line types in real time.
            Say( trigger, interrupt: true, realtime: true );
        }

        private static BarkTrigger TriggerFor( CatchReason reason ) {
            return reason switch {
                CatchReason.HeardSneaking => BarkTrigger.CaughtHeardSneaking,
                CatchReason.StashFound => BarkTrigger.CaughtStashFound,
                CatchReason.Snitched => BarkTrigger.CaughtSnitched,
                CatchReason.SuspicionMaxed => BarkTrigger.CaughtSuspicionMaxed,
                _ => BarkTrigger.CaughtSeen
            };
        }

        private static string FillName( string text ) {
            return text.Replace( "{first}", FirstName ).Replace( "{middle}", MiddleName ).Replace( "{last}", LastName );
        }
    }
}
