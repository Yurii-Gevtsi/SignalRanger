using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core.Localization;
using Game.Data.Progression;
using Game.Data.Save;
using Game.Gameplay.Battle.Weapons;
using NUnit.Framework;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class WeaponUiCatalogAvailabilityTests
    {
        [Test]
        public void RuntimeCatalogContainsEveryCanonicalWeaponAndBaseShopContainsOnlyCoreShop()
        {
            var expectedIds = new[]
            {
                "weapon.bur", "weapon.impulse", "weapon.granata",
                "weapon.rope", "weapon.plasma-sword", "weapon.ion-shotgun", "weapon.c4", "weapon.mine",
                "weapon.heal", "weapon.titanium-disc", "weapon.bur-rocket", "weapon.titanium-barrier",
                "weapon.mortira", "weapon.sticky-grenade", "weapon.teleport", "weapon.rail-rifle",
                "weapon.cryo-grenade", "weapon.hopping-grenade", "weapon.incendiary-grenade", "weapon.repair-jammer",
                "weapon.missle", "weapon.orbital-rocket",
                "weapon.guardian-drone", "weapon.emp", "weapon.turn-transfer", "weapon.electric-harpoon",
                "weapon.swap-jump", "weapon.grounder", "weapon.force-field",
                "weapon.boost", "weapon.sky-hook", "weapon.spider-mine", "weapon.virus", "weapon.bomber",
                "weapon.ufo-carrier", "weapon.self-destruct", "weapon.nuclear-charge",
                "weapon.laser-fence", "weapon.freeze", "weapon.fire-gun", "weapon.toxic"
            };

            CollectionAssert.AreEquivalent(expectedIds,
                WeaponProgressionCatalog.All.Select(item => item.WeaponId).ToArray());

            var coreShop = WeaponProgressionCatalog.All
                .Where(WeaponCatalogAvailability.IsVisibleInBaseShop)
                .Select(item => item.WeaponId)
                .ToArray();
            Assert.That(coreShop, Has.Length.EqualTo(19));
            Assert.That(coreShop, Does.Not.Contain("weapon.emp"));
            Assert.That(coreShop, Does.Not.Contain("weapon.boost"));
            Assert.That(coreShop, Does.Not.Contain("weapon.freeze"));
        }

        [Test]
        public void BaseShopContainsOnlyCurrentCoreShopWeapons()
        {
            var actual = WeaponProgressionCatalog.All
                .Where(WeaponCatalogAvailability.IsVisibleInBaseShop)
                .Select(item => item.WeaponId)
                .ToArray();

            CollectionAssert.AreEquivalent(new[]
            {
                "weapon.rope",
                "weapon.plasma-sword",
                "weapon.ion-shotgun",
                "weapon.mine",
                "weapon.c4",
                "weapon.heal",
                "weapon.titanium-disc",
                "weapon.teleport",
                "weapon.bur-rocket",
                "weapon.titanium-barrier",
                "weapon.mortira",
                "weapon.sticky-grenade",
                "weapon.rail-rifle",
                "weapon.cryo-grenade",
                "weapon.hopping-grenade",
                "weapon.incendiary-grenade",
                "weapon.repair-jammer",
                "weapon.missle",
                "weapon.orbital-rocket"
            }, actual);
            Assert.That(actual, Does.Not.Contain("weapon.emp"));
            Assert.That(actual, Does.Not.Contain("weapon.freeze"));
            Assert.That(actual, Does.Not.Contain("weapon.fpv-drone"));
        }

        [Test]
        public void ArsenalKeepsEveryOwnedActiveWeaponButHidesRetiredEntries()
        {
            Assert.That(WeaponCatalogAvailability.IsVisibleInArsenal("weapon.impulse"), Is.True);
            Assert.That(WeaponCatalogAvailability.IsVisibleInArsenal("weapon.emp"), Is.True);
            Assert.That(WeaponCatalogAvailability.IsVisibleInArsenal("weapon.freeze"), Is.True);
            Assert.That(WeaponCatalogAvailability.IsVisibleInArsenal("weapon.blaster-caset"), Is.False);
            Assert.That(WeaponCatalogAvailability.IsVisibleInArsenal("weapon.fpv-drone"), Is.False);
            Assert.That(WeaponCatalogAvailability.IsVisibleInArsenal("weapon.bonus-box"), Is.False);
        }

        [Test]
        public void WorkshopRequiresAnActiveWeaponAndAtLeastOneUpgradeNode()
        {
            var impulse = Find(PrototypeWeaponLibrary.CreateAll(), "weapon.impulse");
            Assert.That(WeaponProgressionCatalog.TryGet(impulse.WeaponId, out var developed), Is.True);
            Assert.That(WeaponCatalogAvailability.IsVisibleInWorkshop(impulse.WeaponId, developed), Is.True);

            var noDevelopment = new WeaponProgressionDefinition(
                "weapon.test", 1, 0, 0, false, string.Empty, string.Empty,
                Array.Empty<WeaponUpgradeNodeDefinition>(), 0, 0, 0, 0);
            Assert.That(WeaponCatalogAvailability.IsVisibleInWorkshop(impulse.WeaponId, noDevelopment), Is.False);
            Assert.That(WeaponCatalogAvailability.IsVisibleInWorkshop(
                "weapon.fpv-drone", developed), Is.False);
        }

        [Test]
        public void RankTenChoiceDoesNotReplaceSkinChosenAtRankFive()
        {
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.impulse", out var definition), Is.True);

            var progress = new SaveWeaponProgressData
            {
                weaponId = "weapon.impulse",
                rank = 10,
                evolutionChoices = new List<SaveWeaponEvolutionChoiceData>
                {
                    new() { rank = 5, optionId = "double-impulse" }
                }
            };

            Assert.That(
                WeaponEvolutionIconResolver.ResolveCurrentIconResource("impulse", definition, progress),
                Is.EqualTo("UI/Weapons/impulse_5_10_B"));
        }

        [Test]
        public void CurrentDescriptionRetainsBothSelectedSpecialEffects()
        {
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.impulse", out var definition), Is.True);
            var progress = new SaveWeaponProgressData
            {
                weaponId = "weapon.impulse",
                rank = 10,
                evolutionChoices = new List<SaveWeaponEvolutionChoiceData>
                {
                    new() { rank = 5, optionId = "double-impulse" }
                }
            };

            var description = WeaponUpgradePresentation.GetCurrentDescription(
                Find(PrototypeWeaponLibrary.CreateAll(), "weapon.impulse"), definition, progress);

            Assert.That(description, Does.Contain(GameText.WeaponEvolutionDescription("double-impulse", string.Empty)));
        }

        private static WeaponDefinition Find(IEnumerable<WeaponDefinition> weapons, string weaponId)
        {
            return weapons.Single(weapon => weapon.WeaponId == weaponId);
        }
    }
}
