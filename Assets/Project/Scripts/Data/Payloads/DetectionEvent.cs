using UnityEngine;

namespace TroUble.Data {
    /// <summary>Payload of PlayerDetected.</summary>
    [System.Serializable]
    public struct DetectionEvent {
        public Transform Watcher;
        public DetectionLevel Level;
        public float Awareness;
    }
}
