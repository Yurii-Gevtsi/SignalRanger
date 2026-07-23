namespace Game.Economy
{
    public interface IEconomyService
    {
        int SoftCurrency { get; }

        EconomyTransactionResult Grant(int amount, string reason);
        EconomyTransactionResult Grant(int amount, string sourceType, string sourceId, string transactionId);

        bool CanSpend(int amount);

        EconomyTransactionResult TrySpend(int amount, string reason);
        EconomyTransactionResult TrySpend(int amount, string sourceType, string sourceId, string transactionId);
    }
}
