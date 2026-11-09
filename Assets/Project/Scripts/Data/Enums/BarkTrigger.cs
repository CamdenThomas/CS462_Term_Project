namespace TroUble.Data {
    /// <summary>
    /// What makes a character say a line. A BarkSet holds lines for each one; a character with no lines for a
    /// trigger just stays quiet. New values go at the END so existing assets keep their numbers.
    /// </summary>
    public enum BarkTrigger {
        // Parents: suspicion rising (the full-name meter)
        Suspicious,
        SuspicionHalf,
        SuspicionHigh,

        // Parents: caught, one per CatchReason
        CaughtSeen,
        CaughtHeardSneaking,
        CaughtStashFound,
        CaughtSnitched,
        CaughtSuspicionMaxed,

        // Parents: everyday
        DistractionFound,
        SameTrickTwice,
        FoundHiding,
        BathroomTooLong,
        ChoreDone,
        ChoreSkipped,
        NightCheck,

        // Brother
        Talk,
        FavorDistract,
        FavorRetrieve,
        FavorTakeTheFall,
        FavorRefused,
        Snitch,
        StashNoticed
    }
}
