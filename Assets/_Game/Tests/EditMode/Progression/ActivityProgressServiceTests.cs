using System;
using Game.Data.Save;
using Game.Progression;
using NUnit.Framework;

namespace Game.Tests.EditMode.Progression
{
    public sealed class ActivityProgressServiceTests
    {
        [Test]
        public void RecordsGameplayMetricsForDailyAndWeeklyPeriods()
        {
            var now = new DateTime(2026, 8, 4, 12, 0, 0, DateTimeKind.Utc);
            var service = new ActivityProgressService(SaveData.CreateNew(), () => now);

            service.RecordLogin();
            service.RecordBattleFinished(playerWon: true, isStoryBattle: true, isArenaBattle: false);
            service.RecordStoryMissionCompleted();
            service.RecordPlayerDamage(600);
            service.RecordTerrainDestroyed(120);
            service.RecordResourcesCollected(2);
            service.RecordShotFired("weapon.missle");
            service.RecordShotFired("weapon.granata");
            service.RecordPlayerMovement(3.5f, wasJumping: false);
            service.RecordPlayerMovement(2.25f, wasJumping: true);

            Assert.That(service.GetProgress(ActivityMetric.Logins, ActivityPeriod.Daily), Is.EqualTo(1));
            Assert.That(service.GetProgress(ActivityMetric.BattlesWon, ActivityPeriod.Daily), Is.EqualTo(1));
            Assert.That(service.GetProgress(ActivityMetric.StoryBattlesPlayed, ActivityPeriod.Daily), Is.EqualTo(1));
            Assert.That(service.GetProgress(ActivityMetric.StoryMissionsCompleted, ActivityPeriod.Daily), Is.EqualTo(1));
            Assert.That(service.GetProgress(ActivityMetric.DamageDealt, ActivityPeriod.Daily), Is.EqualTo(600));
            Assert.That(service.GetProgress(ActivityMetric.TerrainCellsDestroyed, ActivityPeriod.Daily), Is.EqualTo(120));
            Assert.That(service.GetProgress(ActivityMetric.ResourcesCollected, ActivityPeriod.Daily), Is.EqualTo(2));
            Assert.That(service.GetProgress(ActivityMetric.RocketShotsFired, ActivityPeriod.Daily), Is.EqualTo(1));
            Assert.That(service.GetProgress(ActivityMetric.GrenadeShotsFired, ActivityPeriod.Daily), Is.EqualTo(1));
            Assert.That(service.GetProgress(ActivityMetric.WalkDistance, ActivityPeriod.Daily), Is.EqualTo(3));
            Assert.That(service.GetProgress(ActivityMetric.JumpDistance, ActivityPeriod.Weekly), Is.EqualTo(2));
        }

        [Test]
        public void NewDayResetsDailyCountersButKeepsWeeklyCounters()
        {
            var now = new DateTime(2026, 8, 4, 12, 0, 0, DateTimeKind.Utc);
            var service = new ActivityProgressService(SaveData.CreateNew(), () => now);
            service.RecordPlayerDamage(250);

            now = now.AddDays(1);

            Assert.That(service.GetProgress(ActivityMetric.DamageDealt, ActivityPeriod.Daily), Is.EqualTo(0));
            Assert.That(service.GetProgress(ActivityMetric.DamageDealt, ActivityPeriod.Weekly), Is.EqualTo(250));
        }

        [Test]
        public void DailyQuestSelectionIsStableForTheWholePeriod()
        {
            var periodStart = new DateTime(2026, 8, 4, 0, 0, 0, DateTimeKind.Utc).Ticks;

            var first = ActivityQuestCatalog.GetVisibleQuests(ActivityPeriod.Daily, periodStart);
            var second = ActivityQuestCatalog.GetVisibleQuests(ActivityPeriod.Daily, periodStart);

            Assert.That(first, Has.Length.EqualTo(ActivityQuestCatalog.VisibleQuestCount));
            Assert.That(first[0].Metric, Is.EqualTo(ActivityMetric.Logins));
            for (var i = 0; i < first.Length; i++)
            {
                Assert.That(second[i].Id, Is.EqualTo(first[i].Id));
            }
        }
    }
}
