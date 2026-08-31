using System;
using Game.Data.Progression;
using Game.Economy;

namespace Game.Progression
{
    public sealed class BattleRewardService
    {
        private readonly IEconomyService _economy;

        public BattleRewardService(IEconomyService economy)
        {
            _economy = economy ?? throw new ArgumentNullException(nameof(economy));
        }

        public ProgressionReward CalculateArenaReward(
            BattleRewardDifficulty difficulty,
            BattleRewardOutcome outcome,
            int creditPerformanceBonusPercent,
            int blueprintPerformanceBonusPercent,
            string blueprintWeaponId,
            int dailyScalePercent = 100)
        {
            var baseReward = BattleRewardCatalog.GetArenaReward(difficulty, outcome, blueprintWeaponId);
            var withBonus = BattleRewardCatalog.ApplyPerformanceBonus(
                baseReward,
                creditPerformanceBonusPercent,
                blueprintPerformanceBonusPercent);
            return BattleRewardCatalog.ApplyScale(withBonus, dailyScalePercent);
        }

        public EconomyTransactionResult GrantArenaReward(
            BattleRewardDifficulty difficulty,
            BattleRewardOutcome outcome,
            int creditPerformanceBonusPercent,
            int blueprintPerformanceBonusPercent,
            string blueprintWeaponId,
            string battleId)
        {
            return GrantArenaReward(
                difficulty,
                outcome,
                creditPerformanceBonusPercent,
                blueprintPerformanceBonusPercent,
                blueprintWeaponId,
                battleId,
                100,
                out _);
        }

        /// <summary>
        /// Banks an arena reward and hands back exactly what was paid, so the result
        /// card shows the granted numbers rather than a second, separate estimate.
        /// The transaction id is derived from the battle, so re-banking the same
        /// finished battle cannot pay twice.
        /// </summary>
        public EconomyTransactionResult GrantArenaReward(
            BattleRewardDifficulty difficulty,
            BattleRewardOutcome outcome,
            int creditPerformanceBonusPercent,
            int blueprintPerformanceBonusPercent,
            string blueprintWeaponId,
            string battleId,
            int dailyScalePercent,
            out ProgressionReward granted)
        {
            granted = default;
            if (string.IsNullOrWhiteSpace(battleId))
            {
                return new EconomyTransactionResult(false, _economy.SoftCurrency, "battle.arena");
            }

            granted = CalculateArenaReward(
                difficulty,
                outcome,
                creditPerformanceBonusPercent,
                blueprintPerformanceBonusPercent,
                blueprintWeaponId,
                dailyScalePercent);
            return _economy.Grant(
                granted,
                "battle.arena",
                difficulty.ToString().ToLowerInvariant(),
                $"battle.arena.{battleId}");
        }
    }
}
