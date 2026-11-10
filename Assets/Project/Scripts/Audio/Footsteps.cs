using UnityEngine;
using TroUble.Data;
using TroUble.Events;
using TroUble.Player;

namespace TroUble.Audio {
    /// <summary>
    /// Plays a footstep every stride by watching how far this object moves, so it works on the player and on
    /// parents (NavMesh) alike. On the player, link the PlayerController: crouching is quiet, sprinting is loud,
    /// and every step is also raised on NoiseMade so parents can hear what the player hears.
    /// </summary>
    public class Footsteps : MonoBehaviour {
        [SerializeField] private SfxCue step;
        [Tooltip( "Metres per step. A kid's stride is short (about 0.5), a parent's long (about 0.8)." )]
        [SerializeField] private float strideLength = 0.5f;
        [Tooltip( "Only on the player. Leave empty on NPCs." )]
        [SerializeField] private PlayerController player;
        [Tooltip( "Optional: tell parents about the noise. Uses the cue's Noise Radius, scaled by how the player moves." )]
        [SerializeField] private NoiseChannel noiseMade;

        [Header( "Player volume and noise by move mode" )]
        [SerializeField] private float crouchScale = 0.3f;
        [SerializeField] private float walkScale = 0.7f;
        [SerializeField] private float sprintScale = 1f;
        [SerializeField] private float sprintNoiseMultiplier = 2f;

        private Vector3 lastPosition;
        private float travelled;

        private void OnEnable() {
            lastPosition = transform.position;
            travelled = 0f;
        }

        private void Update() {
            if ( step == null ) { return; }

            Vector3 delta = transform.position - lastPosition;
            lastPosition = transform.position;
            delta.y = 0f;
            float moved = delta.magnitude;
            // A teleport (respawn, climbing out of a vent) isn't walking.
            if ( moved > 1.5f ) { return; }

            MoveMode mode = player != null ? player.Mode : MoveMode.Walk;
            if ( mode == MoveMode.Hidden ) { travelled = 0f; return; }

            travelled += moved;
            float stride = mode == MoveMode.Sprint ? strideLength * 1.4f : strideLength;
            if ( travelled < stride ) { return; }
            travelled = 0f;

            float scale = player == null ? 1f : mode switch {
                MoveMode.Crouch => crouchScale,
                MoveMode.Sprint => sprintScale,
                _ => walkScale
            };
            step.PlayAt( transform.position, scale );

            if ( noiseMade != null && step.NoiseRadius > 0f ) {
                float radius = step.NoiseRadius * ( mode == MoveMode.Sprint ? sprintNoiseMultiplier : scale );
                noiseMade.Raise( new NoiseEvent { Position = transform.position, Radius = radius, Source = name } );
            }
        }
    }
}
