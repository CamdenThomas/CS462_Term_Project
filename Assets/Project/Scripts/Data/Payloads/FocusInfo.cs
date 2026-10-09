namespace TroUble.Data {
    /// <summary>Payload of FocusChanged ("E: Wash dishes").</summary>
    [System.Serializable]
    public struct FocusInfo {
        public bool HasFocus;
        public string Prompt;
        public float HoldTime;
    }
}
