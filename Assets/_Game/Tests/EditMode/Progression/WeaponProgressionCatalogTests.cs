using System;
using System.Linq;
using Game.Data.Progression;
using Game.Data.Save;
using Game.Gameplay.Battle.Weapons;
using Game.Economy;
using Game.Progression;
using NUnit.Framework;

namespace Game.Tests.EditMode.Progression
{
    public sealed class WeaponProgressionCatalogTests
    {
        [Test]
        public void CatalogContainsTheCanonicalFortyOneWeaponsWithPerWeaponCaps()
        {
            Assert.That(WeaponProgressionCatalog.All.Count, Is.EqualTo(41));
            Assert.That(WeaponProgressionCatalog.All.Select(item => item.WeaponId).Distinct().Count(), Is.EqualTo(41));

            Assert.That(WeaponProgressionCatalog.TryGet("weapon.impulse", out var impulse), Is.True);
            Assert.That(impulse.CoreUpgradeLimit, Is.EqualTo(4));
            Assert.That(impulse.AimUpgradeLimit, Is.EqualTo(4));
            Assert.That(impulse.LoadoutUpgradeLimit, Is.Zero);
            Assert.That(impulse.RadiusUpgradeLimit, Is.EqualTo(4));
            Assert.That(impulse.Special5PurchaseCount, Is.EqualTo(3));
            Assert.That(impulse.Special10PurchaseCount, Is.Zero);
            Assert.That(impulse.MaxRank, Is.EqualTo(16));

            Assert.That(WeaponProgressionCatalog.TryGet("weapon.mortira", out var mortar), Is.True);
            Assert.That(mortar.GetEvolutionOptions(5).Select(option => option.Id),
                Is.EquivalentTo(new[] { "fragment-mortar", "cascade-mortar" }));
            Assert.That(mortar.Special5PurchaseCount, Is.EqualTo(4));
            Assert.That(mortar.MaxRank, Is.EqualTo(19));

            Assert.That(WeaponProgressionCatalog.TryGet("weapon.rope", out var rope), Is.True);
            Assert.That(rope.HasEvolutionOptions, Is.False);
            Assert.That(rope.MaxRank, Is.EqualTo(9));
        }

        [Test]
        public void RepairJammer_AllowsExactlyTwoDurationUpgrades()
        {
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.repair-jammer", out var jammer), Is.True);
            Assert.That(jammer.RadiusUpgradeLimit, Is.EqualTo(2));
        }

        [Test]
        public void Grounder_AllowsFourCoreAndFourRadiusUpgrades()
        {
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.grounder", out var grounder), Is.True);
            Assert.That(grounder.CoreUpgradeLimit, Is.EqualTo(4));
            Assert.That(grounder.RadiusUpgradeLimit, Is.EqualTo(4));
        }

        [Test]
        public void Grounder_FourRadiusPurchasesAddFortyPercentToDetectionRange()
        {
            var weapon = Game.Gameplay.Battle.Weapons.WeaponDefinition.CreateRuntime(
                "weapon.grounder", "weapon.grounder.name",
                Game.Gameplay.Battle.Weapons.WeaponRole.DefensiveTool,
                0, 1.4f, 0.14f, 0f, 8f, 6f, 20f, 1, null, null);
            var progress = new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                upgradeChoices = new System.Collections.Generic.List<SaveWeaponUpgradeChoiceData>
                {
                    new() { rank = 2, path = (int)WeaponUpgradePath.Radius },
                    new() { rank = 3, path = (int)WeaponUpgradePath.Radius },
                    new() { rank = 4, path = (int)WeaponUpgradePath.Radius },
                    new() { rank = 5, path = (int)WeaponUpgradePath.Radius }
                }
            };

            Assert.That(WeaponUpgradeChoiceResolver.ResolveImpactRadiusPercent(weapon, progress),
                Is.EqualTo(40));
            Assert.That(WeaponUpgradeChoiceResolver.ResolveValue(
                    weapon, null, progress, WeaponUpgradePath.Radius),
                Is.EqualTo(1.96f).Within(0.0001f));
        }

        [Test]
        public void UpgradeEconomyUsesTheRankCurveAndSpecialChoiceMultipliers()
        {
            Assert.That(WeaponUpgradeEconomy.TryGetCost(14, out var plateau), Is.True);
            Assert.That(plateau.Rank, Is.EqualTo(14));
            Assert.That(plateau.Credits, Is.EqualTo(7000));
            Assert.That(plateau.Blueprints, Is.EqualTo(220));

            Assert.That(WeaponUpgradeEconomy.TryGetSpecialCost(6, 5, true, out var initialFive), Is.True);
            Assert.That(initialFive.Credits, Is.EqualTo(3240));
            Assert.That(initialFive.Blueprints, Is.EqualTo(108));
            Assert.That(initialFive.Crystals, Is.EqualTo(33));

            Assert.That(WeaponUpgradeEconomy.TryGetSpecialCost(11, 10, true, out var initialTen), Is.True);
            Assert.That(initialTen.Credits, Is.EqualTo(17500));
            Assert.That(initialTen.Blueprints, Is.EqualTo(550));
            Assert.That(initialTen.Crystals, Is.EqualTo(175));
        }

        [Test]
        public void BaseTthCannotBePurchasedBeyondItsCanonicalLimit()
        {
            var saveData = SaveData.CreateNew();
            var economy = SeedEconomy(saveData);
            var progress = saveData.inventory.GetOrCreateWeaponProgress("weapon.impulse");
            progress.rank = 6;
            progress.upgradeChoices.AddRange(new[]
            {
                new SaveWeaponUpgradeChoiceData { rank = 2, path = (int)WeaponUpgradePath.Core },
                new SaveWeaponUpgradeChoiceData { rank = 3, path = (int)WeaponUpgradePath.Core },
                new SaveWeaponUpgradeChoiceData { rank = 4, path = (int)WeaponUpgradePath.Core },
                new SaveWeaponUpgradeChoiceData { rank = 5, path = (int)WeaponUpgradePath.Core }
            });

            var result = new WeaponUpgradeService(saveData, economy)
                .TryUpgrade("weapon.impulse", WeaponUpgradePath.Core);

            Assert.That(result.Applied, Is.False);
            Assert.That(progress.rank, Is.EqualTo(6));
        }

        [Test]
        public void SelectedMortarSpecialCanBePurchasedFourTimesAndThenStops()
        {
            var saveData = SaveData.CreateNew();
            if (!saveData.inventory.unlockedWeaponIds.Contains("weapon.mortira"))
            {
                saveData.inventory.unlockedWeaponIds.Add("weapon.mortira");
            }
            var economy = SeedEconomy(saveData);
            var progress = saveData.inventory.GetOrCreateWeaponProgress("weapon.mortira");
            progress.rank = 5;
            progress.blueprints = 10000;
            var service = new WeaponUpgradeService(saveData, economy);

            Assert.That(service.TryPurchaseEvolution("weapon.mortira", 5, "fragment-mortar").Applied, Is.True);
            Assert.That(service.TryPurchaseEvolution("weapon.mortira", 5, "fragment-mortar").Applied, Is.True);
            Assert.That(service.TryPurchaseEvolution("weapon.mortira", 5, "fragment-mortar").Applied, Is.True);
            Assert.That(service.TryPurchaseEvolution("weapon.mortira", 5, "fragment-mortar").Applied, Is.True);
            Assert.That(service.TryPurchaseEvolution("weapon.mortira", 5, "fragment-mortar").Applied, Is.False);

            Assert.That(progress.rank, Is.EqualTo(9));
            Assert.That(progress.specialUpgrades.Single(item => item.rank == 5).purchaseCount, Is.EqualTo(4));
        }

        [Test]
        public void ResetClearsBothBaseAndSpecialPurchaseHistory()
        {
            var saveData = SaveData.CreateNew();
            var progress = saveData.inventory.GetOrCreateWeaponProgress("weapon.impulse");
            progress.rank = 7;
            progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData { rank = 2, path = (int)WeaponUpgradePath.Aim });
            progress.evolutionChoices.Add(new SaveWeaponEvolutionChoiceData { rank = 5, optionId = "double-impulse" });
            progress.specialUpgrades.Add(new SaveWeaponSpecialUpgradeData { rank = 5, purchaseCount = 2 });

            Assert.That(new WeaponUpgradeService(saveData, SeedEconomy(saveData)).TryReset("weapon.impulse"), Is.True);
            Assert.That(progress.rank, Is.EqualTo(1));
            Assert.That(progress.upgradeChoices, Is.Empty);
            Assert.That(progress.evolutionChoices, Is.Empty);
            Assert.That(progress.specialUpgrades, Is.Empty);
        }

        private static EconomyService SeedEconomy(SaveData saveData)
        {
            var economy = new EconomyService(saveData);
            economy.Grant(new ProgressionReward(300000, 100000, 10000, "test"), "test", string.Empty, Guid.NewGuid().ToString());
            return economy;
        }

        [Test]
        public void TitaniumBarrier_SizeFeatureAndFourLaterRadiusPurchasesUseThePaidService()
        {
            const string weaponId = "weapon.titanium-barrier";
            var save = SaveData.CreateNew();
            save.inventory.unlockedWeaponIds.Add(weaponId);
            var economy = SeedEconomy(save);
            var progress = save.inventory.GetOrCreateWeaponProgress(weaponId);
            progress.blueprints = 10000;
            var service = new WeaponUpgradeService(save, economy);
            Assert.That(service.TryUpgrade(weaponId, WeaponUpgradePath.Radius).Applied, Is.False);
            Assert.That(service.TryPurchaseEvolution(weaponId, 5, "size-unlock").Applied, Is.False);
            Assert.That(progress.rank, Is.EqualTo(1));
            for (var i = 0; i < 4; i++)
                Assert.That(service.TryUpgrade(weaponId, WeaponUpgradePath.Loadout).Applied, Is.True);
            Assert.That(progress.rank, Is.EqualTo(5));
            Assert.That(service.TryUpgrade(weaponId, WeaponUpgradePath.Radius).Applied, Is.False,
                "rank five alone does not buy resizing");
            Assert.That(service.TryPurchaseEvolution(weaponId, 5, "size-unlock").Applied, Is.True);
            Assert.That(service.TryPurchaseEvolution(weaponId, 5, "size-unlock").Applied, Is.False);
            for (var i = 0; i < 4; i++)
                Assert.That(service.TryUpgrade(weaponId, WeaponUpgradePath.Radius).Applied, Is.True);
            Assert.That(service.TryUpgrade(weaponId, WeaponUpgradePath.Radius).Applied, Is.False);
            Assert.That(progress.rank, Is.EqualTo(10));
            Assert.That(WeaponProgressionCatalog.TryGet(weaponId, out var definition), Is.True);
            Assert.That(definition.CountBaseUpgrades(progress, WeaponUpgradePath.Radius), Is.EqualTo(4));
            var weapon = PrototypeWeaponLibrary.CreateAll().Single(item => item.WeaponId == weaponId);
            Assert.That(WeaponUpgradeChoiceResolver.ResolveValue(weapon, definition, progress, WeaponUpgradePath.Radius),
                Is.EqualTo(weapon.ImpactRadius * 1.5f).Within(0.001f));
            Assert.That(WeaponUpgradeChoiceResolver.ResolveValue(weapon, definition, progress, WeaponUpgradePath.Loadout),
                Is.EqualTo(6f));
        }
    }
}
