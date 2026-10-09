namespace TroUble.Data {
    /// <summary>One line of an NPC schedule: at this hour, go here and do this.</summary>
    [System.Serializable]
    public struct ScheduleEntry {
        public DayPhase Phase;
        public float StartHour;
        public string WaypointId;
        public string Activity;
    }
}
