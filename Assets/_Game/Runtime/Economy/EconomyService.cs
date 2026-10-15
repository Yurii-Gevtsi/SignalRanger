using System;
using Game.Data.Progression;
using Game.Data.Save;

namespace Game.Economy
{
    public sealed class EconomyService : IEconomyService
    {
        private readonly SaveData _saveData;

        /// <summary>Raised after a grant changes commander experience or progression materials.</summary>
        public event Action ProgressionChanged;
        public event Action BalanceChanged;

        public EconomyService(SaveData saveData)
        {
            _saveData = saveData ?? throw new ArgumentNullException(nameof(saveData));
            _saveData.EnsureDefaults();
        }

        public int SoftCurrency => _saveData.economy.softCurrency;
        public int PremiumCurrency => _saveData.economy.premiumCurrency;
        public int CommanderExperience => _saveData.profile.commanderExperience;
        public bool AdminModeEnabled => _saveData.settings.adminModeEnabled;

        public EconomyTransactionResult Grant(int amount, string reason)
        {
            return Grant(amount, reason, string.Empty, null);
        }

        public EconomyTransactionResult Grant(int amount, string sourceType, string sourceId, string transactionId)
        {
            return Grant(new ProgressionReward(amount, 0, 0), sourceType, sourceId, transactionId);
        }

        public EconomyTransactionResult Grant(
            ProgressionReward reward,
            string sourceType,
            string sourceId,
            string transactionId)
        {
            if (reward.IsEmpty)
            {
                return new EconomyTransactionResult(false, SoftCurrency, sourceType);
            }

            if (_saveData.ledger.ContainsTransaction(transactionId))
            {
                return new EconomyTransactionResult(true, SoftCurrency, sourceType, transactionId, true);
            }

            _saveData.economy.softCurrency += reward.Credits;
            _saveData.profile.commanderExperience += reward.CommanderExperience;
            var appliedBlueprints = !string.IsNullOrWhiteSpace(reward.BlueprintWeaponId)
                ? reward.Blueprints
                : 0;
            if (appliedBlueprints > 0)
            {
                _saveData.inventory.GetOrCreateWeaponProgress(reward.BlueprintWeaponId).blueprints += appliedBlueprints;
            }

            var recordedId = RecordTransaction(
                transactionId,
                sourceType,
                sourceId,
                reward.Credits,
                0,
                reward.CommanderExperience,
                reward.BlueprintWeaponId,
                appliedBlueprints);
            ProgressionChanged?.Invoke();
            BalanceChanged?.Invoke();
            return new EconomyTransactionResult(true, SoftCurrency, sourceType, recordedId, false);
        }

        public bool CanSpend(int amount)
        {
            return AdminModeEnabled || (amount >= 0 && SoftCurrency >= amount);
        }

        public EconomyTransactionResult GrantPremium(int amount, string sourceType, string sourceId, string transactionId)
        {
            if (amount < 0)
            {
                return new EconomyTransactionResult(false, PremiumCurrency, sourceType);
            }

            if (_saveData.ledger.ContainsTransaction(transactionId))
            {
                return new EconomyTransactionResult(true, PremiumCurrency, sourceType, transactionId, true);
            }

            _saveData.economy.premiumCurrency += amount;
            var recordedId = RecordTransaction(transactionId, sourceType, sourceId, 0, amount, 0, string.Empty, 0);
            BalanceChanged?.Invoke();
            return new EconomyTransactionResult(true, PremiumCurrency, sourceType, recordedId, false);
        }

        public bool CanSpendPremium(int amount)
        {
            return AdminModeEnabled || (amount >= 0 && PremiumCurrency >= amount);
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

            if (AdminModeEnabled)
            {
                return new EconomyTransactionResult(true, SoftCurrency, sourceType, transactionId, false);
            }

            _saveData.economy.softCurrency -= amount;
            var recordedId = RecordTransaction(transactionId, sourceType, sourceId, -amount, 0, 0, string.Empty, 0);
            BalanceChanged?.Invoke();
            return new EconomyTransactionResult(true, SoftCurrency, sourceType, recordedId, false);
        }

        public EconomyTransactionResult TrySpendPremium(int amount, string sourceType, string sourceId, string transactionId)
        {
            if (amount < 0)
            {
                return new EconomyTransactionResult(false, PremiumCurrency, sourceType);
            }

            if (_saveData.ledger.ContainsTransaction(transactionId))
            {
                return new EconomyTransactionResult(true, PremiumCurrency, sourceType, transactionId, true);
            }

            if (!CanSpendPremium(amount))
            {
                return new EconomyTransactionResult(false, PremiumCurrency, sourceType);
            }

            if (AdminModeEnabled)
            {
                return new EconomyTransactionResult(true, PremiumCurrency, sourceType, transactionId, false);
            }

            _saveData.economy.premiumCurrency -= amount;
            var recordedId = RecordTransaction(transactionId, sourceType, sourceId, 0, -amount, 0, string.Empty, 0);
            BalanceChanged?.Invoke();
            return new EconomyTransactionResult(true, PremiumCurrency, sourceType, recordedId, false);
        }

        public EconomyTransactionResult TrySpendWeaponUpgrade(
            int credits,
            string weaponId,
            int blueprints,
            string sourceType,
            string sourceId,
            string transactionId)
        {
            if (credits < 0 || blueprints < 0 || string.IsNullOrWhiteSpace(weaponId))
            {
                return new EconomyTransactionResult(false, SoftCurrency, sourceType);
            }

            if (_saveData.ledger.ContainsTransaction(transactionId))
            {
                return new EconomyTransactionResult(true, SoftCurrency, sourceType, transactionId, true);
            }

            var progress = _saveData.inventory.GetOrCreateWeaponProgress(weaponId);
            if (!AdminModeEnabled && (!CanSpend(credits) || progress.blueprints < blueprints))
            {
                return new EconomyTransactionResult(false, SoftCurrency, sourceType);
            }

            if (AdminModeEnabled)
            {
                return new EconomyTransactionResult(true, SoftCurrency, sourceType, transactionId, false);
            }

            _saveData.economy.softCurrency -= credits;
            progress.blueprints -= blueprints;
            var recordedId = RecordTransaction(
                transactionId,
                sourceType,
                sourceId,
                -credits,
                0,
                0,
                weaponId,
                -blueprints);
            BalanceChanged?.Invoke();
            return new EconomyTransactionResult(true, SoftCurrency, sourceType, recordedId, false);
        }

        private string RecordTransaction(
            string transactionId,
            string sourceType,
            string sourceId,
            int softCurrencyDelta,
            int premiumCurrencyDelta,
            int commanderExperienceDelta,
            string blueprintWeaponId,
            int blueprintDelta)
        {
            var recordedId = string.IsNullOrWhiteSpace(transactionId)
                ? Guid.NewGuid().ToString("N")
                : transactionId;

            _saveData.ledger.Append(new SaveLedgerEntry
            {
                transactionId = recordedId,
                sourceType = sourceType ?? string.Empty,
                sourceId = sourceId ?? string.Empty,
                softCurrencyDelta = softCurrencyDelta,
                premiumCurrencyDelta = premiumCurrencyDelta,
                commanderExperienceDelta = commanderExperienceDelta,
                blueprintWeaponId = blueprintWeaponId ?? string.Empty,
                blueprintDelta = blueprintDelta,
                createdUtcTicks = DateTime.UtcNow.Ticks,
                pendingSync = true
            });

            return recordedId;
        }
    }
}
