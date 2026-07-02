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
    /// effect registered for the weapon id or role (falling back to the base projectile effect).
    /// Keeps the battle scene from knowing how any individual weapon behaves.
    /// </summary>
    public sealed class WeaponExecutor
    {
        private readonly Dictionary<WeaponRole, IWeaponEffect> _effectsByRole = new();
        private readonly Dictionary<string, IWeaponEffect> _effectsByWeaponId = new(System.StringComparer.Ordinal);
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

        public void RegisterEffect(string weaponId, IWeaponEffect effect)
        {
            if (string.IsNullOrWhiteSpace(weaponId) || effect == null)
            {
                return;
            }

            _effectsByWeaponId[weaponId] = effect;
        }

        public IWeaponEffect ResolveEffect(WeaponRole role)
        {
            return _effectsByRole.TryGetValue(role, out var effect) ? effect : _defaultEffect;
        }

        public IWeaponEffect ResolveEffect(WeaponDefinition weapon)
        {
            if (weapon != null
                && _effectsByWeaponId.TryGetValue(weapon.WeaponId, out var weaponEffect))
            {
                return weaponEffect;
            }

            return ResolveEffect(weapon?.Role ?? WeaponRole.DirectProjectile);
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

            ResolveEffect(weapon).Fire(context);
            return WeaponFireResult.Fired;
        }
    }
}
