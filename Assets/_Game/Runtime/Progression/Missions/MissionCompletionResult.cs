namespace Game.Progression.Missions
{
    public readonly struct MissionCompletionResult
    {
        public MissionCompletionResult(
            bool applied,
            int grantedSoftCurrency,
            string unlockedWeaponId,
            int highestUnlockedMissionIndex)
        {
            Applied = applied;
            GrantedSoftCurrency = grantedSoftCurrency < 0 ? 0 : grantedSoftCurrency;
            UnlockedWeaponId = unlockedWeaponId ?? string.Empty;
            HighestUnlockedMissionIndex = highestUnlockedMissionIndex < 0 ? 0 : highestUnlockedMissionIndex;
        }

        public bool Applied { get; }
        public int GrantedSoftCurrency { get; }
        public string UnlockedWeaponId { get; }
        public int HighestUnlockedMissionIndex { get; }
        public bool HasUnlock => !string.IsNullOrEmpty(UnlockedWeaponId);
    }
}
