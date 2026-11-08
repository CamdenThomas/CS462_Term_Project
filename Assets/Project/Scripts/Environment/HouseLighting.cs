using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TroUble.Data;
using TroUble.Events;

namespace TroUble.Environment {
    /// <summary>
    /// Time of day for the house. Holds a list of LightingPresets pinned to hours (dawn, dusk, night...) and blends
    /// between the two on either side of the current hour: sun angle and color, ambient, fog, the sky in the windows,
    /// and the color grade. Follows the game clock when one is hooked up; until then it lets the hour drift on its
    /// own so the sun keeps sinking while you play. Runs in the editor too, so dragging Hour previews the look.
    /// </summary>
    [ExecuteAlways]
    public class HouseLighting : MonoBehaviour {
        [Serializable]
        public struct Key {
            [Range( 0f, 24f )] public float hour;
            public LightingPreset preset;
        }

        private static readonly int SkyHorizonId = Shader.PropertyToID( "_HouseSkyHorizon" );
        private static readonly int SkyZenithId = Shader.PropertyToID( "_HouseSkyZenith" );
        private static readonly int SkySilhouetteId = Shader.PropertyToID( "_HouseSkySilhouette" );
        private static readonly int SkyActiveId = Shader.PropertyToID( "_HouseSkyActive" );

        [SerializeField] private Light sun;
        [Tooltip( "Keep these in order of hour. The day wraps, so the last key blends into the first one overnight." )]
        [SerializeField] private List<Key> keys = new();

        [Header( "Color grade: two global volumes, cross-faded between the two presets" )]
        [SerializeField] private Volume gradeFrom;
        [SerializeField] private Volume gradeTo;

        [Header( "Time" )]
        [Tooltip( "Optional. When the game clock raises this, the lighting follows its hour and stops drifting." )]
        [SerializeField] private ClockChannel clockTicked;
        [SerializeField, Range( 0f, 24f )] private float hour = 17.6f;
        [Tooltip( "In-game hours that pass per real minute while no clock is hooked up. 0 = frozen." )]
        [SerializeField] private float driftHoursPerMinute = 0.06f;

        private bool followingClock;

        public float Hour => hour;

        /// <summary>Jump the lighting to an hour (0-24), e.g. for a cutscene or a debug key.</summary>
        public void SetHour( float newHour ) {
            hour = Mathf.Repeat( newHour, 24f );
            Apply();
        }

        private void OnEnable() {
            followingClock = false;
            if ( Application.isPlaying && clockTicked != null ) { clockTicked.Subscribe( OnClock ); }
            Apply();
        }

        private void OnDisable() {
            if ( clockTicked != null ) { clockTicked.Unsubscribe( OnClock ); }
            Shader.SetGlobalFloat( SkyActiveId, 0f );
        }

        private void OnValidate() { Apply(); }

        private void Update() {
            if ( Application.isPlaying && !followingClock && driftHoursPerMinute != 0f ) {
                hour = Mathf.Repeat( hour + driftHoursPerMinute * Time.deltaTime / 60f, 24f );
            }
            Apply();
        }

        private void OnClock( ClockTime time ) {
            followingClock = true;
            hour = Mathf.Repeat( time.Hour, 24f );
        }

        // Find the keys on either side of the hour and how far we are from the first toward the second.
        private bool FindBlend( out LightingPreset a, out LightingPreset b, out float t ) {
            a = null; b = null; t = 0f;
            if ( keys.Count == 0 ) { return false; }

            int before = keys.Count - 1;    // if no key is at or before the hour, we are still in last night's span
            for ( int i = 0; i < keys.Count; i++ ) {
                if ( keys[i].hour <= hour ) { before = i; }
            }
            int after = ( before + 1 ) % keys.Count;

            a = keys[before].preset;
            b = keys[after].preset;
            float span = Mathf.Repeat( keys[after].hour - keys[before].hour, 24f );
            float into = Mathf.Repeat( hour - keys[before].hour, 24f );
            t = span > 0f ? Mathf.SmoothStep( 0f, 1f, into / span ) : 0f;

            if ( a == null ) { a = b; }
            if ( b == null ) { b = a; }
            return a != null;
        }

        private void Apply() {
            if ( !FindBlend( out LightingPreset a, out LightingPreset b, out float t ) ) { return; }

            if ( sun != null ) {
                float elevation = Mathf.Lerp( a.sunElevation, b.sunElevation, t );
                float heading = Mathf.LerpAngle( a.sunHeading, b.sunHeading, t );
                sun.transform.rotation = Quaternion.Euler( elevation, heading, 0f );
                sun.color = Color.Lerp( a.sunColor, b.sunColor, t );
                sun.intensity = Mathf.Lerp( a.sunIntensity, b.sunIntensity, t );
            }

            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = Color.Lerp( a.ambientSky, b.ambientSky, t );
            RenderSettings.ambientEquatorColor = Color.Lerp( a.ambientEquator, b.ambientEquator, t );
            RenderSettings.ambientGroundColor = Color.Lerp( a.ambientGround, b.ambientGround, t );

            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogColor = Color.Lerp( a.fogColor, b.fogColor, t );
            RenderSettings.fogDensity = Mathf.Lerp( a.fogDensity, b.fogDensity, t );

            // The window shader reads these globals, so every window changes without touching its material.
            Shader.SetGlobalColor( SkyHorizonId, Color.Lerp( a.skyHorizon, b.skyHorizon, t ) );
            Shader.SetGlobalColor( SkyZenithId, Color.Lerp( a.skyZenith, b.skyZenith, t ) );
            Shader.SetGlobalColor( SkySilhouetteId, Color.Lerp( a.skySilhouette, b.skySilhouette, t ) );
            Shader.SetGlobalFloat( SkyActiveId, 1f );

            // Swapping which profile a volume points at never edits the profile asset itself.
            SetGrade( gradeFrom, a.grade, 1f - t );
            SetGrade( gradeTo, b.grade, a == b ? 0f : t );
        }

        private static void SetGrade( Volume volume, VolumeProfile profile, float weight ) {
            if ( volume == null ) { return; }
            if ( volume.sharedProfile != profile ) { volume.sharedProfile = profile; }
            volume.weight = profile != null ? weight : 0f;
        }
    }
}
