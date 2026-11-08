using System.Collections.Generic;
using UnityEngine;
using TroUble.Interaction;

namespace TroUble.Environment {
    /// <summary>
    /// The strip of lamplight under a closed door, and every so often the shadow of feet passing on the other side.
    /// The strip is split into segments (left to right) that dim one after another, so the shadow reads as moving.
    /// Goes dark while the door stands open, since then the real room light is pouring out instead.
    /// </summary>
    public class DoorGapLight : MonoBehaviour {
        private static readonly int EmissionId = Shader.PropertyToID( "_EmissionColor" );

        [System.Serializable]
        public struct Segment {
            public Light light;
            public Renderer strip;
        }

        [SerializeField] private List<Segment> segments = new();
        [Tooltip( "Optional. The door this light leaks under." )]
        [SerializeField] private Door door;
        [Tooltip( "Seconds between someone walking past, picked at random in this range." )]
        [SerializeField] private Vector2 passEvery = new( 6f, 16f );
        [Tooltip( "Seconds one pass takes to cross the whole strip." )]
        [SerializeField] private float passTime = 1.6f;
        [SerializeField, Range( 0f, 1f )] private float footDepth = 0.9f;

        private readonly List<float> baseIntensity = new();
        private readonly List<Color> baseEmission = new();
        private MaterialPropertyBlock block;
        private float passStart = -99f;
        private float nextPass;
        private bool leftToRight;

        private void Awake() {
            block = new MaterialPropertyBlock();
            foreach ( Segment s in segments ) {
                baseIntensity.Add( s.light != null ? s.light.intensity : 0f );
                baseEmission.Add( s.strip != null ? s.strip.sharedMaterial.GetColor( EmissionId ) : Color.black );
            }
            nextPass = Time.time + Random.Range( passEvery.x, passEvery.y );
        }

        private void Update() {
            if ( Time.time >= nextPass ) {
                passStart = Time.time;
                leftToRight = Random.value > 0.5f;
                nextPass = Time.time + passTime + Random.Range( passEvery.x, passEvery.y );
            }

            bool open = door != null && door.IsOpen;
            for ( int i = 0; i < segments.Count; i++ ) {
                float level = open ? 0f : 1f - footDepth * Shadow( i );
                if ( segments[i].light != null ) { segments[i].light.intensity = baseIntensity[i] * level; }
                if ( segments[i].strip != null ) {
                    segments[i].strip.GetPropertyBlock( block );
                    block.SetColor( EmissionId, baseEmission[i] * level );
                    segments[i].strip.SetPropertyBlock( block );
                }
            }
        }

        // How much of segment i is covered right now: two feet, one after the other, crossing the strip.
        private float Shadow( int i ) {
            float t = ( Time.time - passStart ) / passTime;
            if ( t < 0f || t > 1f || segments.Count == 0 ) { return 0f; }
            float place = ( i + 0.5f ) / segments.Count;
            if ( !leftToRight ) { place = 1f - place; }
            float body = Bump( t, place, 0.35f ) * 0.35f;      // the person blocking the lamp
            float stepA = Bump( t, place - 0.08f, 0.09f );
            float stepB = Bump( t, place + 0.08f, 0.09f );
            return Mathf.Clamp01( body + Mathf.Max( stepA, stepB ) );
        }

        private static float Bump( float x, float center, float width ) {
            float d = ( x - center ) / width;
            return Mathf.Exp( -d * d );
        }
    }
}
