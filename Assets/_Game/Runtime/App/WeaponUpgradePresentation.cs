using Game.Core.Localization;
using Game.Data.Progression;
using Game.Gameplay.Battle.Weapons;
using UnityEngine;

namespace Game.App
{
    /// <summary>Player-facing workshop text resolved exclusively through localization.</summary>
    public static class WeaponUpgradePresentation
    {
        public static string GetTitle(WeaponProgressionDefinition definition, int rank, WeaponUpgradeBranch branch)
        {
            if (rank == 1)
            {
                return GameText.UpgradeBaseConfigTitle();
            }

            if (!definition.TryGetNode(rank, branch, out var node))
            {
                return GameText.UpgradeGenericTitle();
            }

            if (node.IsEvolution)
            {
                if (!string.IsNullOrWhiteSpace(node.EvolutionName))
                {
                    return GameText.WeaponEvolutionName(node.MechanicId, node.EvolutionName);
                }

                return rank == 5 ? GameText.UpgradeEvolutionChoiceTitle() : GameText.UpgradeFinalEvolutionTitle();
            }

            return GameText.UpgradeStatBonusTitle(GetStatName(node.Stat), node.Amount);
        }

        public static string GetDescription(WeaponProgressionDefinition definition, int rank, WeaponUpgradeBranch branch)
        {
            if (rank == 1 || !definition.TryGetNode(rank, branch, out var node))
            {
                return string.Empty;
            }

            if (!node.IsEvolution)
            {
                return GameText.UpgradeStatImprovement(GetStatName(node.Stat), node.Amount);
            }

            return GameText.WeaponEvolutionDescription(node.MechanicId, string.Empty);
        }

        public static string GetActiveEvolutionText(WeaponProgressionDefinition definition, int rank, WeaponUpgradeBranch branch)
        {
            if (definition == null || rank < 5 || branch == WeaponUpgradeBranch.None)
            {
                return string.Empty;
            }

            return GetDescription(definition, rank >= 10 ? 10 : 5, branch);
        }

        public static int ResolveCurrentDamage(WeaponDefinition weapon, WeaponProgressionDefinition definition, int rank, WeaponUpgradeBranch branch)
        {
            if (weapon == null)
            {
                return 0;
            }

            if (definition == null)
            {
                return weapon.Damage;
            }

            var bonusPercent = 0;
            foreach (var node in definition.Nodes)
            {
                if (node.Rank <= rank
                    && node.Stat == WeaponUpgradeStat.Damage
                    && (node.Branch == WeaponUpgradeBranch.None || node.Branch == branch))
                {
                    bonusPercent += node.Amount;
                }
            }

            return Mathf.RoundToInt(weapon.Damage * (1f + bonusPercent / 100f));
        }

        public static string GetWeaponName(WeaponProgressionDefinition definition, int rank,
            WeaponUpgradeBranch branch, string fallbackName)
        {
            if (definition == null || rank < 5 || branch == WeaponUpgradeBranch.None)
            {
                return fallbackName;
            }

            var evolutionRank = rank >= 10 ? 10 : 5;
            return definition.TryGetNode(evolutionRank, branch, out var node)
                && !string.IsNullOrWhiteSpace(node.EvolutionName)
                    ? GameText.WeaponEvolutionName(node.MechanicId, node.EvolutionName)
                    : fallbackName;
        }

        public static string GetStatName(WeaponUpgradeStat stat) =>
            GameText.UpgradeStatName(stat.ToString(), string.Empty);
    }
}
