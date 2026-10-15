using Game.Gameplay.Battle.Motion;
using Game.Gameplay.Battle.Simulation;
using Game.Gameplay.Battle.Terrain;
using Game.Gameplay.Battle.Units;
using UnityEngine;

namespace Game.Gameplay.Battle.AI.Movement
{
    /// <summary>
    /// Predicts where a standard jump lands by replaying a throwaway
    /// <see cref="UnitMotor"/> with the same launch <c>BattleSimulation.TryStartJump</c>
    /// builds (archetype arc, walk-speed distance cap, gravity). Jumps open up
    /// ledges that walking cannot reach; a jump that would drop the unit into the
    /// kill zone is reported as no landing.
    /// </summary>
    public sealed class JumpCandidateGenerator
    {
        private const float StepSeconds = 1f / 120f;
        private const int StepBudget = 4000;

        private readonly TerrainGrid _terrain;
        private readonly UnitMotionProfile _profile;
        private readonly CharacterArchetype _archetype;
        private readonly float _killY;

        public JumpCandidateGenerator(TerrainGrid terrain, UnitMotionProfile profile, CharacterArchetype archetype, float killY)
        {
            _terrain = terrain;
            _profile = profile;
            _archetype = archetype;
            _killY = killY;
        }

        /// <summary>
        /// Returns the settled landing position of a jump from <paramref name="start"/>
        /// toward <paramref name="direction"/> (-1/+1), or false if it never settles
        /// on safe ground.
        /// </summary>
        public bool TryJumpLanding(Vector2 start, int direction, out Vector2 landing)
        {
            landing = start;
            if (direction == 0)
            {
                return false;
            }

            var motor = new UnitMotor(_terrain, _profile, start, _killY);
            if (!motor.IsStable)
            {
                return false;
            }

            var apexHeight = Mathf.Lerp(_archetype.FrontJumpHeight, _archetype.BackJumpHeight, 0.5f);
            var desiredDistance = Mathf.Lerp(_archetype.FrontJumpDistance, _archetype.BackJumpDistance, 0.5f);
            var verticalSpeed = Mathf.Sqrt(2f * _profile.Gravity * apexHeight);
            var flightTime = 2f * verticalSpeed / _profile.Gravity;
            var runSpeedCap = _profile.WalkSpeed * BattleSimulation.StandardJumpHorizontalSpeedRatio * flightTime;
            var horizontalDistance = Mathf.Min(desiredDistance, runSpeedCap) * (direction > 0 ? 1 : -1);

            motor.StartJump(horizontalDistance, apexHeight);
            for (var i = 0; i < StepBudget && !motor.IsStable; i++)
            {
                if (motor.Step(StepSeconds) == MotorStepOutcome.FellOutOfBounds)
                {
                    return false;
                }
            }

            if (!motor.IsStable)
            {
                return false;
            }

            landing = motor.Position;
            return true;
        }
    }
}
