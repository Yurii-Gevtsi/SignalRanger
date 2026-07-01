namespace Game.Gameplay.Battle.Weapons
{
    public static class PrototypeWeaponLibrary
    {
        public static WeaponDefinition[] CreateAll()
        {
            return new[]
            {
                WeaponDefinition.CreateRuntime(
                    "weapon.steady-orb",
                    "weapon.steady_orb.name",
                    WeaponRole.DirectProjectile,
                    34,
                    1.05f,
                    0.11f,
                    0.55f,
                    7f,
                    10f,
                    23f,
                    -1,
                    "weapon.steady_orb.hint",
                    new[] { "starter", "direct", "low-arc" }),
                WeaponDefinition.CreateRuntime(
                    "weapon.pulse-shell",
                    "weapon.pulse_shell.name",
                    WeaponRole.ArcingExplosive,
                    42,
                    1.65f,
                    0.13f,
                    1f,
                    8f,
                    8f,
                    20f,
                    -1,
                    "weapon.pulse_shell.hint",
                    new[] { "starter", "arc", "explosive" }),
                WeaponDefinition.CreateRuntime(
                    "weapon.close-pop",
                    "weapon.close_pop.name",
                    WeaponRole.ShortRangeBurst,
                    28,
                    1.25f,
                    0.16f,
                    1.25f,
                    3.4f,
                    5f,
                    13f,
                    -1,
                    "weapon.close_pop.hint",
                    new[] { "starter", "short-range", "burst" }),
                // Precision niche: flat, fast, small blast, high single-target damage.
                // Flatness comes from the low gravityScale, not from extreme power, so the
                // power range stays close to the other weapons for a consistent aim feel.
                WeaponDefinition.CreateRuntime(
                    "weapon.long-lance",
                    "weapon.long_lance.name",
                    WeaponRole.DirectProjectile,
                    46,
                    0.75f,
                    0.09f,
                    0.35f,
                    8f,
                    9f,
                    24f,
                    -1,
                    "weapon.long_lance.hint",
                    new[] { "precision", "direct", "long-range" }),
                // Area niche: high lob, wide but soft blast to flush units out of cover.
                // Low damage per hit so it pressures position rather than deleting full-HP units.
                WeaponDefinition.CreateRuntime(
                    "weapon.scatter-arc",
                    "weapon.scatter_arc.name",
                    WeaponRole.ArcingExplosive,
                    24,
                    2.35f,
                    0.14f,
                    1.35f,
                    9f,
                    9f,
                    19f,
                    -1,
                    "weapon.scatter_arc.hint",
                    new[] { "area", "arc", "flush" })
            };
        }
    }
}
