using UnityEngine;

namespace TroUble.Environment {
    /// <summary>
    /// A light that won't hold steady: a dying bulb, a buzzing tube, a screen in the next room.
    /// Dims the bulb or fixture it belongs to along with the light, so the glow and the light agree.
    /// </summary>
    [RequireComponent( typeof( Light ) )]
    public class FlickerLight : MonoBehaviour {
        public enum Style {
            DyingBulb,      // steady, then a burst of stutters, sometimes going dark for a moment
            Fluorescent,    // a faint fast hum with the odd blink
            Screen          // a TV or monitor: restless, jumping level on every cut
        }

        private static readonly int EmissionId = Shader.PropertyToID( "_EmissionColor" );

        [SerializeField] private Style style = Style.DyingBulb;
        [Tooltip( "Optional. The bulb or fixture mesh whose glow dims with the light." )]
        [SerializeField] private Renderer glow;
        [Tooltip( "Seconds between bursts of flicker, picked at random in this range." )]
        [SerializeField] private Vector2 burstEvery = new( 3f, 9f );
        [SerializeField, Range( 0f, 1f )] private float depth = 0.85f;

        private Light target;
        private float baseIntensity;
        private Color baseEmission;
        private MaterialPropertyBlock block;
        private float seed;
        private float nextBurst;
        private float burstEnd;
        private float screenLevel = 1f;
        private float nextCut;

        private void Awake() {
            target = GetComponent<Light>();
            baseIntensity = target.intensity;
            seed = Random.value * 100f;
            if ( glow != null ) {
                block = new MaterialPropertyBlock();
                baseEmission = glow.sharedMaterial.GetColor( EmissionId );
            }
            nextBurst = Time.time + Random.Range( burstEvery.x, burstEvery.y );
        }

        private void OnDisable() {
            if ( target != null ) { target.intensity = baseIntensity; }
            SetGlow( 1f );
        }

        private void Update() {
            float level = style switch {
                Style.DyingBulb => DyingBulb(),
                Style.Fluorescent => Fluorescent(),
                _ => ScreenGlow()
            };
            level = Mathf.Lerp( 1f, level, depth );
            target.intensity = baseIntensity * level;
            SetGlow( level );
        }

        private float DyingBulb() {
            float steady = 0.92f + 0.08f * Mathf.PerlinNoise( seed, Time.time * 3f );
            if ( Time.time >= nextBurst ) {
                burstEnd = Time.time + Random.Range( 0.15f, 1.1f );
                nextBurst = burstEnd + Random.Range( burstEvery.x, burstEvery.y );
            }
            if ( Time.time >= burstEnd ) { return steady; }
            // Inside a burst: hard on/off stutter, mostly off.
            return Mathf.PerlinNoise( seed + 7f, Time.time * 28f ) > 0.62f ? steady : 0.05f;
        }

        private float Fluorescent() {
            float hum = 0.96f + 0.04f * Mathf.Sin( Time.time * 120f + seed );
            if ( Time.time >= nextBurst ) {
                burstEnd = Time.time + Random.Range( 0.05f, 0.3f );
                nextBurst = burstEnd + Random.Range( burstEvery.x, burstEvery.y );
            }
            return Time.time < burstEnd ? 0.2f : hum;
        }

        private float ScreenGlow() {
            if ( Time.time >= nextCut ) {
                screenLevel = Random.Range( 0.45f, 1f );
                nextCut = Time.time + Random.Range( 0.4f, 3f );
            }
            return screenLevel * ( 0.8f + 0.2f * Mathf.PerlinNoise( seed, Time.time * 9f ) );
        }

        private void SetGlow( float level ) {
            if ( glow == null || block == null ) { return; }
            glow.GetPropertyBlock( block );
            block.SetColor( EmissionId, baseEmission * level );
            glow.SetPropertyBlock( block );
        }
    }
}
