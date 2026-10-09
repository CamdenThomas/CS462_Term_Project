using UnityEngine;

namespace TroUble.Data {
    /// <summary>Payload of PlayerCaught. Item is null when no contraband is involved.</summary>
    [System.Serializable]
    public struct CatchReport {
        public CatchReason Reason;
        public ItemData Item;
        public Vector3 Where;
    }
}
