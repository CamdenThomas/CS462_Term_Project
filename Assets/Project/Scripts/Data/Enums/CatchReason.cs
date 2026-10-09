namespace TroUble.Data {
    /// <summary>Why the player got caught; used for the consequence and the parent's comment.</summary>
    public enum CatchReason {
        Seen,
        HeardSneaking,
        StashFound,
        Snitched,
        SuspicionMaxed
    }
}
