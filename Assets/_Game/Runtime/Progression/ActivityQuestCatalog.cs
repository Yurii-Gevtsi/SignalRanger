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
            "dailyLogin", "Увійти в гру", ActivityMetric.Logins, 1, ActivityRewardKind.Credits, 200, "quest_login");

        private static readonly ActivityQuestDefinition[] DailyPool =
        {
            Quest("winBattles", "Перемогти 3 рази", ActivityMetric.BattlesWon, 3, ActivityRewardKind.Crystals, 10, "quest_win"),
            Quest("destroyTerrain", "Знищити 1500 одиниць землі", ActivityMetric.TerrainCellsDestroyed, 1500, ActivityRewardKind.Chip, 15, "quest_terrain"),
            Quest("dealDamage", "Завдати 6000 шкоди", ActivityMetric.DamageDealt, 6000, ActivityRewardKind.Crystals, 20, "quest_damage"),
            Quest("arenaBattles", "Зіграти 2 бої на арені", ActivityMetric.ArenaBattlesPlayed, 2, ActivityRewardKind.Credits, 450, "quest_arena"),
            Quest("storyBattles", "Зіграти 2 сюжетні бої", ActivityMetric.StoryBattlesPlayed, 2, ActivityRewardKind.Chip, 10, "quest_arena"),
            Quest("storyMissions", "Виконати 5 сюжетних місій", ActivityMetric.StoryMissionsCompleted, 5, ActivityRewardKind.Crystals, 25, "quest_win"),
            Quest("resourcesCollected", "Підібрати 5 ресурсів", ActivityMetric.ResourcesCollected, 5, ActivityRewardKind.Chest, 1, "quest_cinema"),
            Quest("cinemaAds", "Переглянути рекламу в кінотеатрі 5 разів", ActivityMetric.RewardedAdsWatched, 5, ActivityRewardKind.Chest, 1, "quest_cinema"),
            Quest("walkDistance", "Пройти 75 метрів", ActivityMetric.WalkDistance, 75, ActivityRewardKind.Credits, 350, "quest_terrain"),
            Quest("jumpDistance", "Пролетіти стрибками 50 метрів", ActivityMetric.JumpDistance, 50, ActivityRewardKind.Crystals, 15, "quest_super"),
            Quest("shotsFired", "Випустити 20 снарядів", ActivityMetric.ShotsFired, 20, ActivityRewardKind.Chip, 8, "quest_damage"),
            Quest("rocketShots", "Випустити 10 ракет", ActivityMetric.RocketShotsFired, 10, ActivityRewardKind.Crystals, 12, "quest_damage"),
            Quest("grenadeShots", "Кинути 10 гранат", ActivityMetric.GrenadeShotsFired, 10, ActivityRewardKind.Chip, 12, "quest_damage")
        };

        private static readonly ActivityQuestDefinition[] WeeklyPool =
        {
            Quest("weeklyWins", "Перемогти у 15 боях", ActivityMetric.BattlesWon, 15, ActivityRewardKind.Credits, 1500, "quest_win"),
            Quest("weeklyTerrain", "Знищити 10000 одиниць землі", ActivityMetric.TerrainCellsDestroyed, 10000, ActivityRewardKind.Crystals, 60, "quest_terrain"),
            Quest("weeklyDamage", "Завдати 40000 шкоди", ActivityMetric.DamageDealt, 40000, ActivityRewardKind.Chip, 40, "quest_damage"),
            Quest("weeklyArena", "Зіграти 10 боїв на арені", ActivityMetric.ArenaBattlesPlayed, 10, ActivityRewardKind.Credits, 1200, "quest_arena"),
            Quest("weeklyStoryBattles", "Зіграти 8 сюжетних боїв", ActivityMetric.StoryBattlesPlayed, 8, ActivityRewardKind.Crystals, 45, "quest_arena"),
            Quest("weeklyStoryMissions", "Виконати 10 сюжетних місій", ActivityMetric.StoryMissionsCompleted, 10, ActivityRewardKind.Chip, 35, "quest_win"),
            Quest("weeklyResources", "Підібрати 25 ресурсів", ActivityMetric.ResourcesCollected, 25, ActivityRewardKind.Chest, 2, "quest_cinema"),
            Quest("weeklyAds", "Переглянути рекламу 20 разів", ActivityMetric.RewardedAdsWatched, 20, ActivityRewardKind.Chest, 2, "quest_cinema"),
            Quest("weeklyWalk", "Пройти 500 метрів", ActivityMetric.WalkDistance, 500, ActivityRewardKind.Credits, 1800, "quest_terrain"),
            Quest("weeklyJump", "Пролетіти стрибками 300 метрів", ActivityMetric.JumpDistance, 300, ActivityRewardKind.Crystals, 55, "quest_super"),
            Quest("weeklyShots", "Випустити 120 снарядів", ActivityMetric.ShotsFired, 120, ActivityRewardKind.Chip, 50, "quest_damage")
        };

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
