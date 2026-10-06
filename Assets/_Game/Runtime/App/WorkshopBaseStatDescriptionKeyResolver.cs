using System.Collections.Generic;
using Game.Data.Progression;

namespace Game.App
{
    /// <summary>
    /// Selects the localized description key for a Workshop base-stat card.
    /// Radius is an effect-scale category, so only its configured weapons need
    /// more specific wording; all other paths remain shared.
    /// </summary>
    public static class WorkshopBaseStatDescriptionKeyResolver
    {
        private const string RadiusDescriptionRoot = "architect.detail.radius.";

        private static readonly Dictionary<string, string> RadiusOverrides = new()
        {
            { "weapon.bur", RadiusDescriptionRoot + "drillRocket" },
            { "weapon.rope", RadiusDescriptionRoot + "rope" },
            { "weapon.ion-shotgun", RadiusDescriptionRoot + "ionShotgun" },
            { "weapon.titanium-disc", RadiusDescriptionRoot + "titaniumDisc" },
            { "weapon.bur-rocket", RadiusDescriptionRoot + "drillRocket" },
            { "weapon.boost", RadiusDescriptionRoot + "boost" },
            { "weapon.guardian-drone", RadiusDescriptionRoot + "guardianDrone" },
            { "weapon.spider-mine", RadiusDescriptionRoot + "spiderMine" },
            { "weapon.emp", RadiusDescriptionRoot + "emp" },
            { "weapon.titanium-barrier", RadiusDescriptionRoot + "titaniumBarrier" },
            { "weapon.electric-harpoon", RadiusDescriptionRoot + "electricHarpoon" },
            { "weapon.hopping-grenade", RadiusDescriptionRoot + "hoppingGrenade" },
            { "weapon.cryo-grenade", RadiusDescriptionRoot + "cryoGrenade" },
            { "weapon.toxic", RadiusDescriptionRoot + "toxic" },
            { "weapon.incendiary-grenade", RadiusDescriptionRoot + "incendiaryGrenade" },
            { "weapon.freeze", RadiusDescriptionRoot + "cryoCannon" },
            { "weapon.fire-gun", RadiusDescriptionRoot + "lavaCannon" },
            { "weapon.repair-jammer", RadiusDescriptionRoot + "repairJammer" },
            { "weapon.laser-fence", RadiusDescriptionRoot + "laserFence" },
            { "weapon.grounder", RadiusDescriptionRoot + "grounder" },
            { "weapon.bomber", RadiusDescriptionRoot + "bomber" },
            { "weapon.ufo-carrier", RadiusDescriptionRoot + "transporter" },
            { "weapon.force-field", RadiusDescriptionRoot + "forceField" }
        };

        public static string Resolve(WeaponUpgradePath path, string weaponId)
        {
            return path switch
            {
                WeaponUpgradePath.Core when weaponId == "weapon.grounder" => "architect.detail.core.grounder",
                WeaponUpgradePath.Core when weaponId == "weapon.heal" => "architect.detail.core.repairKit",
                WeaponUpgradePath.Core => "architect.detail.coreDescription",
                WeaponUpgradePath.Aim => "architect.detail.aimDescription",
                WeaponUpgradePath.Loadout => "architect.detail.loadoutDescription",
                WeaponUpgradePath.Radius when RadiusOverrides.TryGetValue(weaponId ?? string.Empty, out var key) => key,
                _ => "architect.detail.effectScaleDescription"
            };
        }
    }
}
