using System.Collections.Generic;
using Game.Gameplay.Battle.AI.Ballistics;
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
        Aiming,
        Firing
    }

    /// <summary>
    /// Drives the opponent's turn as a small state machine: plan once, pause so
    /// the player can read the intent, then fire through the same
    /// <c>BattleSimulation.TryFire</c> the player uses. No scene, no input — the
    /// scene controller only ticks this and reflects <see cref="Activity"/>.
    /// Movement steps arrive in Stage 3; for now the bot aims and shoots in place.
    /// </summary>
    public sealed class BotTurnController
    {
        private enum Phase
        {
            Idle,
            Aiming,
            Firing,
            Done
        }

        private readonly BattleSimulation _simulation;
        private readonly IBotShotPlanner _planner;
        private readonly BotProfile _profile;
        private readonly IBotRandom _random;
        private readonly float _unitCollisionHalfWidth;
        private readonly CharacterArchetype _playerArchetype;
        private readonly CharacterArchetype _opponentArchetype;
        private readonly AimSettings _baseAimSettings;
        private readonly List<TrajectoryUnitBody> _bodies = new();

        private Phase _phase = Phase.Idle;
        private float _timer;
        private BotShotPlan? _plan;
        private int _plannedTurn = -1;

        public BotTurnController(
            BattleSimulation simulation,
            IBotShotPlanner planner,
            BotProfile profile,
            IBotRandom random,
            float unitCollisionHalfWidth,
            CharacterArchetype playerArchetype,
            CharacterArchetype opponentArchetype,
            AimSettings baseAimSettings)
        {
            _simulation = simulation;
            _planner = planner;
            _profile = profile;
            _random = random;
            _unitCollisionHalfWidth = unitCollisionHalfWidth;
            _playerArchetype = playerArchetype;
            _opponentArchetype = opponentArchetype;
            _baseAimSettings = baseAimSettings;
        }

        public BotActivity Activity { get; private set; } = BotActivity.Idle;

        public void Tick(float deltaTime)
        {
            var state = _simulation.State;
            if (_simulation.Result.IsFinished || state.Turn.ActiveTeam != BattleTeamId.Opponent)
            {
                _phase = Phase.Idle;
                Activity = BotActivity.Idle;
                return;
            }

            // Let shots and knockback settle before deciding anything.
            if (_simulation.HasProjectilesInFlight || _simulation.HasActiveMotion)
            {
                return;
            }

            // A fresh opponent turn (new turn number) restarts the plan even though
            // Tick is only called during the opponent's turn, never in between.
            if (_phase != Phase.Idle && state.Turn.TurnNumber != _plannedTurn)
            {
                _phase = Phase.Idle;
            }

            switch (_phase)
            {
                case Phase.Idle:
                    _plannedTurn = state.Turn.TurnNumber;
                    _plan = BuildPlan();
                    _timer = _profile.ThinkPauseSeconds + _profile.AimPauseSeconds;
                    _phase = Phase.Aiming;
                    Activity = BotActivity.Aiming;
                    break;
                case Phase.Aiming:
                    _timer -= deltaTime;
                    if (_timer <= 0f)
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

        private void Execute()
        {
            if (_plan.HasValue)
            {
                Activity = BotActivity.Firing;
                _simulation.TryFire(_plan.Value.Aim);
                return;
            }

            // Nothing to fire (no target or no usable weapon): yield the turn.
            _simulation.AdvanceTurnDueToNoUsableWeapon();
        }

        private BotShotPlan? BuildPlan()
        {
            var state = _simulation.State;
            var activeUnitId = state.Turn.ActiveUnitId;
            if (string.IsNullOrEmpty(activeUnitId))
            {
                return null;
            }

            var unit = state.GetUnit(activeUnitId);
            if (unit == null || unit.IsDefeated || unit.TeamId != BattleTeamId.Opponent)
            {
                return null;
            }

            var inventory = _simulation.ActiveInventory;
            if (inventory == null || !inventory.TrySelectNextUsable())
            {
                return null;
            }

            var weapon = inventory.Selected;
            if (weapon == null)
            {
                return null;
            }

            var target = SelectTarget(unit);
            if (target == null)
            {
                return null;
            }

            unit.SetFacing(target.Position.x >= unit.Position.x ? 1 : -1);

            var origin = unit.Position;
            var scatteredTarget = TargetScatter.Apply(target.Position, origin, _profile, _random);
            var aimSettings = weapon.CreateAimSettings(_baseAimSettings);
            var context = new BotShotContext(origin, scatteredTarget, weapon, aimSettings, BuildBodies(unit, target));

            var plan = _planner.PlanShot(context);
            if (plan == null)
            {
                // Target unreachable at every sampled power: desperation shot so
                // the bot still acts (and likely reshapes terrain for next turn).
                var direction = (scatteredTarget - origin).sqrMagnitude > 0.01f
                    ? (scatteredTarget - origin).normalized
                    : Vector2.up;
                var power = Mathf.Lerp(aimSettings.MinPower, aimSettings.MaxPower, 0.8f);
                plan = new BotShotPlan(new AimState(origin, direction, power, true), weapon.WeaponId, 0f);
            }

            return plan;
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
                        isTarget: unit.UnitId == target.UnitId));
                }
            }

            return _bodies;
        }

        private CharacterArchetype ArchetypeFor(BattleTeamId teamId)
        {
            return teamId == BattleTeamId.Player ? _playerArchetype : _opponentArchetype;
        }
    }
}
