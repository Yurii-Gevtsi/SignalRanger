using Game.Gameplay.Battle.Weapons;
using NUnit.Framework;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class WeaponPresentationTextTests
    {
        [Test]
        public void PrototypeWeapons_HaveReadableRoleAndHintPresentation()
        {
            foreach (var weapon in PrototypeWeaponLibrary.CreateAll())
            {
                Assert.That(WeaponPresentationText.DescribeRole(weapon), Is.Not.Empty, weapon.WeaponId);
                Assert.That(WeaponPresentationText.DescribeBattleHint(weapon), Is.Not.Empty, weapon.WeaponId);
            }
        }

        [Test]
        public void PrototypeWeapons_UseMechanicCategoryLabels()
        {
            var weapons = PrototypeWeaponLibrary.CreateAll();

            Assert.That(WeaponPresentationText.DescribeCategory(Find(weapons, "weapon.impulse")), Is.EqualTo("навісна"));
            Assert.That(WeaponPresentationText.DescribeCategory(Find(weapons, "weapon.blaster-caset")), Is.EqualTo("пряма далека"));
            Assert.That(WeaponPresentationText.DescribeCategory(Find(weapons, "weapon.bur")), Is.EqualTo("пряма ближня / інженерна"));
            Assert.That(WeaponPresentationText.DescribeCategory(Find(weapons, "weapon.c4")), Is.EqualTo("наземна"));
            Assert.That(WeaponPresentationText.DescribeCategory(Find(weapons, "weapon.fpv-drone")), Is.EqualTo("керована"));
            Assert.That(WeaponPresentationText.DescribeCategory(Find(weapons, "weapon.bonus-box")), Is.EqualTo("дроп на карті"));
        }

        private static WeaponDefinition Find(WeaponDefinition[] weapons, string weaponId)
        {
            foreach (var weapon in weapons)
            {
                if (weapon.WeaponId == weaponId)
                {
                    return weapon;
                }
            }

            return null;
        }
    }
}
