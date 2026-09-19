using Game.Data.Progression;
using Game.Data.Save;

namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>
    /// Runtime multipliers derived from the player's explicit four-path choices.
    /// Percent-based so <c>default</c> naturally means "no bonus" - unlike
    /// <see cref="WeaponEvolutionProfile"/>, whose fields are multipliers/counts and
    /// therefore need an explicit <c>Base</c> value instead of relying on
    /// <c>default</c>.
    /// </summary>
    public readonly struct WeaponUpgradeModifiers
    {
        public WeaponUpgradeModifiers(int damagePercent, int impactRadiusPercent)
        {
            DamagePercent = damagePercent;
            ImpactRadiusPercent = impactRadiusPercent;
        }

        public int DamagePercent { get; }
        public int ImpactRadiusPercent { get; }

        public float DamageMultiplier => 1f + DamagePercent / 100f;
        public float ImpactRadiusMultiplier => 1f + ImpactRadiusPercent / 100f;
    }

    /// <summary>
    /// Resolves the Damage/ImpactRadius choices purchased up to the current rank.
    /// Separate from
    /// <see cref="WeaponEvolutionRuntimeResolver"/>, which resolves the rank 5/10
    /// branch mechanics (salvo counts, freeze turns, etc.) - the two apply
    /// independently and both only ever affect the player's own loadout; AI
    /// opponents never modernize (see WeaponEvolutionRuntimeResolver remarks).
    /// </summary>
    public static class WeaponUpgradeModifierResolver
    {
        public static WeaponUpgradeModifiers Resolve(WeaponDefinition weapon, SaveWeaponProgressData progress)
        {
            if (weapon == null
                || progress == null
                || !WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var definition))
            {
                return default;
            }

            var damagePercent = WeaponUpgradeChoiceResolver.ResolveDamagePercent(weapon, progress, definition);
            var impactRadiusPercent = WeaponUpgradeChoiceResolver.ResolveImpactRadiusPercent(weapon, progress, definition);
            return new WeaponUpgradeModifiers(damagePercent, impactRadiusPercent);
        }
    }
}
