namespace Game.App
{
    /// <summary>
    /// Determines whether the Workshop special card can be opened. A completed
    /// one-time special remains inspectable even when no further special step
    /// is available.
    /// </summary>
    public readonly struct WorkshopSpecialCardState
    {
        public WorkshopSpecialCardState(bool isAvailable, bool isPurchased)
        {
            IsAvailable = isAvailable;
            IsPurchased = isPurchased;
        }

        public bool IsAvailable { get; }
        public bool IsPurchased { get; }
    }

    public static class WorkshopSpecialCardStateResolver
    {
        public static WorkshopSpecialCardState Resolve(int playerRank, int nextPurchaseRank,
            bool hasPurchasedSpecial)
        {
            var isNextPurchaseAvailable = nextPurchaseRank > 0 && playerRank >= nextPurchaseRank;
            return new WorkshopSpecialCardState(isNextPurchaseAvailable || hasPurchasedSpecial,
                hasPurchasedSpecial);
        }
    }
}
