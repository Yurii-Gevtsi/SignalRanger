namespace Game.Gameplay.Battle.AI
{
    /// <summary>
    /// Runtime default bot profiles per difficulty, mirroring how the weapon
    /// library seeds prototype content. Real projects can override these with
    /// authored <see cref="BotProfile"/> assets later; these keep the game
    /// playable with no assets present.
    /// </summary>
    public static class BotProfileLibrary
    {
        public static BotProfile For(AiDifficulty difficulty)
        {
            return difficulty switch
            {
                AiDifficulty.Hard => Hard(),
                AiDifficulty.Normal => Normal(),
                _ => Easy()
            };
        }

        /// <summary>
        /// Easy leaves the aiming to the heuristic shooter (which adds its own
        /// error), so scatter is disabled and every shot is treated as "perfect"
        /// by the scatter stage. Movement is off — the tutorial opponent stands.
        /// </summary>
        public static BotProfile Easy()
        {
            return BotProfile.CreateRuntime(
                powerSteps: 4,
                angleStepDegrees: 3f,
                minClearPathRatio: 0.6f,
                scatterBase: 0f,
                scatterPerDistance: 0f,
                perfectShotChance: 1f,
                scoreJitter: 0.6f,
                allySafetyWeight: 0.5f,
                evaluateAllWeapons: false,
                movementEnabled: false,
                jumpEnabled: false,
                minEnemySeparation: 0f,
                maxWalkBudgetSeconds: 0f,
                maxPositionCandidates: 1,
                retreatAfterShot: false,
                thinkPauseSeconds: 0.7f,
                aimPauseSeconds: 0.5f);
        }

        public static BotProfile Normal()
        {
            return BotProfile.CreateRuntime(
                powerSteps: 8,
                angleStepDegrees: 2f,
                minClearPathRatio: 0.9f,
                scatterBase: 0.45f,
                scatterPerDistance: 0.05f,
                perfectShotChance: 0.05f,
                scoreJitter: 0.35f,
                allySafetyWeight: 0.8f,
                evaluateAllWeapons: false,
                movementEnabled: true,
                jumpEnabled: false,
                minEnemySeparation: 2f,
                maxWalkBudgetSeconds: 1.5f,
                maxPositionCandidates: 8,
                retreatAfterShot: false,
                thinkPauseSeconds: 0.55f,
                aimPauseSeconds: 0.35f);
        }

        public static BotProfile Hard()
        {
            return BotProfile.CreateRuntime(
                powerSteps: 12,
                angleStepDegrees: 2f,
                minClearPathRatio: 0.95f,
                scatterBase: 0.15f,
                scatterPerDistance: 0.015f,
                perfectShotChance: 0.05f,
                scoreJitter: 0.2f,
                allySafetyWeight: 1f,
                evaluateAllWeapons: true,
                movementEnabled: true,
                jumpEnabled: true,
                minEnemySeparation: 2.5f,
                maxWalkBudgetSeconds: 2f,
                maxPositionCandidates: 16,
                retreatAfterShot: true,
                thinkPauseSeconds: 0.45f,
                aimPauseSeconds: 0.3f);
        }
    }
}
