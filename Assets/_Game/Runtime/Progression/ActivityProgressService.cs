using System;
using System.Collections.Generic;
using Game.Data.Save;

namespace Game.Progression
{
    public enum ActivityPeriod
    {
        Daily,
        Weekly
    }

    public enum ActivityMetric
    {
        Logins,
        BattlesPlayed,
        BattlesWon,
        ArenaBattlesPlayed,
        StoryBattlesPlayed,
        StoryMissionsCompleted,
        DamageDealt,
        TerrainCellsDestroyed,
        ResourcesCollected,
        RewardedAdsWatched,
        ShotsFired,
        RocketShotsFired,
        GrenadeShotsFired,
        WalkDistance,
        JumpDistance
    }

    public sealed class ActivityProgressService
    {
        private readonly SaveData _saveData;
        private readonly Func<DateTime> _utcNow;

        public ActivityProgressService(SaveData saveData, Func<DateTime> utcNow = null)
        {
            _saveData = saveData ?? throw new ArgumentNullException(nameof(saveData));
            _utcNow = utcNow ?? (() => DateTime.UtcNow);
            _saveData.EnsureDefaults();
            EnsureCurrentPeriods();
        }

        public bool RecordLogin()
        {
            EnsureCurrentPeriods();
            var activity = _saveData.activity;
            if (activity.lastLoginDailyPeriodStartUtcTicks == activity.dailyPeriodStartUtcTicks)
            {
                return false;
            }

            activity.lastLoginDailyPeriodStartUtcTicks = activity.dailyPeriodStartUtcTicks;
            AddToActivePeriods(counter => counter.logins++);
            return true;
        }

        public void RecordBattleFinished(bool playerWon, bool isStoryBattle, bool isArenaBattle)
        {
            AddToActivePeriods(counter =>
            {
                counter.battlesPlayed++;
                if (playerWon)
                {
                    counter.battlesWon++;
                }

                if (isStoryBattle)
                {
                    counter.storyBattlesPlayed++;
                }

                if (isArenaBattle)
                {
                    counter.arenaBattlesPlayed++;
                }
            });
        }

        public void RecordPlayerDamage(int amount) => AddToActivePeriods(counter => counter.damageDealt += Positive(amount));

        public void RecordStoryMissionCompleted() => AddToActivePeriods(counter => counter.storyMissionsCompleted++);

        public void RecordTerrainDestroyed(int cells) => AddToActivePeriods(counter => counter.terrainCellsDestroyed += Positive(cells));

        public void RecordResourcesCollected(int count) => AddToActivePeriods(counter => counter.resourcesCollected += Positive(count));

        public void RecordRewardedAdCompleted() => AddToActivePeriods(counter => counter.rewardedAdsWatched++);

        public void RecordShotFired(string weaponId)
        {
            AddToActivePeriods(counter =>
            {
                counter.shotsFired++;
                if (weaponId == "weapon.missle" || weaponId == "weapon.bur-rocket")
                {
                    counter.rocketShotsFired++;
                }

                if (weaponId == "weapon.granata")
                {
                    counter.grenadeShotsFired++;
                }
            });
        }

        public void RecordPlayerMovement(float distance, bool wasJumping)
        {
            var centiUnits = Positive((int)Math.Round(Math.Max(0f, distance) * 100f));
            if (centiUnits <= 0)
            {
                return;
            }

            AddToActivePeriods(counter =>
            {
                if (wasJumping)
                {
                    counter.jumpDistanceCentiUnits += centiUnits;
                }
                else
                {
                    counter.walkDistanceCentiUnits += centiUnits;
                }
            });
        }

        public int GetProgress(ActivityMetric metric, ActivityPeriod period)
        {
            EnsureCurrentPeriods();
            var counter = period == ActivityPeriod.Daily ? _saveData.activity.daily : _saveData.activity.weekly;
            return metric switch
            {
                ActivityMetric.Logins => counter.logins,
                ActivityMetric.BattlesPlayed => counter.battlesPlayed,
                ActivityMetric.BattlesWon => counter.battlesWon,
                ActivityMetric.ArenaBattlesPlayed => counter.arenaBattlesPlayed,
                ActivityMetric.StoryBattlesPlayed => counter.storyBattlesPlayed,
                ActivityMetric.StoryMissionsCompleted => counter.storyMissionsCompleted,
                ActivityMetric.DamageDealt => counter.damageDealt,
                ActivityMetric.TerrainCellsDestroyed => counter.terrainCellsDestroyed,
                ActivityMetric.ResourcesCollected => counter.resourcesCollected,
                ActivityMetric.RewardedAdsWatched => counter.rewardedAdsWatched,
                ActivityMetric.ShotsFired => counter.shotsFired,
                ActivityMetric.RocketShotsFired => counter.rocketShotsFired,
                ActivityMetric.GrenadeShotsFired => counter.grenadeShotsFired,
                ActivityMetric.WalkDistance => counter.walkDistanceCentiUnits / 100,
                ActivityMetric.JumpDistance => counter.jumpDistanceCentiUnits / 100,
                _ => 0
            };
        }

        public long GetPeriodStartUtcTicks(ActivityPeriod period)
        {
            EnsureCurrentPeriods();
            return period == ActivityPeriod.Daily
                ? _saveData.activity.dailyPeriodStartUtcTicks
                : _saveData.activity.weeklyPeriodStartUtcTicks;
        }

        public bool IsClaimed(ActivityPeriod period, string questId)
        {
            EnsureCurrentPeriods();
            var claimed = period == ActivityPeriod.Daily
                ? _saveData.activity.dailyClaimedQuestIds
                : _saveData.activity.weeklyClaimedQuestIds;
            return !string.IsNullOrWhiteSpace(questId) && claimed.Contains(questId);
        }

        public bool TryClaim(ActivityPeriod period, string questId, ActivityMetric metric, int target)
        {
            if (string.IsNullOrWhiteSpace(questId) || GetProgress(metric, period) < Math.Max(1, target) || IsClaimed(period, questId))
            {
                return false;
            }

            var claimed = period == ActivityPeriod.Daily
                ? _saveData.activity.dailyClaimedQuestIds
                : _saveData.activity.weeklyClaimedQuestIds;
            claimed.Add(questId);
            return true;
        }

        public void EnsureCurrentPeriods()
        {
            _saveData.EnsureDefaults();
            var now = _utcNow().ToUniversalTime().Date;
            var dailyStartTicks = now.Ticks;
            var weekStart = now.AddDays(-((int)now.DayOfWeek + 6) % 7);
            var weeklyStartTicks = weekStart.Ticks;
            var activity = _saveData.activity;

            if (activity.dailyPeriodStartUtcTicks != dailyStartTicks)
            {
                activity.dailyPeriodStartUtcTicks = dailyStartTicks;
                activity.daily = SaveActivityCounterData.CreateDefault();
                activity.dailyClaimedQuestIds.Clear();
            }

            if (activity.weeklyPeriodStartUtcTicks != weeklyStartTicks)
            {
                activity.weeklyPeriodStartUtcTicks = weeklyStartTicks;
                activity.weekly = SaveActivityCounterData.CreateDefault();
                activity.weeklyClaimedQuestIds.Clear();
            }
        }

        private void AddToActivePeriods(Action<SaveActivityCounterData> add)
        {
            if (add == null)
            {
                return;
            }

            EnsureCurrentPeriods();
            add(_saveData.activity.daily);
            add(_saveData.activity.weekly);
        }

        private static int Positive(int value) => value < 0 ? 0 : value;
    }
}
