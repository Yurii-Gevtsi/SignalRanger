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

            Assert.That(weapons, Has.Length.GreaterThanOrEqualTo(3));
            Assert.That(weapons.Select(weapon => weapon.WeaponId).Distinct().Count(), Is.EqualTo(weapons.Length));
            foreach (var weapon in weapons)
            {
                Assert.That(weapon.IsValid, Is.True);
                Assert.That(weapon.DisplayKey, Is.Not.Empty);
                Assert.That(weapon.MaxPower, Is.GreaterThanOrEqualTo(weapon.MinPower));
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
