namespace Game.Gameplay.Battle.Results
{
    public readonly struct BattleRewardPreview
    {
        public BattleRewardPreview(int softCurrency, string unlockText)
        {
            SoftCurrency = softCurrency;
            UnlockText = unlockText ?? string.Empty;
        }

        public int SoftCurrency { get; }
        public string UnlockText { get; }
        public bool HasUnlock => !string.IsNullOrWhiteSpace(UnlockText);
    }
}
