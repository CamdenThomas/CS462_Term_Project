using UnityEngine;

namespace TroUble.Environment {
    /// <summary>
    /// Turns slowly around one axis, a little unevenly, like an old ceiling fan. With a light beside the blades
    /// it sweeps their shadows around the room.
    /// </summary>
    public class Spinner : MonoBehaviour {
        [SerializeField] private Vector3 axis = Vector3.up;
        [SerializeField] private float degreesPerSecond = 45f;
        [Tooltip( "How much the speed wanders, as a fraction. 0 = perfectly steady." )]
        [SerializeField, Range( 0f, 1f )] private float unevenness = 0.25f;

        private float seed;

        private void Awake() { seed = Random.value * 100f; }

        private void Update() {
            float wobble = 1f + unevenness * ( Mathf.PerlinNoise( seed, Time.time * 0.4f ) * 2f - 1f );
            transform.Rotate( axis, degreesPerSecond * wobble * Time.deltaTime, Space.Self );
        }
    }
}
