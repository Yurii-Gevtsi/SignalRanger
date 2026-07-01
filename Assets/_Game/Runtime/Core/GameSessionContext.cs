namespace Game.Core
{
    public static class GameSessionContext
    {
        public static int SelectedMissionIndex { get; private set; }
        public static int SelectedArenaIndex { get; private set; }
        public static string SelectedMissionId { get; private set; } = string.Empty;
        public static string[] SelectedAllowedWeaponIds { get; private set; } = System.Array.Empty<string>();
        public static string[] SelectedLoadoutWeaponIds { get; private set; } = System.Array.Empty<string>();
        public static int SelectedMissionRewardSoftCurrency { get; private set; }
        public static string SelectedMissionRewardUnlockWeaponId { get; private set; } = string.Empty;
        public static bool HasPendingMissionCompletion { get; private set; }
        public static int PendingCompletedMissionIndex { get; private set; }

        public static void SelectMission(
            int missionIndex,
            int arenaIndex,
            string missionId,
            string[] allowedWeaponIds,
            int rewardSoftCurrency,
            string rewardUnlockWeaponId)
        {
            SelectedMissionIndex = missionIndex < 0 ? 0 : missionIndex;
            SelectedArenaIndex = arenaIndex < 0 ? 0 : arenaIndex;
            SelectedMissionId = missionId ?? string.Empty;
            SelectedAllowedWeaponIds = allowedWeaponIds ?? System.Array.Empty<string>();
            SelectedMissionRewardSoftCurrency = rewardSoftCurrency < 0 ? 0 : rewardSoftCurrency;
            SelectedMissionRewardUnlockWeaponId = rewardUnlockWeaponId ?? string.Empty;
        }

        public static void SelectLoadout(string[] loadoutWeaponIds)
        {
            SelectedLoadoutWeaponIds = loadoutWeaponIds ?? System.Array.Empty<string>();
        }

        public static void ReportMissionCompleted()
        {
            HasPendingMissionCompletion = true;
            PendingCompletedMissionIndex = SelectedMissionIndex;
        }

        public static int ConsumePendingMissionCompletion()
        {
            var missionIndex = PendingCompletedMissionIndex;
            HasPendingMissionCompletion = false;
            PendingCompletedMissionIndex = 0;
            return missionIndex;
        }
    }
}
