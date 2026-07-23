using System;
using Game.Data.Save;

namespace Game.Economy
{
    public sealed class EconomyService : IEconomyService
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
            return Grant(amount, reason, string.Empty, null);
        }

        public EconomyTransactionResult Grant(int amount, string sourceType, string sourceId, string transactionId)
        {
            if (amount <= 0)
            {
                return new EconomyTransactionResult(false, SoftCurrency, sourceType);
            }

            if (_saveData.ledger.ContainsTransaction(transactionId))
            {
                return new EconomyTransactionResult(true, SoftCurrency, sourceType, transactionId, true);
            }

            _saveData.economy.softCurrency += amount;
            var recordedId = RecordTransaction(transactionId, sourceType, sourceId, amount);
            return new EconomyTransactionResult(true, SoftCurrency, sourceType, recordedId, false);
        }

        public bool CanSpend(int amount)
        {
            return amount >= 0 && SoftCurrency >= amount;
        }

        public EconomyTransactionResult TrySpend(int amount, string reason)
        {
            return TrySpend(amount, reason, string.Empty, null);
        }

        public EconomyTransactionResult TrySpend(int amount, string sourceType, string sourceId, string transactionId)
        {
            if (amount < 0)
            {
                return new EconomyTransactionResult(false, SoftCurrency, sourceType);
            }

            if (_saveData.ledger.ContainsTransaction(transactionId))
            {
                return new EconomyTransactionResult(true, SoftCurrency, sourceType, transactionId, true);
            }

            if (!CanSpend(amount))
            {
                return new EconomyTransactionResult(false, SoftCurrency, sourceType);
            }

            _saveData.economy.softCurrency -= amount;
            var recordedId = RecordTransaction(transactionId, sourceType, sourceId, -amount);
            return new EconomyTransactionResult(true, SoftCurrency, sourceType, recordedId, false);
        }

        private string RecordTransaction(string transactionId, string sourceType, string sourceId, int delta)
        {
            var recordedId = string.IsNullOrWhiteSpace(transactionId)
                ? Guid.NewGuid().ToString("N")
                : transactionId;

            _saveData.ledger.Append(new SaveLedgerEntry
            {
                transactionId = recordedId,
                sourceType = sourceType ?? string.Empty,
                sourceId = sourceId ?? string.Empty,
                softCurrencyDelta = delta,
                createdUtcTicks = DateTime.UtcNow.Ticks,
                pendingSync = true
            });

            return recordedId;
        }
    }
}
