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
            string blueprintWeaponId)
        {
            var baseReward = BattleRewardCatalog.GetArenaReward(difficulty, outcome, blueprintWeaponId);
            return BattleRewardCatalog.ApplyPerformanceBonus(
                baseReward,
                creditPerformanceBonusPercent,
                blueprintPerformanceBonusPercent);
        }

        public EconomyTransactionResult GrantArenaReward(
            BattleRewardDifficulty difficulty,
            BattleRewardOutcome outcome,
            int creditPerformanceBonusPercent,
            int blueprintPerformanceBonusPercent,
            string blueprintWeaponId,
            string battleId)
        {
            if (string.IsNullOrWhiteSpace(battleId))
            {
                return new EconomyTransactionResult(false, _economy.SoftCurrency, "battle.arena");
            }

            var reward = CalculateArenaReward(
                difficulty,
                outcome,
                creditPerformanceBonusPercent,
                blueprintPerformanceBonusPercent,
                blueprintWeaponId);
            return _economy.Grant(
                reward,
                "battle.arena",
                difficulty.ToString().ToLowerInvariant(),
                $"battle.arena.{battleId}");
        }
    }
}
