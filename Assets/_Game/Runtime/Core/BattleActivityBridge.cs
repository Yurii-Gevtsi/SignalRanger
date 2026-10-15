using System;

namespace Game.Core
{
    /// <summary>
    /// Optional application-level sink for gameplay activity. The battle layer
    /// reports semantic events without taking a dependency on save or UI code.
    /// </summary>
    public static class BattleActivityBridge
    {
        public static Action<string> ShotFired { get; set; }
        public static Action<int> PlayerDamageDealt { get; set; }
        public static Action<int> TerrainDestroyed { get; set; }
        public static Action<float, bool> PlayerMoved { get; set; }
        public static Action<int> ResourcesCollected { get; set; }
        public static Action<bool, bool, bool> BattleFinished { get; set; }

        public static void ReportShotFired(string weaponId) => ShotFired?.Invoke(weaponId);
        public static void ReportPlayerDamageDealt(int amount) => PlayerDamageDealt?.Invoke(amount);
        public static void ReportTerrainDestroyed(int cells) => TerrainDestroyed?.Invoke(cells);
        public static void ReportPlayerMoved(float distance, bool wasJumping) => PlayerMoved?.Invoke(distance, wasJumping);
        public static void ReportResourcesCollected(int count) => ResourcesCollected?.Invoke(count);
        public static void ReportBattleFinished(bool playerWon, bool isStoryBattle, bool isArenaBattle) =>
            BattleFinished?.Invoke(playerWon, isStoryBattle, isArenaBattle);
    }
}
