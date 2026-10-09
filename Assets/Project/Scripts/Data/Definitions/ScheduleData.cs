using System.Collections.Generic;
using UnityEngine;

namespace TroUble.Data {
    /// <summary>One NPC's daily routine for one stage.</summary>
    [CreateAssetMenu( menuName = "TroUble/Data/Schedule" )]
    public class ScheduleData : ScriptableObject {
        [field: SerializeField] public string NpcId { get; private set; }
        [field: SerializeField] public List<ScheduleEntry> Entries { get; private set; } = new();

        /// <summary>
        /// The entry in effect at this hour: the latest one that has already started.
        /// Before the first entry starts, returns the earliest entry.
        /// </summary>
        public ScheduleEntry EntryAt( float hour ) {
            ScheduleEntry current = default;
            ScheduleEntry earliest = default;
            bool found = false;
            for ( int i = 0; i < Entries.Count; i++ ) {
                ScheduleEntry entry = Entries[i];
                if ( i == 0 || entry.StartHour < earliest.StartHour ) earliest = entry;
                if ( entry.StartHour <= hour && ( !found || entry.StartHour >= current.StartHour ) ) {
                    current = entry;
                    found = true;
                }
            }
            return found ? current : earliest;
        }
    }
}
