using System;
using Game.Core.Modes;
using Game.Data.Progression;

namespace Game.Infrastructure
{
    /// <summary>
    /// Duel matchmaking with no online backend behind it. There is no queue to join
    /// yet, so every search runs its clock out and is filled by a generated
    /// opponent - which is also what the live queue will do early on, when there are
    /// not enough commanders online to pair inside the power band.
    /// <para>
    /// The networked adapter replaces this class, not the flow: it will search the
    /// real queue and fall back to the same backfill on the same schedule.
    /// </para>
    /// </summary>
    public sealed class LocalPvpMatchmakingService : IMatchmakingService
    {
        private readonly float _backfillAfterSeconds;
        private readonly Func<int> _searchSeedProvider;
        private int _searchCounter;
        private MatchmakingRequest _request;
        private Action<MatchmakingResult> _onCompleted;
        private int _searchSeed;

        /// <summary>
        /// <paramref name="backfillAfterSeconds"/> defaults to the authored wait and
        /// is injectable so tests do not have to burn it. <paramref name="searchSeedProvider"/>
        /// pins which opponent is generated; left alone it counts searches, so each
        /// queue gives a different commander.
        /// </summary>
        public LocalPvpMatchmakingService(
            float backfillAfterSeconds = PvpMatchmakingPolicy.BackfillAfterSeconds,
            Func<int> searchSeedProvider = null)
        {
            _backfillAfterSeconds = backfillAfterSeconds < 0f ? 0f : backfillAfterSeconds;
            _searchSeedProvider = searchSeedProvider;
        }

        public bool IsSearching { get; private set; }
        public float SearchElapsedSeconds { get; private set; }

        public void FindOpponent(MatchmakingRequest request, Action<MatchmakingResult> onCompleted)
        {
            if (onCompleted == null)
            {
                return;
            }

            IsSearching = false;
            SearchElapsedSeconds = 0f;
            _request = null;
            _onCompleted = null;

            if (request == null)
            {
                onCompleted(MatchmakingResult.Denied(MatchmakingDenial.ServiceUnavailable));
                return;
            }

            // The anti-spoiler arena rule needs a shared pool; with no liberated
            // planet there is no legal map, so the duel is closed rather than
            // silently played on a story arena the player has not reached.
            if (request.UnlockedArenaIds.Length == 0)
            {
                onCompleted(MatchmakingResult.Denied(MatchmakingDenial.NoUnlockedArena));
                return;
            }

            _request = request;
            _onCompleted = onCompleted;
            _searchSeed = _searchSeedProvider?.Invoke() ?? _searchCounter++;
            IsSearching = true;

            // A zero wait is a development convenience, not the shipped behaviour.
            if (_backfillAfterSeconds <= 0f)
            {
                CompleteWithBackfill();
            }
        }

        public void Tick(float deltaSeconds)
        {
            if (!IsSearching || deltaSeconds <= 0f)
            {
                return;
            }

            SearchElapsedSeconds += deltaSeconds;
            if (SearchElapsedSeconds >= _backfillAfterSeconds)
            {
                CompleteWithBackfill();
            }
        }

        public void CancelSearch()
        {
            IsSearching = false;
            SearchElapsedSeconds = 0f;
            _request = null;
            _onCompleted = null;
        }

        private void CompleteWithBackfill()
        {
            var request = _request;
            var onCompleted = _onCompleted;
            var elapsed = SearchElapsedSeconds;
            IsSearching = false;
            _request = null;
            _onCompleted = null;
            if (request == null || onCompleted == null)
            {
                return;
            }

            var matchId = $"match.{request.PlayerId}.{_searchSeed}";
            var opponent = PvpBotOpponentFactory.Create(
                request,
                matchId,
                PvpMatchmakingPolicy.ResolveBandFraction(elapsed));
            onCompleted(MatchmakingResult.Success(matchId, opponent));
        }
    }
}
