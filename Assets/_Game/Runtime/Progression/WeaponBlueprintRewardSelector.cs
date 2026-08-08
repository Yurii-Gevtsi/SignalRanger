using System;
using System.Collections.Generic;
using Game.Data.Progression;
using Game.Data.Save;

namespace Game.Progression
{
    /// <summary>
    /// Picks which weapon a "chip" (blueprint) reward applies to, wherever it is
    /// granted from - daily login, quests, loot chests. Per the economy design the
    /// target is random among weapons the player has unlocked and hasn't fully
    /// modernized yet (rank &lt; 10); a maxed-out weapon has nothing left to spend
    /// blueprints on, so it is never selected.
    /// </summary>
    public static class WeaponBlueprintRewardSelector
    {
        public static bool TrySelectEligibleWeaponId(SaveData saveData, Random random, out string weaponId)
        {
            weaponId = string.Empty;
            if (saveData?.inventory?.unlockedWeaponIds == null)
            {
                return false;
            }

            var candidates = new List<string>();
            var ids = saveData.inventory.unlockedWeaponIds;
            for (var i = 0; i < ids.Count; i++)
            {
                var id = ids[i];
                if (!WeaponProgressionCatalog.TryGet(id, out _))
                {
                    continue;
                }

                var progress = saveData.inventory.GetOrCreateWeaponProgress(id);
                if (progress.rank < 10)
                {
                    candidates.Add(id);
                }
            }

            if (candidates.Count == 0)
            {
                return false;
            }

            random ??= new Random();
            weaponId = candidates[random.Next(candidates.Count)];
            return true;
        }
    }
}
