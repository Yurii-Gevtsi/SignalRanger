namespace Game.App
{
    /// <summary>
    /// Resolves the Workshop card interactions that are independent of Unity UI.
    /// </summary>
    public readonly struct WorkshopCardPresentationState
    {
        public WorkshopCardPresentationState(bool isInteractable, bool showsUnavailableExplanation,
            bool showsProgressScale)
        {
            IsInteractable = isInteractable;
            ShowsUnavailableExplanation = showsUnavailableExplanation;
            ShowsProgressScale = showsProgressScale;
        }

        public bool IsInteractable { get; }
        public bool ShowsUnavailableExplanation { get; }
        public bool ShowsProgressScale { get; }
    }

    public static class WorkshopCardPresentationStateResolver
    {
        public static WorkshopCardPresentationState Resolve(bool isSpecial, bool isAvailable,
            int specialPurchaseLimit)
        {
            var showsUnavailableExplanation = !isSpecial && !isAvailable;
            return new WorkshopCardPresentationState(
                isAvailable || showsUnavailableExplanation,
                showsUnavailableExplanation,
                !isSpecial ? isAvailable : isAvailable && specialPurchaseLimit > 1);
        }
    }
}
