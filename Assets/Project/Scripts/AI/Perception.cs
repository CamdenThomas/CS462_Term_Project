using System;
using UnityEngine;
using TroUble.Data;
using TroUble.Events;
using TroUble.Player;

namespace TroUble.AI {
    /// <summary>
    /// An NPC's eyes. Checks whether the player is inside the view cone with nothing in the way. Seeing the player
    /// is fine on its own (it is their house too); awareness only builds while she watches them misbehave.
    /// Awareness 0 means nothing noticed, 1 means caught in the act. Hearing comes later.
    /// </summary>
    public class Perception : MonoBehaviour {
        [SerializeField] private Transform eye;
        [SerializeField] private PlayerAnchor player;
        [SerializeField] private float viewAngle = 100f;
        [SerializeField] private float viewRange = 7f;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private DetectionChannel playerDetected;

        [Header( "Seconds of watching the player misbehave until they are caught" )]
        [SerializeField] private float nearSpotTime = 0.5f;
        [SerializeField] private float farSpotTime = 2.5f;
        [SerializeField] private float forgetTime = 3f;
        [SerializeField] private float watchedHideWindow = 0.5f;    // seen this recently = she watched them hide

        private float viewMultiplier = 1f;
        private float lastRaised = -1f;
        private bool spotted;
        private bool wasHidden;
        private bool watchedThemHide;
        private float lastSeenTime = -99f;

        public event Action<Vector3> Spotted;

        public float Awareness { get; private set; }
        public Vector3 LastKnownPosition { get; private set; }

        private float Range => viewRange * viewMultiplier;

        private void Update() {
            // Only a problem if she knows where the player is AND they are up to something.
            if ( KnowsWherePlayerIs() && player.Interactor.IsMisbehaving ) {
                Vector3 where = player.Controller.transform.position;
                LastKnownPosition = where;
                // Close up she notices almost at once; at the edge of her sight it takes a while.
                float distance01 = Mathf.Clamp01( Vector3.Distance( eye.position, where ) / Range );
                Awareness += Time.deltaTime / Mathf.Lerp( nearSpotTime, farSpotTime, distance01 );
            } else {
                Awareness -= Time.deltaTime / forgetTime;
            }
            Awareness = Mathf.Clamp01( Awareness );

            if ( Awareness >= 1f && !spotted ) {
                spotted = true;
                Spotted?.Invoke( LastKnownPosition );
            } else if ( Awareness < 1f ) {
                spotted = false;
            }

            // Tell the HUD only when the number actually moved.
            if ( !Mathf.Approximately( Awareness, lastRaised ) ) {
                lastRaised = Awareness;
                playerDetected.Raise( new DetectionEvent { Watcher = transform, Level = LevelFor( Awareness ), Awareness = Awareness } );
            }
        }

        // She knows where the player is while she can see them, and also after watching them climb into a
        // hiding spot: a vent is no use if she saw you go in. Coming back out clears that.
        private bool KnowsWherePlayerIs() {
            if ( !player.IsSet ) { return false; }

            bool inSight = CanSeePlayer();
            if ( inSight ) { lastSeenTime = Time.time; }

            bool hidden = player.Interactor.IsHidden;
            if ( hidden && !wasHidden ) { watchedThemHide = Time.time - lastSeenTime < watchedHideWindow; }
            if ( !hidden ) { watchedThemHide = false; }
            wasHidden = hidden;

            return inSight || watchedThemHide;
        }

        public bool CanSeePlayer() {
            if ( !player.IsSet || player.Interactor.IsHidden ) { return false; }

            Transform body = player.Controller.transform;
            Vector3 toPlayer = body.position - eye.position;
            if ( toPlayer.magnitude > Range ) { return false; }

            // The cone is measured flat on the ground, so a kid right under her nose still counts.
            Vector3 flatForward = eye.forward;
            flatForward.y = 0f;
            toPlayer.y = 0f;
            if ( Vector3.Angle( flatForward, toPlayer ) > viewAngle * 0.5f ) { return false; }

            // Seen if either the head or the middle of the body has a clear line to her eyes.
            float height = player.Controller.Mode == MoveMode.Crouch ? 0.8f : 1.2f;
            Vector3 head = body.position + Vector3.up * ( height - 0.15f );
            Vector3 middle = body.position + Vector3.up * ( height * 0.5f );
            return !Physics.Linecast( eye.position, head, obstacleMask ) || !Physics.Linecast( eye.position, middle, obstacleMask );
        }

        /// <summary>Stages make parents see farther by raising this.</summary>
        public void SetViewMultiplier( float multiplier ) { viewMultiplier = multiplier; }

        private static DetectionLevel LevelFor( float awareness ) {
            if ( awareness >= 1f ) { return DetectionLevel.Spotted; }
            return awareness > 0f ? DetectionLevel.Suspicious : DetectionLevel.None;
        }

        // Select the NPC in the Scene view to see the edges of the cone.
        private void OnDrawGizmosSelected() {
            if ( eye == null ) { return; }
            Vector3 flatForward = eye.forward;
            flatForward.y = 0f;
            Gizmos.color = new Color( 1f, 0.55f, 0.2f );
            Gizmos.DrawRay( eye.position, Quaternion.Euler( 0f, -viewAngle * 0.5f, 0f ) * flatForward.normalized * viewRange );
            Gizmos.DrawRay( eye.position, Quaternion.Euler( 0f, viewAngle * 0.5f, 0f ) * flatForward.normalized * viewRange );
        }
    }
}
