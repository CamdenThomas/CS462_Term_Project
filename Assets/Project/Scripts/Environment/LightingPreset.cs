using UnityEngine;
using UnityEngine.Rendering;

namespace TroUble.Environment {
    /// <summary>
    /// How the house looks at one moment of the day: where the sun sits, what color the light and the shadows are,
    /// how thick the air is, what the sky in the windows looks like, and the color grade on top.
    /// HouseLighting blends between two of these as the clock moves.
    /// </summary>
    [CreateAssetMenu( menuName = "TroUble/Lighting Preset" )]
    public class LightingPreset : ScriptableObject {
        [Header( "Sun" )]
        [Tooltip( "Degrees above the horizon. Low numbers make long shadows that reach across whole rooms." )]
        [Range( 0f, 90f )] public float sunElevation = 8f;
        [Tooltip( "Which way the sunlight travels, in degrees. 0 = toward the back wall (+Z), 90 = toward the east wall (+X), -90 = toward the west wall." )]
        [Range( -180f, 180f )] public float sunHeading = 60f;
        public Color sunColor = new( 1f, 0.45f, 0.2f );
        [Min( 0f )] public float sunIntensity = 3f;

        [Header( "Ambient: the light inside the shadows. Darker = blacker shadows" )]
        public Color ambientSky = new( 0.12f, 0.1f, 0.16f );
        public Color ambientEquator = new( 0.08f, 0.1f, 0.11f );
        public Color ambientGround = new( 0.03f, 0.025f, 0.02f );

        [Header( "Fog" )]
        public Color fogColor = new( 0.1f, 0.09f, 0.14f );
        [Min( 0f )] public float fogDensity = 0.04f;

        [Header( "Sky in the windows" )]
        [ColorUsage( false, true )] public Color skyHorizon = new( 1.6f, 0.55f, 0.2f );
        [ColorUsage( false, true )] public Color skyZenith = new( 0.25f, 0.12f, 0.3f );
        [Tooltip( "The trees and roofs outside, seen as a dark shape against the sky." )]
        public Color skySilhouette = new( 0.03f, 0.02f, 0.04f );

        [Header( "Color grade" )]
        [Tooltip( "Post-processing profile for this time of day: color adjustments, white balance, split toning." )]
        public VolumeProfile grade;
    }
}
