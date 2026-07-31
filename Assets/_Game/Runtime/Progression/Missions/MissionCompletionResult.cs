namespace Game.Progression.Missions
{
    public readonly struct MissionCompletionResult
    {
        public MissionCompletionResult(
            bool applied,
            int grantedSoftCurrency,
            int grantedCommanderExperience,
            int grantedBlueprints,
            string blueprintWeaponId,
            string unlockedWeaponId,
            int highestUnlockedMissionIndex)
        {
            Applied = applied;
            GrantedSoftCurrency = grantedSoftCurrency < 0 ? 0 : grantedSoftCurrency;
            GrantedCommanderExperience = grantedCommanderExperience < 0 ? 0 : grantedCommanderExperience;
            GrantedBlueprints = grantedBlueprints < 0 ? 0 : grantedBlueprints;
            BlueprintWeaponId = blueprintWeaponId ?? string.Empty;
            UnlockedWeaponId = unlockedWeaponId ?? string.Empty;
            HighestUnlockedMissionIndex = highestUnlockedMissionIndex < 0 ? 0 : highestUnlockedMissionIndex;
        }

        public bool Applied { get; }
        public int GrantedSoftCurrency { get; }
        public int GrantedCommanderExperience { get; }
        public int GrantedBlueprints { get; }
        public string BlueprintWeaponId { get; }
        public string UnlockedWeaponId { get; }
        public int HighestUnlockedMissionIndex { get; }
        public bool HasUnlock => !string.IsNullOrEmpty(UnlockedWeaponId);
    }
}
