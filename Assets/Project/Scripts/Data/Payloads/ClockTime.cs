namespace TroUble.Data {
    /// <summary>Payload of ClockTicked.</summary>
    [System.Serializable]
    public struct ClockTime {
        public int Day;
        public float Hour;
        public DayPhase Phase;
    }
}
