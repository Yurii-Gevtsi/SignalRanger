using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>
    /// Runtime weapon selection and ammo state for one combatant.
    /// Pure logic: no MonoBehaviour, no scene, so it can be covered by edit-mode tests.
    /// </summary>
    public sealed class WeaponInventoryRuntime
    {
        private readonly List<WeaponDefinition> _weapons = new();
        private readonly Dictionary<string, int> _remainingAmmo = new();
        private int _selectedIndex;

        public WeaponInventoryRuntime(IEnumerable<WeaponDefinition> weapons, int startIndex = 0)
        {
            if (weapons != null)
            {
                foreach (var weapon in weapons)
                {
                    if (weapon == null || _remainingAmmo.ContainsKey(weapon.WeaponId))
                    {
                        continue;
                    }

                    _weapons.Add(weapon);
                    _remainingAmmo[weapon.WeaponId] = weapon.HasLimitedAmmo ? weapon.AmmoCount : -1;
                }
            }

            _selectedIndex = _weapons.Count == 0
                ? 0
                : (startIndex % _weapons.Count + _weapons.Count) % _weapons.Count;
        }

        public IReadOnlyList<WeaponDefinition> Weapons => _weapons;
        public int Count => _weapons.Count;
        public int SelectedIndex => _selectedIndex;
        public bool HasWeapons => _weapons.Count > 0;

        public WeaponDefinition Selected =>
            _weapons.Count == 0 ? null : _weapons[_selectedIndex];

        /// <summary>Selected weapon can fire when it has unlimited ammo or remaining rounds.</summary>
        public bool CanFireSelected => HasAmmo(Selected);

        public void Select(int index)
        {
            if (_weapons.Count == 0)
            {
                return;
            }

            _selectedIndex = (index % _weapons.Count + _weapons.Count) % _weapons.Count;
        }

        public void SelectNext() => Select(_selectedIndex + 1);

        public void SelectPrevious() => Select(_selectedIndex - 1);

        public bool TrySelectNextUsable()
        {
            if (_weapons.Count == 0)
            {
                return false;
            }

            if (CanFireSelected)
            {
                return true;
            }

            var startIndex = _selectedIndex;
            for (var offset = 1; offset < _weapons.Count + 1; offset++)
            {
                Select(startIndex + offset);
                if (CanFireSelected)
                {
                    return true;
                }
            }

            Select(startIndex);
            return false;
        }

        public int GetRemainingAmmo(WeaponDefinition weapon)
        {
            if (weapon == null || !_remainingAmmo.TryGetValue(weapon.WeaponId, out var ammo))
            {
                return 0;
            }

            return ammo;
        }

        public int GetSelectedRemainingAmmo() => GetRemainingAmmo(Selected);

        /// <summary>
        /// Applies a battle-only ammo override without changing the shared weapon definition.
        /// Used by workshop evolutions that expand a limited weapon's magazine.
        /// </summary>
        public void SetRemainingAmmo(WeaponDefinition weapon, int amount)
        {
            if (weapon == null || !weapon.HasLimitedAmmo || !_remainingAmmo.ContainsKey(weapon.WeaponId))
            {
                return;
            }

            _remainingAmmo[weapon.WeaponId] = Mathf.Max(0, amount);
        }

        /// <summary>
        /// Consumes one round of the selected weapon.
        /// Unlimited weapons always succeed; limited weapons decrement and fail at zero.
        /// </summary>
        public bool TryConsumeSelectedAmmo()
        {
            var weapon = Selected;
            if (weapon == null)
            {
                return false;
            }

            if (!weapon.HasLimitedAmmo)
            {
                return true;
            }

            var remaining = _remainingAmmo[weapon.WeaponId];
            if (remaining <= 0)
            {
                return false;
            }

            _remainingAmmo[weapon.WeaponId] = remaining - 1;
            return true;
        }

        private bool HasAmmo(WeaponDefinition weapon)
        {
            if (weapon == null)
            {
                return false;
            }

            return !weapon.HasLimitedAmmo || GetRemainingAmmo(weapon) > 0;
        }
    }
}
