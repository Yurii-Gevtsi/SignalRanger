namespace Game.Economy
{
    public interface IEconomyService
    {
        int SoftCurrency { get; }
        int PremiumCurrency { get; }
        int CommanderExperience { get; }
        bool AdminModeEnabled { get; }

        EconomyTransactionResult Grant(int amount, string reason);
        EconomyTransactionResult Grant(int amount, string sourceType, string sourceId, string transactionId);
        EconomyTransactionResult GrantPremium(int amount, string sourceType, string sourceId, string transactionId);
        EconomyTransactionResult Grant(
            Game.Data.Progression.ProgressionReward reward,
            string sourceType,
            string sourceId,
            string transactionId);

        bool CanSpend(int amount);
        bool CanSpendPremium(int amount);

        EconomyTransactionResult TrySpend(int amount, string reason);
        EconomyTransactionResult TrySpend(int amount, string sourceType, string sourceId, string transactionId);
        EconomyTransactionResult TrySpendPremium(int amount, string sourceType, string sourceId, string transactionId);
        EconomyTransactionResult TrySpendWeaponUpgrade(
            int credits,
            string weaponId,
            int blueprints,
            string sourceType,
            string sourceId,
            string transactionId);
    }
}
