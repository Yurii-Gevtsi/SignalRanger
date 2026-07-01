using UnityEngine;

namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>
    /// Base weapon effect for the MVP: launches a single ballistic projectile built
    /// from the weapon's own definition. Used by every starter weapon until specialised
    /// effects (cover, defensive, movement) are added in later phases.
    /// </summary>
    public sealed class ProjectileWeaponEffect : IWeaponEffect
    {
        private const float MuzzleOffset = 0.45f;

        public void Fire(in WeaponFireContext context)
        {
            var weapon = context.Weapon;
            if (weapon == null || context.Spawner == null)
            {
                return;
            }

            var request = new ProjectileSpawnRequest(
                weapon.CreateProjectileDefinition(),
                context.OwnerUnitId,
                context.Origin + context.Direction * MuzzleOffset,
                context.Direction * context.Power,
                weapon.ProjectileRadius * 2f,
                GetProjectileColor(weapon.Role),
                context.OwnerCollider);

            context.Spawner.Spawn(request);
        }

        public static Color GetProjectileColor(WeaponRole role)
        {
            return role switch
            {
                WeaponRole.DirectProjectile => new Color(0.12f, 0.18f, 0.24f),
                WeaponRole.ArcingExplosive => new Color(0.11f, 0.09f, 0.08f),
                WeaponRole.ShortRangeBurst => new Color(0.48f, 0.16f, 0.12f),
                WeaponRole.CoverTool => new Color(0.25f, 0.42f, 0.32f),
                WeaponRole.DefensiveTool => new Color(0.18f, 0.34f, 0.72f),
                WeaponRole.MovementTool => new Color(0.52f, 0.28f, 0.72f),
                _ => Color.black
            };
        }
    }
}
