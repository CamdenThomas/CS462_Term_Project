namespace TroUble.Data {
    /// <summary>How fast each meter drains in a stage.</summary>
    [System.Serializable]
    public struct MeterRates {
        public float HungerPerHour;
        public float EnergyPerHour;
        public float BoredomPerHour;
        public float SuspicionDecayPerHour;
    }
}
