using Game.Gameplay.Battle.Units;
using UnityEngine;

namespace Game.App.Content
{
    [CreateAssetMenu(menuName = "Game/Content/Unit Archetype Definition", fileName = "UnitArchetypeDefinition")]
    public sealed class UnitArchetypeDefinition : ScriptableObject
    {
        [SerializeField] private string _archetypeId = "archetype.basic";
        [SerializeField] private string _displayName = "Basic";
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private float _bodyHeight = 1f;
        [SerializeField] private float _walkBudget = 2.4f;
        [SerializeField] private float _walkSpeed = 2.9f;
        [SerializeField] private float _walkStepHeight = 0.42f;
        [SerializeField] private float _frontJumpDistance = 1.95f;
        [SerializeField] private float _frontJumpHeight = 1.15f;
        [SerializeField] private float _backJumpDistance = 1.15f;
        [SerializeField] private float _backJumpHeight = 1.4f;
        [SerializeField] private float _maxWalkSlopeDegrees = 60f;

        public string ArchetypeId => _archetypeId;
        public string DisplayName => _displayName;
        public int MaxHealth => Mathf.Max(1, _maxHealth);
        public float BodyHeight => Mathf.Max(0.2f, _bodyHeight);
        public float WalkBudget => Mathf.Max(0.5f, _walkBudget);
        public float WalkSpeed => Mathf.Max(0.5f, _walkSpeed);
        public float WalkStepHeight => Mathf.Max(0.15f, _walkStepHeight);
        public float FrontJumpDistance => Mathf.Max(0.5f, _frontJumpDistance);
        public float FrontJumpHeight => Mathf.Max(0.25f, _frontJumpHeight);
        public float BackJumpDistance => Mathf.Max(0.35f, _backJumpDistance);
        public float BackJumpHeight => Mathf.Max(0.25f, _backJumpHeight);
        public float MaxWalkSlopeDegrees => Mathf.Clamp(_maxWalkSlopeDegrees, 20f, 80f);

        public CharacterArchetype ToRuntime()
        {
            return new CharacterArchetype(
                ArchetypeId,
                DisplayName,
                MaxHealth,
                BodyHeight,
                WalkBudget,
                WalkSpeed,
                WalkStepHeight,
                FrontJumpDistance,
                FrontJumpHeight,
                BackJumpDistance,
                BackJumpHeight,
                MaxWalkSlopeDegrees);
        }

        public static UnitArchetypeDefinition CreateRuntime(
            string archetypeId,
            string displayName,
            int maxHealth,
            float bodyHeight,
            float walkBudget,
            float walkSpeed,
            float walkStepHeight,
            float frontJumpDistance,
            float frontJumpHeight,
            float backJumpDistance,
            float backJumpHeight,
            float maxWalkSlopeDegrees = 60f)
        {
            var definition = CreateInstance<UnitArchetypeDefinition>();
            definition._archetypeId = archetypeId ?? string.Empty;
            definition._displayName = displayName ?? string.Empty;
            definition._maxHealth = Mathf.Max(1, maxHealth);
            definition._bodyHeight = Mathf.Max(0.2f, bodyHeight);
            definition._walkBudget = Mathf.Max(0.5f, walkBudget);
            definition._walkSpeed = Mathf.Max(0.5f, walkSpeed);
            definition._walkStepHeight = Mathf.Max(0.15f, walkStepHeight);
            definition._frontJumpDistance = Mathf.Max(0.5f, frontJumpDistance);
            definition._frontJumpHeight = Mathf.Max(0.25f, frontJumpHeight);
            definition._backJumpDistance = Mathf.Max(0.35f, backJumpDistance);
            definition._backJumpHeight = Mathf.Max(0.25f, backJumpHeight);
            definition._maxWalkSlopeDegrees = Mathf.Clamp(maxWalkSlopeDegrees, 20f, 80f);
            return definition;
        }
    }
}
