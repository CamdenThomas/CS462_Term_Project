using System.Collections.Generic;
using UnityEngine;

namespace TroUble.Data {
    /// <summary>All tuning for one life stage. A new stage is a new asset, not new code.</summary>
    [CreateAssetMenu( menuName = "TroUble/Data/Stage" )]
    public class StageData : ScriptableObject {
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public int MinAge { get; private set; }
        [field: SerializeField] public int MaxAge { get; private set; }
        [field: SerializeField] public int WeekCount { get; private set; } = 2;
        [field: SerializeField] public TraitType FocusTrait { get; private set; }
        [field: SerializeField] public MeterRates Rates { get; private set; }
        [field: SerializeField] public float ParentViewMultiplier { get; private set; } = 1f;
        [field: SerializeField] public float PatrolInterval { get; private set; }
        [field: SerializeField] public float EyeHeight { get; private set; } = 1.1f;
        [field: SerializeField] public List<string> UnlockedRooms { get; private set; } = new();
        [field: SerializeField] public List<TaskData> WeeklyTasks { get; private set; } = new();
        [field: SerializeField] public List<ScheduleData> Schedules { get; private set; } = new();
        [field: SerializeField] public AudioClip CalmMusic { get; private set; }
        [field: SerializeField] public AudioClip TensionMusic { get; private set; }

        /// <summary>The schedule for one NPC in this stage, or null if none.</summary>
        public ScheduleData ScheduleFor( string npcId ) {
            foreach ( ScheduleData schedule in Schedules ) {
                if ( schedule != null && schedule.NpcId == npcId ) return schedule;
            }
            return null;
        }
    }
}
