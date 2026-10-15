namespace Game.Infrastructure
{
    public sealed class NoOpPurchaseService : IPurchaseService
    {
        public bool IsAvailable => false;

        public PurchaseResult Purchase(string productId)
        {
            return new PurchaseResult(false, productId, "Purchases are not available in this build.");
        }
    }
}
