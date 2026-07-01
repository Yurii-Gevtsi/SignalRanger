using UnityEngine;

namespace Game.Gameplay.Battle.Units
{
    /// <summary>
    /// A combatant template: how much health it brings and how tall its body reads.
    /// Pure data (no MonoBehaviour) so archetype selection can be edit-mode tested.
    /// </summary>
    public sealed class CharacterArchetype
    {
        public CharacterArchetype(string id, string displayName, int maxHealth, float bodyHeight)
        {
            Id = string.IsNullOrWhiteSpace(id) ? "archetype.unknown" : id;
            DisplayName = displayName ?? string.Empty;
            MaxHealth = Mathf.Max(1, maxHealth);
            BodyHeight = Mathf.Max(0.4f, bodyHeight);
        }

        public string Id { get; }
        public string DisplayName { get; }
        public int MaxHealth { get; }
        public float BodyHeight { get; }
    }
}
