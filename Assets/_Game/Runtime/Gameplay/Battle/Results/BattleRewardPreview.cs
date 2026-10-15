namespace Game.Gameplay.Battle.Results
{
    public readonly struct BattleRewardPreview
    {
        public BattleRewardPreview(
            int softCurrency,
            string unlockText,
            int commanderExperience = 0,
            int blueprints = 0,
            bool isReduced = false)
        {
            SoftCurrency = softCurrency;
            UnlockText = unlockText ?? string.Empty;
            CommanderExperience = commanderExperience;
            Blueprints = blueprints;
            IsReduced = isReduced;
        }

        public int SoftCurrency { get; }
        public string UnlockText { get; }

        /// <summary>Commander XP banked by this battle; 0 for a campaign mission, which pays on the map.</summary>
        public int CommanderExperience { get; }

        /// <summary>Weapon blueprints banked by this battle.</summary>
        public int Blueprints { get; }

        /// <summary>
        /// Whether this was paid at the reduced rate because the day's full-reward
        /// budget was already spent. Shown on the card so a smaller payout is
        /// explained rather than looking like a bug.
        /// </summary>
        public bool IsReduced { get; }

        public bool HasUnlock => !string.IsNullOrWhiteSpace(UnlockText);
        public bool HasPayout => SoftCurrency > 0 || CommanderExperience > 0 || Blueprints > 0;
    }
}
