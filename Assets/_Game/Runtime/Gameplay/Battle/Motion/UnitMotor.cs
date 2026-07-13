using Game.Gameplay.Battle.Terrain;
using UnityEngine;

namespace Game.Gameplay.Battle.Motion
{
    public enum MotorStepOutcome
    {
        /// <summary>No motion happened (already stable).</summary>
        Resting,
        /// <summary>Still moving through the air.</summary>
        Airborne,
        /// <summary>Touched down and settled this step.</summary>
        Landed,
        /// <summary>Left the arena below the kill height.</summary>
        FellOutOfBounds
    }

    public enum WalkOutcome
    {
        None,
        Moved,
        Blocked,
        /// <summary>Walked off a ledge; the unit is now airborne.</summary>
        StartedFalling
    }

    /// <summary>
    /// The single movement model for a battle unit. Walking, jumping,
    /// explosion knockback and falling all resolve through the same
    /// cell-stepped integration against the terrain mask, so a unit can
    /// never tunnel through ground or need stuck-recovery timers.
    /// </summary>
    public sealed class UnitMotor
    {
        private readonly TerrainGrid _terrain;
        private readonly UnitMotionProfile _profile;
        private readonly float _killY;
        private float _walkEnergy;

        public UnitMotor(TerrainGrid terrain, UnitMotionProfile profile, Vector2 position, float killY)
        {
            _terrain = terrain;
            _profile = profile;
            Position = position;
            _killY = killY;
        }

        /// <summary>Body center in world units.</summary>
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }

        /// <summary>Stable units rest on ground and skip integration.</summary>
        public bool IsStable { get; private set; } = true;

        public UnitMotionProfile Profile => _profile;

        public void Teleport(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.zero;
            IsStable = true;
        }

        /// <summary>Knockback entry point: push the unit and let physics resolve the rest.</summary>
        public void AddImpulse(Vector2 velocity)
        {
            Velocity += velocity;
            if (Velocity.sqrMagnitude > 0.0001f)
            {
                IsStable = false;
            }
        }

        /// <summary>Ground under the unit was destroyed (or never existed): re-check support.</summary>
        public void WakeIfUnsupported()
        {
            if (!IsStable || IsGrounded(Position))
            {
                return;
            }

            IsStable = false;
        }

        /// <summary>
        /// Converts the archetype's designed jump arc (distance + apex height on
        /// flat ground) into a launch velocity and hands control to physics.
        /// </summary>
        public void StartJump(float horizontalDistance, float apexHeight)
        {
            var height = Mathf.Max(0.15f, apexHeight);
            var verticalSpeed = Mathf.Sqrt(2f * _profile.Gravity * height);
            var flightTime = 2f * verticalSpeed / _profile.Gravity;
            Velocity = new Vector2(horizontalDistance / flightTime, verticalSpeed);
            IsStable = false;

            // Lift off the surface so the first step does not immediately re-land.
            var liftedPosition = Position + Vector2.up * _terrain.CellSize.y;
            if (!IsBodyBlocked(liftedPosition))
            {
                Position = liftedPosition;
            }
        }

        /// <summary>
        /// Advances the airborne simulation by one fixed tick, moving at most
        /// one terrain cell per sub-step so collisions can never be skipped.
        /// </summary>
        public MotorStepOutcome Step(float deltaTime)
        {
            if (IsStable)
            {
                return MotorStepOutcome.Resting;
            }

            var velocity = Velocity;
            velocity.y -= _profile.Gravity * deltaTime;
            if (velocity.magnitude > _profile.MaxSpeed)
            {
                velocity = velocity.normalized * _profile.MaxSpeed;
            }

            Velocity = velocity;

            var displacement = Velocity * deltaTime;
            var cell = Mathf.Min(_terrain.CellSize.x, _terrain.CellSize.y);
            var stepCount = Mathf.Max(1, Mathf.CeilToInt(Mathf.Max(
                Mathf.Abs(displacement.x),
                Mathf.Abs(displacement.y)) / cell));
            var step = displacement / stepCount;

            for (var i = 0; i < stepCount; i++)
            {
                var previous = Position;
                Position += step;

                if (Position.y + _profile.HalfHeight < _killY)
                {
                    return MotorStepOutcome.FellOutOfBounds;
                }

                if (!IsBodyBlocked(Position))
                {
                    continue;
                }

                return ResolveCollision(previous);
            }

            return MotorStepOutcome.Airborne;
        }

        /// <summary>
        /// Walks one tick worth of distance. The unit hugs the terrain: climbs
        /// ledges up to the step height, descends slopes, and starts falling
        /// (through regular physics) when it walks off an edge.
        /// </summary>
        public WalkOutcome Walk(int direction, float deltaTime)
        {
            if (!IsStable || direction == 0)
            {
                return WalkOutcome.None;
            }

            _walkEnergy += _profile.WalkSpeed * deltaTime;
            var cellX = _terrain.CellSize.x;
            var moved = false;
            while (_walkEnergy >= cellX)
            {
                _walkEnergy -= cellX;
                var outcome = TryWalkOneCell(direction);
                if (outcome != WalkOutcome.Moved)
                {
                    _walkEnergy = 0f;
                    return outcome;
                }

                moved = true;
            }

            return moved ? WalkOutcome.Moved : WalkOutcome.None;
        }

        public void ResetWalk()
        {
            _walkEnergy = 0f;
        }

        public bool IsGrounded(Vector2 position)
        {
            var footY = position.y - _profile.HalfHeight - _terrain.CellSize.y * 0.5f;
            return _terrain.IsSolidWorld(new Vector2(position.x, footY))
                || _terrain.IsSolidWorld(new Vector2(position.x - _profile.HalfWidth * 0.7f, footY))
                || _terrain.IsSolidWorld(new Vector2(position.x + _profile.HalfWidth * 0.7f, footY));
        }

        private WalkOutcome TryWalkOneCell(int direction)
        {
            var candidate = new Vector2(Position.x + direction * _terrain.CellSize.x, Position.y);
            if (IsBodyBlocked(candidate))
            {
                // Wall ahead: try to climb it within the step height.
                var lift = _terrain.CellSize.y;
                while (lift <= _profile.WalkStepHeight)
                {
                    var raised = new Vector2(candidate.x, Position.y + lift);
                    if (!IsBodyBlocked(raised))
                    {
                        Position = raised;
                        return WalkOutcome.Moved;
                    }

                    lift += _terrain.CellSize.y;
                }

                return WalkOutcome.Blocked;
            }

            // Free ahead: follow the ground down within the step height.
            var drop = 0f;
            while (drop <= _profile.WalkStepHeight)
            {
                var lowered = new Vector2(candidate.x, Position.y - drop);
                if (!IsBodyBlocked(lowered) && IsGrounded(lowered))
                {
                    Position = lowered;
                    return WalkOutcome.Moved;
                }

                drop += _terrain.CellSize.y;
            }

            // Walked off a ledge: keep a little forward drift and fall.
            Position = candidate;
            Velocity = new Vector2(direction * 0.6f, 0f);
            IsStable = false;
            return WalkOutcome.StartedFalling;
        }

        private MotorStepOutcome ResolveCollision(Vector2 previous)
        {
            var falling = Velocity.y <= 0f;
            if (!falling && IsBottomBlocked(Position) && !IsTopBlocked(Position))
            {
                // Launching with feet still clipped into the surface (jump or
                // knockback from a slope): surface out and keep flying.
                var surfaced = Position;
                var surfaceLift = 0f;
                while (surfaceLift <= _profile.CrawlHeight && IsBodyBlocked(surfaced))
                {
                    surfaced.y += _terrain.CellSize.y;
                    surfaceLift += _terrain.CellSize.y;
                }

                if (!IsBodyBlocked(surfaced))
                {
                    Position = surfaced;
                    return MotorStepOutcome.Airborne;
                }
            }

            if (falling && IsBottomBlocked(Position))
            {
                // Soft ground contact: crawl up out of the surface and settle.
                var lifted = Position;
                var lift = 0f;
                while (lift <= _profile.CrawlHeight && IsBodyBlocked(lifted))
                {
                    lifted.y += _terrain.CellSize.y;
                    lift += _terrain.CellSize.y;
                }

                if (!IsBodyBlocked(lifted))
                {
                    var gentleFall = Mathf.Abs(Velocity.x) <= Mathf.Abs(Velocity.y) * 0.8f
                        || Velocity.magnitude <= _profile.RestSpeed * 4f;
                    if (gentleFall)
                    {
                        return Settle(lifted);
                    }

                    // Fast glancing hit: shed speed, keep sliding along the surface.
                    Position = lifted;
                    Velocity = new Vector2(Velocity.x * (1f - _profile.BounceDampingX), 0f);
                    if (Velocity.magnitude <= _profile.RestSpeed)
                    {
                        return Settle(lifted);
                    }

                    return MotorStepOutcome.Airborne;
                }

                // Too deep to crawl out: bounce off the ground.
                Position = previous;
                Velocity = new Vector2(
                    Velocity.x * -_profile.BounceDampingX,
                    Mathf.Abs(Velocity.y) * _profile.BounceDampingY);
                return DampenOrRest(previous);
            }

            if (!falling && IsTopBlocked(Position))
            {
                // Ceiling: reflect downwards, keep horizontal motion.
                Position = previous;
                Velocity = new Vector2(Velocity.x, -Mathf.Abs(Velocity.y) * _profile.BounceDampingY);
                return MotorStepOutcome.Airborne;
            }

            // Side hit (or an overhang snag): never hang on it — revert the
            // horizontal motion, dampen, and keep falling past it.
            var verticalOnly = new Vector2(previous.x, Position.y);
            if (!IsBodyBlocked(verticalOnly))
            {
                Position = verticalOnly;
                Velocity = new Vector2(Velocity.x * -_profile.BounceDampingX, Velocity.y);
                return MotorStepOutcome.Airborne;
            }

            // Wedged: both the full step and a pure vertical retreat are
            // blocked. Whatever snags the body also supports it, so bleed the
            // motion off here — keeping the velocity would let gravity feed a
            // block that never resolves and the unit would hang unstable forever.
            Position = previous;
            if (Velocity.y > 0f)
            {
                Velocity = new Vector2(
                    Velocity.x * -_profile.BounceDampingX,
                    -Mathf.Abs(Velocity.y) * _profile.BounceDampingY);
                return MotorStepOutcome.Airborne;
            }

            Velocity = new Vector2(Velocity.x * -_profile.BounceDampingX, 0f);
            if (Velocity.magnitude <= _profile.RestSpeed)
            {
                return Settle(previous);
            }

            return MotorStepOutcome.Airborne;
        }

        private MotorStepOutcome DampenOrRest(Vector2 position)
        {
            if (Velocity.magnitude > _profile.RestSpeed || !IsGrounded(position))
            {
                return MotorStepOutcome.Airborne;
            }

            return Settle(position);
        }

        private MotorStepOutcome Settle(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.zero;
            IsStable = true;
            return MotorStepOutcome.Landed;
        }

        private bool IsBodyBlocked(Vector2 center)
        {
            var halfWidth = _profile.HalfWidth;
            var halfHeight = _profile.HalfHeight;

            // Diamond-ish sample: feet, head, flanks and shoulders. One-cell
            // sub-steps guarantee no solid region can slip between samples.
            return _terrain.IsSolidWorld(new Vector2(center.x, center.y - halfHeight))
                || _terrain.IsSolidWorld(new Vector2(center.x, center.y + halfHeight))
                || _terrain.IsSolidWorld(new Vector2(center.x - halfWidth, center.y))
                || _terrain.IsSolidWorld(new Vector2(center.x + halfWidth, center.y))
                || _terrain.IsSolidWorld(new Vector2(center.x - halfWidth * 0.7f, center.y - halfHeight * 0.6f))
                || _terrain.IsSolidWorld(new Vector2(center.x + halfWidth * 0.7f, center.y - halfHeight * 0.6f))
                || _terrain.IsSolidWorld(new Vector2(center.x - halfWidth * 0.7f, center.y + halfHeight * 0.6f))
                || _terrain.IsSolidWorld(new Vector2(center.x + halfWidth * 0.7f, center.y + halfHeight * 0.6f));
        }

        private bool IsBottomBlocked(Vector2 center)
        {
            var footY = center.y - _profile.HalfHeight;
            return _terrain.IsSolidWorld(new Vector2(center.x, footY))
                || _terrain.IsSolidWorld(new Vector2(center.x - _profile.HalfWidth * 0.7f, footY))
                || _terrain.IsSolidWorld(new Vector2(center.x + _profile.HalfWidth * 0.7f, footY));
        }

        private bool IsTopBlocked(Vector2 center)
        {
            var headY = center.y + _profile.HalfHeight;
            return _terrain.IsSolidWorld(new Vector2(center.x, headY))
                || _terrain.IsSolidWorld(new Vector2(center.x - _profile.HalfWidth * 0.7f, headY))
                || _terrain.IsSolidWorld(new Vector2(center.x + _profile.HalfWidth * 0.7f, headY));
        }
    }
}
