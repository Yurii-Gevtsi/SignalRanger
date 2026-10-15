using System;
using Game.Data.Progression;
using Game.Data.Save;
using Game.Economy;

namespace Game.Progression
{
    public readonly struct WeaponUpgradeResult
    {
        public WeaponUpgradeResult(bool applied, int rank, WeaponUpgradeBranch branch, WeaponUpgradeCost cost,
            int requiredCommanderLevel = 1, WeaponUpgradePath path = WeaponUpgradePath.None)
        {
            Applied = applied;
            Rank = rank;
            Branch = branch;
            Cost = cost;
            RequiredCommanderLevel = requiredCommanderLevel < 1 ? 1 : requiredCommanderLevel;
            Path = path;
        }

        public bool Applied { get; }
        public int Rank { get; }
        public WeaponUpgradeBranch Branch { get; }
        public WeaponUpgradeCost Cost { get; }
        public int RequiredCommanderLevel { get; }
        public WeaponUpgradePath Path { get; }
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

        public WeaponUpgradeResult TryUpgrade(
            string weaponId,
            WeaponUpgradePath path,
            bool useCrystals = false)
        {
            if (string.IsNullOrWhiteSpace(weaponId)
                || !_saveData.inventory.unlockedWeaponIds.Contains(weaponId)
                || !WeaponProgressionCatalog.TryGet(weaponId, out var definition)
                || !WeaponUpgradeEconomy.IsValidPath(path))
            {
                return default;
            }

            var progress = _saveData.inventory.GetOrCreateWeaponProgress(weaponId);
            if (!definition.CanPurchaseBaseUpgrade(progress, path))
            {
                return new WeaponUpgradeResult(false, progress.rank, (WeaponUpgradeBranch)progress.branch, default,
                    path: path);
            }

            if (progress.rank >= definition.MaxRank)
            {
                return new WeaponUpgradeResult(false, progress.rank, (WeaponUpgradeBranch)progress.branch, default,
                    path: path);
            }

            var targetRank = progress.rank + 1;
            var selectedBranch = (WeaponUpgradeBranch)progress.branch;
            if (!WeaponUpgradeEconomy.TryGetCost(targetRank, path, out var cost))
            {
                return new WeaponUpgradeResult(false, progress.rank, selectedBranch, default, path: path);
            }

            var requiredCommanderLevel = WeaponUpgradeEconomy.GetFreeChoiceRequiredCommanderLevel(definition, targetRank);
            if (CommanderProgressionTable.GetLevel(_economy.CommanderExperience) < requiredCommanderLevel)
            {
                return new WeaponUpgradeResult(false, progress.rank, selectedBranch, cost, requiredCommanderLevel, path);
            }

            // A reset intentionally lets this same rank be bought again. The reset cycle
            // keeps this purchase idempotent without colliding with its old purchase.
            var currency = useCrystals ? "crystals" : "credits";
            var transactionId = $"weapon.upgrade.{weaponId}.cycle.{progress.resetCycle}.r{targetRank}.p{(int)path}.{currency}";
            var spend = useCrystals
                ? _economy.TrySpendPremium(
                    cost.Crystals,
                    "weapon.upgrade.premium",
                    $"{weaponId}.rank.{targetRank}.path.{path}",
                    transactionId)
                : _economy.TrySpendWeaponUpgrade(
                    cost.Credits,
                    weaponId,
                    cost.Blueprints,
                    "weapon.upgrade",
                    $"{weaponId}.rank.{targetRank}.path.{path}",
                    transactionId);
            if (!spend.Success || spend.WasDuplicate)
            {
                return new WeaponUpgradeResult(false, progress.rank, selectedBranch, cost, requiredCommanderLevel, path);
            }

            progress.rank = targetRank;
            progress.upgradeChoices ??= new System.Collections.Generic.List<SaveWeaponUpgradeChoiceData>();
            progress.upgradeChoices.RemoveAll(choice => choice != null && choice.rank == targetRank);
            progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
            {
                rank = targetRank,
                path = (int)path
            });

            return new WeaponUpgradeResult(true, progress.rank, selectedBranch, cost, requiredCommanderLevel, path);
        }

        /// <summary>
        /// Compatibility entry point for older callers. New UI and gameplay code
        /// must select one of the four free TTH paths explicitly.
        /// </summary>
        public WeaponUpgradeResult TryUpgrade(string weaponId, WeaponUpgradeBranch branch = WeaponUpgradeBranch.None)
        {
            return TryUpgrade(weaponId, WeaponUpgradePath.Core);
        }

        /// <summary>
        /// Purchases the initial evolution choice or one of its configured follow-up
        /// steps. Every paid step counts as a weapon rank.
        /// </summary>
        public WeaponUpgradeResult TryPurchaseEvolution(
            string weaponId,
            int milestoneRank,
            string optionId,
            bool useCrystals = false)
        {
            if (string.IsNullOrWhiteSpace(weaponId)
                || (milestoneRank != 5 && milestoneRank != 10)
                || string.IsNullOrWhiteSpace(optionId)
                || !_saveData.inventory.unlockedWeaponIds.Contains(weaponId)
                || !WeaponProgressionCatalog.TryGet(weaponId, out var definition)
                || !definition.TryGetEvolutionOption(milestoneRank, optionId, out var option))
            {
                return default;
            }

            var progress = _saveData.inventory.GetOrCreateWeaponProgress(weaponId);
            var hasChoice = HasEvolutionChoice(progress, milestoneRank);
            var purchasedSteps = definition.GetSpecialPurchaseCount(progress, milestoneRank);
            var purchaseLimit = definition.GetSpecialPurchaseLimit(milestoneRank);
            if (purchaseLimit <= 0
                || progress.rank >= definition.MaxRank
                || (!hasChoice && progress.rank < milestoneRank)
                || (hasChoice && !HasEvolutionChoice(progress, milestoneRank, optionId))
                || purchasedSteps >= purchaseLimit)
            {
                return new WeaponUpgradeResult(false, progress.rank, (WeaponUpgradeBranch)progress.branch, default);
            }

            var targetRank = progress.rank + 1;
            if (!WeaponUpgradeEconomy.TryGetSpecialCost(targetRank, milestoneRank, !hasChoice, out var cost))
            {
                return new WeaponUpgradeResult(false, progress.rank, (WeaponUpgradeBranch)progress.branch, default);
            }

            var requiredCommanderLevel = WeaponUpgradeEconomy.GetFreeChoiceRequiredCommanderLevel(definition, targetRank);
            if (CommanderProgressionTable.GetLevel(_economy.CommanderExperience) < requiredCommanderLevel)
            {
                return new WeaponUpgradeResult(false, progress.rank, (WeaponUpgradeBranch)progress.branch, cost,
                    requiredCommanderLevel);
            }

            var currency = useCrystals ? "crystals" : "credits";
            var transactionId = $"weapon.evolution.{weaponId}.cycle.{progress.resetCycle}.r{targetRank}.m{milestoneRank}.s{purchasedSteps + 1}.{optionId}.{currency}";
            var spend = useCrystals
                ? _economy.TrySpendPremium(cost.Crystals, "weapon.evolution.premium", $"{weaponId}.evolution.{optionId}", transactionId)
                : _economy.TrySpendWeaponUpgrade(cost.Credits, weaponId, cost.Blueprints,
                    "weapon.evolution", $"{weaponId}.evolution.{optionId}", transactionId);
            if (!spend.Success || spend.WasDuplicate)
            {
                return new WeaponUpgradeResult(false, progress.rank, (WeaponUpgradeBranch)progress.branch, cost);
            }

            if (!hasChoice)
            {
                progress.evolutionChoices ??= new System.Collections.Generic.List<SaveWeaponEvolutionChoiceData>();
                progress.evolutionChoices.RemoveAll(choice => choice != null && choice.rank == milestoneRank);
                progress.evolutionChoices.Add(new SaveWeaponEvolutionChoiceData { rank = milestoneRank, optionId = optionId });
            }
            progress.specialUpgrades ??= new System.Collections.Generic.List<SaveWeaponSpecialUpgradeData>();
            progress.specialUpgrades.RemoveAll(upgrade => upgrade != null && upgrade.rank == milestoneRank);
            progress.specialUpgrades.Add(new SaveWeaponSpecialUpgradeData
            {
                rank = milestoneRank,
                purchaseCount = purchasedSteps + 1
            });
            progress.rank = targetRank;
            // Kept only as a legacy representation for old callers/saves.
            if (milestoneRank == 5)
            {
                progress.branch = (int)option.LegacyBranch;
            }
            return new WeaponUpgradeResult(true, progress.rank, option.LegacyBranch, cost, requiredCommanderLevel);
        }

        /// <summary>
        /// Reverts a weapon's modernization back to rank 1 and refunds 50% of the
        /// baseline credits/chips spent reaching its current rank. The per-rank
        /// parameter choices are cleared so the player can build a new path.
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
            progress.upgradeChoices?.Clear();
            progress.evolutionChoices?.Clear();
            progress.specialUpgrades?.Clear();
            progress.blueprints += refundBlueprints;
            progress.resetCycle++;

            if (refundCredits > 0)
            {
                _economy.Grant(refundCredits, "weapon.upgrade.reset", weaponId, null);
            }

            return true;
        }

        private static bool HasEvolutionChoice(SaveWeaponProgressData progress, int milestoneRank)
        {
            if (progress?.evolutionChoices == null)
            {
                return false;
            }

            for (var index = 0; index < progress.evolutionChoices.Count; index++)
            {
                if (progress.evolutionChoices[index]?.rank == milestoneRank)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool HasEvolutionChoice(SaveWeaponProgressData progress, int milestoneRank, string optionId)
        {
            if (progress?.evolutionChoices == null)
            {
                return false;
            }

            for (var index = 0; index < progress.evolutionChoices.Count; index++)
            {
                var choice = progress.evolutionChoices[index];
                if (choice != null && choice.rank == milestoneRank
                    && string.Equals(choice.optionId, optionId, StringComparison.Ordinal))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
