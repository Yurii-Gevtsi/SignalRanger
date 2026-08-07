using System.Linq;
using Game.App.Content;
using Game.Core;
using Game.Data.Save;
using Game.Progression.Missions;

namespace Game.App
{
    public sealed class GameSessionService
    {
        private readonly SaveData _saveData;
        private readonly CampaignProgressionService _campaign;
        private readonly RuntimeGameContentCatalog _content;

        public GameSessionService(SaveData saveData, CampaignProgressionService campaign, RuntimeGameContentCatalog content)
        {
            _saveData = saveData ?? SaveData.CreateNew();
            _campaign = campaign;
            _content = content;
            SelectedMissionIndex = _saveData.campaign.highestUnlockedMissionIndex;
            PublishSelectedMission();
        }

        public int SelectedMissionIndex { get; private set; }
        public MissionDefinition SelectedMission => _campaign?.GetMission(SelectedMissionIndex);

        public void SelectNextMission()
        {
            SelectedMissionIndex = _saveData.campaign.highestUnlockedMissionIndex;
            PublishSelectedMission();
        }

        public void SelectMission(int missionIndex)
        {
            SelectedMissionIndex = missionIndex < 0 ? 0 : missionIndex;
            PublishSelectedMission();
        }

        public void SelectTrainingSkirmish()
        {
            SelectMission(0);
            GameSessionContext.ConfigureTrainingSkirmish();
        }

        private void PublishSelectedMission()
        {
            var mission = SelectedMission;
            var arenaIndex = _content?.ResolveArenaIndex(mission) ?? (mission != null ? mission.ArenaIndex : SelectedMissionIndex);
            var enemyLoadout = mission != null && _content != null && _content.TryGetEnemyLoadout(mission.EnemyLoadoutId, out var resolvedLoadout)
                ? resolvedLoadout
                : null;
            var difficultyProfile = mission != null && _content != null && _content.TryGetDifficultyProfile(mission.DifficultyProfileId, out var resolvedProfile)
                ? resolvedProfile
                : null;
            GameSessionContext.SelectMission(
                SelectedMissionIndex,
                arenaIndex,
                mission != null ? mission.MissionId : string.Empty,
                mission != null ? mission.ArenaId : string.Empty,
                mission != null ? mission.AllowedWeaponIds : null,
                mission != null ? mission.TutorialStepIds : null,
                mission != null ? mission.EnemyLoadoutId : string.Empty,
                enemyLoadout != null ? enemyLoadout.WeaponIds : null,
                mission != null ? mission.DifficultyProfileId : string.Empty,
                difficultyProfile != null ? difficultyProfile.OpponentArchetypeId : string.Empty,
                mission != null ? mission.Reward.softCurrency : 0,
                mission != null ? mission.Reward.unlockWeaponId : string.Empty,
                difficultyProfile != null ? difficultyProfile.AiDifficultyTier : mission != null ? mission.Difficulty : 0,
                mission == null || mission.SupportDropsEnabled);
            GameSessionContext.SelectLoadout(_saveData?.inventory?.selectedLoadoutWeaponIds?.ToArray());
        }
    }
}
