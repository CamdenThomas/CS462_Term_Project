namespace TroUble.Data {
    /// <summary>Payload of MetersChanged, so the UI never needs a reference to PlayerStats.</summary>
    [System.Serializable]
    public struct MeterSnapshot {
        public float Hunger;
        public float Energy;
        public float Boredom;
        public float Suspicion;
    }
}
