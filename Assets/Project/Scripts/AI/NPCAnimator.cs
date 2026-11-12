using UnityEngine;
using UnityEngine.AI;

namespace TroUble.AI {
    /// <summary>
    /// Plays the right body animation for how fast an NPC is moving: idle while standing, walk while walking.
    /// Each frame it hands the NavMeshAgent's speed (metres per second) to the Animator's "Speed" parameter, and the
    /// Animator Controller blends between Idle and Walking by that number.
    /// </summary>
    public class NPCAnimator : MonoBehaviour {
        private static readonly int Speed = Animator.StringToHash( "Speed" );

        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent agent;
        [Tooltip( "Seconds to ease into a new speed, so starting and stopping don't snap." )]
        [SerializeField] private float damping = 0.12f;

        private void Update() {
            if ( animator == null ) { return; }
            float speed = 0f;
            if ( agent != null && agent.enabled ) {
                Vector3 v = agent.velocity;
                speed = new Vector2( v.x, v.z ).magnitude;
            }
            animator.SetFloat( Speed, speed, damping, Time.deltaTime );
        }
    }
}
