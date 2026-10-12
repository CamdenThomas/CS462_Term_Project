using UnityEngine;

namespace TroUble.Interaction {
    /// <summary>
    /// What an interactable is allowed to know about whoever used it. Keeps Interaction independent of Player.
    /// </summary>
    public interface IInteractor {
        Transform Transform { get; }
        IItemHolder Items { get; }
    }
}
