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
        private const int IonShotgunPelletCount = 5;
        private const float IonShotgunSpreadDegrees = 8f;
        private const float IonShotgunTerrainImpactRadiusMultiplier = 1f / 3f;

        public void Fire(in WeaponFireContext context)
        {
            var weapon = context.Weapon;
            if (weapon == null || context.Spawner == null)
            {
                return;
            }

            if (weapon.WeaponId == "weapon.ion-shotgun")
            {
                FireIonShotgun(context);
                return;
            }

            var modifiers = context.Modifiers;
            var terrainBounceLimit = context.Evolution.TerrainBounceLimit;
            if (weapon.WeaponId == "weapon.titanium-disc")
            {
                terrainBounceLimit = Mathf.Max(3, terrainBounceLimit);
            }

            var request = new ProjectileSpawnRequest(
                weapon.CreateProjectileDefinition(
                    Mathf.RoundToInt(weapon.Damage * modifiers.DamageMultiplier),
                    impactRadiusOverride: weapon.ImpactRadius * modifiers.ImpactRadiusMultiplier,
                    terrainBounceLimit: terrainBounceLimit),
                context.OwnerUnitId,
                context.Origin + context.Direction * MuzzleOffset,
                context.Direction * context.Power,
                weapon.ProjectileRadius * 2f,
                GetProjectileColor(weapon.Role));

            context.Spawner.Spawn(request);
        }

        private static void FireIonShotgun(in WeaponFireContext context)
        {
            var weapon = context.Weapon;
            var modifiers = context.Modifiers;
            var totalDamage = Mathf.RoundToInt(weapon.Damage * modifiers.DamageMultiplier);
            var baseDamage = totalDamage / IonShotgunPelletCount;
            var remainder = totalDamage % IonShotgunPelletCount;
            var impactRadius = weapon.ImpactRadius
                * modifiers.ImpactRadiusMultiplier
                * IonShotgunTerrainImpactRadiusMultiplier;
            var baseAngle = Mathf.Atan2(context.Direction.y, context.Direction.x) * Mathf.Rad2Deg;
            var step = IonShotgunSpreadDegrees / (IonShotgunPelletCount - 1);

            for (var pelletIndex = 0; pelletIndex < IonShotgunPelletCount; pelletIndex++)
            {
                var angle = baseAngle - IonShotgunSpreadDegrees * 0.5f + step * pelletIndex;
                var angleRadians = angle * Mathf.Deg2Rad;
                var direction = new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));
                var damage = baseDamage + (pelletIndex < remainder ? 1 : 0);
                var request = new ProjectileSpawnRequest(
                    weapon.CreateProjectileDefinition(damage, impactRadiusOverride: impactRadius),
                    context.OwnerUnitId,
                    context.Origin + direction * MuzzleOffset,
                    direction * context.Power,
                    weapon.ProjectileRadius * 2f,
                    GetProjectileColor(weapon.Role));
                context.Spawner.Spawn(request);
            }
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
