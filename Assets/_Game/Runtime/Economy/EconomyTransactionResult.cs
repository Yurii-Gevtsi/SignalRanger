namespace Game.Economy
{
    public readonly struct EconomyTransactionResult
    {
        public EconomyTransactionResult(bool success, int balance, string reason)
            : this(success, balance, reason, string.Empty, false)
        {
        }

        public EconomyTransactionResult(bool success, int balance, string reason, string transactionId, bool wasDuplicate)
        {
            Success = success;
            Balance = balance < 0 ? 0 : balance;
            Reason = reason ?? string.Empty;
            TransactionId = transactionId ?? string.Empty;
            WasDuplicate = wasDuplicate;
        }

        public bool Success { get; }
        public int Balance { get; }
        public string Reason { get; }
        public string TransactionId { get; }
        public bool WasDuplicate { get; }
    }
}
