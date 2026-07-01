using System;
using System.Linq;
using Game.Data.Save;

namespace Game.Progression.Missions
{
    public sealed class CampaignProgressionService
    {
        private readonly SaveData _saveData;
        private readonly MissionDefinition[] _missions;

        public CampaignProgressionService(SaveData saveData, MissionDefinition[] missions)
        {
            _saveData = saveData ?? throw new ArgumentNullException(nameof(saveData));
            _saveData.EnsureDefaults();
            _missions = missions?.Where(mission => mission != null && mission.IsValid()).ToArray()
                ?? Array.Empty<MissionDefinition>();
        }

        public int MissionCount => _missions.Length;

        public MissionDefinition GetMission(int missionIndex)
        {
            if (_missions.Length == 0)
            {
                return null;
            }

            var clampedIndex = ClampMissionIndex(missionIndex);
            return _missions[clampedIndex];
        }

        public MissionDefinition GetNextMission()
        {
            return GetMission(_saveData.campaign.highestUnlockedMissionIndex);
        }

        public bool IsUnlocked(int missionIndex)
        {
            return missionIndex >= 0 && missionIndex <= _saveData.campaign.highestUnlockedMissionIndex && missionIndex < _missions.Length;
        }

        public MissionCompletionResult CompleteMission(int missionIndex)
        {
            var mission = GetMission(missionIndex);
            if (mission == null)
            {
                return new MissionCompletionResult(false, 0, string.Empty, _saveData.campaign.highestUnlockedMissionIndex);
            }

            var alreadyCompleted = _saveData.campaign.completedMissionIds.Contains(mission.MissionId);
            if (alreadyCompleted)
            {
                UnlockNextMission(missionIndex);
                return new MissionCompletionResult(false, 0, string.Empty, _saveData.campaign.highestUnlockedMissionIndex);
            }

            _saveData.campaign.completedMissionIds.Add(mission.MissionId);
            UnlockNextMission(missionIndex);

            var reward = mission.Reward;
            _saveData.economy.softCurrency += reward.softCurrency;

            var unlockedWeaponId = string.Empty;
            if (!string.IsNullOrWhiteSpace(reward.unlockWeaponId)
                && !_saveData.inventory.unlockedWeaponIds.Contains(reward.unlockWeaponId))
            {
                _saveData.inventory.unlockedWeaponIds.Add(reward.unlockWeaponId);
                unlockedWeaponId = reward.unlockWeaponId;
            }

            return new MissionCompletionResult(
                true,
                reward.softCurrency,
                unlockedWeaponId,
                _saveData.campaign.highestUnlockedMissionIndex);
        }

        private void UnlockNextMission(int missionIndex)
        {
            if (_missions.Length == 0)
            {
                return;
            }

            var nextIndex = Math.Min(missionIndex + 1, _missions.Length - 1);
            if (nextIndex > _saveData.campaign.highestUnlockedMissionIndex)
            {
                _saveData.campaign.highestUnlockedMissionIndex = nextIndex;
            }
        }

        private int ClampMissionIndex(int missionIndex)
        {
            if (missionIndex < 0)
            {
                return 0;
            }

            return missionIndex >= _missions.Length ? _missions.Length - 1 : missionIndex;
        }
    }
}
