using System;
using System.Collections.Generic;
using Game.Core.Modes;
using Game.Data.Progression;
using Game.Progression.Missions;

namespace Game.Progression.Modes
{
    /// <summary>Everything needed to start one duel, resolved before the scene loads.</summary>
    public readonly struct PvpBattleSetup
    {
        public PvpBattleSetup(
            bool isAvailable,
            string arenaId,
            string matchId,
            PvpOpponentProfile opponent,
            string[] opponentWeaponIds,
            int[] opponentWeaponRanks,
            int[] opponentWeaponBranches,
            int aiDifficultyTier)
        {
            IsAvailable = isAvailable;
            ArenaId = arenaId ?? string.Empty;
            MatchId = matchId ?? string.Empty;
            Opponent = opponent;
            OpponentWeaponIds = opponentWeaponIds ?? Array.Empty<string>();
            OpponentWeaponRanks = opponentWeaponRanks ?? Array.Empty<int>();
            OpponentWeaponBranches = opponentWeaponBranches ?? Array.Empty<int>();
            AiDifficultyTier = aiDifficultyTier;
        }

        /// <summary>False when the pair shares no arena either side may legally see.</summary>
        public bool IsAvailable { get; }
        public string ArenaId { get; }
        public string MatchId { get; }
        public PvpOpponentProfile Opponent { get; }
        public string[] OpponentWeaponIds { get; }
        public int[] OpponentWeaponRanks { get; }

        /// <summary>Evolution branch per weapon, aligned by index; 0 below rank 5.</summary>
        public int[] OpponentWeaponBranches { get; }

        /// <summary>
        /// Only meaningful while the opponent is a stand-in. A real duel is driven by
        /// the other player's own commands, so no AI tier applies.
        /// </summary>
        public int AiDifficultyTier { get; }
    }

    /// <summary>
    /// Turns a match into a battle: applies the anti-spoiler arena rule and resolves
    /// the opponent's weapon ranks. Knows nothing about how the match was found, so
    /// the offline stand-in and the future networked matchmaker feed it the same way.
    /// </summary>
    public sealed class PvpSetupService
    {
        /// <summary>
        /// Decision quality a backfill opponent duels at. Mid tier on purpose: it
        /// stands in for a typical commander, not for the best or the worst one.
        /// </summary>
        public const int BackfillOpponentAiTier = 1;

        private readonly IReadOnlyList<MissionDefinition> _missions;

        public PvpSetupService(IReadOnlyList<MissionDefinition> missions)
        {
            _missions = missions ?? Array.Empty<MissionDefinition>();
        }

        public string[] GetUnlockedArenas(IReadOnlyCollection<string> completedMissionIds)
        {
            return CampaignArenaUnlockService.ResolveUnlockedArenas(_missions, completedMissionIds);
        }

        public PvpBattleSetup CreateSetup(
            MatchmakingRequest localRequest,
            MatchmakingResult match)
        {
            if (localRequest == null || !match.Matched || match.Opponent == null)
            {
                return Unavailable();
            }

            var opponent = match.Opponent;
            var eligible = PvpArenaSelector.ResolveEligibleArenas(
                localRequest.CommanderLevel,
                localRequest.HistoricalPower,
                localRequest.PlayerId,
                localRequest.UnlockedArenaIds,
                opponent.CommanderLevel,
                opponent.HistoricalPower,
                opponent.PlayerId,
                opponent.UnlockedArenaIds);

            var arenaId = PvpArenaSelector.SelectArena(match.MatchId, eligible);
            if (string.IsNullOrEmpty(arenaId))
            {
                return Unavailable();
            }

            var weaponIds = opponent.LoadoutWeaponIds;
            var ranks = opponent.LoadoutWeaponRanks != null
                        && opponent.LoadoutWeaponRanks.Length == weaponIds.Length
                ? opponent.LoadoutWeaponRanks
                : OpponentLoadoutBuilder.ResolveRanks(
                    weaponIds,
                    opponent.CommanderLevel,
                    0,
                    AiWeaponRankResolver.MaxRank);

            // A real opponent reports its own branch choices; anything else is derived
            // from the match id, so both clients land on the same evolution.
            var branches = opponent.LoadoutWeaponBranches != null
                           && opponent.LoadoutWeaponBranches.Length == weaponIds.Length
                ? opponent.LoadoutWeaponBranches
                : OpponentLoadoutBuilder.ResolveBranches(weaponIds, ranks, match.MatchId);

            return new PvpBattleSetup(
                true,
                arenaId,
                match.MatchId,
                opponent,
                weaponIds,
                ranks,
                branches,
                BackfillOpponentAiTier);
        }

        private static PvpBattleSetup Unavailable()
        {
            return new PvpBattleSetup(
                false,
                string.Empty,
                string.Empty,
                null,
                Array.Empty<string>(),
                Array.Empty<int>(),
                Array.Empty<int>(),
                BackfillOpponentAiTier);
        }
    }
}
