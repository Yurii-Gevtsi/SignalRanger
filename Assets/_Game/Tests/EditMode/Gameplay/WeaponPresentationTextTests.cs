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
    }
}
