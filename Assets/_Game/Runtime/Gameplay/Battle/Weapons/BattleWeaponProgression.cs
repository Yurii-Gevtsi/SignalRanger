using System;
using System.Collections.Generic;
using Game.Data.Progression;
using Game.Data.Save;

namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>
    /// How developed one side's weapons are in this battle: the rank and selected
    /// TTH choices each one fights at, plus legacy evolution mechanics when a saved
    /// branch is present.
    /// <para>
    /// This exists so weapon development is a property of the combatant rather than a
    /// lookup into the player's save. The battle used to read
    /// <see cref="WeaponProgressionBridge"/> directly, which meant only the player
    /// could ever be developed - fine while every opponent was a rank-1 story bot,
    /// wrong once raids and duels field opponents scaled to a commander level.
    /// </para>
    /// </summary>
    public sealed class BattleWeaponProgression
    {
        private readonly Dictionary<string, WeaponEvolutionProfile> _evolutions = new(StringComparer.Ordinal);
        private readonly Dictionary<string, WeaponUpgradeModifiers> _modifiers = new(StringComparer.Ordinal);
        private readonly Dictionary<string, SaveWeaponProgressData> _progress = new(StringComparer.Ordinal);

        private BattleWeaponProgression()
        {
        }

        /// <summary>A side whose weapons are all at base rank, with no bonuses at all.</summary>
        public static BattleWeaponProgression Empty { get; } = new();

        public bool IsEmpty => _progress.Count == 0;

        /// <summary>
        /// Resolves progression for every weapon <paramref name="resolver"/> knows
        /// about. Weapons it returns null for stay at base rank, so a partially known
        /// loadout degrades to "undeveloped" rather than failing.
        /// </summary>
        public static BattleWeaponProgression FromResolver(
            IEnumerable<WeaponDefinition> weapons,
            Func<string, SaveWeaponProgressData> resolver)
        {
            var progression = new BattleWeaponProgression();
            if (weapons == null || resolver == null)
            {
                return progression;
            }

            foreach (var weapon in weapons)
            {
                if (weapon == null || string.IsNullOrWhiteSpace(weapon.WeaponId))
                {
                    continue;
                }

                var progress = resolver(weapon.WeaponId);
                if (progress == null)
                {
                    continue;
                }

                progression.Add(weapon, progress);
            }

            return progression;
        }

        /// <summary>
        /// Builds progression from parallel id/rank/branch arrays, the shape the
        /// session context carries for an opponent. Ranks and branches shorter than
        /// the id list simply leave the remaining weapons at base rank.
        /// </summary>
        public static BattleWeaponProgression FromRanks(
            IReadOnlyDictionary<string, WeaponDefinition> weaponsById,
            IReadOnlyList<string> weaponIds,
            IReadOnlyList<int> ranks,
            IReadOnlyList<int> branches)
        {
            var progression = new BattleWeaponProgression();
            if (weaponsById == null || weaponIds == null)
            {
                return progression;
            }

            for (var i = 0; i < weaponIds.Count; i++)
            {
                var weaponId = weaponIds[i];
                if (string.IsNullOrWhiteSpace(weaponId) || !weaponsById.TryGetValue(weaponId, out var weapon))
                {
                    continue;
                }

                var rank = ranks != null && i < ranks.Count ? ranks[i] : 1;
                if (rank <= 1)
                {
                    continue;
                }

                progression.Add(weapon, new SaveWeaponProgressData
                {
                    weaponId = weaponId,
                    rank = rank,
                    branch = branches != null && i < branches.Count ? branches[i] : 0
                });
            }

            return progression;
        }

        /// <summary>
        /// Progression stated directly as evolution profiles, for cases that already
        /// know the mechanics and have no rank to derive them from - chiefly tests
        /// pinning one specific evolution.
        /// </summary>
        public static BattleWeaponProgression FromEvolutions(
            IReadOnlyDictionary<string, WeaponEvolutionProfile> evolutions)
        {
            var progression = new BattleWeaponProgression();
            if (evolutions == null)
            {
                return progression;
            }

            foreach (var pair in evolutions)
            {
                if (string.IsNullOrWhiteSpace(pair.Key))
                {
                    continue;
                }

                progression._evolutions[pair.Key] = pair.Value;
                progression._progress[pair.Key] = new SaveWeaponProgressData
                {
                    weaponId = pair.Key,
                    rank = 1
                };
            }

            return progression;
        }

        public WeaponEvolutionProfile ResolveEvolution(string weaponId)
        {
            return weaponId != null && _evolutions.TryGetValue(weaponId, out var evolution)
                ? evolution
                : WeaponEvolutionProfile.Base;
        }

        public WeaponUpgradeModifiers ResolveModifiers(string weaponId)
        {
            return weaponId != null && _modifiers.TryGetValue(weaponId, out var modifiers)
                ? modifiers
                : default;
        }

        public int ResolveUpgradeCount(string weaponId, WeaponUpgradePath path)
        {
            return weaponId != null && _progress.TryGetValue(weaponId, out var progress)
                ? WeaponUpgradeChoiceResolver.Count(progress, path)
                : 0;
        }

        public bool HasPurchasedEvolution(string weaponId, int milestoneRank, string optionId)
        {
            return _progress.TryGetValue(weaponId, out var progress) && progress.rank >= milestoneRank
                && WeaponProgressionCatalog.TryGet(weaponId, out var definition)
                && WeaponEvolutionSelectionResolver.TryGetChoice(definition, progress, milestoneRank, out var option)
                && option.Id == optionId && definition.GetSpecialPurchaseCount(progress, milestoneRank) > 0;
        }

        /// <summary>Rank this side fights the weapon at; 1 when it is undeveloped.</summary>
        public int ResolveRank(string weaponId)
        {
            return weaponId != null && _progress.TryGetValue(weaponId, out var progress) ? progress.rank : 1;
        }

        /// <summary>Evolution branch chosen for the weapon; 0 when none has been.</summary>
        public int ResolveBranch(string weaponId)
        {
            return weaponId != null && _progress.TryGetValue(weaponId, out var progress) ? progress.branch : 0;
        }

        /// <summary>The purchased rank-five choice owns the visible weapon body.</summary>
        public string ResolveCurrentIconId(WeaponDefinition weapon, string iconId = null)
        {
            if (weapon == null)
            {
                return iconId;
            }

            iconId ??= weapon.IconId;
            if (string.IsNullOrWhiteSpace(iconId)
                || !_progress.TryGetValue(weapon.WeaponId, out var progress)
                || progress.rank < 5
                || !WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var definition))
            {
                return iconId;
            }

            var resource = WeaponEvolutionIconResolver.ResolveCurrentIconResource(iconId, definition, progress);
            const string prefix = "UI/Weapons/";
            return resource.StartsWith(prefix, StringComparison.Ordinal)
                ? resource.Substring(prefix.Length)
                : iconId;
        }

        /// <summary>Highest rank across the whole loadout, for a one-glance readout.</summary>
        public int ResolveHighestRank()
        {
            var highest = 1;
            foreach (var progress in _progress.Values)
            {
                if (progress.rank > highest)
                {
                    highest = progress.rank;
                }
            }

            return highest;
        }

        private void Add(WeaponDefinition weapon, SaveWeaponProgressData progress)
        {
            progress.EnsureDefaults();
            _progress[weapon.WeaponId] = progress;
            _evolutions[weapon.WeaponId] = WeaponEvolutionRuntimeResolver.Resolve(weapon, progress);
            _modifiers[weapon.WeaponId] = WeaponUpgradeModifierResolver.Resolve(weapon, progress);
        }
    }
}
