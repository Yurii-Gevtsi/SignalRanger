using Game.Data.Progression;

namespace Game.Progression.Modes
{
    /// <summary>Difficulty the player picks before an offline raid.</summary>
    public enum RaidDifficulty
    {
        Easy = 0,
        Normal = 1,
        Hard = 2
    }

    /// <summary>
    /// What a raid difficulty actually changes. Per the campaign AI strategy it
    /// moves decision quality and how developed the opponent's weapons are - never
    /// damage, health or ammunition behind the player's back.
    /// </summary>
    public readonly struct RaidDifficultyRules
    {
        private RaidDifficultyRules(
            RaidDifficulty difficulty,
            string difficultyProfileId,
            int rankBonus,
            int rankCap,
            BattleRewardDifficulty rewardDifficulty)
        {
            Difficulty = difficulty;
            DifficultyProfileId = difficultyProfileId;
            RankBonus = rankBonus;
            RankCap = rankCap;
            RewardDifficulty = rewardDifficulty;
        }

        public RaidDifficulty Difficulty { get; }

        /// <summary>Content id resolving the AI profile and the opponent archetype.</summary>
        public string DifficultyProfileId { get; }

        /// <summary>Shift applied to the economically plausible weapon rank.</summary>
        public int RankBonus { get; }

        /// <summary>
        /// Ceiling on the opponent's weapon rank. Ranks 8-10 stay reserved for bosses
        /// and late endgame, so even a hard raid tops out at 7.
        /// </summary>
        public int RankCap { get; }

        public BattleRewardDifficulty RewardDifficulty { get; }

        /// <summary>AI tier the battle scene reads: 0 easy, 1 normal, 2 hard.</summary>
        public int AiDifficultyTier => (int)Difficulty;

        public static RaidDifficultyRules For(RaidDifficulty difficulty)
        {
            return difficulty switch
            {
                RaidDifficulty.Easy => new RaidDifficultyRules(
                    RaidDifficulty.Easy, "difficulty.easy", -1, 3, BattleRewardDifficulty.Easy),
                RaidDifficulty.Hard => new RaidDifficultyRules(
                    RaidDifficulty.Hard, "difficulty.hard", 1, 7, BattleRewardDifficulty.Hard),
                _ => new RaidDifficultyRules(
                    RaidDifficulty.Normal, "difficulty.normal", 0, 5, BattleRewardDifficulty.Medium)
            };
        }
    }
}
