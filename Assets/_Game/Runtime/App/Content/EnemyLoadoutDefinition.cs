using System;
using UnityEngine;

namespace Game.App.Content
{
    [CreateAssetMenu(menuName = "Game/Content/Enemy Loadout Definition", fileName = "EnemyLoadoutDefinition")]
    public sealed class EnemyLoadoutDefinition : ScriptableObject
    {
        [SerializeField] private string _loadoutId = "enemy.basic";
        [SerializeField] private string _displayName = "Enemy Loadout";
        [SerializeField] private string[] _weaponIds;

        public string LoadoutId => _loadoutId;
        public string DisplayName => _displayName;
        public string[] WeaponIds => _weaponIds ?? Array.Empty<string>();

        public static EnemyLoadoutDefinition CreateRuntime(string loadoutId, string displayName, string[] weaponIds)
        {
            var definition = CreateInstance<EnemyLoadoutDefinition>();
            definition._loadoutId = loadoutId ?? string.Empty;
            definition._displayName = displayName ?? string.Empty;
            definition._weaponIds = weaponIds ?? Array.Empty<string>();
            return definition;
        }
    }
}
