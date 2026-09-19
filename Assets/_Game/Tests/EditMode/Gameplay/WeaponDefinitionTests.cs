using System.Linq;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Weapons;
using NUnit.Framework;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class WeaponDefinitionTests
    {
        [Test]
        public void PrototypeWeapons_AreValidAndHaveFortyOneDistinctIds()
        {
            var weapons = PrototypeWeaponLibrary.CreateAll();

            Assert.That(weapons, Has.Length.EqualTo(41));
            Assert.That(weapons.Select(weapon => weapon.WeaponId).Distinct().Count(), Is.EqualTo(weapons.Length));
            foreach (var weapon in weapons)
            {
                Assert.That(weapon.IsValid, Is.True, weapon.WeaponId);
                Assert.That(weapon.DisplayKey, Is.Not.Empty);
                Assert.That(weapon.DescriptionKey, Is.Not.Empty);
                Assert.That(weapon.VfxId, Is.Not.Empty);
                Assert.That(weapon.SfxId, Is.Not.Empty);
                Assert.That(weapon.MaxPower, Is.GreaterThanOrEqualTo(weapon.MinPower));
            }
        }

        [Test]
        public void PrototypeWeapons_CoverTheRequiredTacticalRoles()
        {
            var roles = PrototypeWeaponLibrary.CreateAll().Select(weapon => weapon.Role).Distinct().ToArray();

            Assert.That(roles, Contains.Item(WeaponRole.DirectProjectile));
            Assert.That(roles, Contains.Item(WeaponRole.ArcingExplosive));
            Assert.That(roles, Contains.Item(WeaponRole.ShortRangeBurst));
            Assert.That(roles, Contains.Item(WeaponRole.DefensiveTool));
            Assert.That(roles, Contains.Item(WeaponRole.MovementTool));
        }

        [Test]
        public void PrototypeWeapons_ContainNoRetiredOrDropOnlyEntries()
        {
            var ids = PrototypeWeaponLibrary.CreateAll().Select(weapon => weapon.WeaponId).ToArray();

            Assert.That(ids, Does.Not.Contain("weapon.blaster-caset"));
            Assert.That(ids, Does.Not.Contain("weapon.fpv-drone"));
            Assert.That(ids, Does.Not.Contain("weapon.bonus-box"));
            Assert.That(ids, Contains.Item("weapon.bomber"));
            Assert.That(ids, Contains.Item("weapon.ufo-carrier"));
        }

        [Test]
        public void PrototypeWeapons_CannotOneShotFullHealthUnit()
        {
            foreach (var weapon in PrototypeWeaponLibrary.CreateAll())
            {
                Assert.That(weapon.Damage, Is.LessThan(100),
                    $"{weapon.WeaponId} can delete a full-health unit in one hit.");
            }
        }

        [Test]
        public void CreateAimSettings_UsesWeaponPowerRange()
        {
            var weapon = PrototypeWeaponLibrary.CreateAll()[0];
            var settings = weapon.CreateAimSettings(AimSettings.Default);

            Assert.That(settings.MinPower, Is.EqualTo(weapon.MinPower));
            Assert.That(settings.MaxPower, Is.EqualTo(weapon.MaxPower));
        }
    }
}
