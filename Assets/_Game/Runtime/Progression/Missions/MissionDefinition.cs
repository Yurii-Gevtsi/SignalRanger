using System;
using UnityEngine;

namespace Game.Progression.Missions
{
    [CreateAssetMenu(menuName = "Game/Progression/Mission Definition", fileName = "MissionDefinition")]
    public sealed class MissionDefinition : ScriptableObject
    {
        [SerializeField] private string _missionId;
        [SerializeField] private string _chapterId;
        [SerializeField] private string _displayNameKey;
        [SerializeField] private string _descriptionKey;
        [SerializeField] private string _title;
        [SerializeField] private string _description;
        [SerializeField] private int _arenaIndex;
        [SerializeField] private string _arenaId;
        [SerializeField] private string _enemyLoadoutId;
        [SerializeField] private string _difficultyProfileId;
        [SerializeField] private string _playerTeamPresetId;
        [SerializeField] private string _winConditionId = "win.eliminate-all";
        [SerializeField] private string[] _allowedWeaponIds;
        [SerializeField] private string[] _tutorialStepIds;
        [SerializeField] private MissionRewardDefinition _reward;
        [SerializeField] private int _difficulty;
        [SerializeField] private bool _supportDropsEnabled = true;
        [SerializeField] private MissionObjectiveKind _objectiveKind;
        [SerializeField] private int _objectiveThreshold;

        public string MissionId => _missionId;
        public string ChapterId => _chapterId;
        public string DisplayNameKey => _displayNameKey;
        public string DescriptionKey => _descriptionKey;
        public string Title => _title;
        public string Description => _description;
        public int ArenaIndex => _arenaIndex < 0 ? 0 : _arenaIndex;
        public string ArenaId => _arenaId;
        public string EnemyLoadoutId => _enemyLoadoutId;
        public string DifficultyProfileId => _difficultyProfileId;
        public string PlayerTeamPresetId => _playerTeamPresetId;
        public string WinConditionId => string.IsNullOrWhiteSpace(_winConditionId) ? "win.eliminate-all" : _winConditionId;
        public string[] AllowedWeaponIds => _allowedWeaponIds ?? Array.Empty<string>();
        public string[] TutorialStepIds => _tutorialStepIds ?? Array.Empty<string>();
        public MissionRewardDefinition Reward => _reward ?? new MissionRewardDefinition(0);

        /// <summary>AI difficulty tier for this mission: 0 = easy, 1 = normal, 2 = hard.</summary>
        public int Difficulty => Mathf.Clamp(_difficulty, 0, 2);
        public bool SupportDropsEnabled => _supportDropsEnabled;

        /// <summary>Extra condition beyond winning; <see cref="MissionObjectiveKind.None"/> means a plain win.</summary>
        public MissionObjective Objective => new(_objectiveKind, _objectiveThreshold);

        public static MissionDefinition CreateRuntime(
            string missionId,
            string title,
            string description,
            int arenaIndex,
            string[] allowedWeaponIds,
            string[] tutorialStepIds,
            MissionRewardDefinition reward,
            int difficulty = 0,
            string chapterId = "",
            string displayNameKey = "",
            string descriptionKey = "",
            string arenaId = "",
            string enemyLoadoutId = "",
            string difficultyProfileId = "",
            string playerTeamPresetId = "",
            string winConditionId = "win.eliminate-all",
            bool supportDropsEnabled = true,
            MissionObjectiveKind objectiveKind = MissionObjectiveKind.None,
            int objectiveThreshold = 0)
        {
            var definition = CreateInstance<MissionDefinition>();
            definition._missionId = missionId ?? string.Empty;
            definition._chapterId = chapterId ?? string.Empty;
            definition._displayNameKey = string.IsNullOrWhiteSpace(displayNameKey)
                ? $"{definition._missionId}.name"
                : displayNameKey;
            definition._descriptionKey = string.IsNullOrWhiteSpace(descriptionKey)
                ? $"{definition._missionId}.description"
                : descriptionKey;
            definition._title = title ?? string.Empty;
            definition._description = description ?? string.Empty;
            definition._arenaIndex = arenaIndex < 0 ? 0 : arenaIndex;
            definition._arenaId = arenaId ?? string.Empty;
            definition._enemyLoadoutId = enemyLoadoutId ?? string.Empty;
            definition._difficultyProfileId = difficultyProfileId ?? string.Empty;
            definition._playerTeamPresetId = playerTeamPresetId ?? string.Empty;
            definition._winConditionId = string.IsNullOrWhiteSpace(winConditionId) ? "win.eliminate-all" : winConditionId;
            definition._allowedWeaponIds = allowedWeaponIds ?? Array.Empty<string>();
            definition._tutorialStepIds = tutorialStepIds ?? Array.Empty<string>();
            definition._reward = reward ?? new MissionRewardDefinition(0);
            definition._difficulty = Mathf.Clamp(difficulty, 0, 2);
            definition._supportDropsEnabled = supportDropsEnabled;
            definition._objectiveKind = objectiveKind;
            definition._objectiveThreshold = objectiveThreshold;
            return definition;
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(_missionId)
                && !string.IsNullOrWhiteSpace(_chapterId)
                && !string.IsNullOrWhiteSpace(_displayNameKey)
                && !string.IsNullOrWhiteSpace(_descriptionKey)
                && !string.IsNullOrWhiteSpace(_arenaId)
                && !string.IsNullOrWhiteSpace(_enemyLoadoutId)
                && !string.IsNullOrWhiteSpace(_difficultyProfileId)
                && _reward != null;
        }
    }
}
