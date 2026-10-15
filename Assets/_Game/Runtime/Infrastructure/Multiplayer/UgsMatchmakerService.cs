using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Game.Core.Modes;
using Game.Data.Progression;
using Unity.Services.Multiplayer;
using UnityEngine;

namespace Game.Infrastructure.Multiplayer
{
    /// <summary>
    /// Duel matchmaking over Multiplayer Services sessions.
    /// <para>
    /// The band rules are the same ones the offline adapter follows and the economy
    /// sheet authors: ±10% of historical power, widening to ±20% after 20 seconds,
    /// and at 45 seconds the match is filled by a generated opponent rather than
    /// left to time out. That last rule is what keeps the mode playable while the
    /// queue is still nearly empty, and it is why this class must not simply
    /// surface the service's own timeout.
    /// </para>
    /// </summary>
    public sealed class UgsMatchmakerService : IMatchmakingService
    {
        /// <summary>Session type, so duels never match into another mode's sessions.</summary>
        public const string SessionType = "duel";

        /// <summary>Filterable index holding the commander's historical power.</summary>
        public const PropertyIndex PowerIndex = PropertyIndex.Number1;

        private const FilterField PowerField = FilterField.NumberIndex1;
        private const int DuelPlayers = 2;

        private readonly Func<int> _backfillSeedProvider;
        private MatchmakingRequest _request;
        private Action<MatchmakingResult> _onCompleted;
        private ISession _session;
        private bool _widened;
        private bool _searchInFlight;
        private int _searchCounter;

        public UgsMatchmakerService(Func<int> backfillSeedProvider = null)
        {
            _backfillSeedProvider = backfillSeedProvider;
        }

        public bool IsSearching { get; private set; }
        public float SearchElapsedSeconds { get; private set; }

        /// <summary>The joined session, handed to the transport once a duel is agreed.</summary>
        public ISession CurrentSession => _session;

        public void FindOpponent(MatchmakingRequest request, Action<MatchmakingResult> onCompleted)
        {
            if (onCompleted == null)
            {
                return;
            }

            ResetSearch();
            if (request == null)
            {
                onCompleted(MatchmakingResult.Denied(MatchmakingDenial.ServiceUnavailable));
                return;
            }

            // Without a liberated planet there is no arena either side may legally
            // see, so the duel is closed before the queue is ever touched.
            if (request.UnlockedArenaIds.Length == 0)
            {
                onCompleted(MatchmakingResult.Denied(MatchmakingDenial.NoUnlockedArena));
                return;
            }

            _request = request;
            _onCompleted = onCompleted;
            IsSearching = true;
            StartQuickJoin(PvpMatchmakingPolicy.InitialBandFraction);
        }

        public void Tick(float deltaSeconds)
        {
            if (!IsSearching || deltaSeconds <= 0f)
            {
                return;
            }

            SearchElapsedSeconds += deltaSeconds;

            if (_session != null && _session.PlayerCount >= DuelPlayers)
            {
                CompleteWithSession();
                return;
            }

            if (PvpMatchmakingPolicy.ShouldBackfill(SearchElapsedSeconds))
            {
                CompleteWithBackfill();
                return;
            }

            // One widening, once the opening band has had its 20 seconds - and only
            // while we are not already sitting in a session of our own. Re-running
            // quick join with a session in hand would create a second one and leave
            // the first orphaned in the queue.
            if (!_widened
                && !_searchInFlight
                && _session == null
                && SearchElapsedSeconds >= PvpMatchmakingPolicy.WidenAfterSeconds)
            {
                _widened = true;
                StartQuickJoin(PvpMatchmakingPolicy.WidenedBandFraction);
            }
        }

        public void CancelSearch()
        {
            var session = _session;
            ResetSearch();
            LeaveQuietly(session);
        }

        private async void StartQuickJoin(float bandFraction)
        {
            var request = _request;
            if (request == null)
            {
                return;
            }

            _searchInFlight = true;
            try
            {
                var joined = await MultiplayerService.Instance.MatchmakeSessionAsync(
                    BuildQuickJoinOptions(request, bandFraction),
                    BuildSessionOptions(request));

                if (!IsSearching)
                {
                    // The player left the queue while the request was in flight.
                    LeaveQuietly(joined);
                    return;
                }

                _session = joined;
                if (joined != null && joined.PlayerCount >= DuelPlayers)
                {
                    CompleteWithSession();
                }
            }
            catch (Exception exception)
            {
                // A failed search is not a failed duel: the backfill deadline still
                // applies, so the player gets a battle either way.
                Debug.LogWarning($"Duel matchmaking request failed: {exception.Message}");
            }
            finally
            {
                _searchInFlight = false;
            }
        }

        private QuickJoinOptions BuildQuickJoinOptions(MatchmakingRequest request, float bandFraction)
        {
            var low = (long)Math.Floor(request.HistoricalPower * (1f - bandFraction));
            var high = (long)Math.Ceiling(request.HistoricalPower * (1f + bandFraction));
            return new QuickJoinOptions
            {
                // Creating on miss is what makes this client the host others join.
                CreateSession = true,
                Timeout = TimeSpan.FromSeconds(PvpMatchmakingPolicy.WidenAfterSeconds),
                Filters = new List<FilterOption>
                {
                    new(PowerField, low.ToString(CultureInfo.InvariantCulture), FilterOperation.GreaterOrEqual),
                    new(PowerField, high.ToString(CultureInfo.InvariantCulture), FilterOperation.LessOrEqual)
                }
            };
        }

        private SessionOptions BuildSessionOptions(MatchmakingRequest request)
        {
            var options = new SessionOptions
            {
                MaxPlayers = DuelPlayers,
                Type = SessionType,
                IsPrivate = false,
                SessionProperties = new Dictionary<string, SessionProperty>
                {
                    {
                        "power",
                        new SessionProperty(
                            request.HistoricalPower.ToString(CultureInfo.InvariantCulture),
                            VisibilityPropertyOptions.Public,
                            PowerIndex)
                    }
                },
                PlayerProperties = new Dictionary<string, PlayerProperty>
                {
                    { "name", new PlayerProperty(request.DisplayName) },
                    { "level", new PlayerProperty(request.CommanderLevel.ToString(CultureInfo.InvariantCulture)) },
                    { "power", new PlayerProperty(request.HistoricalPower.ToString(CultureInfo.InvariantCulture)) },
                    { "arenas", new PlayerProperty(string.Join(",", request.UnlockedArenaIds)) },
                    { "loadout", new PlayerProperty(string.Join(",", request.LoadoutWeaponIds)) }
                }
            };

            return options.WithRelayNetwork();
        }

        private void CompleteWithSession()
        {
            var request = _request;
            var onCompleted = _onCompleted;
            var session = _session;
            IsSearching = false;
            _request = null;
            _onCompleted = null;
            if (request == null || onCompleted == null || session == null)
            {
                return;
            }

            var opponent = ReadOpponent(session, request);
            if (opponent == null)
            {
                LeaveQuietly(session);
                _session = null;
                onCompleted(MatchmakingResult.Denied(MatchmakingDenial.NoOpponentFound));
                return;
            }

            onCompleted(MatchmakingResult.Success(session.Id, opponent));
        }

        /// <summary>
        /// Builds the opponent from the properties they published when they joined.
        /// Their liberated arenas travel with them because the anti-spoiler rule
        /// intersects both pools - without the other side's list the rule cannot run.
        /// </summary>
        private static PvpOpponentProfile ReadOpponent(ISession session, MatchmakingRequest request)
        {
            var players = session.Players;
            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                if (player == null
                    || string.IsNullOrEmpty(player.Id)
                    || string.Equals(player.Id, request.PlayerId, StringComparison.Ordinal))
                {
                    continue;
                }

                var arenas = SplitProperty(player, "arenas");
                var weapons = SplitProperty(player, "loadout");
                return new PvpOpponentProfile(
                    player.Id,
                    ReadProperty(player, "name"),
                    ReadInt(player, "level", 1),
                    ReadLong(player, "power", 0),
                    arenas,
                    weapons,
                    OpponentLoadoutBuilder.ResolveRanks(
                        weapons, ReadInt(player, "level", 1), 0, AiWeaponRankResolver.MaxRank),
                    null,
                    "archetype.striker",
                    isBotBackfill: false);
            }

            return null;
        }

        private void CompleteWithBackfill()
        {
            var request = _request;
            var onCompleted = _onCompleted;
            var session = _session;
            IsSearching = false;
            _request = null;
            _onCompleted = null;
            _session = null;
            LeaveQuietly(session);

            if (request == null || onCompleted == null)
            {
                return;
            }

            var seed = _backfillSeedProvider?.Invoke() ?? _searchCounter++;
            var matchId = $"backfill.{request.PlayerId}.{seed}";
            onCompleted(MatchmakingResult.Success(
                matchId,
                PvpBotOpponentFactory.Create(
                    request,
                    matchId,
                    PvpMatchmakingPolicy.ResolveBandFraction(PvpMatchmakingPolicy.BackfillAfterSeconds))));
        }

        private void ResetSearch()
        {
            IsSearching = false;
            SearchElapsedSeconds = 0f;
            _widened = false;
            _request = null;
            _onCompleted = null;
            _session = null;
        }

        private static void LeaveQuietly(ISession session)
        {
            if (session == null)
            {
                return;
            }

            try
            {
                _ = session.LeaveAsync();
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Leaving the duel session failed: {exception.Message}");
            }
        }

        private static string ReadProperty(IReadOnlyPlayer player, string key)
        {
            return player.Properties != null && player.Properties.TryGetValue(key, out var property)
                ? property?.Value ?? string.Empty
                : string.Empty;
        }

        private static string[] SplitProperty(IReadOnlyPlayer player, string key)
        {
            var raw = ReadProperty(player, key);
            return string.IsNullOrEmpty(raw)
                ? Array.Empty<string>()
                : raw.Split(',', StringSplitOptions.RemoveEmptyEntries);
        }

        private static int ReadInt(IReadOnlyPlayer player, string key, int fallback)
        {
            return int.TryParse(ReadProperty(player, key), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)
                ? value
                : fallback;
        }

        private static long ReadLong(IReadOnlyPlayer player, string key, long fallback)
        {
            return long.TryParse(ReadProperty(player, key), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)
                ? value
                : fallback;
        }
    }
}
