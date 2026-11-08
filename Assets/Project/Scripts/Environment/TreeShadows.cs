using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace TroUble.Environment {
    /// <summary>
    /// Shadows of trees you never see. On the sun: draws a tiling texture of branches when the game starts, hands it
    /// to the light as its cookie, then slides it back and forth a little so the branch shadows move in the wind.
    /// The texture is made in code, so there is no image file to sync. Cookie Size on the light sets how big it is.
    /// </summary>
    [RequireComponent( typeof( Light ) )]
    public class TreeShadows : MonoBehaviour {
        [Tooltip( "How far the branches move, in meters." )]
        [SerializeField] private float sway = 0.12f;
        [SerializeField] private float gustSpeed = 0.35f;
        [Tooltip( "Same number, same branches every time." )]
        [SerializeField] private int treeSeed = 462;
        [Tooltip( "How dark a branch shadow is. 0 = black, 1 = no shadow." )]
        [SerializeField, Range( 0f, 1f )] private float branchShade = 0.22f;

        private const int Size = 512;

        private Light sun;
        private UniversalAdditionalLightData lightData;
        private Texture2D cookie;
        private Vector2 restOffset;
        private float seed;

        private void OnEnable() {
            sun = GetComponent<Light>();
            lightData = GetComponent<UniversalAdditionalLightData>();
            if ( lightData != null ) { restOffset = lightData.lightCookieOffset; }
            seed = Random.value * 100f;
            if ( cookie == null ) { cookie = DrawBranches(); }
            sun.cookie = cookie;
        }

        private void OnDisable() {
            if ( lightData != null ) { lightData.lightCookieOffset = restOffset; }
            if ( sun != null ) { sun.cookie = null; }
        }

        private void OnDestroy() {
            if ( cookie != null ) { Destroy( cookie ); }
        }

        private void Update() {
            if ( lightData == null ) { return; }
            // Two layers of noise: a slow lean plus quicker flutter on top.
            float t = Time.time * gustSpeed;
            float x = ( Mathf.PerlinNoise( seed, t ) - 0.5f ) + 0.3f * ( Mathf.PerlinNoise( seed + 3f, t * 4f ) - 0.5f );
            float y = ( Mathf.PerlinNoise( seed + 9f, t ) - 0.5f ) * 0.5f;
            lightData.lightCookieOffset = restOffset + new Vector2( x, y ) * ( 2f * sway );
        }

        // White = sunlight, gray = branch. Everything wraps around the edges so the texture tiles.
        private Texture2D DrawBranches() {
            var shade = new float[Size * Size];
            for ( int i = 0; i < shade.Length; i++ ) { shade[i] = 1f; }
            var rng = new System.Random( treeSeed );

            for ( int t = 0; t < 4; t++ ) {
                Branch( shade, rng, Next( rng ) * Size, Next( rng ) * Size, Next( rng ) * Mathf.PI * 2f,
                        150f + Next( rng ) * 80f, 5f + Next( rng ) * 3f, 6 );
            }
            Blur( shade );
            Blur( shade );

            var pixels = new Color32[shade.Length];
            for ( int i = 0; i < shade.Length; i++ ) {
                byte b = (byte)( Mathf.Clamp01( shade[i] ) * 255f );
                pixels[i] = new Color32( b, b, b, b );
            }
            var tex = new Texture2D( Size, Size, TextureFormat.RGBA32, true ) {
                name = "TreeShadows (generated)",
                wrapMode = TextureWrapMode.Repeat,
                hideFlags = HideFlags.DontSave
            };
            tex.SetPixels32( pixels );
            tex.Apply( true, true );
            return tex;
        }

        private static float Next( System.Random rng ) { return (float)rng.NextDouble(); }

        // One limb: wander forward a step at a time, then split into thinner limbs. Leaf clumps at the tips.
        private void Branch( float[] shade, System.Random rng, float x, float y, float angle, float length, float thickness, int depth ) {
            int steps = Mathf.Max( 2, (int)( length / 2f ) );
            for ( int s = 0; s < steps; s++ ) {
                angle += ( Next( rng ) - 0.5f ) * 0.18f;
                x += Mathf.Cos( angle ) * 2f;
                y += Mathf.Sin( angle ) * 2f;
                Stamp( shade, x, y, Mathf.Lerp( thickness, thickness * 0.7f, (float)s / steps ), branchShade );
            }
            if ( depth <= 0 || thickness < 0.7f ) {
                for ( int k = 0; k < 5; k++ ) {
                    Stamp( shade, x + ( Next( rng ) - 0.5f ) * 18f, y + ( Next( rng ) - 0.5f ) * 18f, 2f + Next( rng ) * 4f,
                           Mathf.Lerp( branchShade, 1f, 0.3f ) );
                }
                return;
            }
            int children = Next( rng ) > 0.6f ? 3 : 2;
            for ( int k = 0; k < children; k++ ) {
                Branch( shade, rng, x, y, angle + ( Next( rng ) - 0.5f ) * 1.4f, length * ( 0.55f + Next( rng ) * 0.25f ),
                        thickness * 0.62f, depth - 1 );
            }
        }

        // A soft round dot, keeping whichever is darker.
        private static void Stamp( float[] shade, float cx, float cy, float radius, float value ) {
            int r = Mathf.CeilToInt( radius + 1f );
            for ( int dy = -r; dy <= r; dy++ ) {
                for ( int dx = -r; dx <= r; dx++ ) {
                    float edge = Mathf.Clamp01( radius + 1f - Mathf.Sqrt( dx * dx + dy * dy ) );
                    if ( edge <= 0f ) { continue; }
                    int px = Wrap( (int)cx + dx ), py = Wrap( (int)cy + dy );
                    float v = Mathf.Lerp( 1f, value, edge );
                    if ( v < shade[py * Size + px] ) { shade[py * Size + px] = v; }
                }
            }
        }

        // The tree is far away, so its shadow is soft. A 5-wide box blur, across then down.
        private static void Blur( float[] shade ) {
            var tmp = new float[shade.Length];
            for ( int y = 0; y < Size; y++ ) {
                for ( int x = 0; x < Size; x++ ) {
                    float sum = 0f;
                    for ( int k = -2; k <= 2; k++ ) { sum += shade[y * Size + Wrap( x + k )]; }
                    tmp[y * Size + x] = sum / 5f;
                }
            }
            for ( int y = 0; y < Size; y++ ) {
                for ( int x = 0; x < Size; x++ ) {
                    float sum = 0f;
                    for ( int k = -2; k <= 2; k++ ) { sum += tmp[Wrap( y + k ) * Size + x]; }
                    shade[y * Size + x] = sum / 5f;
                }
            }
        }

        private static int Wrap( int i ) { return ( ( i % Size ) + Size ) % Size; }
    }
}
