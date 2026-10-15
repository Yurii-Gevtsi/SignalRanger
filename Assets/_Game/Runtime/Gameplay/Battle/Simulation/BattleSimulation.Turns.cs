using System;
using System.Collections.Generic;
using Game.Data.Progression;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Motion;
using Game.Gameplay.Battle.Projectiles;
using Game.Gameplay.Battle.Terrain;
using Game.Gameplay.Battle.Units;
using Game.Gameplay.Battle.Weapons;
using UnityEngine;

namespace Game.Gameplay.Battle.Simulation
{
    public sealed partial class BattleSimulation
    {
        public void SetPlayerTurnTimerPaused(bool paused)
        {
            _playerTurnTimerPaused = paused;
        }

        /// <summary>
        /// Ends the active turn without an action, the same way the turn timer does.
        /// Used when a combatant gives their turn up explicitly - a remote player
        /// passing, or a battle being resynchronised past a turn nobody completed.
        /// </summary>
        public void EndActiveTurn()
        {
            if (Result.IsFinished || GetActiveUnitOrNull() == null)
            {
                return;
            }

            EndPlayerTurnFromTimer();
        }

        public void AdvanceTurnDueToNoUsableWeapon()
        {
            ProcessPoisonForEndingActiveTurn();
            ProcessElementalForEndingActiveTurn(_chillEffects);
            ProcessElementalForEndingActiveTurn(_fireEffects);
            ProcessElementalForEndingActiveTurn(_electricEffects);
            ProcessRepairJammerForEndingActiveTurn();
            ProcessVirusForEndingActiveTurn();
            ProcessSelfDestructForEndingActiveTurn();
            ConsumeDisorientationForActiveTurn();
            ConsumeFreezeForActiveTurn();
            ConsumeAimJamForActiveTurn();
            ConsumeEmpStunForActiveTurn();
            ExpireGroundersForEndingTurn();
            _turnService.AdvanceTurn(_state);
            _turnSequence++;
            ResetMotionForNewTurn();
            ProcessRepairEffectsForActiveTurn();
            ProcessBurnForActiveTurn();
            ProcessEmpWeakenForActiveTurn();
            LaunchPendingOrbitalStrikeIfDue();
            TurnAdvanced?.Invoke(_state.Turn.ActiveTeam, TurnAdvanceReason.OpponentSkippedNoUsableWeapon);
            UnitsMoved?.Invoke();
        }

        /// <summary>
        /// Ends the acting team's turn. The player gets the movement-only
        /// retreat window first (bounded by the turn timer); the bot hands the
        /// turn over immediately.
        /// </summary>
        private void AdvanceTurnOrStartRetreat(TurnAdvanceReason reason)
        {
            if (_state.Turn.ActiveTeam == BattleTeamId.Player)
            {
                var retreat = Mathf.Min(RetreatSeconds, _playerMovement.RemainingTurnSeconds);
                if (retreat > 0.01f)
                {
                    // Instant utilities (C4, heal) reach here without a spent
                    // action; spending it here keeps TryFire blocked while
                    // movement stays allowed via IsPlayerRetreatActive.
                    _turnService.SpendAction(_state);
                    _retreatRemaining = retreat;
                    _pendingRetreatReason = reason;
                    UnitsMoved?.Invoke();
                    return;
                }
            }

            AdvanceTurnNow(reason);
        }

        private void AdvanceTurnNow(TurnAdvanceReason reason)
        {
            ProcessPoisonForEndingActiveTurn();
            ProcessElementalForEndingActiveTurn(_chillEffects);
            ProcessElementalForEndingActiveTurn(_fireEffects);
            ProcessElementalForEndingActiveTurn(_electricEffects);
            ProcessRepairJammerForEndingActiveTurn();
            ProcessVirusForEndingActiveTurn();
            ProcessSelfDestructForEndingActiveTurn();
            ConsumeDisorientationForActiveTurn();
            ConsumeFreezeForActiveTurn();
            ConsumeAimJamForActiveTurn();
            ConsumeEmpStunForActiveTurn();
            ExpireGroundersForEndingTurn();
            _turnService.AdvanceTurn(_state);
            _turnSequence++;
            ResetMotionForNewTurn();
            ProcessRepairEffectsForActiveTurn();
            ProcessBurnForActiveTurn();
            ProcessEmpWeakenForActiveTurn();
            LaunchPendingOrbitalStrikeIfDue();
            TurnAdvanced?.Invoke(_state.Turn.ActiveTeam, reason);
            UnitsMoved?.Invoke();
        }

        private void StepRetreat(float deltaTime)
        {
            if (_retreatRemaining <= 0f || Result.IsFinished)
            {
                return;
            }

            _retreatRemaining -= deltaTime;
            if (_retreatRemaining <= 0f)
            {
                _retreatRemaining = -1f;
                AdvanceTurnNow(_pendingRetreatReason);
            }
        }

        private void ResetMotionForNewTurn()
        {
            _turnTransfer = null;
            EndBoost();
            if (_ropeTraversal != null)
            {
                _ropeTraversal.Detach(GetMotor(_ropeTraversal.OwnerUnitId));
                _ropeTraversal = null;
            }
            _walkIntentDirection = 0;
            _retreatRemaining = -1f;
            _controlledProjectileInput = Vector2.zero;
            if (_projectiles.Count == 0)
            {
                _controlledProjectileId = -1;
            }
            _blasterShotsThisTurn = 0;
            _minesDeployedThisTurn = 0;
            _laserFenceNodesDeployedThisTurn = 0;
            _activeShotEndsTurn = true;
            foreach (var motor in _motors.Values)
            {
                motor.ResetWalk();
            }

            _playerMovement.Reset(_settings.PlayerTurnDurationSeconds);
        }

        /// <summary>
        /// First press of the turn transfer: the choice opens on the nearest living ally on the side the droid faces
        /// (or the nearest one). Without an ally nothing happens and nothing is spent.
        /// </summary>
        private BattleFireResult BeginTurnTransfer(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon)
        {
            if (!inventory.CanFireSelected)
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            BattleUnitState best = null;
            var bestScore = float.PositiveInfinity;
            foreach (var ally in _state.GetTeam(unit.TeamId).Units)
            {
                if (ally == null || ally.IsDefeated || ally.UnitId == unit.UnitId)
                {
                    continue;
                }

                var dx = ally.Position.x - unit.Position.x;
                var onFacingSide = Mathf.Sign(dx) == Mathf.Sign(unit.FacingDirectionX == 0 ? 1 : unit.FacingDirectionX);
                var score = Mathf.Abs(dx) + (onFacingSide ? 0f : 1000f);
                if (score < bestScore)
                {
                    bestScore = score;
                    best = ally;
                }
            }

            if (best == null)
            {
                return new BattleFireResult(BattleFireOutcome.Blocked, unit.TeamId, weapon);
            }

            _turnTransfer = new TurnTransferSelection(unit.UnitId, weapon, best.UnitId);
            UnitsMoved?.Invoke();
            return new BattleFireResult(BattleFireOutcome.UtilityUsed, unit.TeamId, weapon, best.Position);
        }

        private bool HasLivingAllyOfActiveUnit()
        {
            foreach (var ally in _state.GetTeam(_state.Turn.ActiveTeam).Units)
            {
                if (ally != null && !ally.IsDefeated && ally.UnitId != _state.Turn.ActiveUnitId)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Left/right browses the allies one by one in that direction from the current choice.</summary>
        public bool SelectTurnTransferTarget(int direction)
        {
            if (!HasTurnTransferSelection || direction == 0)
            {
                return false;
            }

            var current = FindUnit(_turnTransfer.TargetUnitId);
            var owner = FindUnit(_turnTransfer.OwnerUnitId);
            var originX = current?.Position.x ?? owner?.Position.x ?? 0f;
            BattleUnitState best = null;
            var bestDistance = float.PositiveInfinity;
            foreach (var ally in _state.GetTeam(owner?.TeamId ?? _state.Turn.ActiveTeam).Units)
            {
                if (ally == null || ally.IsDefeated || ally.UnitId == _turnTransfer.OwnerUnitId
                    || ally.UnitId == _turnTransfer.TargetUnitId)
                {
                    continue;
                }

                var dx = ally.Position.x - originX;
                if (Mathf.Sign(dx) != Mathf.Sign(direction) || Mathf.Abs(dx) >= bestDistance)
                {
                    continue;
                }

                bestDistance = Mathf.Abs(dx);
                best = ally;
            }

            if (best == null)
            {
                return false;
            }

            _turnTransfer.TargetUnitId = best.UnitId;
            UnitsMoved?.Invoke();
            return true;
        }

        /// <summary>Changing the weapon closes an unconfirmed choice; nothing was spent.</summary>
        public void CancelTurnTransfer()
        {
            _turnTransfer = null;
        }

        /// <summary>
        /// Second press: the charge is spent and the chosen ally continues the same turn with the time that is left.
        /// The turn does not end, no status ticks and the action is still available.
        /// </summary>
        private BattleFireResult ConfirmTurnTransfer()
        {
            var selection = _turnTransfer;
            _turnTransfer = null;
            var owner = FindUnit(selection.OwnerUnitId);
            var target = FindUnit(selection.TargetUnitId);
            var inventory = owner != null ? GetInventoryFor(owner.TeamId) : null;
            if (owner == null || target == null || target.IsDefeated || inventory == null
                || inventory.Selected?.WeaponId != TurnTransferWeaponId || !inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.Blocked, _state.Turn.ActiveTeam, selection.Weapon);
            }

            _walkIntentDirection = 0;
            if (!_turnService.TransferTurn(_state, target.UnitId))
            {
                return new BattleFireResult(BattleFireOutcome.Blocked, _state.Turn.ActiveTeam, selection.Weapon);
            }

            ShotFired?.Invoke(owner.TeamId, selection.Weapon, false);
            TurnTransferred?.Invoke(owner.UnitId, target.UnitId);
            UnitsMoved?.Invoke();
            return new BattleFireResult(BattleFireOutcome.UtilityUsed, owner.TeamId, selection.Weapon, target.Position);
        }

        private sealed class TurnTransferSelection
        {
            public TurnTransferSelection(string ownerUnitId, WeaponDefinition weapon, string targetUnitId)
            {
                OwnerUnitId = ownerUnitId;
                Weapon = weapon;
                TargetUnitId = targetUnitId;
            }

            public string OwnerUnitId { get; }
            public WeaponDefinition Weapon { get; }
            public string TargetUnitId { get; set; }
        }

        private void EndPlayerTurnFromTimer()
        {
            _walkIntentDirection = 0;
            _orbitalTargetMarker = null;
            ProcessPoisonForEndingActiveTurn();
            ProcessElementalForEndingActiveTurn(_chillEffects);
            ProcessElementalForEndingActiveTurn(_fireEffects);
            ProcessElementalForEndingActiveTurn(_electricEffects);
            ProcessRepairJammerForEndingActiveTurn();
            ProcessVirusForEndingActiveTurn();
            ProcessSelfDestructForEndingActiveTurn();
            ConsumeDisorientationForActiveTurn();
            ConsumeFreezeForActiveTurn();
            ConsumeAimJamForActiveTurn();
            ConsumeEmpStunForActiveTurn();
            ExpireGroundersForEndingTurn();
            _turnService.AdvanceTurn(_state);
            _turnSequence++;
            ResetMotionForNewTurn();
            ProcessRepairEffectsForActiveTurn();
            ProcessBurnForActiveTurn();
            ProcessEmpWeakenForActiveTurn();
            TurnAdvanced?.Invoke(_state.Turn.ActiveTeam, TurnAdvanceReason.PlayerTimerExpired);
            UnitsMoved?.Invoke();
        }

        private void StepPlayerTurnTimer(float tickSeconds)
        {
            // HasPendingGroundAction: a burning C4 fuse must resolve within the
            // owner's turn - if the timer expired mid-fuse the turn would pass
            // to the bot while TryFire stays blocked, deadlocking its turn.
            if (_playerTurnTimerPaused
                || _state.Turn.ActiveTeam != BattleTeamId.Player
                || _state.Turn.ActionSpent
                || HasProjectilesInFlight
                || HasPendingGroundAction
                || Result.IsFinished)
            {
                return;
            }

            _playerMovement.Tick(tickSeconds);
            if (!_playerMovement.HasTimeRemaining)
            {
                EndPlayerTurnFromTimer();
            }
        }
    }
}
