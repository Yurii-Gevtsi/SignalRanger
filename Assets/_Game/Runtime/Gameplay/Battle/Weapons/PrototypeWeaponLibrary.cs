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
                    5f,
                    17f,
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
                    4f,
                    16f,
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
                    3f,
                    10f,
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
                    6f,
                    19f,
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
                    4f,
                    15f,
                    -1,
                    "weapon.scatter_arc.hint",
                    new[] { "area", "arc", "flush" }),
                WeaponDefinition.CreateRuntime(
                    "weapon.rampart-kit",
                    "weapon.rampart_kit.name",
                    WeaponRole.CoverTool,
                    0,
                    1.2f,
                    0.12f,
                    0.9f,
                    7f,
                    3f,
                    14f,
                    2,
                    "weapon.rampart_kit.hint",
                    new[] { "utility", "cover", "defensive-line" }),
                WeaponDefinition.CreateRuntime(
                    "weapon.guard-pulse",
                    "weapon.guard_pulse.name",
                    WeaponRole.DefensiveTool,
                    18,
                    1.8f,
                    0.12f,
                    0.8f,
                    6f,
                    3f,
                    13f,
                    2,
                    "weapon.guard_pulse.hint",
                    new[] { "utility", "defense", "restore" }),
                WeaponDefinition.CreateRuntime(
                    "weapon.relay-step",
                    "weapon.relay_step.name",
                    WeaponRole.MovementTool,
                    0,
                    2.6f,
                    0.11f,
                    0.5f,
                    6f,
                    4f,
                    14f,
                    2,
                    "weapon.relay_step.hint",
                    new[] { "utility", "movement", "reposition" })
            };
        }
    }
}
