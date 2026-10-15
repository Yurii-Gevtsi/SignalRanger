using Game.Core.Modes;

namespace Game.Core.Battle
{
    /// <summary>How a repeatable battle ended, for reward purposes.</summary>
    public enum BattleArenaOutcome
    {
        Victory = 0,
        Draw = 1,
        Defeat = 2
    }

    /// <summary>What the battle layer reports when a raid or duel finishes.</summary>
    public readonly struct BattleArenaRewardRequest
    {
        public BattleArenaRewardRequest(
            BattleMode mode,
            int difficultyTier,
            BattleArenaOutcome outcome,
            int playerHealthPercent,
            string battleId)
        {
            Mode = mode;
            DifficultyTier = difficultyTier < 0 ? 0 : difficultyTier > 2 ? 2 : difficultyTier;
            Outcome = outcome;
            PlayerHealthPercent = playerHealthPercent < 0 ? 0 : playerHealthPercent > 100 ? 100 : playerHealthPercent;
            BattleId = battleId ?? string.Empty;
        }

        public BattleMode Mode { get; }

        /// <summary>Raid difficulty the player chose: 0 easy, 1 normal, 2 hard.</summary>
        public int DifficultyTier { get; }
        public BattleArenaOutcome Outcome { get; }

        /// <summary>Health the player finished on, which drives the performance bonus.</summary>
        public int PlayerHealthPercent { get; }

        /// <summary>
        /// Identifies this battle so the grant is idempotent: banking the same
        /// finished battle twice must never pay twice.
        /// </summary>
        public string BattleId { get; }
    }

    /// <summary>What the economy actually granted, so the result card can show it.</summary>
    public readonly struct BattleArenaReward
    {
        public BattleArenaReward(
            bool granted,
            int credits,
            int commanderExperience,
            int blueprints,
            string blueprintWeaponId,
            bool isReduced)
        {
            Granted = granted;
            Credits = credits < 0 ? 0 : credits;
            CommanderExperience = commanderExperience < 0 ? 0 : commanderExperience;
            Blueprints = blueprints < 0 ? 0 : blueprints;
            BlueprintWeaponId = blueprintWeaponId ?? string.Empty;
            IsReduced = isReduced;
        }

        /// <summary>False when nothing was paid - a story battle, or no app layer wired.</summary>
        public bool Granted { get; }
        public int Credits { get; }
        public int CommanderExperience { get; }
        public int Blueprints { get; }
        public string BlueprintWeaponId { get; }

        /// <summary>
        /// True when the day's full-reward budget was already spent and this battle
        /// paid the reduced rate. The card says so rather than quietly paying less.
        /// </summary>
        public bool IsReduced { get; }

        public bool IsEmpty => Credits == 0 && CommanderExperience == 0 && Blueprints == 0;

        public static BattleArenaReward None => new(false, 0, 0, 0, string.Empty, false);
    }
}
