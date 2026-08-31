using System;
using System.Collections.Generic;
using Game.Core.Modes;
using Game.Data.Progression;
using Game.Progression.Missions;

namespace Game.Progression.Modes
{
    /// <summary>Everything needed to start one raid, resolved before the scene loads.</summary>
    public readonly struct RaidBattleSetup
    {
        public RaidBattleSetup(
            bool isAvailable,
            string arenaId,
            RaidDifficultyRules rules,
            string[] opponentWeaponIds,
            int[] opponentWeaponRanks,
            int[] opponentWeaponBranches)
        {
            IsAvailable = isAvailable;
            ArenaId = arenaId ?? string.Empty;
            Rules = rules;
            OpponentWeaponIds = opponentWeaponIds ?? Array.Empty<string>();
            OpponentWeaponRanks = opponentWeaponRanks ?? Array.Empty<int>();
            OpponentWeaponBranches = opponentWeaponBranches ?? Array.Empty<int>();
        }

        /// <summary>False when no planet has been liberated yet; the arena id is then empty.</summary>
        public bool IsAvailable { get; }
        public string ArenaId { get; }
        public RaidDifficultyRules Rules { get; }
        public string[] OpponentWeaponIds { get; }
        public int[] OpponentWeaponRanks { get; }

        /// <summary>Evolution branch per weapon, aligned by index; 0 below rank 5.</summary>
        public int[] OpponentWeaponBranches { get; }

        /// <summary>
        /// Localization key naming the raid opponent. Every raid is an incursion by
        /// the campaign's antagonist onto a planet the player already liberated, so
        /// the opponent is always Sigma - no per-raid identity to author.
        /// </summary>
        public string OpponentNameKey => RaidSetupService.OpponentNameKey;
    }

    /// <summary>
    /// Assembles a raid: an arena the player has liberated, an opponent scaled to
    /// their commander level and the chosen difficulty, and nothing that touches
    /// campaign progression.
    /// </summary>
    public sealed class RaidSetupService
    {
        /// <summary>The antagonist behind every raid; see <see cref="RaidBattleSetup.OpponentNameKey"/>.</summary>
        public const string OpponentNameKey = "content.opponent.sigma";

        private readonly IReadOnlyList<MissionDefinition> _missions;

        public RaidSetupService(IReadOnlyList<MissionDefinition> missions)
        {
            _missions = missions ?? Array.Empty<MissionDefinition>();
        }

        /// <summary>Arenas the player may raid right now, in campaign order.</summary>
        public string[] GetUnlockedArenas(IReadOnlyCollection<string> completedMissionIds)
        {
            return CampaignArenaUnlockService.ResolveUnlockedArenas(_missions, completedMissionIds);
        }

        /// <summary>
        /// Builds the raid. The arena is drawn from the liberated pool by the seed
        /// rather than chosen: the player picks a difficulty and Sigma picks where it
        /// lands, which is both the fiction and one fewer screen to walk through.
        /// </summary>
        public RaidBattleSetup CreateSetup(
            IReadOnlyCollection<string> completedMissionIds,
            int commanderLevel,
            RaidDifficulty difficulty,
            string seed)
        {
            var rules = RaidDifficultyRules.For(difficulty);
            var unlocked = GetUnlockedArenas(completedMissionIds);
            if (unlocked.Length == 0)
            {
                return new RaidBattleSetup(
                    false, string.Empty, rules, Array.Empty<string>(), Array.Empty<int>(), Array.Empty<int>());
            }

            var arenaId = unlocked[DeterministicSeed.Index(DeterministicSeed.Hash(seed), unlocked.Length)];
            var loadout = OpponentLoadoutBuilder.Build(
                commanderLevel,
                rules.RankBonus,
                rules.RankCap,
                DeterministicSeed.Hash(seed, arenaId, difficulty.ToString()).ToString());

            return new RaidBattleSetup(
                true, arenaId, rules, loadout.WeaponIds, loadout.WeaponRanks, loadout.WeaponBranches);
        }
    }
}
