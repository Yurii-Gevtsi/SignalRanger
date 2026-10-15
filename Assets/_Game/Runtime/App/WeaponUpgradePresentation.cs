using Game.Core.Localization;
using Game.Data.Progression;
using Game.Data.Save;
using Game.Gameplay.Battle.Weapons;
using UnityEngine;

namespace Game.App
{
    /// <summary>Compatibility facade for menu callers; implementation lives in gameplay.</summary>
    public static class WeaponUpgradePresentation
    {
        public static string GetTitle(WeaponProgressionDefinition definition, int rank, WeaponUpgradeBranch branch)
            => global::Game.Gameplay.Battle.Weapons.WeaponUpgradePresentation.GetTitle(definition, rank, branch);

        public static string GetDescription(WeaponProgressionDefinition definition, int rank, WeaponUpgradeBranch branch)
            => global::Game.Gameplay.Battle.Weapons.WeaponUpgradePresentation.GetDescription(definition, rank, branch);

        public static string GetActiveEvolutionText(WeaponProgressionDefinition definition, int rank, WeaponUpgradeBranch branch)
            => global::Game.Gameplay.Battle.Weapons.WeaponUpgradePresentation.GetActiveEvolutionText(definition, rank, branch);

        public static string GetCurrentDescription(
            WeaponDefinition weapon,
            WeaponProgressionDefinition definition,
            SaveWeaponProgressData progress)
            => global::Game.Gameplay.Battle.Weapons.WeaponUpgradePresentation.GetCurrentDescription(weapon, definition, progress);

        public static int ResolveCurrentDamage(WeaponDefinition weapon, WeaponProgressionDefinition definition, int rank, WeaponUpgradeBranch branch)
            => global::Game.Gameplay.Battle.Weapons.WeaponUpgradePresentation.ResolveCurrentDamage(weapon, definition, rank, branch);

        public static string GetWeaponName(WeaponProgressionDefinition definition, int rank,
            WeaponUpgradeBranch branch, string fallbackName)
            => global::Game.Gameplay.Battle.Weapons.WeaponUpgradePresentation.GetWeaponName(definition, rank, branch, fallbackName);

        public static string GetWeaponName(WeaponProgressionDefinition definition,
            SaveWeaponProgressData progress, string fallbackName)
            => global::Game.Gameplay.Battle.Weapons.WeaponUpgradePresentation.GetWeaponName(definition, progress, fallbackName);

        public static string GetStatName(WeaponUpgradeStat stat) =>
            global::Game.Gameplay.Battle.Weapons.WeaponUpgradePresentation.GetStatName(stat);
    }
}
