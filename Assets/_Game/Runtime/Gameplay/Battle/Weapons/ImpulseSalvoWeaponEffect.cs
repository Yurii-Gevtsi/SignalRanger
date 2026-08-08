using UnityEngine;

namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>Fires the evolved impulse cannon as a deterministic timed salvo.</summary>
    public sealed class ImpulseSalvoWeaponEffect : IWeaponEffect
    {
        private const float MuzzleOffset = 0.45f;

        public void Fire(in WeaponFireContext context)
        {
            var weapon = context.Weapon;
            if (weapon == null || context.Spawner == null)
            {
                return;
            }

            var salvoCount = Mathf.Max(1, context.Evolution.SalvoCount);
            var knockbackMultiplier = context.Evolution.KnockbackMultiplier > 0f
                ? context.Evolution.KnockbackMultiplier
                : 1f;
            var modifiers = context.Modifiers;
            var delayedSpawner = context.Spawner as IDelayedProjectileSpawner;
            for (var shotIndex = 0; shotIndex < salvoCount; shotIndex++)
            {
                var request = new ProjectileSpawnRequest(
                    weapon.CreateProjectileDefinition(
                        Mathf.RoundToInt(weapon.Damage * modifiers.DamageMultiplier),
                        knockbackMultiplier,
                        impactRadiusOverride: weapon.ImpactRadius * modifiers.ImpactRadiusMultiplier),
                    context.OwnerUnitId,
                    context.Origin + context.Direction * MuzzleOffset,
                    context.Direction * context.Power,
                    weapon.ProjectileRadius * 2f,
                    ProjectileWeaponEffect.GetProjectileColor(weapon.Role));

                if (shotIndex == 0 || delayedSpawner == null)
                {
                    context.Spawner.Spawn(request);
                }
                else
                {
                    delayedSpawner.Schedule(request, shotIndex * context.Evolution.SalvoIntervalSeconds);
                }
            }
        }
    }
}
