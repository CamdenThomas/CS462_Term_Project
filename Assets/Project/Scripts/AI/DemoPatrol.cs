using System.Collections.Generic;
using UnityEngine;
using TroUble.Data;
using TroUble.Events;

namespace TroUble.AI {
    /// <summary>
    /// DEMO ONLY. An NPC's whole brain for the demo: walk a fixed loop of stops, stop and stare when something
    /// catches her eye, and call the player caught once she is sure.
    /// The real version is ParentAI with the schedule and state machine; delete this when that exists.
    /// </summary>
    public class DemoPatrol : MonoBehaviour {
        [System.Serializable]
        public struct Stop {
            public Transform point;
            public float wait;
        }

        [SerializeField] private NPCAgent agent;
        [SerializeField] private List<Stop> stops = new();
        [SerializeField] private float turnSpeed = 240f;

        [Header( "Noticing the player" )]
        [SerializeField] private Perception perception;
        [SerializeField] private CatchChannel playerCaught;
        [SerializeField] private float noticeAt = 0.2f;

        private int index;
        private bool waiting;
        private float waitTimer;
        private bool alerted;

        public int StopIndex => index;
        public bool Waiting => waiting;
        public bool Alerted => alerted;

        private void OnEnable() { perception.Spotted += OnSpotted; }
        private void OnDisable() { perception.Spotted -= OnSpotted; }

        private void Start() {
            if ( stops.Count > 0 ) { agent.GoTo( stops[0].point.position ); }
        }

        private void Update() {
            if ( stops.Count == 0 ) { return; }

            // Something caught her eye: stop where she is and turn toward it until she loses interest.
            if ( perception.Awareness >= noticeAt ) {
                if ( !alerted ) {
                    alerted = true;
                    agent.Stop();
                }
                agent.Face( perception.LastKnownPosition - transform.position, turnSpeed * Time.deltaTime );
                return;
            }
            if ( alerted ) {
                alerted = false;
                if ( !waiting ) { agent.GoTo( stops[index].point.position ); }
            }

            Stop stop = stops[index];
            if ( !waiting ) {
                if ( !agent.HasArrived ) { return; }
                waiting = true;
                waitTimer = stop.wait;
            }

            // Standing at the stop: turn to face its direction and count down.
            agent.Face( stop.point.forward, turnSpeed * Time.deltaTime );
            waitTimer -= Time.deltaTime;
            if ( waitTimer > 0f ) { return; }

            waiting = false;
            index = ( index + 1 ) % stops.Count;
            agent.GoTo( stops[index].point.position );
        }

        private void OnSpotted( Vector3 where ) {
            playerCaught.Raise( new CatchReport { Reason = CatchReason.Seen, Item = null, Where = where } );
        }
    }
}
