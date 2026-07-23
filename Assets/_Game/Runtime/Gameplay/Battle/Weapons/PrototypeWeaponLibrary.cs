namespace Game.Gameplay.Battle.Weapons
{
    public static class PrototypeWeaponLibrary
    {
        public static WeaponDefinition[] CreateAll()
        {
            return new[]
            {
                Weapon("weapon.impulse", WeaponRole.ArcingExplosive, 34, 1.15f, 0.12f, 1f, 8f, 5f, 13f, -1, "impulse", "arc", "starter"),
                Weapon("weapon.blaster-caset", WeaponRole.DirectProjectile, 30, 0.9f, 0.09f, 0f, 4.5f, 14f, 14f, 3, "blaster_caset", "straight-long", "burst"),
                Weapon("weapon.bur", WeaponRole.ShortRangeBurst, 18, 0.7f, 0.08f, 0f, 1.2f, 3f, 3f, 3, "bur", "straight-close", "engineering", "drill"),
                Weapon("weapon.mortira", WeaponRole.ArcingExplosive, 48, 1.8f, 0.16f, 1.15f, 9f, 4f, 12f, 2, "mortira", "arc", "fragment"),
                // Timeout doubles as the grenade fuse: it counts from the throw
                // (reference behaviour), whether the grenade is rolling or resting.
                Weapon("weapon.granata", WeaponRole.ArcingExplosive, 42, 1.55f, 0.14f, 1.1f, 4.5f, 8f, 24f, 3, "granata", "arc", "grenade"),
                Weapon("weapon.teleport", WeaponRole.MovementTool, 0, 2.5f, 0.11f, 1.1f, 8f, 8f, 24f, 2, "teleport", "arc-utility", "teleport"),
                Weapon("weapon.c4", WeaponRole.ShortRangeBurst, 74, 2.25f, 0.19f, 0.9f, 5f, 3f, 8f, 1, "C4", "ground", "explosive"),
                Weapon("weapon.mine", WeaponRole.ArcingExplosive, 54, 1.7f, 0.14f, 1.05f, 8f, 3f, 10f, 2, "mine_off", "ground", "mine"),
                Weapon("weapon.bur-rocket", WeaponRole.ArcingExplosive, 46, 1.25f, 0.12f, 0.8f, 8f, 5f, 13f, 2, "bur_rocket", "arc-special", "drill"),
                Weapon("weapon.heal", WeaponRole.DefensiveTool, 34, 1.4f, 0.12f, 0.65f, 6f, 3f, 9f, 1, "heal", "bonus", "repair"),
                Weapon("weapon.bonus-box", WeaponRole.DefensiveTool, 22, 1.25f, 0.12f, 0.85f, 7f, 3f, 10f, 2, "bonus_box", "drop", "random"),
                Weapon("weapon.toxic", WeaponRole.DirectProjectile, 30, 1.2f, 0.11f, 0f, 4.5f, 13f, 13f, 3, "toxic", "straight-close", "toxic"),
                Weapon("weapon.emp", WeaponRole.DirectProjectile, 38, 1.5f, 0.13f, 0f, 7f, 9f, 9f, 2, "emp", "straight-long", "emp"),
                Weapon("weapon.freeze", WeaponRole.ArcingExplosive, 36, 1.35f, 0.12f, 1.05f, 8f, 4f, 12f, 2, "freeze", "arc", "freeze"),
                Weapon("weapon.fire-gun", WeaponRole.ShortRangeBurst, 40, 1.45f, 0.13f, 0.82f, 3f, 6.4f, 6.4f, 3, "fire_gun", "straight-close", "fire"),
                Weapon("weapon.missle", WeaponRole.ArcingExplosive, 52, 1.4f, 0.13f, 0.8f, 8f, 5f, 13f, 2, "missle", "arc-homing", "homing"),
                Weapon("weapon.fpv-drone", WeaponRole.DirectProjectile, 58, 1.35f, 0.14f, 0f, 8f, 8.5f, 8.5f, 1, "fpv_0", "controlled", "drone")
            };
        }

        private static WeaponDefinition Weapon(string id, WeaponRole role, int damage, float impactRadius,
            float projectileRadius, float gravity, float timeout, float minPower, float maxPower, int ammo,
            string icon, string category, params string[] tags)
        {
            return WeaponDefinition.CreateRuntime(id, id + ".name", role, damage, impactRadius, projectileRadius,
                gravity, timeout, minPower, maxPower, ammo, id + ".hint", Combine(category, tags), iconId: icon);
        }

        private static string[] Combine(string category, string[] tags)
        {
            var result = new string[(tags?.Length ?? 0) + 1];
            result[0] = category;
            if (tags != null)
            {
                System.Array.Copy(tags, 0, result, 1, tags.Length);
            }

            return result;
        }
    }
}
