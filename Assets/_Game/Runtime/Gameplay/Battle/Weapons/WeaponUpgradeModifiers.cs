using Game.Data.Progression;
using Game.Data.Save;

namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>
    /// Runtime multipliers derived from the flat % stat nodes in
    /// <see cref="WeaponProgressionCatalog"/> (ranks 2-4 and 6-9, e.g. "Damage +2%").
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
    /// Sums the Damage/ImpactRadius stat nodes a weapon has actually purchased up to
    /// its current rank/branch into ready-to-use multipliers. Separate from
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

            var branch = (WeaponUpgradeBranch)progress.branch;
            var damagePercent = 0;
            var impactRadiusPercent = 0;
            var nodes = definition.Nodes;
            for (var i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                if (node.Rank > progress.rank
                    || node.IsEvolution
                    || (node.Branch != WeaponUpgradeBranch.None && node.Branch != branch))
                {
                    continue;
                }

                if (node.Stat == WeaponUpgradeStat.Damage)
                {
                    damagePercent += node.Amount;
                }
                else if (node.Stat == WeaponUpgradeStat.ImpactRadius)
                {
                    impactRadiusPercent += node.Amount;
                }
            }

            return new WeaponUpgradeModifiers(damagePercent, impactRadiusPercent);
        }
    }
}
