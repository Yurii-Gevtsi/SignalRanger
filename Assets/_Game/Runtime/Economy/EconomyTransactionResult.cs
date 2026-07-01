namespace Game.Economy
{
    public readonly struct EconomyTransactionResult
    {
        public EconomyTransactionResult(bool success, int balance, string reason)
        {
            Success = success;
            Balance = balance < 0 ? 0 : balance;
            Reason = reason ?? string.Empty;
        }

        public bool Success { get; }
        public int Balance { get; }
        public string Reason { get; }
    }
}
