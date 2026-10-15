namespace Game.Core.Modes
{
    /// <summary>
    /// How wide the duel search looks and how long it waits before giving up on
    /// live opponents. The band ladder is the one authored in the canonical economy
    /// sheet: historical power is the match key, and it never shrinks after a
    /// purchase or an upgrade, so a player cannot dodge into an easier bracket.
    /// </summary>
    public static class PvpMatchmakingPolicy
    {
        /// <summary>Opening band: opponents within ±10% of the player's historical power.</summary>
        public const float InitialBandFraction = 0.10f;

        /// <summary>Widened band once the opening one has come up empty.</summary>
        public const float WidenedBandFraction = 0.20f;

        /// <summary>Seconds before the search widens from the opening band.</summary>
        public const float WidenAfterSeconds = 20f;

        /// <summary>
        /// Seconds before the search stops waiting for a live opponent. Early in the
        /// game's life the queue is nearly empty, and a player who waits longer than
        /// this simply stops queueing - so the match is filled rather than failed.
        /// </summary>
        public const float BackfillAfterSeconds = 45f;

        /// <summary>Band in force at <paramref name="elapsedSeconds"/> into the search.</summary>
        public static float ResolveBandFraction(float elapsedSeconds)
        {
            return elapsedSeconds < WidenAfterSeconds ? InitialBandFraction : WidenedBandFraction;
        }

        /// <summary>Whether the search has waited long enough to fill the match itself.</summary>
        public static bool ShouldBackfill(float elapsedSeconds)
        {
            return elapsedSeconds >= BackfillAfterSeconds;
        }

        /// <summary>Whether an opponent's power falls inside the band at that moment.</summary>
        public static bool IsInBand(long playerPower, long opponentPower, float elapsedSeconds)
        {
            var band = ResolveBandFraction(elapsedSeconds);
            var low = playerPower * (1f - band);
            var high = playerPower * (1f + band);
            return opponentPower >= low && opponentPower <= high;
        }
    }
}
