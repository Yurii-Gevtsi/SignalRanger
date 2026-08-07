using Game.Data.Progression;
using UnityEngine;

namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>
    /// Resolves the Resources path of a weapon's rank 5/10 evolution artwork, if any
    /// exists on disk. Shared between the workshop upgrade tree (small tree icons,
    /// big detail-card art) and the battle projectile visual (rank/branch-aware body
    /// sprite), so both stay on the same file-naming convention.
    /// </summary>
    public static class WeaponEvolutionIconResolver
    {
        public static string ResolveEvolutionIconResource(string iconId, int rank, WeaponUpgradeBranch branch)
        {
            if (string.IsNullOrWhiteSpace(iconId) || rank != 5 && rank != 10)
            {
                return string.Empty;
            }

            var branchCode = branch == WeaponUpgradeBranch.A ? "A" : "B";
            var candidates = new[]
            {
                $"UI/Weapons/{iconId}_{rank}{branchCode}",
                $"UI/Weapons/{iconId}_{rank}_{branchCode}",
                $"UI/Weapons/{iconId}_5_10_{branchCode}",
                $"UI/Weapons/{iconId}_5_10{branchCode}",
                $"UI/Weapons/{iconId}_{rank}"
            };

            for (var i = 0; i < candidates.Length; i++)
            {
                if (Resources.Load<Texture2D>(candidates[i]) != null)
                {
                    return candidates[i];
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// The icon a weapon should show right now, anywhere in the app (arsenal
        /// shelf, shop listing, battle HUD, workshop tree/carousel): the rank 5/10
        /// evolution art once the player has picked a branch at that rank, falling
        /// back to the base <paramref name="iconId"/> otherwise.
        /// </summary>
        public static string ResolveCurrentIconResource(string iconId, int rank, WeaponUpgradeBranch branch)
        {
            if (string.IsNullOrWhiteSpace(iconId))
            {
                return string.Empty;
            }

            var baseResource = $"UI/Weapons/{iconId}";
            if (rank < 5 || branch == WeaponUpgradeBranch.None)
            {
                return baseResource;
            }

            var evolvedRank = rank >= 10 ? 10 : 5;
            var evolved = ResolveEvolutionIconResource(iconId, evolvedRank, branch);
            return string.IsNullOrWhiteSpace(evolved) ? baseResource : evolved;
        }
    }
}
