using System;
using UnityEngine;

namespace Game.Progression.Missions
{
    [CreateAssetMenu(menuName = "Game/Progression/Mission Definition", fileName = "MissionDefinition")]
    public sealed class MissionDefinition : ScriptableObject
    {
        [SerializeField] private string _missionId;
        [SerializeField] private string _title;
        [SerializeField] private string _description;
        [SerializeField] private int _arenaIndex;
        [SerializeField] private string[] _allowedWeaponIds;
        [SerializeField] private string[] _tutorialStepIds;
        [SerializeField] private MissionRewardDefinition _reward;

        public string MissionId => _missionId;
        public string Title => _title;
        public string Description => _description;
        public int ArenaIndex => _arenaIndex < 0 ? 0 : _arenaIndex;
        public string[] AllowedWeaponIds => _allowedWeaponIds ?? Array.Empty<string>();
        public string[] TutorialStepIds => _tutorialStepIds ?? Array.Empty<string>();
        public MissionRewardDefinition Reward => _reward ?? new MissionRewardDefinition(0);

        public static MissionDefinition CreateRuntime(
            string missionId,
            string title,
            string description,
            int arenaIndex,
            string[] allowedWeaponIds,
            string[] tutorialStepIds,
            MissionRewardDefinition reward)
        {
            var definition = CreateInstance<MissionDefinition>();
            definition._missionId = missionId ?? string.Empty;
            definition._title = title ?? string.Empty;
            definition._description = description ?? string.Empty;
            definition._arenaIndex = arenaIndex < 0 ? 0 : arenaIndex;
            definition._allowedWeaponIds = allowedWeaponIds ?? Array.Empty<string>();
            definition._tutorialStepIds = tutorialStepIds ?? Array.Empty<string>();
            definition._reward = reward ?? new MissionRewardDefinition(0);
            return definition;
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(_missionId)
                && !string.IsNullOrWhiteSpace(_title)
                && _reward != null;
        }
    }
}
