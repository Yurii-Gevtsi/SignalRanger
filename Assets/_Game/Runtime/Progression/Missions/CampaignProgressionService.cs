using System;
using System.Linq;
using Game.Data.Progression;
using Game.Data.Save;
using Game.Economy;

namespace Game.Progression.Missions
{
    public sealed class CampaignProgressionService
    {
        private readonly SaveData _saveData;
        private readonly MissionDefinition[] _missions;
        private readonly IEconomyService _economy;

        public CampaignProgressionService(SaveData saveData, MissionDefinition[] missions, IEconomyService economy = null)
        {
            _saveData = saveData ?? throw new ArgumentNullException(nameof(saveData));
            _saveData.EnsureDefaults();
            _missions = missions?.Where(mission => mission != null && mission.IsValid()).ToArray()
                ?? Array.Empty<MissionDefinition>();
            _economy = economy ?? new EconomyService(_saveData);
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
            if (missionIndex < 0 || missionIndex >= _missions.Length)
            {
                return false;
            }

            return _saveData.settings.adminModeEnabled
                || missionIndex <= _saveData.campaign.highestUnlockedMissionIndex;
        }

        public MissionCompletionResult CompleteMission(int missionIndex)
        {
            var mission = GetMission(missionIndex);
            if (mission == null)
            {
                return EmptyResult();
            }

            var alreadyCompleted = _saveData.campaign.completedMissionIds.Contains(mission.MissionId);
            if (alreadyCompleted)
            {
                UnlockNextMission(missionIndex);
                return EmptyResult();
            }

            _saveData.campaign.completedMissionIds.Add(mission.MissionId);
            UnlockNextMission(missionIndex);

            var reward = mission.Reward;
            // Детермінований transactionId: повторна фіксація тієї самої першої
            // перемоги не може продублювати нагороду навіть в обхід completedMissionIds.
            var progressionReward = new ProgressionReward(
                reward.softCurrency,
                reward.commanderExperience,
                reward.blueprints,
                reward.blueprintWeaponId);
            _economy.Grant(progressionReward, "mission.first", mission.MissionId,
                $"mission.first.{mission.MissionId}");

            var unlockedWeaponId = string.Empty;
            if (!string.IsNullOrWhiteSpace(reward.unlockWeaponId)
                && !_saveData.inventory.unlockedWeaponIds.Contains(reward.unlockWeaponId))
            {
                _saveData.inventory.unlockedWeaponIds.Add(reward.unlockWeaponId);
                _saveData.inventory.GetOrCreateWeaponProgress(reward.unlockWeaponId);
                unlockedWeaponId = reward.unlockWeaponId;
            }

            return new MissionCompletionResult(
                true,
                reward.softCurrency,
                reward.commanderExperience,
                reward.blueprints,
                reward.blueprintWeaponId,
                unlockedWeaponId,
                _saveData.campaign.highestUnlockedMissionIndex);
        }

        private MissionCompletionResult EmptyResult()
        {
            return new MissionCompletionResult(
                false,
                0,
                0,
                0,
                string.Empty,
                string.Empty,
                _saveData.campaign.highestUnlockedMissionIndex);
        }

        private void UnlockNextMission(int missionIndex)
        {
            if (_missions.Length == 0)
            {
                return;
            }

            var nextIndex = Math.Min(missionIndex + 1, _missions.Length - 1);
            // Easy is the campaign route mission for a planet. Completing it opens
            // the next planet's easy mission while leaving this planet's medium and
            // hard missions available as optional challenges.
            if (_missions[missionIndex].Difficulty == 0)
            {
                for (var i = missionIndex + 1; i < _missions.Length; i++)
                {
                    if (_missions[i].Difficulty == 0)
                    {
                        nextIndex = i;
                        break;
                    }
                }
            }

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
