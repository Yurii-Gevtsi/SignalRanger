using System;
using System.Collections.Generic;
using System.Linq;
using Game.Data.Save;

namespace Game.Inventory
{
    public sealed class InventoryService
    {
        private readonly SaveData _saveData;

        public InventoryService(SaveData saveData)
        {
            _saveData = saveData ?? throw new ArgumentNullException(nameof(saveData));
            _saveData.EnsureDefaults();
        }

        public IReadOnlyList<string> UnlockedWeaponIds => _saveData.inventory.unlockedWeaponIds;
        public IReadOnlyList<string> SelectedLoadoutWeaponIds => _saveData.inventory.selectedLoadoutWeaponIds;

        public bool IsWeaponUnlocked(string weaponId)
        {
            return !string.IsNullOrWhiteSpace(weaponId)
                && _saveData.inventory.unlockedWeaponIds.Contains(weaponId);
        }

        public bool UnlockWeapon(string weaponId)
        {
            if (string.IsNullOrWhiteSpace(weaponId) || IsWeaponUnlocked(weaponId))
            {
                return false;
            }

            _saveData.inventory.unlockedWeaponIds.Add(weaponId);
            return true;
        }

        public bool TrySetLoadout(IEnumerable<string> weaponIds)
        {
            if (weaponIds == null)
            {
                return false;
            }

            var uniqueWeaponIds = weaponIds
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct()
                .ToArray();

            if (uniqueWeaponIds.Length == 0 || uniqueWeaponIds.Any(id => !IsWeaponUnlocked(id)))
            {
                return false;
            }

            _saveData.inventory.selectedLoadoutWeaponIds.Clear();
            _saveData.inventory.selectedLoadoutWeaponIds.AddRange(uniqueWeaponIds);
            return true;
        }
    }
}
