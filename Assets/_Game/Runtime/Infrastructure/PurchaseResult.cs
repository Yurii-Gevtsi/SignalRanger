namespace Game.Infrastructure
{
    public readonly struct PurchaseResult
    {
        public PurchaseResult(bool success, string productId, string message)
        {
            Success = success;
            ProductId = productId ?? string.Empty;
            Message = message ?? string.Empty;
        }

        public bool Success { get; }
        public string ProductId { get; }
        public string Message { get; }
    }
}
