using UnityEngine;

namespace TroUble.Data {
    /// <summary>Payload of NoiseMade.</summary>
    [System.Serializable]
    public struct NoiseEvent {
        public Vector3 Position;
        public float Radius;
        public string Source;
    }
}
