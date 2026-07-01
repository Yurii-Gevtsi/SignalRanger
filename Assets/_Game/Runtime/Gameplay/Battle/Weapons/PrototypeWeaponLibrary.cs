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
                    new[] { "starter", "short-range", "burst" })
            };
        }
    }
}
