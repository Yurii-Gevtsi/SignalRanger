using UnityEngine;

namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>
    /// Role-specialised effect: fires a short burst of projectiles in a mini fan
    /// around the aim direction. Each projectile carries a fraction of the base
    /// damage so the total roughly matches a single shot but hits a wider area,
    /// giving <see cref="WeaponRole.ShortRangeBurst"/> weapons their own feel.
    /// </summary>
    public sealed class BurstProjectileEffect : IWeaponEffect
    {
        private const float MuzzleOffset = 0.45f;

        private readonly int _shotCount;
        private readonly float _spreadDegrees;

        public BurstProjectileEffect(int shotCount = 3, float spreadDegrees = 12f)
        {
            _shotCount = Mathf.Max(1, shotCount);
            _spreadDegrees = Mathf.Max(0f, spreadDegrees);
        }

        public void Fire(in WeaponFireContext context)
        {
            var weapon = context.Weapon;
            if (weapon == null || context.Spawner == null)
            {
                return;
            }

            var color = ProjectileWeaponEffect.GetProjectileColor(weapon.Role);
            var baseAngleDeg = Mathf.Atan2(context.Direction.y, context.Direction.x) * Mathf.Rad2Deg;
            var halfSpread = _spreadDegrees * 0.5f;
            var step = _shotCount > 1 ? _spreadDegrees / (_shotCount - 1) : 0f;
            var pelletDamage = Mathf.CeilToInt((float)weapon.Damage / _shotCount);

            for (var i = 0; i < _shotCount; i++)
            {
                var angleDeg = baseAngleDeg - halfSpread + step * i;
                var angleRad = angleDeg * Mathf.Deg2Rad;
                var direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

                var request = new ProjectileSpawnRequest(
                    weapon.CreateProjectileDefinition(pelletDamage),
                    context.OwnerUnitId,
                    context.Origin + direction * MuzzleOffset,
                    direction * context.Power,
                    weapon.ProjectileRadius * 2f,
                    color);

                context.Spawner.Spawn(request);
            }
        }
    }
}
