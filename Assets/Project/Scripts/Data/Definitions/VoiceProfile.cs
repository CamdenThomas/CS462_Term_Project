using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace TroUble.Data {
    /// <summary>
    /// How a character "talks": gibberish syllables played while their speech bubble types out, Sims / Animal
    /// Crossing style. One recorded set of syllables can voice everyone; pitch and speed make them sound different.
    /// </summary>
    [CreateAssetMenu( menuName = "TroUble/Data/Voice Profile" )]
    public class VoiceProfile : ScriptableObject {
        [Tooltip( "Short nonsense syllables, about 0.1 to 0.2 s each ('ba', 'meh', 'hm'). Empty = a silent bubble." )]
        [SerializeField] private List<AudioClip> syllables = new();
        [SerializeField] private AudioMixerGroup mixerGroup;
        [SerializeField, Range( 0f, 1f )] private float volume = 0.8f;

        [Header( "Pitch: above 1 is higher, below 1 is deeper" )]
        [SerializeField, Range( 0.5f, 2f )] private float basePitch = 1f;
        [SerializeField, Range( 0f, 0.3f )] private float pitchJitter = 0.05f;
        [Tooltip( "Lines ending in '?' bend the last syllable up by this much; '!' plays it louder." )]
        [SerializeField, Range( 0f, 0.5f )] private float questionLift = 0.15f;

        [Header( "Speed" )]
        [SerializeField] private float lettersPerSecond = 30f;
        [Tooltip( "One syllable every this many letters. Higher = less chatty." )]
        [SerializeField, Min( 1 )] private int lettersPerSyllable = 3;
        [Tooltip( "Extra pause after , . ! ? so lines breathe." )]
        [SerializeField] private float punctuationPause = 0.12f;

        public AudioMixerGroup MixerGroup => mixerGroup;
        public float Volume => volume;
        public float BasePitch => basePitch;
        public float PitchJitter => pitchJitter;
        public float QuestionLift => questionLift;
        public float LettersPerSecond => Mathf.Max( 1f, lettersPerSecond );
        public int LettersPerSyllable => lettersPerSyllable;
        public float PunctuationPause => punctuationPause;
        public bool HasSyllables => syllables.Count > 0;

        public AudioClip RandomSyllable() {
            return syllables.Count == 0 ? null : syllables[Random.Range( 0, syllables.Count )];
        }
    }
}
