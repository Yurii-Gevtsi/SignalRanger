using Game.Data.Save;
using Game.Data.Progression;
using Game.Economy;
using NUnit.Framework;

namespace Game.Tests.EditMode.Economy
{
    public sealed class EconomyServiceTests
    {
        [Test]
        public void Grant_AddsSoftCurrency()
        {
            var saveData = SaveData.CreateNew();
            var service = new EconomyService(saveData);

            var result = service.Grant(50, "test.reward");

            Assert.That(result.Success, Is.True);
            Assert.That(result.Balance, Is.EqualTo(50));
            Assert.That(saveData.economy.softCurrency, Is.EqualTo(50));
        }

        [Test]
        public void TrySpend_BlocksWhenBalanceIsInsufficient()
        {
            var saveData = SaveData.CreateNew();
            var service = new EconomyService(saveData);

            var result = service.TrySpend(10, "test.spend");

            Assert.That(result.Success, Is.False);
            Assert.That(result.Balance, Is.EqualTo(0));
            Assert.That(saveData.economy.softCurrency, Is.EqualTo(0));
        }

        [Test]
        public void TrySpend_SubtractsWhenBalanceIsEnough()
        {
            var saveData = SaveData.CreateNew();
            var service = new EconomyService(saveData);
            service.Grant(25, "test.reward");

            var result = service.TrySpend(15, "test.spend");

            Assert.That(result.Success, Is.True);
            Assert.That(result.Balance, Is.EqualTo(10));
            Assert.That(saveData.economy.softCurrency, Is.EqualTo(10));
        }

        [Test]
        public void PremiumCurrencyCanBeGrantedAndSpentWithItsOwnLedgerDelta()
        {
            var saveData = SaveData.CreateNew();
            var service = new EconomyService(saveData);

            service.GrantPremium(40, "iap", "crystal.pack.small", "premium-grant-1");
            var spend = service.TrySpendPremium(16, "shop.weapon.premium", "weapon.bur", "premium-buy-1");

            Assert.That(spend.Success, Is.True);
            Assert.That(service.PremiumCurrency, Is.EqualTo(24));
            Assert.That(saveData.economy.softCurrency, Is.Zero);
            Assert.That(saveData.ledger.entries[1].premiumCurrencyDelta, Is.EqualTo(-16));
        }

        [Test]
        public void Grant_RecordsPendingLedgerEntry()
        {
            var saveData = SaveData.CreateNew();
            var service = new EconomyService(saveData);

            var result = service.Grant(50, "battle", "arena.easy", "tx-battle-1");

            Assert.That(result.TransactionId, Is.EqualTo("tx-battle-1"));
            Assert.That(saveData.ledger.entries, Has.Count.EqualTo(1));
            Assert.That(saveData.ledger.entries[0].sourceType, Is.EqualTo("battle"));
            Assert.That(saveData.ledger.entries[0].sourceId, Is.EqualTo("arena.easy"));
            Assert.That(saveData.ledger.entries[0].softCurrencyDelta, Is.EqualTo(50));
            Assert.That(saveData.ledger.entries[0].pendingSync, Is.True);
        }

        [Test]
        public void Grant_WithSameTransactionId_IsIdempotent()
        {
            var saveData = SaveData.CreateNew();
            var service = new EconomyService(saveData);

            service.Grant(50, "battle", "arena.easy", "tx-battle-1");
            var duplicate = service.Grant(50, "battle", "arena.easy", "tx-battle-1");

            Assert.That(duplicate.Success, Is.True);
            Assert.That(duplicate.WasDuplicate, Is.True);
            Assert.That(saveData.economy.softCurrency, Is.EqualTo(50));
            Assert.That(saveData.ledger.entries, Has.Count.EqualTo(1));
        }

        [Test]
        public void TrySpend_WithSameTransactionId_IsIdempotent()
        {
            var saveData = SaveData.CreateNew();
            var service = new EconomyService(saveData);
            service.Grant(100, "test.reward");

            service.TrySpend(60, "shop", "weapon.test", "tx-buy-1");
            var duplicate = service.TrySpend(60, "shop", "weapon.test", "tx-buy-1");

            Assert.That(duplicate.Success, Is.True);
            Assert.That(duplicate.WasDuplicate, Is.True);
            Assert.That(saveData.economy.softCurrency, Is.EqualTo(40));
        }

        [Test]
        public void Grant_WithoutTransactionId_GeneratesUniqueIds()
        {
            var saveData = SaveData.CreateNew();
            var service = new EconomyService(saveData);

            var first = service.Grant(10, "test.reward");
            var second = service.Grant(10, "test.reward");

            Assert.That(first.TransactionId, Is.Not.Empty);
            Assert.That(second.TransactionId, Is.Not.EqualTo(first.TransactionId));
            Assert.That(saveData.economy.softCurrency, Is.EqualTo(20));
            Assert.That(saveData.ledger.entries, Has.Count.EqualTo(2));
        }

        [Test]
        public void Grant_ProgressionRewardUpdatesAllBalancesAtomically()
        {
            var saveData = SaveData.CreateNew();
            var service = new EconomyService(saveData);

            var result = service.Grant(
                new ProgressionReward(100, 40, 8, "weapon.impulse"),
                "battle.arena",
                "easy",
                "battle.arena.test-1");

            Assert.That(result.Success, Is.True);
            Assert.That(saveData.economy.softCurrency, Is.EqualTo(100));
            Assert.That(saveData.profile.commanderExperience, Is.EqualTo(40));
            Assert.That(saveData.inventory.GetOrCreateWeaponProgress("weapon.impulse").blueprints, Is.EqualTo(8));
            Assert.That(saveData.ledger.entries[0].commanderExperienceDelta, Is.EqualTo(40));
            Assert.That(saveData.ledger.entries[0].blueprintDelta, Is.EqualTo(8));
        }

        [Test]
        public void TrySpendWeaponUpgradeRequiresBothBalances()
        {
            var saveData = SaveData.CreateNew();
            var service = new EconomyService(saveData);
            service.Grant(new ProgressionReward(300, 0, 9, "weapon.impulse"), "test", "", "grant-1");

            var blocked = service.TrySpendWeaponUpgrade(300, "weapon.impulse", 10, "upgrade", "rank.2", "upgrade-1");

            Assert.That(blocked.Success, Is.False);
            Assert.That(saveData.economy.softCurrency, Is.EqualTo(300));
            Assert.That(saveData.inventory.GetOrCreateWeaponProgress("weapon.impulse").blueprints, Is.EqualTo(9));
        }
    }
}
