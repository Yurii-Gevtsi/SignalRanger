using System;
using Game.Core.Battle;

namespace Game.Core
{
    /// <summary>
    /// Static bridge so battle-layer code can hand supply-box loot to the economy and
    /// read commander progress without a compile-time dependency on the app assembly
    /// (which owns the live services). The app layer wires the handlers during startup,
    /// mirroring <see cref="BattleActivityBridge"/> and the other App-to-Gameplay bridges.
    /// </summary>
    public static class BattleRewardBridge
    {
        public static Action<int, string, string> WeaponBlueprintsGranted { get; set; }
        public static Action<int, string, string> CoinsGranted { get; set; }
        public static Func<int> CommanderExperienceResolver { get; set; }

        /// <summary>
        /// Banks the reward for a finished raid or duel and reports what was paid.
        /// The battle knows how the fight went; the app layer owns the daily budget,
        /// the ledger and which weapon the blueprints land on.
        /// </summary>
        public static Func<BattleArenaRewardRequest, BattleArenaReward> ArenaRewardGranter { get; set; }

        public static void GrantWeaponBlueprints(int amount, string sourceType, string sourceId) =>
            WeaponBlueprintsGranted?.Invoke(amount, sourceType, sourceId);

        public static void GrantCoins(int amount, string sourceType, string sourceId) =>
            CoinsGranted?.Invoke(amount, sourceType, sourceId);

        /// <summary>
        /// Grants the arena reward, or <see cref="BattleArenaReward.None"/> when no app
        /// layer is wired - e.g. EditMode tests that run a battle standalone.
        /// </summary>
        public static BattleArenaReward GrantArenaReward(BattleArenaRewardRequest request) =>
            ArenaRewardGranter?.Invoke(request) ?? BattleArenaReward.None;

        /// <summary>
        /// The player's commander XP, or 0 when the resolver isn't wired yet - e.g.
        /// EditMode tests that build a battle without going through AppServices.Initialize().
        /// </summary>
        public static int CommanderExperience => CommanderExperienceResolver?.Invoke() ?? 0;
    }
}
