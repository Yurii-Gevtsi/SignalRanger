using System;
using System.Linq;
using Game.Data.Progression;
using Game.Data.Save;
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
    }
}
