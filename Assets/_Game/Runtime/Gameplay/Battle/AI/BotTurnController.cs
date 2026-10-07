using System.Collections.Generic;
using Game.Gameplay.Battle.AI.Ballistics;
using Game.Gameplay.Battle.AI.Movement;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Simulation;
using Game.Gameplay.Battle.Units;
using Game.Gameplay.Battle.Weapons;
using UnityEngine;

namespace Game.Gameplay.Battle.AI
{
    /// <summary>What the opponent bot is visibly doing, for HUD status text.</summary>
    public enum BotActivity
    {
        Idle,
        Repositioning,
        Aiming,
        Firing
    }

    /// <summary>
    /// Drives the opponent's turn as a small state machine: pick a position,
    /// walk there through the same movement commands the player uses, pause so
    /// the intent reads, then fire through <c>BattleSimulation.TryFire</c>. No
    /// scene, no input — the scene controller only ticks this and reflects
    /// <see cref="Activity"/>. Every step is bounded by a watchdog so a turn can
    /// never hang. Jumps arrive in Stage 4.
    /// </summary>
    public sealed class BotTurnController
    {
        private enum Phase
        {
            Idle,
            Repositioning,
            RepositionJump,
            WaitStable,
            Aiming,
            Firing,
            Done
        }

        private const float MoveEpsilon = 0.15f;
        private const float StallTimeout = 0.35f;
        private const float TurnHardCapSeconds = 12f;

        private readonly BattleSimulation _simulation;
        private readonly IBotShotPlanner _planner;
        private readonly BotPositionPlanner _positionPlanner;
        private readonly BotProfile _profile;
        private readonly IBotRandom _random;
        private readonly float _unitCollisionHalfWidth;
        private readonly CharacterArchetype _playerArchetype;
        private readonly CharacterArchetype _opponentArchetype;
        private readonly AimSettings _baseAimSettings;
        private readonly List<TrajectoryUnitBody> _bodies = new();

        private Phase _phase = Phase.Idle;
        private float _aimTimer;
        private float _moveTimer;
        private float _stallTimer;
        private float _turnTimer;
        private float _lastMoveX;
        private float _targetX;
        private int _jumpDirection;
        private BotShotPlan? _plan;
        private int _plannedTurn = -1;
        private Vector2 _scatteredTarget;
        private WeaponDefinition _weapon;
        private AimSettings _aimSettings;

        public BotTurnController(
            BattleSimulation simulation,
            IBotShotPlanner planner,
            BotPositionPlanner positionPlanner,
            BotProfile profile,
            IBotRandom random,
            float unitCollisionHalfWidth,
            CharacterArchetype playerArchetype,
            CharacterArchetype opponentArchetype,
            AimSettings baseAimSettings)
        {
            _simulation = simulation;
            _planner = planner;
            _positionPlanner = positionPlanner;
            _profile = profile;
            _random = random;
            _unitCollisionHalfWidth = unitCollisionHalfWidth;
            _playerArchetype = playerArchetype;
            _opponentArchetype = opponentArchetype;
            _baseAimSettings = baseAimSettings;
        }

        public BotActivity Activity { get; private set; } = BotActivity.Idle;

        // --- Telemetry (for a debug overlay / balance tuning) -----------------
        public BotMoveKind LastMoveKind { get; private set; }
        public float LastPlanScore { get; private set; }
        public int LastPositionsConsidered { get; private set; }
        public double LastPlanningMilliseconds { get; private set; }

        public void Tick(float deltaTime)
        {
            var state = _simulation.State;
            if (_simulation.Result.IsFinished || state.Turn.ActiveTeam != BattleTeamId.Opponent)
            {
                ResetToIdle();
                return;
            }

            // Wait for shots and knockback (and mid-move falls) to settle.
            if (_simulation.HasProjectilesInFlight || _simulation.HasActiveMotion)
            {
                return;
            }

            // A new opponent turn restarts the plan even though Tick is only
            // called during the opponent's turn, never in between.
            if (_phase != Phase.Idle && state.Turn.TurnNumber != _plannedTurn)
            {
                _phase = Phase.Idle;
            }

            if (_phase != Phase.Idle && _phase != Phase.Done)
            {
                _turnTimer += deltaTime;
                if (_turnTimer > TurnHardCapSeconds)
                {
                    ForceFire();
                    return;
                }
            }

            switch (_phase)
            {
                case Phase.Idle:
                    BeginTurn(state);
                    break;
                case Phase.Repositioning:
                    StepReposition(deltaTime);
                    break;
                case Phase.RepositionJump:
                    _simulation.TryStartJump(_jumpDirection);
                    _phase = Phase.WaitStable;
                    break;
                case Phase.WaitStable:
                    _plan = PlanShotFromCurrent();
                    _aimTimer = _profile.ThinkPauseSeconds + _profile.AimPauseSeconds;
                    _phase = Phase.Aiming;
                    Activity = BotActivity.Aiming;
                    break;
                case Phase.Aiming:
                    _aimTimer -= deltaTime;
                    if (_aimTimer <= 0f)
                    {
                        _phase = Phase.Firing;
                    }

                    break;
                case Phase.Firing:
                    Execute();
                    _phase = Phase.Done;
                    break;
                case Phase.Done:
                    break;
            }
        }

        private void BeginTurn(BattleState state)
        {
            _plannedTurn = state.Turn.TurnNumber;
            _turnTimer = 0f;
            _plan = null;

            var unit = ActiveOpponentUnit();
            if (unit == null)
            {
                _phase = Phase.Firing; // Execute yields the turn.
                return;
            }

            var inventory = _simulation.ActiveInventory;
            if (inventory == null || !inventory.TrySelectNextUsable() || inventory.Selected == null)
            {
                _phase = Phase.Firing;
                return;
            }

            var target = SelectTarget(unit);
            if (target == null)
            {
                _phase = Phase.Firing;
                return;
            }

            _weapon = inventory.Selected;
            _aimSettings = _weapon.CreateAimSettings(_baseAimSettings);
            _scatteredTarget = TargetScatter.Apply(target.Position, unit.Position, _profile, _random);
            unit.SetFacing(target.Position.x >= unit.Position.x ? 1 : -1);

            if (_profile.MovementEnabled && _positionPlanner != null)
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                var decision = _positionPlanner.SelectPosition(
                    unit.Position, _scatteredTarget, target.Position, _weapon, _aimSettings, BuildBodies(unit, target));
                stopwatch.Stop();
                LastPlanningMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
                LastPositionsConsidered = _positionPlanner.LastCandidateCount;
                LastMoveKind = decision.Kind;
                switch (decision.Kind)
                {
                    case BotMoveKind.Walk:
                        _targetX = decision.Position.x;
                        _moveTimer = 0f;
                        _stallTimer = 0f;
                        _lastMoveX = unit.Position.x;
                        _phase = Phase.Repositioning;
                        Activity = BotActivity.Repositioning;
                        return;
                    case BotMoveKind.Jump:
                        _jumpDirection = decision.JumpDirection;
                        _phase = Phase.RepositionJump;
                        Activity = BotActivity.Repositioning;
                        return;
                }
            }

            LastMoveKind = BotMoveKind.Stay;
            _plan = PlanShotFromCurrent();
            _aimTimer = _profile.ThinkPauseSeconds + _profile.AimPauseSeconds;
            _phase = Phase.Aiming;
            Activity = BotActivity.Aiming;
        }

        private void StepReposition(float deltaTime)
        {
            var unit = ActiveOpponentUnit();
            if (unit == null)
            {
                _simulation.Walk(0, 0f);
                _phase = Phase.Firing;
                return;
            }

            _moveTimer += deltaTime;

            // Stall detector: barely moving for a while means blocked — stop.
            if (Mathf.Abs(unit.Position.x - _lastMoveX) < 0.01f)
            {
                _stallTimer += deltaTime;
            }
            else
            {
                _stallTimer = 0f;
                _lastMoveX = unit.Position.x;
            }

            var arrived = Mathf.Abs(_targetX - unit.Position.x) <= MoveEpsilon;
            var exhausted = _moveTimer > _profile.MaxWalkBudgetSeconds + 1f || _stallTimer > StallTimeout;
            if (arrived || exhausted)
            {
                _simulation.Walk(0, 0f);
                _phase = Phase.WaitStable;
                return;
            }

            var direction = _targetX > unit.Position.x ? 1 : -1;
            if (_simulation.Walk(direction, 0f) == BattleWalkOutcome.NotAllowed)
            {
                _simulation.Walk(0, 0f);
                _phase = Phase.WaitStable;
            }
        }

        private void ForceFire()
        {
            _simulation.Walk(0, 0f);
            _plan ??= PlanShotFromCurrent();
            Execute();
            _phase = Phase.Done;
        }

        private void Execute()
        {
            if (_plan.HasValue)
            {
                Activity = BotActivity.Firing;
                _simulation.TryFire(_plan.Value.Aim);
                return;
            }

            _simulation.AdvanceTurnDueToNoUsableWeapon();
        }

        private BotShotPlan? PlanShotFromCurrent()
        {
            var unit = ActiveOpponentUnit();
            if (unit == null || _weapon == null || !_simulation.CanActiveUnitFire()
                || !_profile.EvaluateAllWeapons && (!BattleSimulation.IsBattleMechanicAvailable(_weapon)
                    || !_simulation.CanActiveUnitUseWeapon(_weapon)))
            {
                return null;
            }

            var target = SelectTarget(unit);
            if (target == null)
            {
                return null;
            }

            _scatteredTarget = TargetScatter.Apply(target.Position, unit.Position, _profile, _random);
            var bodies = BuildBodies(unit, target);
            var plan = _profile.EvaluateAllWeapons
                ? PlanBestWeapon(unit.Position, bodies)
                : _planner.PlanShot(new BotShotContext(unit.Position, _scatteredTarget, _weapon, _aimSettings, bodies));
            if (plan != null)
            {
                LastPlanScore = plan.Value.Score;
                return plan;
            }

            // Unreachable at every sampled power: desperation shot so the bot
            // still acts (and likely reshapes terrain for next turn).
            var toTarget = _scatteredTarget - unit.Position;
            var direction = toTarget.sqrMagnitude > 0.01f ? toTarget.normalized : Vector2.up;
            var power = Mathf.Lerp(_aimSettings.MinPower, _aimSettings.MaxPower, 0.8f);
            return new BotShotPlan(new AimState(unit.Position, direction, power, true), _weapon.WeaponId, 0f);
        }

        /// <summary>
        /// Hard-difficulty weapon choice: plan a shot with every usable weapon
        /// from the given origin and keep the one with the best expected value
        /// (hit likelihood x damage), leaving that weapon selected so the fired
        /// shot matches the plan.
        /// </summary>
        private BotShotPlan? PlanBestWeapon(Vector2 origin, IReadOnlyList<TrajectoryUnitBody> bodies)
        {
            var inventory = _simulation.ActiveInventory;
            if (inventory == null || inventory.Count == 0)
            {
                return _planner.PlanShot(new BotShotContext(origin, _scatteredTarget, _weapon, _aimSettings, bodies));
            }

            var bestIndex = inventory.SelectedIndex;
            BotShotPlan? bestPlan = null;
            var bestValue = float.NegativeInfinity;

            for (var i = 0; i < inventory.Count; i++)
            {
                inventory.Select(i);
                if (!inventory.CanFireSelected || !BattleSimulation.IsBattleMechanicAvailable(inventory.Selected)
                    || !_simulation.CanActiveUnitUseWeapon(inventory.Selected))
                {
                    continue;
                }

                var weapon = inventory.Selected;
                var aimSettings = weapon.CreateAimSettings(_baseAimSettings);
                var plan = _planner.PlanShot(new BotShotContext(origin, _scatteredTarget, weapon, aimSettings, bodies));
                if (plan == null)
                {
                    continue;
                }

                var expectedValue = Mathf.Clamp01(plan.Value.Score) * weapon.Damage;
                if (bestPlan == null || expectedValue > bestValue)
                {
                    bestValue = expectedValue;
                    bestPlan = plan;
                    bestIndex = i;
                }
            }

            inventory.Select(bestIndex);
            if (bestPlan != null)
            {
                _weapon = inventory.Selected;
                _aimSettings = _weapon.CreateAimSettings(_baseAimSettings);
            }

            return bestPlan;
        }

        private BattleUnitState ActiveOpponentUnit()
        {
            var activeUnitId = _simulation.State.Turn.ActiveUnitId;
            if (string.IsNullOrEmpty(activeUnitId))
            {
                return null;
            }

            var unit = _simulation.State.GetUnit(activeUnitId);
            return unit != null && !unit.IsDefeated && unit.TeamId == BattleTeamId.Opponent ? unit : null;
        }

        private BattleUnitState SelectTarget(BattleUnitState from)
        {
            BattleUnitState best = null;
            var bestDistanceSq = float.PositiveInfinity;
            foreach (var candidate in _simulation.State.GetLivingUnits(BattleTeamId.Player))
            {
                var distanceSq = (candidate.Position - from.Position).sqrMagnitude;
                if (distanceSq < bestDistanceSq)
                {
                    bestDistanceSq = distanceSq;
                    best = candidate;
                }
            }

            return best;
        }

        private IReadOnlyList<TrajectoryUnitBody> BuildBodies(BattleUnitState owner, BattleUnitState target)
        {
            _bodies.Clear();
            foreach (var team in _simulation.State.Teams)
            {
                var halfHeight = ArchetypeFor(team.TeamId).BodyHeight * 0.5f;
                foreach (var unit in team.Units)
                {
                    if (unit == null || unit.IsDefeated || unit.UnitId == owner.UnitId)
                    {
                        continue;
                    }

                    _bodies.Add(new TrajectoryUnitBody(
                        unit.Position,
                        _unitCollisionHalfWidth,
                        halfHeight,
                        isAlly: unit.TeamId == owner.TeamId,
                        isTarget: target != null && unit.UnitId == target.UnitId));
                }
            }

            return _bodies;
        }

        private CharacterArchetype ArchetypeFor(BattleTeamId teamId)
        {
            return teamId == BattleTeamId.Player ? _playerArchetype : _opponentArchetype;
        }

        private void ResetToIdle()
        {
            if (_phase != Phase.Idle)
            {
                _simulation.Walk(0, 0f);
            }

            _phase = Phase.Idle;
            Activity = BotActivity.Idle;
        }
    }
}
