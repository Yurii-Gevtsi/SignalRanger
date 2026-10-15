using System;
using System.Collections.Generic;
using Game.Core.Modes;
using UnityEngine;

namespace Game.Data.Progression
{
    /// <summary>Three-slot opponent loadout with the rank and branch each weapon fights at.</summary>
    public readonly struct OpponentLoadout
    {
        public OpponentLoadout(string[] weaponIds, int[] weaponRanks, int[] weaponBranches)
        {
            WeaponIds = weaponIds ?? Array.Empty<string>();
            WeaponRanks = weaponRanks ?? Array.Empty<int>();
            WeaponBranches = weaponBranches ?? Array.Empty<int>();
        }

        public string[] WeaponIds { get; }

        /// <summary>Rank per weapon, aligned by index with <see cref="WeaponIds"/>.</summary>
        public int[] WeaponRanks { get; }

        /// <summary>
        /// Evolution branch per weapon as a <see cref="WeaponUpgradeBranch"/> value,
        /// aligned by index with <see cref="WeaponIds"/>. Zero below rank 5, where no
        /// branch has been chosen yet.
        /// </summary>
        public int[] WeaponBranches { get; }
        public bool IsEmpty => WeaponIds.Length == 0;
    }

    /// <summary>
    /// Builds an AI opponent's loadout from the authored slot table. The pick is
    /// deterministic for a given seed, which keeps a raid reproducible for tests and
    /// lets two duel clients derive the same stand-in without exchanging it.
    /// </summary>
    public static class OpponentLoadoutBuilder
    {
        /// <summary>Rank at which a weapon has an evolution branch to choose.</summary>
        public const int EvolutionRank = 5;

        public static OpponentLoadout Build(int commanderLevel, int rankBonus, int rankCap, string seed)
        {
            var safeLevel = Mathf.Max(1, commanderLevel);
            var weaponIds = new List<string>(3);

            var primary = PickForSlot(OpponentWeaponSlot.Primary, safeLevel, seed, "primary");
            if (!string.IsNullOrEmpty(primary))
            {
                weaponIds.Add(primary);
            }

            var tactical = PickForSlot(OpponentWeaponSlot.Tactical, safeLevel, seed, "tactical");
            if (!string.IsNullOrEmpty(tactical))
            {
                weaponIds.Add(tactical);
            }

            // The fallback closes the loadout so the opponent always has a legal move,
            // even at level 1 where no primary or tactical weapon exists yet.
            if (!weaponIds.Contains(OpponentWeaponRoleTable.FallbackWeaponId))
            {
                weaponIds.Add(OpponentWeaponRoleTable.FallbackWeaponId);
            }

            var ids = weaponIds.ToArray();
            var ranks = ResolveRanks(ids, safeLevel, rankBonus, rankCap);
            return new OpponentLoadout(ids, ranks, ResolveBranches(ids, ranks, seed));
        }

        /// <summary>
        /// Ranks for a loadout that is already fixed - a real duel opponent's own
        /// weapons, for instance, where only the rank still has to be resolved.
        /// </summary>
        public static int[] ResolveRanks(string[] weaponIds, int commanderLevel, int rankBonus, int rankCap)
        {
            if (weaponIds == null || weaponIds.Length == 0)
            {
                return Array.Empty<int>();
            }

            var safeLevel = Mathf.Max(1, commanderLevel);
            var ranks = new int[weaponIds.Length];
            for (var i = 0; i < weaponIds.Length; i++)
            {
                ranks[i] = AiWeaponRankResolver.ResolveEncounterRank(weaponIds[i], safeLevel, rankBonus, rankCap);
            }

            return ranks;
        }

        /// <summary>
        /// Which evolution branch each evolved weapon took. Both branches of a weapon
        /// are authored to be comparable, so picking one from the seed varies how an
        /// opponent plays without making it stronger or weaker than the other choice.
        /// </summary>
        public static int[] ResolveBranches(string[] weaponIds, int[] ranks, string seed)
        {
            if (weaponIds == null || weaponIds.Length == 0)
            {
                return Array.Empty<int>();
            }

            var branches = new int[weaponIds.Length];
            for (var i = 0; i < weaponIds.Length; i++)
            {
                var rank = ranks != null && i < ranks.Length ? ranks[i] : 1;
                if (rank < EvolutionRank)
                {
                    continue;
                }

                var pick = DeterministicSeed.Index(DeterministicSeed.Hash(seed, weaponIds[i], "branch"), 2);
                branches[i] = pick == 0 ? (int)WeaponUpgradeBranch.A : (int)WeaponUpgradeBranch.B;
            }

            return branches;
        }

        private static string PickForSlot(OpponentWeaponSlot slot, int commanderLevel, string seed, string slotSalt)
        {
            var candidates = OpponentWeaponRoleTable.GetCandidates(slot, commanderLevel);
            if (candidates.Count == 0)
            {
                return string.Empty;
            }

            var index = DeterministicSeed.Index(DeterministicSeed.Hash(seed, slotSalt), candidates.Count);
            return candidates[index];
        }
    }
}
