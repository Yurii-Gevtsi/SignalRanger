using Game.App;
using Game.Data.Progression;
using NUnit.Framework;

namespace Game.Tests.EditMode.App
{
    public sealed class WorkshopBaseStatDescriptionKeyResolverTests
    {
        [TestCase(WeaponUpgradePath.Core, "weapon.rope", "architect.detail.coreDescription")]
        [TestCase(WeaponUpgradePath.Aim, "weapon.rope", "architect.detail.aimDescription")]
        [TestCase(WeaponUpgradePath.Loadout, "weapon.rope", "architect.detail.loadoutDescription")]
        public void SharedBasePaths_UseTheirSharedDescriptionKeys(
            WeaponUpgradePath path, string weaponId, string expectedKey)
        {
            Assert.That(WorkshopBaseStatDescriptionKeyResolver.Resolve(path, weaponId), Is.EqualTo(expectedKey));
        }

        [Test]
        public void GrounderCore_DescribesAddedDuration()
        {
            Assert.That(WorkshopBaseStatDescriptionKeyResolver.Resolve(
                    WeaponUpgradePath.Core, "weapon.grounder"),
                Is.EqualTo("architect.detail.core.grounder"));
        }

        [TestCase("weapon.rope", "architect.detail.radius.rope")]
        [TestCase("weapon.ion-shotgun", "architect.detail.radius.ionShotgun")]
        [TestCase("weapon.titanium-disc", "architect.detail.radius.titaniumDisc")]
        [TestCase("weapon.bur", "architect.detail.radius.drillRocket")]
        [TestCase("weapon.bur-rocket", "architect.detail.radius.drillRocket")]
        [TestCase("weapon.boost", "architect.detail.radius.boost")]
        [TestCase("weapon.guardian-drone", "architect.detail.radius.guardianDrone")]
        [TestCase("weapon.spider-mine", "architect.detail.radius.spiderMine")]
        [TestCase("weapon.emp", "architect.detail.radius.emp")]
        [TestCase("weapon.titanium-barrier", "architect.detail.radius.titaniumBarrier")]
        [TestCase("weapon.electric-harpoon", "architect.detail.radius.electricHarpoon")]
        [TestCase("weapon.hopping-grenade", "architect.detail.radius.hoppingGrenade")]
        [TestCase("weapon.cryo-grenade", "architect.detail.radius.cryoGrenade")]
        [TestCase("weapon.toxic", "architect.detail.radius.toxic")]
        [TestCase("weapon.incendiary-grenade", "architect.detail.radius.incendiaryGrenade")]
        [TestCase("weapon.freeze", "architect.detail.radius.cryoCannon")]
        [TestCase("weapon.fire-gun", "architect.detail.radius.lavaCannon")]
        [TestCase("weapon.repair-jammer", "architect.detail.radius.repairJammer")]
        [TestCase("weapon.laser-fence", "architect.detail.radius.laserFence")]
        [TestCase("weapon.grounder", "architect.detail.radius.grounder")]
        [TestCase("weapon.bomber", "architect.detail.radius.bomber")]
        [TestCase("weapon.ufo-carrier", "architect.detail.radius.transporter")]
        [TestCase("weapon.force-field", "architect.detail.radius.forceField")]
        public void Radius_UsesTheWeaponSpecificDescriptionWhenConfigured(string weaponId, string expectedKey)
        {
            Assert.That(WorkshopBaseStatDescriptionKeyResolver.Resolve(WeaponUpgradePath.Radius, weaponId),
                Is.EqualTo(expectedKey));
        }

        [Test]
        public void Radius_UsesTheGenericDescriptionWhenNoOverrideExists()
        {
            Assert.That(WorkshopBaseStatDescriptionKeyResolver.Resolve(WeaponUpgradePath.Radius, "weapon.impulse"),
                Is.EqualTo("architect.detail.effectScaleDescription"));
        }
    }
}
