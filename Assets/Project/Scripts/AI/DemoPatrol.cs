using System.Collections.Generic;
using UnityEngine;

namespace TroUble.AI {
    /// <summary>
    /// DEMO ONLY. Walks an NPC around a fixed loop of stops, pausing at each and facing the way the stop faces.
    /// The real version is the schedule (ScheduleData + GameClock + ScheduleState); delete this when that exists.
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

        private int index;
        private bool waiting;
        private float waitTimer;

        public int StopIndex => index;
        public bool Waiting => waiting;

        private void Start() {
            if ( stops.Count > 0 ) { agent.GoTo( stops[0].point.position ); }
        }

        private void Update() {
            if ( stops.Count == 0 ) { return; }
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
    }
}
