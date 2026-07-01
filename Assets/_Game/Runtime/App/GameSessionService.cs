using System.Linq;
using Game.Core;
using Game.Data.Save;
using Game.Progression.Missions;

namespace Game.App
{
    public sealed class GameSessionService
    {
        private readonly SaveData _saveData;
        private readonly CampaignProgressionService _campaign;

        public GameSessionService(SaveData saveData, CampaignProgressionService campaign)
        {
            _saveData = saveData ?? SaveData.CreateNew();
            _campaign = campaign;
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

        private void PublishSelectedMission()
        {
            var mission = SelectedMission;
            GameSessionContext.SelectMission(
                SelectedMissionIndex,
                mission != null ? mission.ArenaIndex : SelectedMissionIndex,
                mission != null ? mission.MissionId : string.Empty,
                mission != null ? mission.AllowedWeaponIds : null,
                mission != null ? mission.Reward.softCurrency : 0,
                mission != null ? mission.Reward.unlockWeaponId : string.Empty,
                mission != null ? mission.Difficulty : 0);
            GameSessionContext.SelectLoadout(_saveData?.inventory?.selectedLoadoutWeaponIds?.ToArray());
        }
    }
}
