using UnityEngine;

namespace Game.Data.Progression
{
    /// <summary>
    /// Works out how developed an opponent's weapon should be at a given commander
    /// level. The rule from the campaign AI strategy: mirror what the economy makes
    /// plausible at that level, never copy the player's inventory and never inflate
    /// damage or health behind the scenes.
    /// </summary>
    public static class AiWeaponRankResolver
    {
        public const int MinRank = 1;
        public const int MaxRank = 10;

        /// <summary>
        /// Highest rank whose commander-level requirement is met at
        /// <paramref name="commanderLevel"/>. Rank 1 for a weapon that is not even
        /// unlocked yet, so a caller who asks about a late weapon early still gets a
        /// usable answer rather than nothing.
        /// </summary>
        public static int ResolveEconomicallyPlausibleRank(string weaponId, int commanderLevel)
        {
            if (!WeaponProgressionCatalog.TryGet(weaponId, out var definition))
            {
                return MinRank;
            }

            return ResolveEconomicallyPlausibleRank(definition, commanderLevel);
        }

        public static int ResolveEconomicallyPlausibleRank(WeaponProgressionDefinition definition, int commanderLevel)
        {
            if (definition == null)
            {
                return MinRank;
            }

            var rank = MinRank;
            for (var candidate = MinRank + 1; candidate <= MaxRank; candidate++)
            {
                if (WeaponUpgradeEconomy.GetRequiredCommanderLevel(definition, candidate) > commanderLevel)
                {
                    break;
                }

                rank = candidate;
            }

            return rank;
        }

        /// <summary>
        /// The plausible rank shifted by an encounter's pressure and held under its
        /// cap. The cap is what protects a player from meeting a fully evolved
        /// weapon in a routine fight; the bonus is what makes "hard" feel harder
        /// without touching the damage numbers.
        /// </summary>
        public static int ResolveEncounterRank(string weaponId, int commanderLevel, int rankBonus, int rankCap)
        {
            var baseRank = ResolveEconomicallyPlausibleRank(weaponId, commanderLevel);
            var safeCap = Mathf.Clamp(rankCap, MinRank, MaxRank);
            return Mathf.Clamp(baseRank + rankBonus, MinRank, safeCap);
        }
    }
}
