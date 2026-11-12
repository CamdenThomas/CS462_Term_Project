using UnityEngine;

namespace TroUble.Environment {
    /// <summary>
    /// A house plant moving in a draught. Every child of this object (each leaf or stem, pivoted at its base) leans
    /// back and forth a few degrees on slow, uneven noise, so the plant never quite repeats and the shadows it throws
    /// on the walls drift. Put it on the plant's foliage root, not on the pot.
    /// </summary>
    public class PlantSway : MonoBehaviour {
        [Tooltip( "Largest lean, in degrees." )]
        [SerializeField] private float angle = 6f;
        [Tooltip( "How fast the draught changes. Lower is lazier." )]
        [SerializeField] private float speed = 0.35f;
        [Tooltip( "Which way the draught blows, in this object's space." )]
        [SerializeField] private Vector3 breeze = new( 1f, 0f, 0.3f );

        private Transform[] leaves;
        private Quaternion[] rest;
        private float[] offset;

        private void Awake() {
            int count = transform.childCount;
            leaves = new Transform[count];
            rest = new Quaternion[count];
            offset = new float[count];
            for ( int i = 0; i < count; i++ ) {
                leaves[i] = transform.GetChild( i );
                rest[i] = leaves[i].localRotation;
                offset[i] = Random.value * 100f;
            }
        }

        private void Update() {
            Vector3 axis = Vector3.Cross( Vector3.up, breeze.normalized );
            if ( axis.sqrMagnitude < 0.0001f ) { axis = Vector3.right; }
            float gust = Mathf.PerlinNoise( Time.time * speed * 0.3f, 7.1f );      // the whole plant: calm, then a gust
            for ( int i = 0; i < leaves.Length; i++ ) {
                float t = Time.time * speed + offset[i];
                float lean = ( Mathf.PerlinNoise( t, offset[i] ) - 0.5f ) * 2f;     // this leaf: -1..1
                float flutter = Mathf.Sin( t * 5.3f ) * 0.15f;
                float degrees = angle * ( 0.35f + gust ) * ( lean + flutter );
                leaves[i].localRotation = Quaternion.AngleAxis( degrees, axis ) * rest[i];
            }
        }

        private void OnDisable() {
            if ( leaves == null ) { return; }
            for ( int i = 0; i < leaves.Length; i++ ) { if ( leaves[i] != null ) { leaves[i].localRotation = rest[i]; } }
        }
    }
}
