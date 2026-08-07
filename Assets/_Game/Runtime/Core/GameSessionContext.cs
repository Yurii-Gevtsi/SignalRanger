namespace Game.Core
{
    public static class GameSessionContext
    {
        public static int SelectedMissionIndex { get; private set; }
        public static int SelectedArenaIndex { get; private set; }
        public static string SelectedMissionId { get; private set; } = string.Empty;
        public static string SelectedArenaId { get; private set; } = string.Empty;
        public static string[] SelectedAllowedWeaponIds { get; private set; } = System.Array.Empty<string>();
        public static string[] SelectedTutorialStepIds { get; private set; } = System.Array.Empty<string>();
        public static string SelectedEnemyLoadoutId { get; private set; } = string.Empty;
        public static string[] SelectedOpponentWeaponIds { get; private set; } = System.Array.Empty<string>();
        public static string SelectedDifficultyProfileId { get; private set; } = string.Empty;
        public static string SelectedOpponentArchetypeId { get; private set; } = string.Empty;
        public static string[] SelectedLoadoutWeaponIds { get; private set; } = System.Array.Empty<string>();
        public static int SelectedMissionRewardSoftCurrency { get; private set; }
        public static string SelectedMissionRewardUnlockWeaponId { get; private set; } = string.Empty;
        public static int SelectedAiDifficulty { get; private set; }
        public static bool IsTrainingSkirmish { get; private set; }
        public static bool SupportDropsEnabled { get; private set; } = true;
        public static bool HasPendingMissionCompletion { get; private set; }
        public static int PendingCompletedMissionIndex { get; private set; }

        public static void SelectMission(
            int missionIndex,
            int arenaIndex,
            string missionId,
            string arenaId,
            string[] allowedWeaponIds,
            string[] tutorialStepIds,
            string enemyLoadoutId,
            string[] opponentWeaponIds,
            string difficultyProfileId,
            string opponentArchetypeId,
            int rewardSoftCurrency,
            string rewardUnlockWeaponId,
            int aiDifficulty,
            bool supportDropsEnabled = true)
        {
            SelectedMissionIndex = missionIndex < 0 ? 0 : missionIndex;
            SelectedArenaIndex = arenaIndex < 0 ? 0 : arenaIndex;
            SelectedMissionId = missionId ?? string.Empty;
            SelectedArenaId = arenaId ?? string.Empty;
            SelectedAllowedWeaponIds = allowedWeaponIds ?? System.Array.Empty<string>();
            SelectedTutorialStepIds = tutorialStepIds ?? System.Array.Empty<string>();
            SelectedEnemyLoadoutId = enemyLoadoutId ?? string.Empty;
            SelectedOpponentWeaponIds = opponentWeaponIds ?? System.Array.Empty<string>();
            SelectedDifficultyProfileId = difficultyProfileId ?? string.Empty;
            SelectedOpponentArchetypeId = opponentArchetypeId ?? string.Empty;
            SelectedMissionRewardSoftCurrency = rewardSoftCurrency < 0 ? 0 : rewardSoftCurrency;
            SelectedMissionRewardUnlockWeaponId = rewardUnlockWeaponId ?? string.Empty;
            SelectedAiDifficulty = aiDifficulty < 0 ? 0 : aiDifficulty > 2 ? 2 : aiDifficulty;
            SupportDropsEnabled = supportDropsEnabled;
            IsTrainingSkirmish = false;
        }

        public static void ConfigureTrainingSkirmish()
        {
            IsTrainingSkirmish = true;
            SelectedDifficultyProfileId = "difficulty.normal";
            SelectedOpponentArchetypeId = "archetype.striker";
            SelectedAiDifficulty = 1;
            SupportDropsEnabled = true;
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
