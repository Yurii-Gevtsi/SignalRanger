using UnityEngine;

namespace Game.Gameplay.Battle.Units
{
    /// <summary>
    /// A combatant template: how much health it brings and how tall its body reads.
    /// Pure data (no MonoBehaviour) so archetype selection can be edit-mode tested.
    /// </summary>
    public sealed class CharacterArchetype
    {
        public CharacterArchetype(
            string id,
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
            Id = string.IsNullOrWhiteSpace(id) ? "archetype.unknown" : id;
            DisplayName = displayName ?? string.Empty;
            MaxHealth = Mathf.Max(1, maxHealth);
            BodyHeight = Mathf.Max(0.4f, bodyHeight);
            WalkBudget = Mathf.Max(0.5f, walkBudget);
            WalkSpeed = Mathf.Max(0.5f, walkSpeed);
            WalkStepHeight = Mathf.Max(0.15f, walkStepHeight);
            FrontJumpDistance = Mathf.Max(0.5f, frontJumpDistance);
            FrontJumpHeight = Mathf.Max(0.25f, frontJumpHeight);
            BackJumpDistance = Mathf.Max(0.35f, backJumpDistance);
            BackJumpHeight = Mathf.Max(0.25f, backJumpHeight);
            MaxWalkSlopeDegrees = Mathf.Clamp(maxWalkSlopeDegrees, 20f, 80f);
        }

        public string Id { get; }
        public string DisplayName { get; }
        public int MaxHealth { get; }
        public float BodyHeight { get; }
        public float WalkBudget { get; }
        public float WalkSpeed { get; }
        public float WalkStepHeight { get; }
        public float FrontJumpDistance { get; }
        public float FrontJumpHeight { get; }
        public float BackJumpDistance { get; }
        public float BackJumpHeight { get; }

        /// <summary>Steepest sustained incline this archetype can walk up.</summary>
        public float MaxWalkSlopeDegrees { get; }
    }
}
