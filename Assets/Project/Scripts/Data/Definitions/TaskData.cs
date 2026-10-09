using System.Collections.Generic;
using UnityEngine;

namespace TroUble.Data {
    /// <summary>One chore or homework assignment.</summary>
    [CreateAssetMenu( menuName = "TroUble/Data/Task" )]
    public class TaskData : ScriptableObject {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public TaskKind Kind { get; private set; }
        [field: SerializeField] public float Duration { get; private set; } = 3f;
        [field: SerializeField] public List<TraitDelta> Rewards { get; private set; } = new();
        [field: SerializeField] public float SuspicionIfMissed { get; private set; }
        [field: SerializeField] public int GradesPenaltyIfMissed { get; private set; }
    }
}
