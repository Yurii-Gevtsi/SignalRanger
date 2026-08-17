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
        public void CatalogDefinesEveryPurchasableWeaponAndTwoCompleteBranches()
        {
            Assert.That(WeaponProgressionCatalog.All, Has.Count.EqualTo(16));
            Assert.That(WeaponProgressionCatalog.All.Select(item => item.WeaponId).Distinct(), Has.Count.EqualTo(16));

            foreach (var definition in WeaponProgressionCatalog.All)
            {
                Assert.That(definition.Nodes, Has.Count.EqualTo(15), definition.WeaponId);
                Assert.That(definition.TryGetNode(5, WeaponUpgradeBranch.A, out _), Is.True, definition.WeaponId);
                Assert.That(definition.TryGetNode(5, WeaponUpgradeBranch.B, out _), Is.True, definition.WeaponId);
                Assert.That(definition.TryGetNode(10, WeaponUpgradeBranch.A, out var capA), Is.True, definition.WeaponId);
                Assert.That(definition.TryGetNode(10, WeaponUpgradeBranch.B, out var capB), Is.True, definition.WeaponId);
                Assert.That(capA.MechanicId, Is.Not.Empty, definition.WeaponId);
                Assert.That(capB.MechanicId, Is.Not.Empty, definition.WeaponId);
            }
        }

        [Test]
        public void UpgradeEconomyMatchesApprovedTotals()
        {
            Assert.That(WeaponUpgradeEconomy.All.Sum(cost => cost.Credits), Is.EqualTo(22600));
            Assert.That(WeaponUpgradeEconomy.All.Sum(cost => cost.Blueprints), Is.EqualTo(710));
            Assert.That(WeaponUpgradeEconomy.All.Where(cost => cost.Rank <= 5).Sum(cost => cost.Credits), Is.EqualTo(2800));
            Assert.That(WeaponUpgradeEconomy.All.Where(cost => cost.Rank <= 5).Sum(cost => cost.Blueprints), Is.EqualTo(90));
        }

        [Test]
        public void UpgradeRequirementsScaleFromEachWeaponUnlockLevel()
        {
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.c4", out var charge), Is.True);
            Assert.That(WeaponUpgradeEconomy.GetRequiredCommanderLevel(charge, 2), Is.EqualTo(5));
            Assert.That(WeaponUpgradeEconomy.GetRequiredCommanderLevel(charge, 5), Is.EqualTo(8));
            Assert.That(WeaponUpgradeEconomy.GetRequiredCommanderLevel(charge, 10), Is.EqualTo(20));

            Assert.That(WeaponProgressionCatalog.TryGet("weapon.fpv-drone", out var drone), Is.True);
            Assert.That(WeaponUpgradeEconomy.GetRequiredCommanderLevel(drone, 10), Is.EqualTo(30));
        }

        [Test]
        public void CatalogOrderMovesFromBasicToolsToPowerWeapons()
        {
            AssertCatalogEntry("weapon.bur", 2, 800);
            AssertCatalogEntry("weapon.granata", 3, 1000);
            AssertCatalogEntry("weapon.mine", 4, 1500);
            AssertCatalogEntry("weapon.c4", 5, 1800);
            AssertCatalogEntry("weapon.heal", 6, 2000);
            AssertCatalogEntry("weapon.teleport", 7, 3000);
            AssertCatalogEntry("weapon.bur-rocket", 8, 3500);
            AssertCatalogEntry("weapon.mortira", 9, 4000);
            AssertCatalogEntry("weapon.emp", 10, 4000);
            AssertCatalogEntry("weapon.freeze", 11, 4000);
            AssertCatalogEntry("weapon.fire-gun", 12, 4500);
            AssertCatalogEntry("weapon.toxic", 13, 4500);
            AssertCatalogEntry("weapon.missle", 14, 5000);
            AssertCatalogEntry("weapon.fpv-drone", 15, 7000);
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.bur", out var laser), Is.True);
            Assert.That(laser.PremiumPrice, Is.EqualTo(16));
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.fpv-drone", out var drone), Is.True);
            Assert.That(drone.PremiumPrice, Is.EqualTo(68));
        }

        [Test]
        public void ArenaRewardsAndCommanderThresholdsAreConcrete()
        {
            Assert.That(BattleRewardCatalog.GetArenaReward(BattleRewardDifficulty.Easy, BattleRewardOutcome.Victory).Credits, Is.EqualTo(100));
            Assert.That(BattleRewardCatalog.GetArenaReward(BattleRewardDifficulty.Medium, BattleRewardOutcome.Victory).CommanderExperience, Is.EqualTo(60));
            Assert.That(BattleRewardCatalog.GetArenaReward(BattleRewardDifficulty.Hard, BattleRewardOutcome.Victory).Blueprints, Is.EqualTo(18));
            Assert.That(BattleRewardCatalog.GetArenaReward(BattleRewardDifficulty.Hard, BattleRewardOutcome.Defeat).Credits, Is.EqualTo(110));
            Assert.That(CommanderProgressionTable.GetExperienceRequiredForLevel(2), Is.EqualTo(100));
            Assert.That(CommanderProgressionTable.GetExperienceRequiredForLevel(6), Is.EqualTo(1000));
            Assert.That(CommanderProgressionTable.GetLevel(1260), Is.EqualTo(6));
        }

        [Test]
        public void UpgradeServiceLocksBranchAtRankFive()
        {
            var saveData = SaveData.CreateNew();
            var economy = new EconomyService(saveData);
            economy.Grant(new ProgressionReward(30000, 2000, 1000, "weapon.impulse"), "test", "", "seed");
            var service = new WeaponUpgradeService(saveData, economy);

            Assert.That(service.TryUpgrade("weapon.impulse").Applied, Is.True);
            Assert.That(service.TryUpgrade("weapon.impulse").Applied, Is.True);
            Assert.That(service.TryUpgrade("weapon.impulse").Applied, Is.True);
            Assert.That(service.TryUpgrade("weapon.impulse").Applied, Is.False);
            Assert.That(service.TryUpgrade("weapon.impulse", WeaponUpgradeBranch.B).Applied, Is.True);

            var progress = saveData.inventory.GetOrCreateWeaponProgress("weapon.impulse");
            Assert.That(progress.rank, Is.EqualTo(5));
            Assert.That(progress.branch, Is.EqualTo((int)WeaponUpgradeBranch.B));
            Assert.That(service.TryUpgrade("weapon.impulse", WeaponUpgradeBranch.A).Applied, Is.True);
            Assert.That(progress.branch, Is.EqualTo((int)WeaponUpgradeBranch.B));
        }

        [Test]
        public void UpgradeServiceAllowsRepurchasingRankAfterReset()
        {
            var saveData = SaveData.CreateNew();
            var economy = new EconomyService(saveData);
            economy.Grant(new ProgressionReward(30000, 0, 1000, "weapon.impulse"), "test", "", "seed.reset");
            var service = new WeaponUpgradeService(saveData, economy);

            Assert.That(service.TryUpgrade("weapon.impulse").Applied, Is.True);
            Assert.That(service.TryReset("weapon.impulse"), Is.True);
            Assert.That(service.TryUpgrade("weapon.impulse").Applied, Is.True);

            var progress = saveData.inventory.GetOrCreateWeaponProgress("weapon.impulse");
            Assert.That(progress.rank, Is.EqualTo(2));
            Assert.That(progress.resetCycle, Is.EqualTo(1));
        }

        [Test]
        public void ImpulseEvolutionNodesHaveConfiguredNames()
        {
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.impulse", out var impulse), Is.True);
            Assert.That(impulse.TryGetNode(5, WeaponUpgradeBranch.A, out var doubleImpulse), Is.True);
            Assert.That(impulse.TryGetNode(10, WeaponUpgradeBranch.A, out var tripleImpulse), Is.True);
            Assert.That(impulse.TryGetNode(5, WeaponUpgradeBranch.B, out var impactImpulse), Is.True);
            Assert.That(impulse.TryGetNode(10, WeaponUpgradeBranch.B, out var assaultImpulse), Is.True);

            Assert.That(doubleImpulse.EvolutionName, Is.EqualTo("Гармата подвійного імпульсу"));
            Assert.That(tripleImpulse.EvolutionName, Is.EqualTo("Гармата потрійного імпульсу"));
            Assert.That(impactImpulse.EvolutionName, Is.EqualTo("Ударна імпульсна гармата"));
            Assert.That(assaultImpulse.EvolutionName, Is.EqualTo("Штурмова імпульсна гармата"));
        }

        [Test]
        public void BlasterEvolutionNodesHaveConfiguredNames()
        {
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.blaster-caset", out var blaster), Is.True);
            Assert.That(blaster.TryGetNode(5, WeaponUpgradeBranch.A, out var fourShot), Is.True);
            Assert.That(blaster.TryGetNode(10, WeaponUpgradeBranch.A, out var fiveShot), Is.True);
            Assert.That(blaster.TryGetNode(5, WeaponUpgradeBranch.B, out var ricochet), Is.True);
            Assert.That(blaster.TryGetNode(10, WeaponUpgradeBranch.B, out var doubleRicochet), Is.True);

            Assert.That(fourShot.EvolutionName, Is.EqualTo("Касетний бластер чотирьох зарядів"));
            Assert.That(fiveShot.EvolutionName, Is.EqualTo("Касетний бластер п’яти зарядів"));
            Assert.That(ricochet.EvolutionName, Is.EqualTo("Рикошетний касетний бластер"));
            Assert.That(doubleRicochet.EvolutionName, Is.EqualTo("Касетний бластер подвійного рикошету"));
        }

        [Test]
        public void DrillEvolutionNodesHaveConfiguredNames()
        {
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.bur", out var drill), Is.True);
            Assert.That(drill.TryGetNode(5, WeaponUpgradeBranch.A, out var longRange), Is.True);
            Assert.That(drill.TryGetNode(10, WeaponUpgradeBranch.A, out var deepRange), Is.True);
            Assert.That(drill.TryGetNode(5, WeaponUpgradeBranch.B, out var universal), Is.True);
            Assert.That(drill.TryGetNode(10, WeaponUpgradeBranch.B, out var pioneer), Is.True);

            Assert.That(longRange.EvolutionName, Is.EqualTo("Далекобійний буровий лазер"));
            Assert.That(deepRange.EvolutionName, Is.EqualTo("Глибокобійний буровий лазер"));
            Assert.That(universal.EvolutionName, Is.EqualTo("Універсальний буровий лазер"));
            Assert.That(pioneer.EvolutionName, Is.EqualTo("Прохідницький буровий лазер"));
        }

        [Test]
        public void DrillRocketEvolutionNodesHaveConfiguredNames()
        {
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.bur-rocket", out var rocket), Is.True);
            Assert.That(rocket.TryGetNode(5, WeaponUpgradeBranch.A, out var longRange), Is.True);
            Assert.That(rocket.TryGetNode(10, WeaponUpgradeBranch.A, out var deepRange), Is.True);
            Assert.That(rocket.TryGetNode(5, WeaponUpgradeBranch.B, out var universal), Is.True);
            Assert.That(rocket.TryGetNode(10, WeaponUpgradeBranch.B, out var allTerrain), Is.True);

            Assert.That(longRange.EvolutionName, Is.EqualTo("Далекобійна бурова ракета"));
            Assert.That(deepRange.EvolutionName, Is.EqualTo("Глибокобійна бурова ракета"));
            Assert.That(universal.EvolutionName, Is.EqualTo("Універсальна бурова ракета"));
            Assert.That(allTerrain.EvolutionName, Is.EqualTo("Всепрохідна бурова ракета"));
        }

        [Test]
        public void HomingMissileEvolutionNodesHaveConfiguredNames()
        {
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.missle", out var missile), Is.True);
            Assert.That(missile.TryGetNode(5, WeaponUpgradeBranch.A, out var ballistic), Is.True);
            Assert.That(missile.TryGetNode(10, WeaponUpgradeBranch.A, out var clusterBallistic), Is.True);
            Assert.That(missile.TryGetNode(5, WeaponUpgradeBranch.B, out var maneuvering), Is.True);
            Assert.That(missile.TryGetNode(10, WeaponUpgradeBranch.B, out var tactical), Is.True);

            Assert.That(ballistic.EvolutionName, Is.EqualTo("Балістична ракета"));
            Assert.That(clusterBallistic.EvolutionName, Is.EqualTo("Касетна балістична ракета"));
            Assert.That(maneuvering.EvolutionName, Is.EqualTo("Маневрова самонавідна ракета"));
            Assert.That(tactical.EvolutionName, Is.EqualTo("Тактична самонавідна ракета"));
        }

        [Test]
        public void C4EvolutionNodesHaveConfiguredNames()
        {
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.c4", out var c4), Is.True);
            Assert.That(c4.TryGetNode(5, WeaponUpgradeBranch.A, out var enhanced), Is.True);
            Assert.That(c4.TryGetNode(10, WeaponUpgradeBranch.A, out var breaching), Is.True);
            Assert.That(c4.TryGetNode(5, WeaponUpgradeBranch.B, out var impulse), Is.True);
            Assert.That(c4.TryGetNode(10, WeaponUpgradeBranch.B, out var shockwave), Is.True);

            Assert.That(enhanced.EvolutionName, Is.EqualTo("Посилений вибуховий заряд"));
            Assert.That(breaching.EvolutionName, Is.EqualTo("Проломний вибуховий заряд"));
            Assert.That(impulse.EvolutionName, Is.EqualTo("Імпульсний вибуховий заряд"));
            Assert.That(shockwave.EvolutionName, Is.EqualTo("Ударно-хвильовий заряд"));
        }

        [Test]
        public void GrenadeEvolutionNodesHaveConfiguredNames()
        {
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.granata", out var grenade), Is.True);
            Assert.That(grenade.TryGetNode(5, WeaponUpgradeBranch.A, out var sticky), Is.True);
            Assert.That(grenade.TryGetNode(10, WeaponUpgradeBranch.A, out var magnetic), Is.True);
            Assert.That(grenade.TryGetNode(5, WeaponUpgradeBranch.B, out var gravity), Is.True);
            Assert.That(grenade.TryGetNode(10, WeaponUpgradeBranch.B, out var collapse), Is.True);

            Assert.That(sticky.EvolutionName, Is.EqualTo("Липуча граната"));
            Assert.That(magnetic.EvolutionName, Is.EqualTo("Магнітна липуча граната"));
            Assert.That(gravity.EvolutionName, Is.EqualTo("Гравітаційна граната"));
            Assert.That(collapse.EvolutionName, Is.EqualTo("Колапс-граната"));
        }

        [Test]
        public void MineEvolutionNodesHaveConfiguredNames()
        {
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.mine", out var mine), Is.True);
            Assert.That(mine.TryGetNode(5, WeaponUpgradeBranch.A, out var four), Is.True);
            Assert.That(mine.TryGetNode(10, WeaponUpgradeBranch.A, out var five), Is.True);
            Assert.That(mine.TryGetNode(5, WeaponUpgradeBranch.B, out var recognition), Is.True);
            Assert.That(mine.TryGetNode(10, WeaponUpgradeBranch.B, out var friendly), Is.True);

            Assert.That(four.EvolutionName, Is.EqualTo("4 міни за хід"));
            Assert.That(five.EvolutionName, Is.EqualTo("5 мін за хід"));
            Assert.That(recognition.EvolutionName, Is.EqualTo("Розпізнавальна міна"));
            Assert.That(friendly.EvolutionName, Is.EqualTo("Дружня міна"));
        }

        [Test]
        public void RepairEvolutionNodesHaveConfiguredNames()
        {
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.heal", out var repair), Is.True);
            Assert.That(repair.TryGetNode(5, WeaponUpgradeBranch.A, out var protective), Is.True);
            Assert.That(repair.TryGetNode(10, WeaponUpgradeBranch.A, out var enhancedProtective), Is.True);
            Assert.That(repair.TryGetNode(5, WeaponUpgradeBranch.B, out var regenerative), Is.True);
            Assert.That(repair.TryGetNode(10, WeaponUpgradeBranch.B, out var longRegenerative), Is.True);

            Assert.That(protective.EvolutionName, Is.EqualTo("Захисний ремонт"));
            Assert.That(enhancedProtective.EvolutionName, Is.EqualTo("Посилений захисний ремонт"));
            Assert.That(regenerative.EvolutionName, Is.EqualTo("Відновлювальний ремонт"));
            Assert.That(longRegenerative.EvolutionName, Is.EqualTo("Тривалий відновлювальний ремонт"));
        }

        [Test]
        public void TeleportEvolutionNodesHaveConfiguredNames()
        {
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.teleport", out var teleport), Is.True);
            Assert.That(teleport.TryGetNode(5, WeaponUpgradeBranch.A, out var exchange), Is.True);
            Assert.That(teleport.TryGetNode(10, WeaponUpgradeBranch.A, out var tactical), Is.True);
            Assert.That(teleport.TryGetNode(5, WeaponUpgradeBranch.B, out var impulse), Is.True);
            Assert.That(teleport.TryGetNode(10, WeaponUpgradeBranch.B, out var spatial), Is.True);

            Assert.That(exchange.EvolutionName, Is.EqualTo("Телепорт обміну"));
            Assert.That(tactical.EvolutionName, Is.EqualTo("Тактичний обмін"));
            Assert.That(impulse.EvolutionName, Is.EqualTo("Імпульсний телепорт"));
            Assert.That(spatial.EvolutionName, Is.EqualTo("Просторовий зсув"));
        }

        [Test]
        public void MortarEvolutionNodesHaveConfiguredNames()
        {
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.mortira", out var mortar), Is.True);
            Assert.That(mortar.TryGetNode(5, WeaponUpgradeBranch.A, out var fragment), Is.True);
            Assert.That(mortar.TryGetNode(10, WeaponUpgradeBranch.A, out var assault), Is.True);
            Assert.That(mortar.TryGetNode(5, WeaponUpgradeBranch.B, out var cascade), Is.True);
            Assert.That(mortar.TryGetNode(10, WeaponUpgradeBranch.B, out var multiCascade), Is.True);

            Assert.That(fragment.EvolutionName, Is.EqualTo("Осколкова мортира"));
            Assert.That(assault.EvolutionName, Is.EqualTo("Штурмова мортира"));
            Assert.That(cascade.EvolutionName, Is.EqualTo("Каскадна мортира"));
            Assert.That(multiCascade.EvolutionName, Is.EqualTo("Багатокаскадна мортира"));
        }

        private static void AssertCatalogEntry(string weaponId, int level, int price)
        {
            Assert.That(WeaponProgressionCatalog.TryGet(weaponId, out var definition), Is.True, weaponId);
            Assert.That(definition.RequiredCommanderLevel, Is.EqualTo(level), weaponId);
            Assert.That(definition.CreditPrice, Is.EqualTo(price), weaponId);
        }
    }
}
