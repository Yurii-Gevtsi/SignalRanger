using Game.Data.Save;
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
    }
}
