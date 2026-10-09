using UnityEngine;

namespace TroUble.Data {
    /// <summary>A pick-up-able item: snack, phone, console, car keys.</summary>
    [CreateAssetMenu( menuName = "TroUble/Data/Item" )]
    public class ItemData : ScriptableObject {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public bool IsContraband { get; private set; }
        [field: SerializeField] public float HungerRelief { get; private set; }
        [field: SerializeField] public float BoredomRelief { get; private set; }
    }
}
