using System;

namespace Game.Data.Save
{
    /// <summary>
    /// Static bridge so battle-layer code can read the player's saved weapon rank/branch
    /// without a compile-time dependency on the app assembly (which owns the live
    /// <c>ISaveService</c> instance). The app layer wires <see cref="Resolver"/> during
    /// startup, mirroring the Game.Core.Battle bridges (BattleHudFactoryBridge etc.) used
    /// for the same App-to-Gameplay handoff.
    /// </summary>
    public static class WeaponProgressionBridge
    {
        public static Func<string, SaveWeaponProgressData> Resolver { get; set; }
        public static Func<string, bool> UnlockedResolver { get; set; }

        public static SaveWeaponProgressData Resolve(string weaponId)
        {
            return Resolver?.Invoke(weaponId);
        }

        /// <summary>
        /// Whether the player owns this weapon (unlocked via shop purchase or a
        /// campaign reward). Fails open (true) when the resolver isn't wired yet -
        /// e.g. EditMode tests that build a battle without going through
        /// AppServices.Initialize().
        /// </summary>
        public static bool IsWeaponUnlocked(string weaponId)
        {
            return UnlockedResolver?.Invoke(weaponId) ?? true;
        }
    }
}
