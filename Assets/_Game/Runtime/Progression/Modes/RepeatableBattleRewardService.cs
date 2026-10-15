using System;
using Game.Core.Battle;
using Game.Core.Modes;
using Game.Data.Progression;
using Game.Data.Save;

namespace Game.Progression.Modes
{
    /// <summary>
    /// Banks the reward for one finished raid or duel: picks the tier, applies the
    /// performance bonus and the day's budget, chooses which weapon the blueprints
    /// go to, and puts it all through one idempotent economy transaction.
    /// </summary>
    public sealed class RepeatableBattleRewardService
    {
        private readonly SaveData _saveData;
        private readonly BattleRewardService _rewards;
        private readonly Random _random;

        public RepeatableBattleRewardService(SaveData saveData, BattleRewardService rewards, Random random = null)
        {
            _saveData = saveData ?? throw new ArgumentNullException(nameof(saveData));
            _rewards = rewards ?? throw new ArgumentNullException(nameof(rewards));
            _saveData.EnsureDefaults();
            _random = random;
        }

        /// <summary>
        /// Grants the reward for <paramref name="request"/>. Story and training pay
        /// nothing here - the campaign has its own first-completion rewards - so a
        /// mis-routed call is a no-op rather than a double payment.
        /// </summary>
        public BattleArenaReward Grant(BattleArenaRewardRequest request)
        {
            if (!request.Mode.IsArena() || string.IsNullOrWhiteSpace(request.BattleId))
            {
                return BattleArenaReward.None;
            }

            var victory = request.Outcome == BattleArenaOutcome.Victory;
            var difficulty = RepeatableBattleRewardPolicy.ResolveRewardDifficulty(request.Mode, request.DifficultyTier);

            // The activity counter has already recorded this battle by the time the
            // reward is banked, so it is this battle's ordinal for the day.
            var ordinalToday = _saveData.activity?.daily?.arenaBattlesPlayed ?? 1;
            var scalePercent = RepeatableBattleRewardPolicy.ResolveScalePercent(ordinalToday);

            WeaponBlueprintRewardSelector.TrySelectEligibleWeaponId(_saveData, _random, out var blueprintWeaponId);

            var transaction = _rewards.GrantArenaReward(
                difficulty,
                ResolveOutcome(request.Outcome),
                RepeatableBattleRewardPolicy.ResolveCreditPerformanceBonusPercent(request.PlayerHealthPercent, victory),
                RepeatableBattleRewardPolicy.ResolveBlueprintPerformanceBonusPercent(request.PlayerHealthPercent, victory),
                blueprintWeaponId,
                request.BattleId,
                scalePercent,
                out var granted);

            // Report what actually landed, not what was computed: a re-banked battle
            // adds nothing, and blueprints need a weapon to land on. A card that
            // claims currency the ledger did not move would be lying about the payout.
            if (transaction.WasDuplicate)
            {
                return new BattleArenaReward(true, 0, 0, 0, string.Empty, false);
            }

            var appliedBlueprints = string.IsNullOrWhiteSpace(granted.BlueprintWeaponId) ? 0 : granted.Blueprints;
            return new BattleArenaReward(
                true,
                granted.Credits,
                granted.CommanderExperience,
                appliedBlueprints,
                granted.BlueprintWeaponId,
                RepeatableBattleRewardPolicy.IsReduced(ordinalToday));
        }

        private static BattleRewardOutcome ResolveOutcome(BattleArenaOutcome outcome)
        {
            return outcome switch
            {
                BattleArenaOutcome.Victory => BattleRewardOutcome.Victory,
                BattleArenaOutcome.Draw => BattleRewardOutcome.Draw,
                _ => BattleRewardOutcome.Defeat
            };
        }
    }
}
