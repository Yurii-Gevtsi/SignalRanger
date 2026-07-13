using System.Collections.Generic;
using Game.Gameplay.Battle.Motion;
using Game.Gameplay.Battle.Terrain;
using UnityEngine;

namespace Game.Gameplay.Battle.AI.Movement
{
    /// <summary>
    /// Finds the ground positions the bot can actually walk to, by replaying a
    /// throwaway <see cref="UnitMotor"/> from the start point in each direction.
    /// Reusing the real motor means the scan honours the exact walk rules —
    /// step height, slope descent, walking off a ledge, the kill zone — with no
    /// duplicated movement maths that could drift from the simulation.
    /// </summary>
    public sealed class WalkReachabilityScanner
    {
        private const float StepSeconds = 1f / 120f;
        private const int FallStepBudget = 4000;
        private const int WalkStepBudget = 4000;

        private readonly TerrainGrid _terrain;
        private readonly UnitMotionProfile _profile;
        private readonly float _killY;
        private readonly float _walkTickSeconds;

        public WalkReachabilityScanner(TerrainGrid terrain, UnitMotionProfile profile, float killY)
        {
            _terrain = terrain;
            _profile = profile;
            _killY = killY;
            // One terrain cell of travel per Walk call (a hair over, to beat rounding).
            _walkTickSeconds = _terrain.CellSize.x / _profile.WalkSpeed * 1.001f;
        }

        /// <summary>
        /// Fills <paramref name="results"/> with the start position followed by
        /// reachable, settled positions to the left and right, sampled roughly
        /// every <paramref name="sampleSpacing"/> world units, up to
        /// <paramref name="maxDistance"/> of horizontal travel each way.
        /// </summary>
        public void ScanReachable(Vector2 start, float maxDistance, float sampleSpacing, List<Vector2> results)
        {
            results.Clear();
            results.Add(start);
            ScanDirection(start, -1, maxDistance, sampleSpacing, results);
            ScanDirection(start, 1, maxDistance, sampleSpacing, results);
        }

        private void ScanDirection(Vector2 start, int direction, float maxDistance, float sampleSpacing, List<Vector2> results)
        {
            var motor = new UnitMotor(_terrain, _profile, start, _killY);
            var lastSampledX = start.x;

            for (var guard = 0; guard < WalkStepBudget; guard++)
            {
                if (Mathf.Abs(motor.Position.x - start.x) >= maxDistance)
                {
                    return;
                }

                var before = motor.Position;
                var outcome = motor.Walk(direction, _walkTickSeconds);

                if (outcome == WalkOutcome.Blocked || outcome == WalkOutcome.None)
                {
                    return;
                }

                if (!motor.IsStable && !SettleAfterFall(motor))
                {
                    // Fell out of the world past this point: nothing safe beyond.
                    return;
                }

                if (Mathf.Approximately(motor.Position.x, before.x))
                {
                    return;
                }

                if (motor.IsStable && Mathf.Abs(motor.Position.x - lastSampledX) >= sampleSpacing)
                {
                    results.Add(motor.Position);
                    lastSampledX = motor.Position.x;
                }
            }
        }

        private bool SettleAfterFall(UnitMotor motor)
        {
            for (var i = 0; i < FallStepBudget && !motor.IsStable; i++)
            {
                if (motor.Step(StepSeconds) == MotorStepOutcome.FellOutOfBounds)
                {
                    return false;
                }
            }

            return motor.IsStable;
        }
    }
}
