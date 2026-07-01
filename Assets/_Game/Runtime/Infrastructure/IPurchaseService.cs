namespace Game.Infrastructure
{
    public interface IPurchaseService
    {
        bool IsAvailable { get; }
        PurchaseResult Purchase(string productId);
    }
}
