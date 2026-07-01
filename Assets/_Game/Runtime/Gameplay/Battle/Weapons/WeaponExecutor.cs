using System.Collections.Generic;

namespace Game.Gameplay.Battle.Weapons
{
    public enum WeaponFireResult
    {
        Fired = 0,
        NoWeapon = 1,
        OutOfAmmo = 2
    }

    /// <summary>
    /// Fires the inventory's selected weapon: validates ammo, then dispatches to the
    /// effect registered for the weapon's role (falling back to the base projectile effect).
    /// Keeps the battle scene from knowing how any individual weapon behaves.
    /// </summary>
    public sealed class WeaponExecutor
    {
        private readonly Dictionary<WeaponRole, IWeaponEffect> _effectsByRole = new();
        private readonly IWeaponEffect _defaultEffect;

        public WeaponExecutor(IWeaponEffect defaultEffect = null)
        {
            _defaultEffect = defaultEffect ?? new ProjectileWeaponEffect();
        }

        public void RegisterEffect(WeaponRole role, IWeaponEffect effect)
        {
            if (effect == null)
            {
                return;
            }

            _effectsByRole[role] = effect;
        }

        public IWeaponEffect ResolveEffect(WeaponRole role)
        {
            return _effectsByRole.TryGetValue(role, out var effect) ? effect : _defaultEffect;
        }

        public WeaponFireResult TryFireSelected(WeaponInventoryRuntime inventory, in WeaponFireContext context)
        {
            var weapon = inventory?.Selected;
            if (weapon == null)
            {
                return WeaponFireResult.NoWeapon;
            }

            if (!inventory.TryConsumeSelectedAmmo())
            {
                return WeaponFireResult.OutOfAmmo;
            }

            ResolveEffect(weapon.Role).Fire(context);
            return WeaponFireResult.Fired;
        }
    }
}
