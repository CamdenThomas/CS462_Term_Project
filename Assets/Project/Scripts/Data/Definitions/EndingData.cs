using System.Collections.Generic;
using UnityEngine;

namespace TroUble.Data {
    /// <summary>One ending. EndingResolver checks the conditions.</summary>
    [CreateAssetMenu( menuName = "TroUble/Data/Ending" )]
    public class EndingData : ScriptableObject {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Title { get; private set; }
        [field: SerializeField, TextArea] public string Description { get; private set; }
        [field: SerializeField] public string Hint { get; private set; }
        [field: SerializeField] public Sprite Card { get; private set; }
        [field: SerializeField] public bool IsEarly { get; private set; }
        [field: SerializeField] public int Priority { get; private set; }
        [field: SerializeField] public List<TraitCondition> Conditions { get; private set; } = new();
    }
}
