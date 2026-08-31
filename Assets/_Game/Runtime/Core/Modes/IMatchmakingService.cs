using System;

namespace Game.Core.Modes
{
    /// <summary>What the local commander brings to the queue.</summary>
    public sealed class MatchmakingRequest
    {
        public MatchmakingRequest(
            string playerId,
            string displayName,
            int commanderLevel,
            long historicalPower,
            string[] unlockedArenaIds,
            string[] loadoutWeaponIds)
        {
            PlayerId = playerId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            CommanderLevel = commanderLevel < 1 ? 1 : commanderLevel;
            HistoricalPower = historicalPower < 0 ? 0 : historicalPower;
            UnlockedArenaIds = unlockedArenaIds ?? Array.Empty<string>();
            LoadoutWeaponIds = loadoutWeaponIds ?? Array.Empty<string>();
        }

        public string PlayerId { get; }
        public string DisplayName { get; }
        public int CommanderLevel { get; }
        public long HistoricalPower { get; }
        public string[] UnlockedArenaIds { get; }
        public string[] LoadoutWeaponIds { get; }
    }

    public enum MatchmakingDenial
    {
        None = 0,

        /// <summary>The commander has not liberated a planet yet, so no arena is legal.</summary>
        NoUnlockedArena = 1,

        /// <summary>Nobody in range and backfill was refused; the caller may retry.</summary>
        NoOpponentFound = 2,

        /// <summary>Transport, auth or service failure; the caller may retry.</summary>
        ServiceUnavailable = 3
    }

    public readonly struct MatchmakingResult
    {
        private MatchmakingResult(bool matched, string matchId, PvpOpponentProfile opponent, MatchmakingDenial denial)
        {
            Matched = matched;
            MatchId = matchId ?? string.Empty;
            Opponent = opponent;
            Denial = denial;
        }

        public bool Matched { get; }

        /// <summary>Shared seed both clients derive the arena and coin toss from.</summary>
        public string MatchId { get; }
        public PvpOpponentProfile Opponent { get; }
        public MatchmakingDenial Denial { get; }

        public static MatchmakingResult Success(string matchId, PvpOpponentProfile opponent)
        {
            return new MatchmakingResult(true, matchId, opponent, MatchmakingDenial.None);
        }

        public static MatchmakingResult Denied(MatchmakingDenial denial)
        {
            return new MatchmakingResult(false, string.Empty, null, denial);
        }
    }

    /// <summary>
    /// Finds a duel opponent inside the historical-power band, and fills the match
    /// itself once the wait has run past
    /// <see cref="PvpMatchmakingPolicy.BackfillAfterSeconds"/>. Gameplay only ever
    /// sees this interface, so swapping the offline adapter for the networked one
    /// changes nothing above it.
    /// </summary>
    public interface IMatchmakingService
    {
        /// <summary>True between <see cref="FindOpponent"/> and the completion callback.</summary>
        bool IsSearching { get; }

        /// <summary>How long the current search has run, for the queue's timer display.</summary>
        float SearchElapsedSeconds { get; }

        /// <summary>
        /// Starts a search. <paramref name="onCompleted"/> is invoked exactly once,
        /// on the main thread, with the match or the reason there is none. A search
        /// that cannot resolve immediately completes from <see cref="Tick"/>.
        /// </summary>
        void FindOpponent(MatchmakingRequest request, Action<MatchmakingResult> onCompleted);

        /// <summary>Advances the search clock; the queue screen drives this per frame.</summary>
        void Tick(float deltaSeconds);

        /// <summary>Abandons an in-flight search; the pending callback is not invoked.</summary>
        void CancelSearch();
    }
}
