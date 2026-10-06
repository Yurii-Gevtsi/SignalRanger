using Game.Core.Localization;
using Game.Gameplay.Battle.Weapons;
using NUnit.Framework;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class WeaponPresentationTextTests
    {
        private ILocalizationService _previousLocalization;

        [SetUp]
        public void SetUp()
        {
            _previousLocalization = LocalizationRuntime.Service;
            LocalizationRuntime.Service = new JsonLocalizationService("uk");
        }

        [TearDown]
        public void TearDown()
        {
            LocalizationRuntime.Service = _previousLocalization;
        }

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
            Assert.That(WeaponPresentationText.DescribeCategory(Find(weapons, "weapon.bur")), Is.EqualTo("пряма ближня / інженерна"));
            Assert.That(WeaponPresentationText.DescribeCategory(Find(weapons, "weapon.granata")), Is.EqualTo("навісна"));
            Assert.That(WeaponPresentationText.DescribeCategory(Find(weapons, "weapon.rope")), Is.EqualTo("пряма ближня / інженерна"));
            Assert.That(WeaponPresentationText.DescribeCategory(Find(weapons, "weapon.plasma-sword")), Is.EqualTo("пряма ближня"));
            Assert.That(WeaponPresentationText.DescribeCategory(Find(weapons, "weapon.ion-shotgun")), Is.EqualTo("пряма ближня"));
            Assert.That(WeaponPresentationText.DescribeCategory(Find(weapons, "weapon.c4")), Is.EqualTo("наземна"));
            Assert.That(WeaponPresentationText.DescribeCategory(Find(weapons, "weapon.bomber")), Is.EqualTo("керована"));
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
