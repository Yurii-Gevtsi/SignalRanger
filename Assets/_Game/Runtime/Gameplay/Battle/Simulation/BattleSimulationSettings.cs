using UnityEngine;

namespace Game.Gameplay.Battle.Simulation
{
    /// <summary>
    /// Tunables for <see cref="BattleSimulation"/>, extracted from the scene
    /// controller so the rules layer has no hidden magic numbers of its own.
    /// </summary>
    public sealed class BattleSimulationSettings
    {
        public BattleSimulationSettings(
            float fallDeathY,
            float simulationTickSeconds,
            float playerTurnDurationSeconds,
            float occupiedRadius,
            float unitCollisionHalfWidth,
            float knockbackLandingSearchPadding,
            float groundRayStartHeight,
            float groundRayDistance)
        {
            FallDeathY = fallDeathY;
            SimulationTickSeconds = Mathf.Max(1f / 480f, simulationTickSeconds);
            PlayerTurnDurationSeconds = Mathf.Max(0f, playerTurnDurationSeconds);
            OccupiedRadius = Mathf.Max(0f, occupiedRadius);
            UnitCollisionHalfWidth = Mathf.Max(0.01f, unitCollisionHalfWidth);
            KnockbackLandingSearchPadding = Mathf.Max(0f, knockbackLandingSearchPadding);
            GroundRayStartHeight = Mathf.Max(0f, groundRayStartHeight);
            GroundRayDistance = Mathf.Max(0f, groundRayDistance);
        }

        public float FallDeathY { get; }
        public float SimulationTickSeconds { get; }
        public float PlayerTurnDurationSeconds { get; }
        public float OccupiedRadius { get; }
        public float UnitCollisionHalfWidth { get; }
        public float KnockbackLandingSearchPadding { get; }
        public float GroundRayStartHeight { get; }
        public float GroundRayDistance { get; }
    }
}
