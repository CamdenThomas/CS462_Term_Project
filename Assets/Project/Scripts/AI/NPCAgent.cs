using UnityEngine;
using UnityEngine.AI;

namespace TroUble.AI {
    /// <summary>
    /// The legs of an NPC: walks to a point on the NavMesh and says when it has arrived.
    /// The schedule and state machine from the UML get added on top of this later.
    /// </summary>
    [RequireComponent( typeof( NavMeshAgent ) )]
    public class NPCAgent : MonoBehaviour {
        [SerializeField] private string npcId;

        private NavMeshAgent nav;

        public string NpcId => npcId;

        /// <summary>True once the NPC has reached where GoTo sent it.</summary>
        public bool HasArrived => !nav.pathPending && nav.remainingDistance <= nav.stoppingDistance + 0.05f;

        private void Awake() { nav = GetComponent<NavMeshAgent>(); }

        public void GoTo( Vector3 target ) {
            nav.isStopped = false;
            nav.SetDestination( target );
        }

        /// <summary>Stand still right here. GoTo starts her walking again.</summary>
        public void Stop() { nav.isStopped = true; }

        /// <summary>Turn on the spot toward a direction, by at most this many degrees.</summary>
        public void Face( Vector3 direction, float maxDegrees ) {
            direction.y = 0f;
            if ( direction.sqrMagnitude < 0.0001f ) { return; }
            transform.rotation = Quaternion.RotateTowards( transform.rotation, Quaternion.LookRotation( direction ), maxDegrees );
        }
    }
}
