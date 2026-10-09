namespace TroUble.Data {
    /// <summary>One requirement for an ending (FamilyTrust AtLeast 60).</summary>
    [System.Serializable]
    public struct TraitCondition {
        public TraitType Trait;
        public Comparison Comparison;
        public int Value;
    }
}
