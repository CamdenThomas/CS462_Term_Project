using UnityEngine;

namespace TroUble.Player {
    /// <summary>
    /// A shared asset that always points at the player in the scene. The player fills it in when it wakes up,
    /// so NPCs can find the player through the Inspector without any Find calls.
    /// </summary>
    [CreateAssetMenu( menuName = "TroUble/Player Anchor" )]
    public class PlayerAnchor : ScriptableObject {
        public PlayerController Controller { get; private set; }
        public PlayerInteractor Interactor { get; private set; }
        public Inventory Inventory { get; private set; }

        public bool IsSet => Controller != null;

        public void Set( PlayerController controller, PlayerInteractor interactor, Inventory inventory ) {
            Controller = controller;
            Interactor = interactor;
            Inventory = inventory;
        }

        public void Clear() {
            Controller = null;
            Interactor = null;
            Inventory = null;
        }
    }
}
