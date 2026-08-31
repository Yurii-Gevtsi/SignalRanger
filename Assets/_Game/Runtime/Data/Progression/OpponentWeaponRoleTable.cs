using System;
using System.Collections.Generic;

namespace Game.Data.Progression
{
    /// <summary>Which of the three opponent loadout slots a weapon is authored for.</summary>
    public enum OpponentWeaponSlot
    {
        /// <summary>Reliable damage; the opponent's main answer at any range.</summary>
        Primary = 0,

        /// <summary>Terrain, area, control or repositioning; used sparingly.</summary>
        Tactical = 1,

        /// <summary>Weak, unlimited, always available so a turn is never wasted.</summary>
        Fallback = 2
    }

    /// <summary>
    /// Authored slot for every weapon an AI opponent may carry. This is a table on
    /// purpose: the campaign AI strategy requires opponent loadouts to come from
    /// data rather than from bot heuristics, so that tuning a fight never means
    /// editing behaviour code.
    /// </summary>
    public static class OpponentWeaponRoleTable
    {
        /// <summary>
        /// Unlimited low-damage weapon every opponent keeps. Deliberately weaker
        /// than the player's comparable basic weapon.
        /// </summary>
        public const string FallbackWeaponId = "weapon.impulse";

        private static readonly Dictionary<string, OpponentWeaponSlot> Slots = new(StringComparer.Ordinal)
        {
            { "weapon.impulse", OpponentWeaponSlot.Fallback },
            { "weapon.blaster-caset", OpponentWeaponSlot.Primary },
            { "weapon.bur", OpponentWeaponSlot.Primary },
            { "weapon.granata", OpponentWeaponSlot.Primary },
            { "weapon.mine", OpponentWeaponSlot.Tactical },
            { "weapon.c4", OpponentWeaponSlot.Tactical },
            { "weapon.heal", OpponentWeaponSlot.Tactical },
            { "weapon.teleport", OpponentWeaponSlot.Tactical },
            { "weapon.bur-rocket", OpponentWeaponSlot.Primary },
            { "weapon.mortira", OpponentWeaponSlot.Primary },
            { "weapon.emp", OpponentWeaponSlot.Tactical },
            { "weapon.freeze", OpponentWeaponSlot.Tactical },
            { "weapon.fire-gun", OpponentWeaponSlot.Primary },
            { "weapon.toxic", OpponentWeaponSlot.Tactical },
            { "weapon.missle", OpponentWeaponSlot.Primary },
            { "weapon.fpv-drone", OpponentWeaponSlot.Primary }
        };

        public static bool TryGetSlot(string weaponId, out OpponentWeaponSlot slot)
        {
            return Slots.TryGetValue(weaponId ?? string.Empty, out slot);
        }

        /// <summary>
        /// Weapons authored for <paramref name="slot"/> that a commander of
        /// <paramref name="commanderLevel"/> could plausibly field, in catalog order
        /// so the result never depends on dictionary iteration.
        /// </summary>
        public static List<string> GetCandidates(OpponentWeaponSlot slot, int commanderLevel)
        {
            var candidates = new List<string>();
            var catalog = WeaponProgressionCatalog.All;
            for (var i = 0; i < catalog.Count; i++)
            {
                var definition = catalog[i];
                if (definition == null || definition.RequiredCommanderLevel > commanderLevel)
                {
                    continue;
                }

                if (TryGetSlot(definition.WeaponId, out var authoredSlot) && authoredSlot == slot)
                {
                    candidates.Add(definition.WeaponId);
                }
            }

            return candidates;
        }
    }
}
