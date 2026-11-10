using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace TroUble.Data {
    /// <summary>
    /// One sound effect with a few recorded takes. Each play picks a random take with a little volume and pitch
    /// variation so repeats don't sound copy-pasted. Call PlayAt from anywhere; no AudioSource setup needed.
    /// </summary>
    [CreateAssetMenu( menuName = "TroUble/Data/SFX Cue" )]
    public class SfxCue : ScriptableObject {
        [Tooltip( "Record each sound 3 or 4 times and put every take here." )]
        [SerializeField] private List<AudioClip> clips = new();
        [SerializeField] private AudioMixerGroup mixerGroup;
        [SerializeField] private Vector2 volume = new( 0.8f, 1f );
        [SerializeField] private Vector2 pitch = new( 0.95f, 1.05f );
        [Tooltip( "0 = flat 2D (UI sounds), 1 = fully 3D (comes from a place in the house)." )]
        [SerializeField, Range( 0f, 1f )] private float spatialBlend = 1f;
        [Tooltip( "Beyond this many metres you can't hear it at all." )]
        [SerializeField] private float maxDistance = 15f;

        [Header( "Gameplay" )]
        [Tooltip( "How far parents can hear this, in metres. 0 = parents ignore it. The thing that plays the cue raises the noise." )]
        [SerializeField] private float noiseRadius = 0f;

        public float NoiseRadius => noiseRadius;
        public bool HasClips => clips.Count > 0;

        private int lastClip = -1;

        /// <summary>Plays a random take at a point in the world. volumeScale lets crouching play quieter steps.</summary>
        public AudioSource PlayAt( Vector3 position, float volumeScale = 1f ) {
            if ( clips.Count == 0 ) { return null; }

            int index = Random.Range( 0, clips.Count );
            if ( clips.Count > 1 && index == lastClip ) { index = ( index + 1 ) % clips.Count; }
            lastClip = index;

            AudioSource source = SfxPool.Get();
            source.transform.position = position;
            source.clip = clips[index];
            source.outputAudioMixerGroup = mixerGroup;
            source.volume = Random.Range( volume.x, volume.y ) * volumeScale;
            source.pitch = Random.Range( pitch.x, pitch.y );
            source.spatialBlend = spatialBlend;
            source.maxDistance = maxDistance;
            source.Play();
            return source;
        }

        /// <summary>Flat, non-positional play for UI and stings.</summary>
        public AudioSource Play2D( float volumeScale = 1f ) {
            AudioSource source = PlayAt( Vector3.zero, volumeScale );
            if ( source != null ) { source.spatialBlend = 0f; }
            return source;
        }
    }

    /// <summary>A small pool of AudioSources that survives scene loads, so one-shot sounds need no setup.</summary>
    internal static class SfxPool {
        private static readonly List<AudioSource> sources = new();
        private static Transform root;

        [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.SubsystemRegistration )]
        private static void ResetStatics() {
            sources.Clear();
            root = null;
        }

        public static AudioSource Get() {
            foreach ( AudioSource s in sources ) {
                if ( s != null && !s.isPlaying ) { return s; }
            }

            if ( root == null ) {
                root = new GameObject( "SfxPool" ).transform;
                Object.DontDestroyOnLoad( root.gameObject );
            }
            var go = new GameObject( "Sfx" );
            go.transform.SetParent( root );
            AudioSource source = go.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 1f;
            source.dopplerLevel = 0f;
            sources.Add( source );
            return source;
        }
    }
}
