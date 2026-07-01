using System.Collections.Generic;
using System.Linq;

namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>
    /// Decides which weapons are available in a battle from the player's chosen loadout
    /// and the mission's allowed weapons. Pure logic (ids only) so it is edit-mode tested.
    /// </summary>
    /// <remarks>
    /// Rules, in order:
    ///  - Prefer the player's loadout, restricted to the mission's allowed weapons.
    ///  - Never return empty: fall back to mission-allowed, then to every weapon,
    ///    so a mission is always playable even with an incompatible loadout.
    ///  - Output order follows the preferred source so the HUD is stable.
    /// </remarks>
    public static class BattleLoadoutResolver
    {
        public static IReadOnlyList<string> Resolve(
            IReadOnlyList<string> playerLoadout,
            IReadOnlyList<string> missionAllowed,
            IReadOnlyList<string> allWeaponIds)
        {
            var loadout = Clean(playerLoadout);
            var allowed = Clean(missionAllowed);
            var all = Clean(allWeaponIds);

            List<string> effective;
            if (allowed.Count > 0 && loadout.Count > 0)
            {
                var allowedSet = new HashSet<string>(allowed);
                effective = loadout.Where(allowedSet.Contains).ToList();
            }
            else if (loadout.Count > 0)
            {
                effective = loadout;
            }
            else
            {
                effective = allowed;
            }

            if (effective.Count == 0)
            {
                effective = allowed.Count > 0 ? allowed : all;
            }

            return effective;
        }

        private static List<string> Clean(IReadOnlyList<string> ids)
        {
            if (ids == null)
            {
                return new List<string>();
            }

            var result = new List<string>(ids.Count);
            var seen = new HashSet<string>();
            foreach (var id in ids)
            {
                if (!string.IsNullOrWhiteSpace(id) && seen.Add(id))
                {
                    result.Add(id);
                }
            }

            return result;
        }
    }
}
