using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Weapons;
using System.Linq;
using NUnit.Framework;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class WeaponDefinitionTests
    {
        [Test]
        public void PrototypeWeapons_AreValidAndHaveDistinctIds()
        {
            var weapons = PrototypeWeaponLibrary.CreateAll();

            Assert.That(weapons, Has.Length.InRange(4, 6));
            Assert.That(weapons.Select(weapon => weapon.WeaponId).Distinct().Count(), Is.EqualTo(weapons.Length));
            foreach (var weapon in weapons)
            {
                Assert.That(weapon.IsValid, Is.True);
                Assert.That(weapon.DisplayKey, Is.Not.Empty);
                Assert.That(weapon.MaxPower, Is.GreaterThanOrEqualTo(weapon.MinPower));
            }
        }

        [Test]
        public void PrototypeWeapons_CoverDistinctTacticalRoles()
        {
            var roles = PrototypeWeaponLibrary.CreateAll()
                .Select(weapon => weapon.Role)
                .Distinct()
                .ToArray();

            Assert.That(roles, Contains.Item(WeaponRole.DirectProjectile));
            Assert.That(roles, Contains.Item(WeaponRole.ArcingExplosive));
            Assert.That(roles, Contains.Item(WeaponRole.ShortRangeBurst));
        }

        [Test]
        public void PrototypeWeapons_CannotOneShotFullHealthUnit()
        {
            const int startingUnitHealth = 100;

            foreach (var weapon in PrototypeWeaponLibrary.CreateAll())
            {
                Assert.That(weapon.Damage, Is.LessThan(startingUnitHealth),
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
