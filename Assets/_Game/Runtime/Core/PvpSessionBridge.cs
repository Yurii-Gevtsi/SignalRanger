using System;
using Game.Core.Modes;

namespace Game.Core
{
    /// <summary>
    /// Static bridge so the battle layer can reach the duel transport without a
    /// compile-time dependency on the app assembly that owns it, mirroring
    /// <see cref="BattleRewardBridge"/> and the other App-to-Gameplay bridges.
    /// </summary>
    public static class PvpSessionBridge
    {
        public static Func<IPvpSessionService> Resolver { get; set; }

        /// <summary>
        /// The live duel transport, or null when the game is offline - in which case
        /// a duel simply plays out locally against the opponent it was given.
        /// </summary>
        public static IPvpSessionService Resolve()
        {
            return Resolver?.Invoke();
        }
    }
}
