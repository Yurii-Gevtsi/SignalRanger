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
        // Feet rest this fraction of a cell above the surface top, keeping the
        // body clear of the ground while staying inside the grounded window.
        private const float SkinCellFraction = 0.25f;
        private const float MinimumFallingSpeed = 0.2f;
        private const int SideSnagRecoveryDepthCells = 12;
        private const int SideSnagRecoveryLateralCells = 3;
        // Lateral offset of the outer foot/edge samples, as a fraction of half width.
        private const float BodySampleSpread = 0.7f;
        // Ground-support columns across the body, as fractions of half width.
        private static readonly float[] FootColumnOffsets = { -1f, -BodySampleSpread, 0f, BodySampleSpread, 1f };

        private readonly TerrainGrid _terrain;
        private readonly UnitMotionProfile _profile;
        private readonly float _killY;
        private float _walkEnergy;
        private float _climbDebt;
        private float _jumpElapsed;
        private float _jumpDuration;
        private bool _hasRope;
        private Vector2 _ropeAnchor;
        private float _ropeLength;
        private float _ropeMaximumLength;
        private float _ropeSwingAcceleration;
        private int _ropeHorizontalInput;
        private int _ropeVerticalInput;
        private bool _hovering;
        private bool _hoverRisen;
        private float _hoverTargetY;
        private int _hoverHorizontalInput;
        private int _hoverVerticalInput;
        private System.Func<Vector2, bool> _hoverExtraBlocker;

        public UnitMotor(TerrainGrid terrain, UnitMotionProfile profile, Vector2 position, float killY)
        {
            _terrain = terrain;
            _profile = profile;
            Position = position;
            _killY = killY;
        }

        /// <summary>Body center in world units.</summary>
        public Vector2 Position { get; private set; }
        public Rect BodyBounds => new(Position - new Vector2(_profile.HalfWidth, _profile.HalfHeight),
            new Vector2(_profile.HalfWidth, _profile.HalfHeight) * 2f);
        public Vector2 Velocity { get; private set; }

        /// <summary>Stable units rest on ground and skip integration.</summary>
        public bool IsStable { get; private set; } = true;

        /// <summary>True only for a player or bot initiated jump, not a fall or knockback.</summary>
        public bool IsJumping { get; private set; }
        public bool HasRope => _hasRope;
        public bool IsHovering => _hovering;

        /// <summary>World units per second of the automatic rise when the fuel is lit.</summary>
        public const float HoverRiseSpeed = 4.5f;

        /// <summary>World units per second of the directional control while hovering.</summary>
        // 50% faster than the former 3.2 under the arrow buttons (owner decision 2026-10-09).
        public const float HoverMoveSpeed = 4.8f;

        /// <summary>
        /// Jet-trail scale over the designed jump arc: it grows toward the apex,
        /// then shrinks all the way to zero on the descent.
        /// </summary>
        public float JumpTrailScale
        {
            get
            {
                if (_hovering)
                {
                    return 1f;
                }

                if (!IsJumping || _jumpDuration <= 0f)
                {
                    return 0f;
                }

                var phase = Mathf.Clamp01(_jumpElapsed / _jumpDuration);
                if (phase <= 0.5f)
                {
                    return Mathf.Lerp(0.5f, 1f, Mathf.SmoothStep(0f, 1f, phase * 2f));
                }

                return Mathf.Lerp(1f, 0.5f, Mathf.SmoothStep(0f, 1f, (phase - 0.5f) * 2f));
            }
        }

        public UnitMotionProfile Profile => _profile;

        private float Skin => _terrain.CellSize.y * SkinCellFraction;

        /// <summary>
        /// Validates an initial position before the battle starts. A spawn is
        /// safe only when the body is clear, supported by ground, and can take
        /// at least one cell-sized walk step in either direction.
        /// </summary>
        public bool IsSafeSpawn()
        {
            if (_terrain == null || IsBodyBlocked(Position) || !IsGrounded(Position))
            {
                return false;
            }

            var probeDelta = _terrain.CellSize.x / _profile.WalkSpeed + 0.0001f;
            return CanWalkOneStep(-1, probeDelta) || CanWalkOneStep(1, probeDelta);
        }

        private bool CanWalkOneStep(int direction, float deltaTime)
        {
            var probe = new UnitMotor(_terrain, _profile, Position, _killY);
            return probe.Walk(direction, deltaTime) == WalkOutcome.Moved;
        }

        public void Teleport(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.zero;
            IsStable = true;
            IsJumping = false;
            ResetJumpPhase();
            _climbDebt = 0f;
            StopHover();
            DetachRope();
        }

        /// <summary>
        /// Lights the rocket fuel: the body rises straight up by <paramref name="riseHeight"/> (or until
        /// something blocks it) and then hangs in the air. Gravity does not act until
        /// <see cref="StopHover"/>. <paramref name="extraBlocker"/> lets the caller add bodies the terrain
        /// does not know about, such as other droids.
        /// </summary>
        public void StartHover(float riseHeight, System.Func<Vector2, bool> extraBlocker = null)
        {
            _hovering = true;
            _hoverRisen = false;
            _hoverTargetY = Position.y + Mathf.Max(0.1f, riseHeight);
            _hoverHorizontalInput = 0;
            _hoverVerticalInput = 0;
            _hoverExtraBlocker = extraBlocker;
            Velocity = Vector2.zero;
            IsStable = false;
            IsJumping = false;
            ResetJumpPhase();
            _climbDebt = 0f;
            DetachRope();
        }

        /// <summary>Horizontal and vertical (up positive) steering of the hovering body.</summary>
        public void SetHoverInput(int horizontalDirection, int verticalDirection)
        {
            _hoverHorizontalInput = horizontalDirection == 0 ? 0 : horizontalDirection > 0 ? 1 : -1;
            _hoverVerticalInput = verticalDirection == 0 ? 0 : verticalDirection > 0 ? 1 : -1;
        }

        /// <summary>Cuts the fuel; the body falls under normal physics unless something supports it.</summary>
        public void StopHover()
        {
            if (!_hovering)
            {
                return;
            }

            _hovering = false;
            _hoverHorizontalInput = 0;
            _hoverVerticalInput = 0;
            _hoverExtraBlocker = null;
            Velocity = Vector2.zero;
            IsStable = false;
        }

        public void AttachRope(Vector2 anchor, float length, float maximumLength, float swingAcceleration)
        {
            _ropeAnchor = anchor;
            _ropeMaximumLength = Mathf.Max(0.5f, maximumLength);
            _ropeLength = Mathf.Clamp(length, 0.5f, _ropeMaximumLength);
            _ropeSwingAcceleration = Mathf.Max(0.1f, swingAcceleration);
            _ropeHorizontalInput = 0;
            _ropeVerticalInput = 0;
            _hasRope = true;
            IsStable = false;
            IsJumping = false;
            ResetJumpPhase();
        }

        public void SetRopeInput(int horizontalDirection, int verticalDirection)
        {
            _ropeHorizontalInput = horizontalDirection == 0 ? 0 : horizontalDirection > 0 ? 1 : -1;
            _ropeVerticalInput = verticalDirection == 0 ? 0 : verticalDirection > 0 ? 1 : -1;
        }

        public void DetachRope()
        {
            _hasRope = false;
            _ropeHorizontalInput = 0;
            _ropeVerticalInput = 0;
        }

        /// <summary>Knockback entry point: push the unit and let physics resolve the rest.</summary>
        public void AddImpulse(Vector2 velocity)
        {
            // A blast or throw knocks a hovering body off its fuel: the hover ends and the push is kept.
            if (_hovering && velocity.sqrMagnitude > 0.0001f)
            {
                StopHover();
            }

            IsJumping = false;
            ResetJumpPhase();
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
            IsJumping = false;
            ResetJumpPhase();
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
            IsJumping = true;
            _jumpElapsed = 0f;
            _jumpDuration = flightTime;

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
                _snagSeconds = 0f;
                _snagAnchor = Position;
                return MotorStepOutcome.Resting;
            }

            if (_hovering || _hasRope)
            {
                _snagSeconds = 0f;
                _snagAnchor = Position;
                return _hovering ? StepHover(deltaTime) : StepAirborne(deltaTime);
            }

            var outcome = StepAirborne(deltaTime);
            return outcome == MotorStepOutcome.Airborne ? GuardAgainstSnag(deltaTime) : outcome;
        }

        /// <summary>
        /// A body wedged against a ledge corner can keep bouncing between blocked positions without ever falling
        /// or landing, which would hold the turn forever. A real flight or fall leaves a small area quickly; when an
        /// airborne body has stayed inside one for too long it is put down on the nearest free spot that has
        /// support (or, failing that, where it is).
        /// </summary>
        private MotorStepOutcome GuardAgainstSnag(float deltaTime)
        {
            if ((Position - _snagAnchor).sqrMagnitude > SnagAreaRadius * SnagAreaRadius)
            {
                _snagAnchor = Position;
                _snagSeconds = 0f;
                return MotorStepOutcome.Airborne;
            }

            _snagSeconds += Mathf.Max(0f, deltaTime);
            if (_snagSeconds < SnagSettleSeconds)
            {
                return MotorStepOutcome.Airborne;
            }

            _snagSeconds = 0f;
            _snagAnchor = Position;
            var cell = _terrain.CellSize;
            var maxLift = Mathf.CeilToInt(_profile.HalfHeight * 2f / cell.y) + 2;
            var maxSide = Mathf.CeilToInt(_profile.HalfWidth * 2f / cell.x) + 2;
            // Nearest first: small offsets before large ones, up before down.
            for (var reach = 0; reach <= maxLift + maxSide; reach++)
            {
                for (var liftStep = 0; liftStep <= reach; liftStep++)
                {
                    var side = reach - liftStep;
                    if (side > maxSide || liftStep > maxLift)
                    {
                        continue;
                    }

                    for (var vertical = 1; vertical >= -1; vertical -= 2)
                    {
                        for (var sign = -1; sign <= 1; sign += 2)
                        {
                            var candidate = Position + new Vector2(sign * side * cell.x, vertical * liftStep * cell.y);
                            if (!IsBodyBlocked(candidate) && IsGrounded(candidate))
                            {
                                return Settle(candidate);
                            }

                            if (side == 0)
                            {
                                break;
                            }
                        }

                        if (liftStep == 0)
                        {
                            break;
                        }
                    }
                }
            }

            return Settle(Position);
        }

        // A free fall leaves this area in about a quarter of a second and a jump arc in about half of one.
        private const float SnagAreaRadius = 0.3f;
        private const float SnagSettleSeconds = 0.9f;
        private Vector2 _snagAnchor;
        private float _snagSeconds;

        private MotorStepOutcome StepAirborne(float deltaTime)
        {

            if (IsJumping)
            {
                _jumpElapsed = Mathf.Min(_jumpDuration, _jumpElapsed + Mathf.Max(0f, deltaTime));
            }

            var velocity = Velocity;
            if (_hasRope)
            {
                velocity.x += _ropeHorizontalInput * _ropeSwingAcceleration * deltaTime;
                _ropeLength = Mathf.Clamp(_ropeLength + _ropeVerticalInput * 2.6f * deltaTime, 0.5f, _ropeMaximumLength);
            }
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
                ConstrainToRope();

                if (Position.y + _profile.HalfHeight < _killY)
                {
                    IsJumping = false;
                    ResetJumpPhase();
                    return MotorStepOutcome.FellOutOfBounds;
                }

                if (!IsBodyBlocked(Position))
                {
                    continue;
                }

                var collisionOutcome = ResolveCollision(previous);
                if (collisionOutcome == MotorStepOutcome.Landed)
                {
                    IsJumping = false;
                    ResetJumpPhase();
                }

                return collisionOutcome;
            }

            return MotorStepOutcome.Airborne;
        }

        private MotorStepOutcome StepHover(float deltaTime)
        {
            var velocity = new Vector2(
                _hoverHorizontalInput * HoverMoveSpeed,
                _hoverRisen ? _hoverVerticalInput * HoverMoveSpeed : HoverRiseSpeed);
            Velocity = velocity;
            var displacement = velocity * deltaTime;
            var cell = Mathf.Min(_terrain.CellSize.x, _terrain.CellSize.y);
            var stepCount = Mathf.Max(1, Mathf.CeilToInt(Mathf.Max(
                Mathf.Abs(displacement.x), Mathf.Abs(displacement.y)) / cell));
            var step = displacement / stepCount;
            for (var i = 0; i < stepCount; i++)
            {
                // Axes are tried separately so a wall or a ceiling stops only its own direction.
                if (!Mathf.Approximately(step.x, 0f))
                {
                    var next = Position + new Vector2(step.x, 0f);
                    if (!IsHoverBlocked(next))
                    {
                        Position = next;
                    }
                }

                if (!Mathf.Approximately(step.y, 0f))
                {
                    var next = Position + new Vector2(0f, step.y);
                    if (!IsHoverBlocked(next))
                    {
                        Position = next;
                    }
                    else if (!_hoverRisen && step.y > 0f)
                    {
                        _hoverRisen = true;
                    }
                }

                if (!_hoverRisen && Position.y >= _hoverTargetY)
                {
                    _hoverRisen = true;
                }
            }

            return MotorStepOutcome.Airborne;
        }

        private bool IsHoverBlocked(Vector2 center)
        {
            return center.x - _profile.HalfWidth < _terrain.WorldMinX
                || center.x + _profile.HalfWidth > _terrain.WorldMaxX
                || center.y + _profile.HalfHeight > _terrain.WorldMaxY
                || IsBodyBlocked(center)
                || (_hoverExtraBlocker != null && _hoverExtraBlocker(center));
        }

        /// <summary>
        /// Walks one tick worth of distance. The unit hugs the terrain: climbs
        /// ledges up to the step height (as long as the sustained incline stays
        /// under the profile's walkable slope), descends slopes, and starts
        /// falling (through regular physics) when it walks off an edge.
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

        /// <summary>Walking speed of this unit's profile, in world units per second.</summary>
        public float WalkSpeed => _profile.WalkSpeed;

        /// <summary>Gravity this unit falls with, in world units per second squared.</summary>
        public float Gravity => _profile.Gravity;

        public void ResetWalk()
        {
            _walkEnergy = 0f;
        }

        public bool IsGrounded(Vector2 position)
        {
            var footY = position.y - _profile.HalfHeight - _terrain.CellSize.y * 0.5f;
            for (var i = 0; i < FootColumnOffsets.Length; i++)
            {
                if (_terrain.IsSolidWorld(new Vector2(position.x + FootColumnOffsets[i] * _profile.HalfWidth, footY)))
                {
                    return true;
                }
            }

            return false;
        }

        private WalkOutcome TryWalkOneCell(int direction)
        {
            var candidateX = Position.x + direction * _terrain.CellSize.x;
            var feetY = Position.y - _profile.HalfHeight;
            var stepHeight = _profile.WalkStepHeight;

            // Where would the feet stand at the next column? The highest
            // footing within one step up or down, under any body column: the
            // leading edge counts, so a stair lip catches the front foot the
            // same way it would block the body. Feet then snap exactly onto
            // the surface top, which keeps the body aligned to the grid no
            // matter how the last landing ended — flat ground can never read
            // as a ledge mid-walk.
            if (!TryFindFooting(candidateX, feetY + stepHeight + Skin, 2f * (stepHeight + Skin), out var surfaceY))
            {
                // Nothing to stand on within a step below: walked off a
                // ledge. Keep a little forward drift and fall.
                Position = new Vector2(candidateX, Position.y);
                Velocity = new Vector2(direction * 0.6f, 0f);
                IsStable = false;
                return WalkOutcome.StartedFalling;
            }

            var newFeetY = surfaceY + Skin;

            // Leaky-bucket slope rule: rising steeper than the walkable slope
            // accumulates debt, walking flat or downhill pays it back. A single
            // ledge within the step height passes instantly; a sustained
            // steeper climb (a hull, a near-vertical hillside) blocks within a
            // few cells no matter how gently each cell slices it.
            var drain = _terrain.CellSize.x * _profile.MaxWalkSlopeTangent;
            var debt = Mathf.Max(0f, _climbDebt + (newFeetY - feetY) - drain);
            if (debt > stepHeight)
            {
                return WalkOutcome.Blocked;
            }

            var target = new Vector2(candidateX, newFeetY + _profile.HalfHeight);
            if (IsBodyBlocked(target))
            {
                // Standing on that footing leaves the body inside terrain: a
                // wall taller than the step, or a gap shorter than the unit.
                return WalkOutcome.Blocked;
            }

            Position = target;
            _climbDebt = debt;
            return WalkOutcome.Moved;
        }

        /// <summary>
        /// Finds the highest surface top below <paramref name="scanStartY"/>
        /// under any of the five body columns (center, hips, edges) within
        /// <paramref name="maxDrop"/>, mirroring <see cref="IsGrounded"/>.
        /// </summary>
        private bool TryFindFooting(float centerX, float scanStartY, float maxDrop, out float surfaceY)
        {
            surfaceY = float.MinValue;
            var found = false;
            for (var i = 0; i < FootColumnOffsets.Length; i++)
            {
                var columnX = centerX + FootColumnOffsets[i] * _profile.HalfWidth;
                // One extra cell of drop so a surface sitting exactly on the
                // range boundary is still entered and reported.
                if (_terrain.TryFindSurfaceBelow(columnX, scanStartY, maxDrop + _terrain.CellSize.y, out var columnSurfaceY)
                    && columnSurfaceY > surfaceY)
                {
                    surfaceY = columnSurfaceY;
                    found = true;
                }
            }

            return found;
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
                BeginFallingAfterNonSupportContact();
                return MotorStepOutcome.Airborne;
            }

            // A body can straddle a thin cliff or an overhang: both positions
            // above are blocked even though neither track has support. Find a
            // clear cell below/away from the side instead of repeatedly feeding
            // gravity into the same blocked position.
            if (TryRecoverFromSideSnag(previous, out var recovered))
            {
                Position = recovered;
                BeginFallingAfterNonSupportContact();
                return MotorStepOutcome.Airborne;
            }

            // No nearby clear cell exists. Keep the last valid position and
            // still clear jump state; the next cell-stepped tick can resolve a
            // regular landing or fall rather than leaving the controls locked.
            Position = previous;
            BeginFallingAfterNonSupportContact();
            if (Velocity.magnitude <= _profile.RestSpeed && IsGrounded(previous))
            {
                return Settle(previous);
            }

            return MotorStepOutcome.Airborne;
        }

        private bool TryRecoverFromSideSnag(Vector2 previous, out Vector2 recovered)
        {
            var cell = _terrain.CellSize;
            var horizontalDirection = Mathf.Sign(Position.x - previous.x);
            if (Mathf.Approximately(horizontalDirection, 0f))
            {
                horizontalDirection = Mathf.Sign(Velocity.x);
            }

            if (Mathf.Approximately(horizontalDirection, 0f))
            {
                horizontalDirection = 1f;
            }

            // Move downward first and prefer the direction opposite the hit.
            // All candidates are cell-aligned, so this recovery cannot skip a
            // collision row in the terrain mask.
            for (var depth = 1; depth <= SideSnagRecoveryDepthCells; depth++)
            {
                var y = previous.y - depth * cell.y;
                for (var lateral = 0; lateral <= SideSnagRecoveryLateralCells; lateral++)
                {
                    var offset = lateral * cell.x;
                    var away = new Vector2(previous.x - horizontalDirection * offset, y);
                    if (!IsBodyBlocked(away))
                    {
                        recovered = away;
                        return true;
                    }

                    if (lateral == 0)
                    {
                        continue;
                    }

                    var forward = new Vector2(previous.x + horizontalDirection * offset, y);
                    if (!IsBodyBlocked(forward))
                    {
                        recovered = forward;
                        return true;
                    }
                }
            }

            recovered = previous;
            return false;
        }

        private void BeginFallingAfterNonSupportContact()
        {
            IsJumping = false;
            ResetJumpPhase();
            Velocity = new Vector2(
                Velocity.x * -_profile.BounceDampingX,
                Mathf.Min(Velocity.y, -MinimumFallingSpeed));
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
            if (_hasRope)
            {
                Position = position;
                IsStable = false;
                IsJumping = false;
                ResetJumpPhase();
                return MotorStepOutcome.Airborne;
            }
            Position = SnapFeetToSurface(position);
            Velocity = Vector2.zero;
            IsStable = true;
            IsJumping = false;
            ResetJumpPhase();
            _climbDebt = 0f;
            return MotorStepOutcome.Landed;
        }

        private void ResetJumpPhase()
        {
            _jumpElapsed = 0f;
            _jumpDuration = 0f;
        }

        private void ConstrainToRope()
        {
            if (!_hasRope)
            {
                return;
            }

            var offset = Position - _ropeAnchor;
            var distance = offset.magnitude;
            if (distance <= _ropeLength || distance < 0.0001f)
            {
                return;
            }

            var normal = offset / distance;
            Position = _ropeAnchor + normal * _ropeLength;
            var outwardSpeed = Vector2.Dot(Velocity, normal);
            if (outwardSpeed > 0f)
            {
                Velocity -= normal * outwardSpeed;
            }
        }

        /// <summary>
        /// Rests the feet exactly on the surface top (plus the skin) so a
        /// landing never leaves the body misaligned against the cell grid.
        /// </summary>
        private Vector2 SnapFeetToSurface(Vector2 position)
        {
            var cellY = _terrain.CellSize.y;
            var feetY = position.y - _profile.HalfHeight;
            if (!TryFindFooting(position.x, feetY + cellY, 2f * cellY, out var surfaceY))
            {
                return position;
            }

            var snapped = new Vector2(position.x, surfaceY + Skin + _profile.HalfHeight);
            return IsBodyBlocked(snapped) ? position : snapped;
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
                || _terrain.IsSolidWorld(new Vector2(center.x - halfWidth * BodySampleSpread, center.y - halfHeight * 0.6f))
                || _terrain.IsSolidWorld(new Vector2(center.x + halfWidth * BodySampleSpread, center.y - halfHeight * 0.6f))
                || _terrain.IsSolidWorld(new Vector2(center.x - halfWidth * BodySampleSpread, center.y + halfHeight * 0.6f))
                || _terrain.IsSolidWorld(new Vector2(center.x + halfWidth * BodySampleSpread, center.y + halfHeight * 0.6f));
        }

        private bool IsBottomBlocked(Vector2 center)
        {
            var footY = center.y - _profile.HalfHeight;
            return _terrain.IsSolidWorld(new Vector2(center.x, footY))
                || _terrain.IsSolidWorld(new Vector2(center.x - _profile.HalfWidth * BodySampleSpread, footY))
                || _terrain.IsSolidWorld(new Vector2(center.x + _profile.HalfWidth * BodySampleSpread, footY));
        }

        private bool IsTopBlocked(Vector2 center)
        {
            var headY = center.y + _profile.HalfHeight;
            return _terrain.IsSolidWorld(new Vector2(center.x, headY))
                || _terrain.IsSolidWorld(new Vector2(center.x - _profile.HalfWidth * BodySampleSpread, headY))
                || _terrain.IsSolidWorld(new Vector2(center.x + _profile.HalfWidth * BodySampleSpread, headY));
        }
    }
}
