using System;
using UnityEngine;

namespace Game.App.Content
{
    [CreateAssetMenu(menuName = "Game/Content/AI Profile Definition", fileName = "AiProfileDefinition")]
    public sealed class AiProfileDefinition : ScriptableObject
    {
        [SerializeField] private string _profileId = "ai.basic";
        [SerializeField] private float _aimErrorMin;
        [SerializeField] private float _aimErrorMax = 4f;
        [SerializeField] private string[] _preferredWeaponTags;
        [SerializeField] private float _aggression = 0.5f;
        [SerializeField] private float _defensiveChance = 0.15f;
        [SerializeField] private float _healThreshold = 0.35f;
        [SerializeField] private float _coverChance = 0.2f;
        [SerializeField] private float _movementChance = 0.1f;
        [SerializeField] private float _maxThinkingDelay = 0.55f;

        public string ProfileId => _profileId;
        public float AimErrorMin => Mathf.Min(_aimErrorMin, _aimErrorMax);
        public float AimErrorMax => Mathf.Max(_aimErrorMin, _aimErrorMax);
        public string[] PreferredWeaponTags => _preferredWeaponTags ?? Array.Empty<string>();
        public float Aggression => Mathf.Clamp01(_aggression);
        public float DefensiveChance => Mathf.Clamp01(_defensiveChance);
        public float HealThreshold => Mathf.Clamp01(_healThreshold);
        public float CoverChance => Mathf.Clamp01(_coverChance);
        public float MovementChance => Mathf.Clamp01(_movementChance);
        public float MaxThinkingDelay => Mathf.Max(0f, _maxThinkingDelay);

        public static AiProfileDefinition CreateRuntime(
            string profileId,
            float aimErrorMin,
            float aimErrorMax,
            string[] preferredWeaponTags,
            float aggression,
            float defensiveChance,
            float healThreshold,
            float coverChance,
            float movementChance,
            float maxThinkingDelay)
        {
            var definition = CreateInstance<AiProfileDefinition>();
            definition._profileId = profileId ?? string.Empty;
            definition._aimErrorMin = aimErrorMin;
            definition._aimErrorMax = aimErrorMax;
            definition._preferredWeaponTags = preferredWeaponTags ?? Array.Empty<string>();
            definition._aggression = aggression;
            definition._defensiveChance = defensiveChance;
            definition._healThreshold = healThreshold;
            definition._coverChance = coverChance;
            definition._movementChance = movementChance;
            definition._maxThinkingDelay = maxThinkingDelay;
            return definition;
        }
    }
}
