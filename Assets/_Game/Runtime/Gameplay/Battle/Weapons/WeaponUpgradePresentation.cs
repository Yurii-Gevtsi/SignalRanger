using Game.Core.Localization;
using Game.Data.Progression;
using Game.Data.Save;
using UnityEngine;

namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>Shared localized presentation for weapon progression in gameplay and menus.</summary>
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

        public static string GetCurrentDescription(
            WeaponDefinition weapon,
            WeaponProgressionDefinition definition,
            SaveWeaponProgressData progress)
        {
            var baseDescription = WeaponPresentationText.Describe(weapon);
            if (definition == null || progress == null)
            {
                return baseDescription;
            }

            var description = weapon != null && weapon.WeaponId == "weapon.granata"
                && HasSelectedEvolution(definition, progress)
                ? string.Empty
                : baseDescription;
            AppendSelectedEvolutionDescription(definition, progress, 5, ref description);
            AppendSelectedEvolutionDescription(definition, progress, 10, ref description);
            return description;
        }

        private static void AppendSelectedEvolutionDescription(
            WeaponProgressionDefinition definition,
            SaveWeaponProgressData progress,
            int rank,
            ref string description)
        {
            if (WeaponEvolutionSelectionResolver.TryGetChoice(definition, progress, rank, out var option)
                && definition.TryGetNode(option.Rank, option.LegacyBranch, out var node))
            {
                var evolutionText = GameText.WeaponEvolutionDescription(node.MechanicId, string.Empty);
                if (!string.IsNullOrWhiteSpace(evolutionText))
                {
                    description += string.IsNullOrWhiteSpace(description)
                        ? evolutionText
                        : "\n\n" + evolutionText;
                }
            }
        }

        private static bool HasSelectedEvolution(WeaponProgressionDefinition definition,
            SaveWeaponProgressData progress)
        {
            return WeaponEvolutionSelectionResolver.TryGetChoice(definition, progress, 5, out _)
                || WeaponEvolutionSelectionResolver.TryGetChoice(definition, progress, 10, out _);
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
            return fallbackName;
        }

        public static string GetWeaponName(WeaponProgressionDefinition definition,
            SaveWeaponProgressData progress, string fallbackName)
        {
            return fallbackName;
        }

        public static string GetStatName(WeaponUpgradeStat stat) =>
            GameText.UpgradeStatName(stat.ToString(), string.Empty);
    }
}
