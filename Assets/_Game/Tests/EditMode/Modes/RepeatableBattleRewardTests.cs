using Game.Core.Battle;
using Game.Core.Modes;
using Game.Data.Progression;
using Game.Data.Save;
using Game.Economy;
using Game.Progression;
using Game.Progression.Modes;
using NUnit.Framework;

namespace Game.Tests.EditMode.Modes
{
    public sealed class RepeatableBattleRewardTests
    {
        [Test]
        public void TheDailyBudgetPaysInFullAndThenAtTheReducedRate()
        {
            for (var ordinal = 1; ordinal <= RepeatableBattleRewardPolicy.FullRewardBattlesPerDay; ordinal++)
            {
                Assert.That(RepeatableBattleRewardPolicy.ResolveScalePercent(ordinal), Is.EqualTo(100), $"battle {ordinal}");
                Assert.That(RepeatableBattleRewardPolicy.IsReduced(ordinal), Is.False, $"battle {ordinal}");
            }

            var pastBudget = RepeatableBattleRewardPolicy.FullRewardBattlesPerDay + 1;
            Assert.That(
                RepeatableBattleRewardPolicy.ResolveScalePercent(pastBudget),
                Is.EqualTo(RepeatableBattleRewardPolicy.ReducedRewardPercent));
            Assert.That(RepeatableBattleRewardPolicy.IsReduced(pastBudget), Is.True);
        }

        [Test]
        public void OnlyAWinEarnsThePerformanceBonus()
        {
            Assert.That(RepeatableBattleRewardPolicy.ResolveCreditPerformanceBonusPercent(100, false), Is.EqualTo(0));
            Assert.That(RepeatableBattleRewardPolicy.ResolveBlueprintPerformanceBonusPercent(100, false), Is.EqualTo(0));
        }

        [Test]
        public void ThePerformanceBonusScalesWithRemainingHealthAndIsCapped()
        {
            Assert.That(
                RepeatableBattleRewardPolicy.ResolveCreditPerformanceBonusPercent(100, true),
                Is.EqualTo(RepeatableBattleRewardPolicy.MaxCreditPerformancePercent));
            Assert.That(
                RepeatableBattleRewardPolicy.ResolveCreditPerformanceBonusPercent(0, true),
                Is.EqualTo(0));
            Assert.That(
                RepeatableBattleRewardPolicy.ResolveCreditPerformanceBonusPercent(50, true),
                Is.EqualTo(RepeatableBattleRewardPolicy.MaxCreditPerformancePercent / 2));
            Assert.That(
                RepeatableBattleRewardPolicy.ResolveBlueprintPerformanceBonusPercent(100, true),
                Is.EqualTo(RepeatableBattleRewardPolicy.MaxBlueprintPerformancePercent));
        }

        [Test]
        public void ARaidPaysByTheChosenDifficultyAndADuelAlwaysPaysTheMiddleTier()
        {
            Assert.That(
                RepeatableBattleRewardPolicy.ResolveRewardDifficulty(BattleMode.Raid, 0),
                Is.EqualTo(BattleRewardDifficulty.Easy));
            Assert.That(
                RepeatableBattleRewardPolicy.ResolveRewardDifficulty(BattleMode.Raid, 1),
                Is.EqualTo(BattleRewardDifficulty.Medium));
            Assert.That(
                RepeatableBattleRewardPolicy.ResolveRewardDifficulty(BattleMode.Raid, 2),
                Is.EqualTo(BattleRewardDifficulty.Hard));

            // A duel has no difficulty to choose, and paying differently for a backfill
            // would give the bot away.
            Assert.That(
                RepeatableBattleRewardPolicy.ResolveRewardDifficulty(BattleMode.Pvp, 0),
                Is.EqualTo(BattleRewardDifficulty.Medium));
            Assert.That(
                RepeatableBattleRewardPolicy.ResolveRewardDifficulty(BattleMode.Pvp, 2),
                Is.EqualTo(BattleRewardDifficulty.Medium));
        }

        [Test]
        public void AWonRaidPaysCreditsExperienceAndBlueprints()
        {
            var save = CreateSave(finishedRepeatableBattlesToday: 1);
            var service = CreateService(save, out var economy);

            var reward = service.Grant(Request(BattleMode.Raid, difficultyTier: 1, BattleArenaOutcome.Victory, 100));

            Assert.That(reward.Granted, Is.True);
            Assert.That(reward.Credits, Is.GreaterThan(0));
            Assert.That(reward.CommanderExperience, Is.GreaterThan(0));
            Assert.That(reward.Blueprints, Is.GreaterThan(0));
            Assert.That(reward.IsReduced, Is.False);
            Assert.That(economy.SoftCurrency, Is.EqualTo(reward.Credits));
            Assert.That(economy.CommanderExperience, Is.EqualTo(reward.CommanderExperience));
        }

        [Test]
        public void ALostRaidStillPaysButLessThanAWonOne()
        {
            var won = CreateService(CreateSave(1), out _)
                .Grant(Request(BattleMode.Raid, 1, BattleArenaOutcome.Victory, 0));
            var lost = CreateService(CreateSave(1), out _)
                .Grant(Request(BattleMode.Raid, 1, BattleArenaOutcome.Defeat, 0));

            Assert.That(lost.Credits, Is.GreaterThan(0));
            Assert.That(lost.Credits, Is.LessThan(won.Credits));
        }

        [Test]
        public void PastTheDailyBudgetTheSameBattlePaysLessAndSaysSo()
        {
            var inBudget = CreateService(CreateSave(1), out _)
                .Grant(Request(BattleMode.Raid, 1, BattleArenaOutcome.Victory, 100));
            var pastBudget = CreateService(
                    CreateSave(RepeatableBattleRewardPolicy.FullRewardBattlesPerDay + 1), out _)
                .Grant(Request(BattleMode.Raid, 1, BattleArenaOutcome.Victory, 100));

            Assert.That(pastBudget.IsReduced, Is.True);
            Assert.That(pastBudget.Credits, Is.LessThan(inBudget.Credits));
        }

        [Test]
        public void RaidsAndDuelsShareOneDailyBudget()
        {
            // The counter behind the budget records both modes, so a day spent raiding
            // cannot be topped up by farming duels at the full rate.
            var save = CreateSave(RepeatableBattleRewardPolicy.FullRewardBattlesPerDay + 1);
            var service = CreateService(save, out _);

            var duel = service.Grant(Request(BattleMode.Pvp, 0, BattleArenaOutcome.Victory, 100));

            Assert.That(duel.IsReduced, Is.True);
        }

        [Test]
        public void StoryAndTrainingBattlesArePaidElsewhereAndNotHere()
        {
            var save = CreateSave(1);
            var service = CreateService(save, out var economy);

            var story = service.Grant(Request(BattleMode.Story, 1, BattleArenaOutcome.Victory, 100));
            var training = service.Grant(Request(BattleMode.Training, 1, BattleArenaOutcome.Victory, 100));

            Assert.That(story.Granted, Is.False);
            Assert.That(training.Granted, Is.False);
            Assert.That(economy.SoftCurrency, Is.EqualTo(0));
        }

        [Test]
        public void BankingTheSameBattleTwiceNeverPaysTwice()
        {
            var save = CreateSave(1);
            var service = CreateService(save, out var economy);
            var request = Request(BattleMode.Raid, 1, BattleArenaOutcome.Victory, 100);

            var first = service.Grant(request);
            service.Grant(request);

            Assert.That(economy.SoftCurrency, Is.EqualTo(first.Credits));
        }

        [Test]
        public void ABattleWithoutAnIdentityIsNeverPaid()
        {
            var service = CreateService(CreateSave(1), out var economy);

            var reward = service.Grant(new BattleArenaRewardRequest(
                BattleMode.Raid, 1, BattleArenaOutcome.Victory, 100, string.Empty));

            Assert.That(reward.Granted, Is.False);
            Assert.That(economy.SoftCurrency, Is.EqualTo(0));
        }

        private static BattleArenaRewardRequest Request(
            BattleMode mode,
            int difficultyTier,
            BattleArenaOutcome outcome,
            int healthPercent)
        {
            return new BattleArenaRewardRequest(mode, difficultyTier, outcome, healthPercent, "battle.test.1");
        }

        private static RepeatableBattleRewardService CreateService(SaveData save, out IEconomyService economy)
        {
            economy = new EconomyService(save);
            return new RepeatableBattleRewardService(save, new BattleRewardService(economy));
        }

        private static SaveData CreateSave(int finishedRepeatableBattlesToday)
        {
            var save = SaveData.CreateNew();
            save.EnsureDefaults();
            save.economy.softCurrency = 0;
            save.profile.commanderExperience = 0;
            save.activity.daily.arenaBattlesPlayed = finishedRepeatableBattlesToday;
            return save;
        }
    }
}
