using Game.Core.Modes;

namespace Game.Core
{
    public static class GameSessionContext
    {
        public static int SelectedMissionIndex { get; private set; }
        public static int SelectedArenaIndex { get; private set; }
        public static string SelectedMissionId { get; private set; } = string.Empty;

        /// <summary>Briefing line for the mission, shown on the battle result card.</summary>
        public static string SelectedMissionText { get; private set; } = string.Empty;
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

        /// <summary>Rank per opponent weapon, aligned by index with <see cref="SelectedOpponentWeaponIds"/>.</summary>
        public static int[] SelectedOpponentWeaponRanks { get; private set; } = System.Array.Empty<int>();

        /// <summary>Evolution branch per opponent weapon, aligned the same way; 0 below rank 5.</summary>
        public static int[] SelectedOpponentWeaponBranches { get; private set; } = System.Array.Empty<int>();

        /// <summary>Which mode the next battle belongs to; the single source of truth.</summary>
        public static BattleMode SelectedBattleMode { get; private set; } = BattleMode.Story;

        /// <summary>Opponent shown on the duel card; null outside <see cref="BattleMode.Pvp"/>.</summary>
        public static PvpOpponentProfile SelectedPvpOpponent { get; private set; }

        /// <summary>Shared match seed for the duel; empty outside <see cref="BattleMode.Pvp"/>.</summary>
        public static string SelectedMatchId { get; private set; } = string.Empty;

        /// <summary>This commander's id, so the battle knows which units it may drive.</summary>
        public static string LocalPlayerId { get; private set; } = string.Empty;

        public static bool IsTrainingSkirmish => SelectedBattleMode == BattleMode.Training;
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
            bool supportDropsEnabled = true,
            string missionText = "")
        {
            SelectedMissionText = missionText ?? string.Empty;
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
            SelectedAiDifficulty = ClampDifficulty(aiDifficulty);
            SelectedOpponentWeaponRanks = System.Array.Empty<int>();
            SelectedOpponentWeaponBranches = System.Array.Empty<int>();
            SupportDropsEnabled = supportDropsEnabled;
            SelectedPvpOpponent = null;
            SelectedMatchId = string.Empty;
            SelectedBattleMode = BattleMode.Story;
        }

        public static void ConfigureTrainingSkirmish()
        {
            SelectedBattleMode = BattleMode.Training;
            SelectedDifficultyProfileId = "difficulty.normal";
            SelectedOpponentArchetypeId = "archetype.striker";
            SelectedAiDifficulty = 1;
            SupportDropsEnabled = true;
            SelectedPvpOpponent = null;
            SelectedMatchId = string.Empty;
        }

        /// <summary>
        /// Configures an offline raid on a liberated arena. Raids carry no mission,
        /// so the mission id is deliberately cleared - anything asking "is this a
        /// story battle" must read the mode, not the presence of an id.
        /// </summary>
        public static void ConfigureRaid(
            int arenaIndex,
            string arenaId,
            string difficultyProfileId,
            string opponentArchetypeId,
            string[] opponentWeaponIds,
            int[] opponentWeaponRanks,
            int[] opponentWeaponBranches,
            int aiDifficulty)
        {
            ClearMissionSelection();
            SelectedBattleMode = BattleMode.Raid;
            SelectedArenaIndex = arenaIndex < 0 ? 0 : arenaIndex;
            SelectedArenaId = arenaId ?? string.Empty;
            SelectedDifficultyProfileId = difficultyProfileId ?? string.Empty;
            SelectedOpponentArchetypeId = opponentArchetypeId ?? string.Empty;
            SelectedOpponentWeaponIds = opponentWeaponIds ?? System.Array.Empty<string>();
            SelectedOpponentWeaponRanks = opponentWeaponRanks ?? System.Array.Empty<int>();
            SelectedOpponentWeaponBranches = opponentWeaponBranches ?? System.Array.Empty<int>();
            SelectedAiDifficulty = ClampDifficulty(aiDifficulty);
            SupportDropsEnabled = BattleMode.Raid.AllowsSupportDrops();
        }

        /// <summary>
        /// Configures a duel against a matched commander. Support drops stay off:
        /// handing one side a random weapon mid-fight is not a fair 1v1.
        /// </summary>
        public static void ConfigurePvpMatch(
            int arenaIndex,
            string arenaId,
            string matchId,
            string localPlayerId,
            PvpOpponentProfile opponent,
            string opponentArchetypeId,
            string[] opponentWeaponIds,
            int[] opponentWeaponRanks,
            int[] opponentWeaponBranches,
            int aiDifficulty)
        {
            ClearMissionSelection();
            SelectedBattleMode = BattleMode.Pvp;
            SelectedArenaIndex = arenaIndex < 0 ? 0 : arenaIndex;
            SelectedArenaId = arenaId ?? string.Empty;
            SelectedMatchId = matchId ?? string.Empty;
            LocalPlayerId = localPlayerId ?? string.Empty;
            SelectedPvpOpponent = opponent;
            SelectedDifficultyProfileId = string.Empty;
            SelectedOpponentArchetypeId = opponentArchetypeId ?? string.Empty;
            SelectedOpponentWeaponIds = opponentWeaponIds ?? System.Array.Empty<string>();
            SelectedOpponentWeaponRanks = opponentWeaponRanks ?? System.Array.Empty<int>();
            SelectedOpponentWeaponBranches = opponentWeaponBranches ?? System.Array.Empty<int>();
            SelectedAiDifficulty = ClampDifficulty(aiDifficulty);
            SupportDropsEnabled = BattleMode.Pvp.AllowsSupportDrops();
        }

        private static void ClearMissionSelection()
        {
            SelectedMissionIndex = 0;
            SelectedMissionId = string.Empty;
            SelectedMissionText = string.Empty;
            SelectedAllowedWeaponIds = System.Array.Empty<string>();
            SelectedTutorialStepIds = System.Array.Empty<string>();
            SelectedEnemyLoadoutId = string.Empty;
            SelectedOpponentWeaponBranches = System.Array.Empty<int>();
            SelectedMissionRewardSoftCurrency = 0;
            SelectedMissionRewardUnlockWeaponId = string.Empty;
            SelectedPvpOpponent = null;
            SelectedMatchId = string.Empty;
        }

        private static int ClampDifficulty(int aiDifficulty)
        {
            return aiDifficulty < 0 ? 0 : aiDifficulty > 2 ? 2 : aiDifficulty;
        }

        /// <summary>
        /// Training reuses a mission loadout but owns its arena explicitly, so
        /// campaign arena assignments cannot redirect the training mode.
        /// </summary>
        public static void SelectArena(int arenaIndex, string arenaId)
        {
            SelectedArenaIndex = arenaIndex < 0 ? 0 : arenaIndex;
            SelectedArenaId = arenaId ?? string.Empty;
        }

        public static void SelectLoadout(string[] loadoutWeaponIds)
        {
            SelectedLoadoutWeaponIds = loadoutWeaponIds ?? System.Array.Empty<string>();
        }

        /// <summary>
        /// Mission whose planet the campaign map should open on the next visit. Set when
        /// the player leaves a finished battle through "continue", so they land on the
        /// planet card and can see what they just cleared and what comes next. Carries
        /// the mission id rather than a planet index: mapping missions onto planets is
        /// the app layer's business, not the battle layer's.
        /// </summary>
        public static string PendingCampaignMissionId { get; private set; } = string.Empty;

        public static void RequestCampaignMissionView(string missionId)
        {
            PendingCampaignMissionId = missionId ?? string.Empty;
        }

        public static string ConsumePendingCampaignMissionView()
        {
            var missionId = PendingCampaignMissionId;
            PendingCampaignMissionId = string.Empty;
            return missionId;
        }

        /// <summary>
        /// How the last battle actually ended, for judging the mission's objective once
        /// the menu scene banks it. Kept as plain values rather than a progression type
        /// so the battle layer stays free of the progression assembly.
        /// </summary>
        public static bool LastBattlePlayerWon { get; private set; }
        public static int LastBattleHealthPercent { get; private set; }
        public static bool LastBattleOpponentRingOut { get; private set; }

        public static void ReportBattleFacts(bool playerWon, int healthPercent, bool opponentRingOut)
        {
            LastBattlePlayerWon = playerWon;
            LastBattleHealthPercent = healthPercent < 0 ? 0 : healthPercent > 100 ? 100 : healthPercent;
            LastBattleOpponentRingOut = opponentRingOut;
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
