using UnityEngine;
using TroUble.Data;
using TroUble.Events;
using TroUble.Player;

namespace TroUble.Audio {
    /// <summary>
    /// A floorboard or stair that creaks when the player steps on it: the board outside the parents' door and
    /// stairs 3 and 14. Put it on a thin trigger collider over the spot. Crouching makes it quieter, never silent.
    /// </summary>
    [RequireComponent( typeof( Collider ) )]
    public class CreakyBoard : MonoBehaviour {
        [SerializeField] private SfxCue creak;
        [SerializeField] private NoiseChannel noiseMade;
        [SerializeField, Range( 0f, 1f )] private float crouchScale = 0.5f;
        [Tooltip( "Seconds before it can creak again, so standing on the edge doesn't machine-gun it." )]
        [SerializeField] private float rearm = 0.6f;

        private float lastCreak = -99f;

        private void Reset() { GetComponent<Collider>().isTrigger = true; }

        private void OnTriggerEnter( Collider other ) {
            if ( creak == null || Time.time - lastCreak < rearm ) { return; }
            PlayerController player = other.GetComponentInParent<PlayerController>();
            if ( player == null || player.Mode == MoveMode.Hidden ) { return; }
            lastCreak = Time.time;

            float scale = player.Mode == MoveMode.Crouch ? crouchScale : 1f;
            creak.PlayAt( transform.position, scale );
            if ( noiseMade != null && creak.NoiseRadius > 0f ) {
                noiseMade.Raise( new NoiseEvent { Position = transform.position, Radius = creak.NoiseRadius * scale, Source = name } );
            }
        }
    }
}
