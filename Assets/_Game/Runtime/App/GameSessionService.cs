using Game.Data.Save;

namespace Game.App
{
    public sealed class GameSessionService
    {
        private readonly SaveData _saveData;

        public GameSessionService(SaveData saveData)
        {
            _saveData = saveData ?? SaveData.CreateNew();
            SelectedMissionIndex = _saveData.campaign.highestUnlockedMissionIndex;
        }

        public int SelectedMissionIndex { get; private set; }

        public void SelectNextMission()
        {
            SelectedMissionIndex = _saveData.campaign.highestUnlockedMissionIndex;
        }
    }
}
