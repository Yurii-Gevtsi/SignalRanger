using System;
using System.Collections.Generic;

namespace Game.Progression
{
    public enum ActivityRewardKind
    {
        Credits,
        Crystals,
        Chip,
        Chest
    }

    public readonly struct ActivityQuestDefinition
    {
        public ActivityQuestDefinition(
            string id,
            string fallbackTitle,
            ActivityMetric metric,
            int target,
            ActivityRewardKind rewardKind,
            int rewardAmount,
            string iconId)
        {
            Id = id;
            FallbackTitle = fallbackTitle;
            Metric = metric;
            Target = Math.Max(1, target);
            RewardKind = rewardKind;
            RewardAmount = Math.Max(0, rewardAmount);
            IconId = iconId ?? string.Empty;
        }

        public string Id { get; }
        public string FallbackTitle { get; }
        public ActivityMetric Metric { get; }
        public int Target { get; }
        public ActivityRewardKind RewardKind { get; }
        public int RewardAmount { get; }
        public string IconId { get; }
    }

    public static class ActivityQuestCatalog
    {
        public const int VisibleQuestCount = 7;

        private static readonly ActivityQuestDefinition DailyLogin = Quest(
            "dailyLogin", string.Empty, ActivityMetric.Logins, 1, ActivityRewardKind.Credits, 200, "quest_login");

        private static readonly ActivityQuestDefinition[] DailyPool =
        {
            Quest("winBattles", string.Empty, ActivityMetric.BattlesWon, 3, ActivityRewardKind.Crystals, 10, "quest_win"),
            Quest("destroyTerrain", string.Empty, ActivityMetric.TerrainCellsDestroyed, 1500, ActivityRewardKind.Chip, 15, "quest_terrain"),
            Quest("dealDamage", string.Empty, ActivityMetric.DamageDealt, 6000, ActivityRewardKind.Crystals, 20, "quest_damage"),
            Quest("arenaBattles", string.Empty, ActivityMetric.ArenaBattlesPlayed, 2, ActivityRewardKind.Credits, 450, "quest_arena"),
            Quest("storyBattles", string.Empty, ActivityMetric.StoryBattlesPlayed, 2, ActivityRewardKind.Chip, 10, "quest_arena"),
            Quest("storyMissions", string.Empty, ActivityMetric.StoryMissionsCompleted, 5, ActivityRewardKind.Crystals, 25, "quest_win"),
            Quest("resourcesCollected", string.Empty, ActivityMetric.ResourcesCollected, 5, ActivityRewardKind.Chest, 1, "quest_cinema"),
            Quest("cinemaAds", string.Empty, ActivityMetric.RewardedAdsWatched, 5, ActivityRewardKind.Chest, 1, "quest_cinema"),
            Quest("walkDistance", string.Empty, ActivityMetric.WalkDistance, 75, ActivityRewardKind.Credits, 350, "quest_terrain"),
            Quest("jumpDistance", string.Empty, ActivityMetric.JumpDistance, 50, ActivityRewardKind.Crystals, 15, "quest_super"),
            Quest("shotsFired", string.Empty, ActivityMetric.ShotsFired, 20, ActivityRewardKind.Chip, 8, "quest_damage"),
            Quest("rocketShots", string.Empty, ActivityMetric.RocketShotsFired, 10, ActivityRewardKind.Crystals, 12, "quest_damage"),
            Quest("grenadeShots", string.Empty, ActivityMetric.GrenadeShotsFired, 10, ActivityRewardKind.Chip, 12, "quest_damage")
        };

        private static readonly ActivityQuestDefinition[] WeeklyPool =
        {
            Quest("weeklyWins", string.Empty, ActivityMetric.BattlesWon, 15, ActivityRewardKind.Credits, 1500, "quest_win"),
            Quest("weeklyTerrain", string.Empty, ActivityMetric.TerrainCellsDestroyed, 10000, ActivityRewardKind.Crystals, 60, "quest_terrain"),
            Quest("weeklyDamage", string.Empty, ActivityMetric.DamageDealt, 40000, ActivityRewardKind.Chip, 40, "quest_damage"),
            Quest("weeklyArena", string.Empty, ActivityMetric.ArenaBattlesPlayed, 10, ActivityRewardKind.Credits, 1200, "quest_arena"),
            Quest("weeklyStoryBattles", string.Empty, ActivityMetric.StoryBattlesPlayed, 8, ActivityRewardKind.Crystals, 45, "quest_arena"),
            Quest("weeklyStoryMissions", string.Empty, ActivityMetric.StoryMissionsCompleted, 10, ActivityRewardKind.Chip, 35, "quest_win"),
            Quest("weeklyResources", string.Empty, ActivityMetric.ResourcesCollected, 25, ActivityRewardKind.Chest, 2, "quest_cinema"),
            Quest("weeklyAds", string.Empty, ActivityMetric.RewardedAdsWatched, 20, ActivityRewardKind.Chest, 2, "quest_cinema"),
            Quest("weeklyWalk", string.Empty, ActivityMetric.WalkDistance, 500, ActivityRewardKind.Credits, 1800, "quest_terrain"),
            Quest("weeklyJump", string.Empty, ActivityMetric.JumpDistance, 300, ActivityRewardKind.Crystals, 55, "quest_super"),
            Quest("weeklyShots", string.Empty, ActivityMetric.ShotsFired, 120, ActivityRewardKind.Chip, 50, "quest_damage")
        };

        /// <summary>
        /// Every quest that can ever be shown, daily and weekly. Exposed so the
        /// localization check can prove each one has a translated title: the visible
        /// set rotates, so sampling it would miss whichever quests are off-rotation
        /// today - and those are exactly the ones that go unnoticed.
        /// </summary>
        public static IReadOnlyList<ActivityQuestDefinition> AllQuests
        {
            get
            {
                var all = new List<ActivityQuestDefinition> { DailyLogin };
                all.AddRange(DailyPool);
                all.AddRange(WeeklyPool);
                return all;
            }
        }

        public static ActivityQuestDefinition[] GetVisibleQuests(ActivityPeriod period, long periodStartUtcTicks)
        {
            if (period == ActivityPeriod.Daily)
            {
                var result = new ActivityQuestDefinition[VisibleQuestCount];
                result[0] = DailyLogin;
                CopySelection(DailyPool, result, 1, periodStartUtcTicks ^ 0x3D9E3779L);
                return result;
            }

            var weekly = new ActivityQuestDefinition[VisibleQuestCount];
            CopySelection(WeeklyPool, weekly, 0, periodStartUtcTicks ^ 0x5F356495L);
            return weekly;
        }

        private static ActivityQuestDefinition Quest(
            string id,
            string title,
            ActivityMetric metric,
            int target,
            ActivityRewardKind reward,
            int rewardAmount,
            string iconId)
        {
            return new ActivityQuestDefinition(id, title, metric, target, reward, rewardAmount, iconId);
        }

        private static void CopySelection(
            IReadOnlyList<ActivityQuestDefinition> pool,
            ActivityQuestDefinition[] destination,
            int destinationStart,
            long seed)
        {
            var order = new List<int>(pool.Count);
            for (var i = 0; i < pool.Count; i++)
            {
                order.Add(i);
            }

            var random = new Random(unchecked((int)(seed ^ (seed >> 32))));
            for (var i = order.Count - 1; i > 0; i--)
            {
                var swapIndex = random.Next(i + 1);
                (order[i], order[swapIndex]) = (order[swapIndex], order[i]);
            }

            for (var index = destinationStart; index < destination.Length; index++)
            {
                destination[index] = pool[order[index - destinationStart]];
            }
        }
    }
}
