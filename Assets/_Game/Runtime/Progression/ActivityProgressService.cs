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

        /// <summary>
        /// Whether the player has actually clicked the daily login claim button for the
        /// current period. Distinct from <see cref="RecordLogin"/>'s login-streak marker,
        /// which fires automatically on app start and must not be treated as a claim.
        /// </summary>
        public bool IsDailyLoginRewardClaimed()
        {
            EnsureCurrentPeriods();
            var activity = _saveData.activity;
            return activity.dailyLoginRewardClaimedPeriodStartUtcTicks == activity.dailyPeriodStartUtcTicks;
        }

        public bool TryClaimDailyLoginReward()
        {
            EnsureCurrentPeriods();
            if (IsDailyLoginRewardClaimed())
            {
                return false;
            }

            _saveData.activity.dailyLoginRewardClaimedPeriodStartUtcTicks = _saveData.activity.dailyPeriodStartUtcTicks;
            return true;
        }

        /// <summary>
        /// Which of the 7 login-streak days is up for claim right now (1-7).
        /// </summary>
        public int GetDailyLoginStreakDay()
        {
            EnsureCurrentPeriods();
            return _saveData.activity.dailyLoginStreakDay;
        }

        /// <summary>
        /// Advances the 7-day login streak by one when the player claimed
        /// yesterday's reward (kept the streak alive), or resets it back to day 1
        /// when a day was skipped. Called right before <paramref name="activity"/>'s
        /// period timestamp rolls over to <paramref name="newDailyStartTicks"/>, so
        /// the previous value still reflects "yesterday" at this point.
        /// </summary>
        private static void AdvanceDailyLoginStreak(SaveActivityProgressData activity, long newDailyStartTicks)
        {
            var previousDailyStartTicks = activity.dailyPeriodStartUtcTicks;
            if (previousDailyStartTicks == 0)
            {
                // First session ever - EnsureDefaults already leaves the streak at day 1.
                return;
            }

            var oneDayTicks = TimeSpan.FromDays(1).Ticks;
            var wasConsecutiveDay = newDailyStartTicks - previousDailyStartTicks == oneDayTicks
                && activity.dailyLoginRewardClaimedPeriodStartUtcTicks == previousDailyStartTicks;

            if (wasConsecutiveDay)
            {
                activity.dailyLoginStreakDay = activity.dailyLoginStreakDay >= SaveActivityProgressData.DailyLoginStreakLength
                    ? 1
                    : activity.dailyLoginStreakDay + 1;
            }
            else
            {
                activity.dailyLoginStreakDay = 1;
            }
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
                AdvanceDailyLoginStreak(activity, dailyStartTicks);
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
