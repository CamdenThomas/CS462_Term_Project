namespace TroUble.Data {
    /// <summary>A change to one life trait (+3 Discipline).</summary>
    [System.Serializable]
    public struct TraitDelta {
        public TraitType Trait;
        public int Amount;
    }
}
