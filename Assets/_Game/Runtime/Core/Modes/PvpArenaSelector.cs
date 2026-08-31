using System;
using System.Collections.Generic;

namespace Game.Core.Modes
{
    /// <summary>
    /// Picks the duel arena without spoiling the campaign for the less advanced of
    /// the two commanders. Every planet is a story map, so a map the lower-level
    /// player has not liberated yet would show them content they have not reached.
    /// </summary>
    public static class PvpArenaSelector
    {
        /// <summary>
        /// Arenas both sides may legally see: the intersection of the two liberated
        /// pools, ordered by the lower commander's pool so the result is stable.
        /// The campaign is linear, so in practice the lower pool is a subset of the
        /// higher one - the intersection keeps the rule correct anyway if a future
        /// branch, a rollback or a support grant breaks that assumption.
        /// </summary>
        public static string[] ResolveEligibleArenas(
            int levelA,
            long powerA,
            string playerIdA,
            string[] arenasA,
            int levelB,
            long powerB,
            string playerIdB,
            string[] arenasB)
        {
            var aIsLower = IsLower(levelA, powerA, playerIdA, levelB, powerB, playerIdB);
            var lowerPool = aIsLower ? arenasA : arenasB;
            var otherPool = aIsLower ? arenasB : arenasA;
            if (lowerPool == null || lowerPool.Length == 0 || otherPool == null || otherPool.Length == 0)
            {
                return Array.Empty<string>();
            }

            var other = new HashSet<string>(otherPool, StringComparer.Ordinal);
            var eligible = new List<string>(lowerPool.Length);
            for (var i = 0; i < lowerPool.Length; i++)
            {
                var arenaId = lowerPool[i];
                if (!string.IsNullOrWhiteSpace(arenaId) && other.Contains(arenaId) && !eligible.Contains(arenaId))
                {
                    eligible.Add(arenaId);
                }
            }

            return eligible.ToArray();
        }

        /// <summary>
        /// The chosen arena, or an empty string when the pair shares none. Derived
        /// from the match id alone, so both clients reach the same answer without
        /// exchanging a single byte about it.
        /// </summary>
        public static string SelectArena(string matchId, string[] eligibleArenaIds)
        {
            if (eligibleArenaIds == null || eligibleArenaIds.Length == 0)
            {
                return string.Empty;
            }

            var index = DeterministicSeed.Index(DeterministicSeed.Hash(matchId), eligibleArenaIds.Length);
            return eligibleArenaIds[index];
        }

        /// <summary>
        /// Which side counts as "lower". Level decides; historical power breaks a
        /// level tie; the player id breaks the rest, so two clients ranking the same
        /// pair never disagree about whose pool to use.
        /// </summary>
        public static bool IsLower(
            int levelA,
            long powerA,
            string playerIdA,
            int levelB,
            long powerB,
            string playerIdB)
        {
            if (levelA != levelB)
            {
                return levelA < levelB;
            }

            if (powerA != powerB)
            {
                return powerA < powerB;
            }

            return string.CompareOrdinal(playerIdA ?? string.Empty, playerIdB ?? string.Empty) <= 0;
        }
    }
}
