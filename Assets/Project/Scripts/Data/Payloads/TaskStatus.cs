namespace TroUble.Data {
    /// <summary>One line on the sticky-note to-do list.</summary>
    [System.Serializable]
    public struct TaskStatus {
        public TaskData Task;
        public bool Done;
    }
}
