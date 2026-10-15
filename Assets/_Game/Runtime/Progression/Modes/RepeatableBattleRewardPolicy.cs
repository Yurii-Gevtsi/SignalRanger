using Game.Core.Modes;
using Game.Data.Progression;
using UnityEngine;

namespace Game.Progression.Modes
{
    /// <summary>
    /// What a finished raid or duel is worth. One budget covers both repeatable
    /// modes: they draw on the same daily counter, so a player cannot spend the
    /// raid cap and then farm the same value out of duels - which matters most
    /// while an empty queue means those duels are against backfill opponents.
    /// </summary>
    public static class RepeatableBattleRewardPolicy
    {
        /// <summary>Repeatable battles per UTC day that pay the full reward.</summary>
        public const int FullRewardBattlesPerDay = 10;

        /// <summary>What every battle past the budget pays instead.</summary>
        public const int ReducedRewardPercent = 25;

        /// <summary>Cap on the credit bonus for finishing in good shape.</summary>
        public const int MaxCreditPerformancePercent = BattleRewardCatalog.MaxCreditPerformancePercent;

        /// <summary>Cap on the blueprint bonus for the same.</summary>
        public const int MaxBlueprintPerformancePercent = BattleRewardCatalog.MaxBlueprintPerformancePercent;

        /// <summary>
        /// Scale for the <paramref name="battleOrdinalToday"/>-th repeatable battle of
        /// the day, counting the one being banked. Counting finished battles rather
        /// than wins is deliberate: quitting and re-entering must not reset the budget.
        /// </summary>
        public static int ResolveScalePercent(int battleOrdinalToday)
        {
            return battleOrdinalToday <= FullRewardBattlesPerDay ? 100 : ReducedRewardPercent;
        }

        public static bool IsReduced(int battleOrdinalToday)
        {
            return ResolveScalePercent(battleOrdinalToday) < 100;
        }

        /// <summary>
        /// Credit bonus for the health the player finished on. Only a win earns it:
        /// scaling a loss by how close it was rewards losing slowly.
        /// </summary>
        public static int ResolveCreditPerformanceBonusPercent(int playerHealthPercent, bool victory)
        {
            return ResolveBonus(playerHealthPercent, victory, MaxCreditPerformancePercent);
        }

        public static int ResolveBlueprintPerformanceBonusPercent(int playerHealthPercent, bool victory)
        {
            return ResolveBonus(playerHealthPercent, victory, MaxBlueprintPerformancePercent);
        }

        /// <summary>
        /// Reward tier for the battle. A raid pays by the difficulty the player chose;
        /// a duel has no difficulty to choose, so it pays the middle tier - the same
        /// whether the opponent turned out to be a person or a backfill, since a
        /// reward that differed would give the bot away.
        /// </summary>
        public static BattleRewardDifficulty ResolveRewardDifficulty(BattleMode mode, int difficultyTier)
        {
            if (mode == BattleMode.Pvp)
            {
                return BattleRewardDifficulty.Medium;
            }

            return difficultyTier switch
            {
                0 => BattleRewardDifficulty.Easy,
                2 => BattleRewardDifficulty.Hard,
                _ => BattleRewardDifficulty.Medium
            };
        }

        private static int ResolveBonus(int playerHealthPercent, bool victory, int maxPercent)
        {
            if (!victory)
            {
                return 0;
            }

            var health = Mathf.Clamp(playerHealthPercent, 0, 100);
            return Mathf.RoundToInt(maxPercent * health / 100f);
        }
    }
}
