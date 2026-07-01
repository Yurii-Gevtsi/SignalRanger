using System;
using Game.Data.Save;

namespace Game.Economy
{
    public sealed class EconomyService
    {
        private readonly SaveData _saveData;

        public EconomyService(SaveData saveData)
        {
            _saveData = saveData ?? throw new ArgumentNullException(nameof(saveData));
            _saveData.EnsureDefaults();
        }

        public int SoftCurrency => _saveData.economy.softCurrency;

        public EconomyTransactionResult Grant(int amount, string reason)
        {
            if (amount <= 0)
            {
                return new EconomyTransactionResult(false, SoftCurrency, reason);
            }

            _saveData.economy.softCurrency += amount;
            return new EconomyTransactionResult(true, SoftCurrency, reason);
        }

        public bool CanSpend(int amount)
        {
            return amount >= 0 && SoftCurrency >= amount;
        }

        public EconomyTransactionResult TrySpend(int amount, string reason)
        {
            if (amount < 0 || !CanSpend(amount))
            {
                return new EconomyTransactionResult(false, SoftCurrency, reason);
            }

            _saveData.economy.softCurrency -= amount;
            return new EconomyTransactionResult(true, SoftCurrency, reason);
        }
    }
}
