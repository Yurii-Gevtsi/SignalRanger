using System;
using Game.Data.Progression;
using Game.Data.Save;
using Game.Economy;

namespace Game.Progression
{
    public readonly struct WeaponUpgradeResult
    {
        public WeaponUpgradeResult(bool applied, int rank, WeaponUpgradeBranch branch, WeaponUpgradeCost cost,
            int requiredCommanderLevel = 1)
        {
            Applied = applied;
            Rank = rank;
            Branch = branch;
            Cost = cost;
            RequiredCommanderLevel = requiredCommanderLevel < 1 ? 1 : requiredCommanderLevel;
        }

        public bool Applied { get; }
        public int Rank { get; }
        public WeaponUpgradeBranch Branch { get; }
        public WeaponUpgradeCost Cost { get; }
        public int RequiredCommanderLevel { get; }
    }

    public sealed class WeaponUpgradeService
    {
        private readonly SaveData _saveData;
        private readonly IEconomyService _economy;

        public WeaponUpgradeService(SaveData saveData, IEconomyService economy)
        {
            _saveData = saveData ?? throw new ArgumentNullException(nameof(saveData));
            _economy = economy ?? throw new ArgumentNullException(nameof(economy));
            _saveData.EnsureDefaults();
        }

        public WeaponUpgradeResult TryUpgrade(string weaponId, WeaponUpgradeBranch branch = WeaponUpgradeBranch.None)
        {
            if (string.IsNullOrWhiteSpace(weaponId)
                || !_saveData.inventory.unlockedWeaponIds.Contains(weaponId)
                || !WeaponProgressionCatalog.TryGet(weaponId, out var definition))
            {
                return default;
            }

            var progress = _saveData.inventory.GetOrCreateWeaponProgress(weaponId);
            if (progress.rank >= 10)
            {
                return new WeaponUpgradeResult(false, progress.rank, (WeaponUpgradeBranch)progress.branch, default);
            }

            var targetRank = progress.rank + 1;
            var selectedBranch = targetRank < 5
                ? WeaponUpgradeBranch.None
                : progress.branch == 0 ? branch : (WeaponUpgradeBranch)progress.branch;
            if (targetRank >= 5 && selectedBranch == WeaponUpgradeBranch.None)
            {
                return new WeaponUpgradeResult(false, progress.rank, WeaponUpgradeBranch.None, default);
            }

            if (!definition.TryGetNode(targetRank, selectedBranch, out _)
                || !WeaponUpgradeEconomy.TryGetCost(targetRank, out var cost))
            {
                return new WeaponUpgradeResult(false, progress.rank, selectedBranch, default);
            }

            var requiredCommanderLevel = WeaponUpgradeEconomy.GetRequiredCommanderLevel(definition, targetRank);
            if (CommanderProgressionTable.GetLevel(_economy.CommanderExperience) < requiredCommanderLevel)
            {
                return new WeaponUpgradeResult(false, progress.rank, selectedBranch, cost, requiredCommanderLevel);
            }

            // A reset intentionally lets this same rank be bought again. The reset cycle
            // keeps this purchase idempotent without colliding with its old purchase.
            var transactionId = $"weapon.upgrade.{weaponId}.cycle.{progress.resetCycle}.r{targetRank}.{(int)selectedBranch}";
            var spend = _economy.TrySpendWeaponUpgrade(
                cost.Credits,
                weaponId,
                cost.Blueprints,
                "weapon.upgrade",
                $"{weaponId}.rank.{targetRank}",
                transactionId);
            if (!spend.Success || spend.WasDuplicate)
            {
                return new WeaponUpgradeResult(false, progress.rank, selectedBranch, cost, requiredCommanderLevel);
            }

            progress.rank = targetRank;
            if (targetRank == 5)
            {
                progress.branch = (int)selectedBranch;
            }

            return new WeaponUpgradeResult(true, progress.rank, selectedBranch, cost, requiredCommanderLevel);
        }

        /// <summary>
        /// Reverts a weapon's modernization back to rank 1 and refunds 50% of the
        /// credits/blueprints spent reaching its current rank. Branch choice is cleared
        /// so the player can pick again on the next rank-5 upgrade.
        /// </summary>
        public bool TryReset(string weaponId)
        {
            if (string.IsNullOrWhiteSpace(weaponId)
                || !_saveData.inventory.unlockedWeaponIds.Contains(weaponId))
            {
                return false;
            }

            var progress = _saveData.inventory.GetOrCreateWeaponProgress(weaponId);
            if (progress.rank <= 1)
            {
                return false;
            }

            var (spentCredits, spentBlueprints) = WeaponUpgradeEconomy.GetCumulativeCost(progress.rank);
            var refundCredits = spentCredits / 2;
            var refundBlueprints = spentBlueprints / 2;

            progress.rank = 1;
            progress.branch = 0;
            progress.blueprints += refundBlueprints;
            progress.resetCycle++;

            if (refundCredits > 0)
            {
                _economy.Grant(refundCredits, "weapon.upgrade.reset", weaponId, null);
            }

            return true;
        }
    }
}
