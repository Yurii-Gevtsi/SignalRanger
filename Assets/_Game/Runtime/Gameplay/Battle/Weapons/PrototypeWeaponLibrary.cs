namespace Game.Gameplay.Battle.Weapons
{
    public static class PrototypeWeaponLibrary
    {
        public const float SharedProjectileSpeed = 12f;
        public const float SharedExplosionRadius = 1.55f;
        public const float SharedNearbyRadius = 1.4f;

        public static WeaponDefinition[] CreateAll()
        {
            return new[]
            {
                Weapon("weapon.bur", WeaponRole.ShortRangeBurst, 18, 0.7f, 0.08f, 0f, 1.2f, 3f, 3f, -1, "bur", "straight-close", "engineering", "drill"),
                Weapon("weapon.impulse", WeaponRole.ArcingExplosive, 34, SharedExplosionRadius, 0.12f, 1f, 8f, SharedProjectileSpeed, SharedProjectileSpeed, -1, "impulse", "arc", "starter"),
                // Timeout doubles as the grenade fuse: it counts from the throw
                // (reference behaviour), whether the grenade is rolling or resting.
                Weapon("weapon.granata", WeaponRole.ArcingExplosive, 42, SharedExplosionRadius, 0.14f, 1.1f, 4.5f, SharedProjectileSpeed, SharedProjectileSpeed, 3, "granata", "arc", "grenade"),
                Weapon("weapon.rope", WeaponRole.MovementTool, 0, 0.1f, 0.08f, 0f, 2f, 1f, 1f, -1, "rope", "straight-close", "engineering", "rope", "traversal"),
                Weapon("weapon.plasma-sword", WeaponRole.ShortRangeBurst, 45, 0.55f, 0.08f, 0f, 0.7f, 2f, 2f, -1, "plasma_sword", "straight-close", "melee"),
                Weapon("weapon.ion-shotgun", WeaponRole.DirectProjectile, 38, 0.85f, 0.1f, 0f, 2.5f, SharedProjectileSpeed, SharedProjectileSpeed, 3, "ion_shotgun", "straight-close", "shotgun"),
                Weapon("weapon.c4", WeaponRole.ShortRangeBurst, 65, 1.9375f, 0.17f, 0.9f, 5f, 3f, 8f, 1, "C4", "ground", "explosive"),
                Weapon("weapon.mine", WeaponRole.ArcingExplosive, 54, SharedExplosionRadius, 0.14f, 1.05f, 8f, 3f, 10f, 4, "mine_off", "ground", "mine"),
                Weapon("weapon.heal", WeaponRole.DefensiveTool, 25, 1.4f, 0.12f, 0.65f, 6f, 3f, 9f, 1, "heal", "bonus", "repair"),
                Weapon("weapon.titanium-disc", WeaponRole.DirectProjectile, 35, SharedExplosionRadius, 0.1f, 0f, 5f, SharedProjectileSpeed, SharedProjectileSpeed, 3, "titanium_disc", "straight-long", "ricochet"),
                Weapon("weapon.bur-rocket", WeaponRole.ArcingExplosive, 46, SharedExplosionRadius, 0.12f, 0.8f, 8f, SharedProjectileSpeed, SharedProjectileSpeed, 2, "bur_rocket", "arc-special", "drill"),
                Weapon("weapon.titanium-barrier", WeaponRole.CoverTool, 0, TitaniumBarrierRules.BaseWidth, 0.1f, 0f, 6f, 1f, 1f, 2, "titanium_barrier", "ground", "barrier"),
                Weapon("weapon.mortira", WeaponRole.ArcingExplosive, 48, 1.8f, 0.16f, 1.15f, 9f, 12f, 12f, 2, "mortira", "arc", "fragment"),
                Weapon("weapon.sticky-grenade", WeaponRole.ArcingExplosive, 40, 1.45f, 0.13f, 1.05f, 5f, 7f, 18f, 2, "sticky_grenade", "arc", "toxic"),
                Weapon("weapon.teleport", WeaponRole.MovementTool, 0, 2.5f, 0.11f, 1.1f, 8f, 8f, 24f, 2, "teleport", "arc-utility", "teleport"),
                Weapon("weapon.rail-rifle", WeaponRole.DirectProjectile, 52, 0.35f, 0.07f, 0f, 5f, 15f, 15f, 2, "rail_rifle", "straight-long", "penetration"),
                Weapon("weapon.cryo-grenade", WeaponRole.ArcingExplosive, 34, 1.35f, 0.12f, 1.05f, 6f, 5f, 14f, 2, "cryo_grenade", "arc", "freeze"),
                Weapon("weapon.hopping-grenade", WeaponRole.ArcingExplosive, 37, 1.25f, 0.12f, 1.05f, 7f, 5f, 14f, 3, "hopping_grenade", "arc", "hopping"),
                Weapon("weapon.incendiary-grenade", WeaponRole.ArcingExplosive, 38, 1.4f, 0.12f, 1.05f, 6f, 5f, 14f, 2, "incendiary_grenade", "arc", "fire"),
                Weapon("weapon.repair-jammer", WeaponRole.DefensiveTool, 0, 1.2f, 0.1f, 0f, 5f, 4f, 8f, 2, "repair_jammer", "ground", "jammer"),
                Weapon("weapon.missle", WeaponRole.ArcingExplosive, 52, 1.4f, 0.13f, 0.8f, 8f, 5f, 13f, 2, "missle", "arc-homing", "homing"),
                Weapon("weapon.orbital-rocket", WeaponRole.ArcingExplosive, 65, 2.1f, 0.16f, 1.1f, 6f, 5f, 14f, 2, "missle_5_10_A", "arc", "orbital"),
                Weapon("weapon.guardian-drone", WeaponRole.DefensiveTool, 42, 1.6f, 0.12f, 0f, 7f, 4f, 9f, 2, "guardian_drone", "ground", "drone"),
                Weapon("weapon.emp", WeaponRole.DirectProjectile, 38, 1.5f, 0.13f, 0f, 7f, 9f, 9f, 2, "emp", "straight-long", "emp"),
                Weapon("weapon.turn-transfer", WeaponRole.DefensiveTool, 0, 0.1f, 0.08f, 0f, 2f, 1f, 1f, 1, "turn_transfer", "bonus", "team"),
                Weapon("weapon.electric-harpoon", WeaponRole.DirectProjectile, 40, 1.1f, 0.1f, 0f, 5f, 12f, 12f, 2, "electric_harpoon", "straight-long", "harpoon"),
                Weapon("weapon.swap-jump", WeaponRole.MovementTool, 0, 1.4f, 0.1f, 1f, 5f, 5f, 14f, 1, "teleport_5_10_A", "arc-utility", "swap"),
                Weapon("weapon.grounder", WeaponRole.DefensiveTool, 0, 1.4f, 0.1f, 0f, 5f, 4f, 8f, 1, "grounder", "ground", "grounder"),
                Weapon("weapon.force-field", WeaponRole.DefensiveTool, 0, 1.8f, 0.1f, 0f, 6f, 4f, 8f, 1, "force_field", "ground", "shield"),
                Weapon("weapon.boost", WeaponRole.MovementTool, 0, 0.1f, 0.08f, 0f, 6f, 1f, 1f, 2, "boost", "bonus", "boost"),
                Weapon("weapon.sky-hook", WeaponRole.DirectProjectile, 46, 0.9f, 0.1f, 0f, 5f, 12f, 12f, 1, "sky_hook", "straight-long", "hook"),
                Weapon("weapon.spider-mine", WeaponRole.ArcingExplosive, 45, 1.45f, 0.13f, 1.05f, 7f, 3f, 10f, 2, "spider_mine", "ground", "mine"),
                Weapon("weapon.virus", WeaponRole.DirectProjectile, 48, 0.75f, 0.09f, 0f, 4f, 12f, 12f, 1, "virus", "straight-long", "virus"),
                Weapon("weapon.bomber", WeaponRole.ArcingExplosive, 58, 1.6f, 0.14f, 0.9f, 7f, 4f, 12f, 1, "bomber", "controlled", "bomber"),
                Weapon("weapon.ufo-carrier", WeaponRole.MovementTool, 0, 1.0f, 0.1f, 0f, 7f, 4f, 12f, 1, "ufo_carrier", "controlled", "carrier"),
                Weapon("weapon.self-destruct", WeaponRole.ShortRangeBurst, 72, 2.1f, 0.16f, 0f, 3f, 1f, 1f, 1, "self_destruct", "ground", "self-destruct"),
                Weapon("weapon.nuclear-charge", WeaponRole.ShortRangeBurst, 95, 3.2f, 0.2f, 0f, 5f, 1f, 1f, 1, "nuclear_charge", "ground", "nuclear"),
                Weapon("weapon.laser-fence", WeaponRole.ArcingExplosive, 42, 1.2f, 0.1f, 1.05f, 6f, 5f, 12f, 4, "laser_fence", "ground", "laser"),
                Weapon("weapon.freeze", WeaponRole.ArcingExplosive, 36, 1.35f, 0.12f, 1.05f, 8f, 4f, 12f, 2, "freeze", "arc", "freeze"),
                Weapon("weapon.fire-gun", WeaponRole.ShortRangeBurst, 40, 1.45f, 0.13f, 0.82f, 3f, 6.4f, 6.4f, 3, "fire_gun", "straight-close", "fire"),
                Weapon("weapon.toxic", WeaponRole.DirectProjectile, 30, 1.2f, 0.11f, 0f, 4.5f, 13f, 13f, 3, "toxic", "straight-close", "toxic")
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
