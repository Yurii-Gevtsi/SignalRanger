namespace Game.Data.Progression
{
    public enum WeaponAcquisitionSource
    {
        Starter,
        CoreShop,
        ArenaShop,
        DungeonShop,
        BossReward,
        LegacyHidden
    }

    /// <summary>
    /// Runtime projection of the canonical 41-item weapon catalog. It centralizes
    /// screen visibility so Shop, Arsenal and Workshop cannot diverge.
    /// </summary>
    public static class WeaponCatalogAvailability
    {
        public static WeaponAcquisitionSource GetSource(string weaponId)
        {
            return weaponId switch
            {
                "weapon.bur" or "weapon.impulse" or "weapon.granata" => WeaponAcquisitionSource.Starter,
                "weapon.rope" or "weapon.plasma-sword" or "weapon.ion-shotgun" or "weapon.c4"
                    or "weapon.mine" or "weapon.heal" or "weapon.titanium-disc" or "weapon.bur-rocket"
                    or "weapon.titanium-barrier" or "weapon.mortira" or "weapon.sticky-grenade"
                    or "weapon.teleport" or "weapon.rail-rifle" or "weapon.cryo-grenade"
                    or "weapon.hopping-grenade" or "weapon.incendiary-grenade" or "weapon.repair-jammer"
                    or "weapon.missle" or "weapon.orbital-rocket" => WeaponAcquisitionSource.CoreShop,
                "weapon.guardian-drone" or "weapon.emp" or "weapon.turn-transfer"
                    or "weapon.electric-harpoon" or "weapon.swap-jump" or "weapon.grounder"
                    or "weapon.force-field" => WeaponAcquisitionSource.ArenaShop,
                "weapon.boost" or "weapon.sky-hook" or "weapon.spider-mine" or "weapon.virus"
                    or "weapon.bomber" or "weapon.ufo-carrier" or "weapon.self-destruct"
                    or "weapon.nuclear-charge" => WeaponAcquisitionSource.DungeonShop,
                "weapon.laser-fence" or "weapon.freeze" or "weapon.fire-gun"
                    or "weapon.toxic" => WeaponAcquisitionSource.BossReward,
                _ => WeaponAcquisitionSource.LegacyHidden
            };
        }

        public static bool IsActive(string weaponId) =>
            GetSource(weaponId) != WeaponAcquisitionSource.LegacyHidden;

        public static bool IsVisibleInBaseShop(WeaponProgressionDefinition progression) =>
            progression != null && GetSource(progression.WeaponId) == WeaponAcquisitionSource.CoreShop;

        public static bool IsVisibleInArsenal(string weaponId) =>
            IsActive(weaponId);

        public static bool IsVisibleInWorkshop(string weaponId, WeaponProgressionDefinition progression) =>
            IsVisibleInArsenal(weaponId) && progression != null && progression.HasDevelopment;
    }
}
