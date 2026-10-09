using UnityEngine;

namespace TroUble.Data {
    /// <summary>One favor the brother can do.</summary>
    [CreateAssetMenu( menuName = "TroUble/Data/Favor" )]
    public class FavorData : ScriptableObject {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public FavorKind Kind { get; private set; }
        [field: SerializeField] public int BondCost { get; private set; }
        [field: SerializeField] public Attitude RequiredAttitude { get; private set; }
    }
}
