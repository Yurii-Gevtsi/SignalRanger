using System;
using System.Collections.Generic;

namespace Game.Data.Progression
{
    public enum BattleRewardDifficulty
    {
        Easy = 0,
        Medium = 1,
        Hard = 2
    }

    public enum BattleRewardOutcome
    {
        Victory = 0,
        Draw = 1,
        Defeat = 2
    }

    public readonly struct ProgressionReward
    {
        public ProgressionReward(int credits, int commanderExperience, int blueprints, string blueprintWeaponId = "")
        {
            Credits = credits < 0 ? 0 : credits;
            CommanderExperience = commanderExperience < 0 ? 0 : commanderExperience;
            Blueprints = blueprints < 0 ? 0 : blueprints;
            BlueprintWeaponId = blueprintWeaponId ?? string.Empty;
        }

        public int Credits { get; }
        public int CommanderExperience { get; }
        public int Blueprints { get; }
        public string BlueprintWeaponId { get; }
        public bool IsEmpty => Credits == 0 && CommanderExperience == 0 && Blueprints == 0;
    }

    public static class BattleRewardCatalog
    {
        public const int CampaignReplayPercent = 40;
        public const int NonVictoryPercent = 50;
        public const int MaxCreditPerformancePercent = 30;
        public const int MaxBlueprintPerformancePercent = 10;

        public static ProgressionReward GetArenaReward(
            BattleRewardDifficulty difficulty,
            BattleRewardOutcome outcome,
            string blueprintWeaponId = "")
        {
            var victory = difficulty switch
            {
                BattleRewardDifficulty.Easy => new ProgressionReward(100, 40, 8, blueprintWeaponId),
                BattleRewardDifficulty.Medium => new ProgressionReward(150, 60, 12, blueprintWeaponId),
                BattleRewardDifficulty.Hard => new ProgressionReward(220, 90, 18, blueprintWeaponId),
                _ => new ProgressionReward(100, 40, 8, blueprintWeaponId)
            };

            return outcome == BattleRewardOutcome.Victory
                ? victory
                : Scale(victory, NonVictoryPercent);
        }

        public static ProgressionReward GetCampaignReplayReward(ProgressionReward firstCompletionReward)
        {
            return Scale(firstCompletionReward, CampaignReplayPercent);
        }

        public static ProgressionReward ApplyPerformanceBonus(
            ProgressionReward reward,
            int creditBonusPercent,
            int blueprintBonusPercent)
        {
            var safeCreditBonus = Clamp(creditBonusPercent, 0, MaxCreditPerformancePercent);
            var safeBlueprintBonus = Clamp(blueprintBonusPercent, 0, MaxBlueprintPerformancePercent);
            return new ProgressionReward(
                reward.Credits + PercentOf(reward.Credits, safeCreditBonus),
                reward.CommanderExperience,
                reward.Blueprints + PercentOf(reward.Blueprints, safeBlueprintBonus),
                reward.BlueprintWeaponId);
        }

        private static ProgressionReward Scale(ProgressionReward reward, int percent)
        {
            return new ProgressionReward(
                PercentOf(reward.Credits, percent),
                PercentOf(reward.CommanderExperience, percent),
                PercentOf(reward.Blueprints, percent),
                reward.BlueprintWeaponId);
        }

        private static int PercentOf(int value, int percent)
        {
            return (int)Math.Round(value * percent / 100f, MidpointRounding.AwayFromZero);
        }

        private static int Clamp(int value, int minimum, int maximum)
        {
            return value < minimum ? minimum : value > maximum ? maximum : value;
        }
    }

    public static class CommanderProgressionTable
    {
        private static readonly int[] CumulativeExperience = BuildThresholds();

        public static int MaxLevel => CumulativeExperience.Length;
        public static IReadOnlyList<int> Thresholds => CumulativeExperience;

        /// <summary>
        /// Debug/QA override: when set, <see cref="GetLevel"/> always reports
        /// <see cref="MaxLevel"/> without touching the player's real saved
        /// experience, so turning it back off restores their true level. Set by
        /// AppServices from the settings screen's admin-mode toggle.
        /// </summary>
        public static bool ForceMaxLevel { get; set; }

        public static int GetLevel(int totalExperience)
        {
            if (ForceMaxLevel)
            {
                return MaxLevel;
            }

            var safeExperience = totalExperience < 0 ? 0 : totalExperience;
            for (var index = CumulativeExperience.Length - 1; index >= 0; index--)
            {
                if (safeExperience >= CumulativeExperience[index])
                {
                    return index + 1;
                }
            }

            return 1;
        }

        public static int GetExperienceRequiredForLevel(int level)
        {
            var safeLevel = level < 1 ? 1 : level > MaxLevel ? MaxLevel : level;
            return CumulativeExperience[safeLevel - 1];
        }

        private static int[] BuildThresholds()
        {
            var thresholds = new int[50];
            thresholds[0] = 0;
            for (var level = 2; level <= thresholds.Length; level++)
            {
                var step = level <= 10
                    ? level * 50
                    : 450 + (level - 10) * 75;
                thresholds[level - 1] = thresholds[level - 2] + step;
            }

            return thresholds;
        }
    }
}
