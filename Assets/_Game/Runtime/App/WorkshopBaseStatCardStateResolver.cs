using System;

namespace Game.App
{
    /// <summary>
    /// Player-facing state of one base Workshop stat card. This deliberately keeps
    /// the stat's purchase rank separate from the weapon's overall rank.
    /// </summary>
    public readonly struct WorkshopBaseStatCardState
    {
        public WorkshopBaseStatCardState(int currentLevel, int nextLevel, bool isAtMaximum, bool isSupported)
        {
            CurrentLevel = currentLevel;
            NextLevel = nextLevel;
            IsAtMaximum = isAtMaximum;
            IsSupported = isSupported;
        }

        public int CurrentLevel { get; }
        public int NextLevel { get; }
        public bool IsAtMaximum { get; }
        public bool IsSupported { get; }
        public bool IsVisible => true;
        public bool IsDisabled => !IsSupported;
        public bool IsSelectable => IsSupported;
        public bool ShowsPurchaseControls => IsSupported && !IsAtMaximum;
    }

    public static class WorkshopBaseStatCardStateResolver
    {
        public static WorkshopBaseStatCardState Resolve(int purchasedLevels, int maximumLevels)
        {
            var safeMaximum = Math.Max(0, maximumLevels);
            var currentLevel = Math.Max(0, Math.Min(purchasedLevels, safeMaximum));
            var isAtMaximum = safeMaximum > 0 && currentLevel >= safeMaximum;
            return new WorkshopBaseStatCardState(
                currentLevel,
                isAtMaximum ? currentLevel : currentLevel + 1,
                isAtMaximum,
                safeMaximum > 0);
        }
    }
}
