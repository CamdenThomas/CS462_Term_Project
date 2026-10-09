namespace TroUble.Data {
    /// <summary>Payload of StageCompleted. TraitValues is indexed by TraitType.</summary>
    [System.Serializable]
    public struct StageReport {
        public StageData Stage;
        public int[] TraitValues;
        public string ParentComment;
    }
}
