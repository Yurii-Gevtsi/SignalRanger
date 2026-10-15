using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Core.Modes;
using Game.Core.Battle;
using Game.Data.Progression;
using Game.Data.Save;
using Game.Core.Localization;
using Game.Gameplay.Battle.Arenas;
using Game.Gameplay.Battle.AI;
using Game.Gameplay.Battle.AI.Ballistics;
using Game.Gameplay.Battle.AI.Movement;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Feedback;
using Game.Gameplay.Battle.Motion;
using Game.Gameplay.Battle.Net;
using Game.Gameplay.Battle.Projectiles;
using Game.Gameplay.Battle.Results;
using Game.Gameplay.Battle.Runtime;
using Game.Gameplay.Battle.Simulation;
using Game.Gameplay.Battle.Terrain;
using Game.Gameplay.Battle.Units;
using Game.Gameplay.Battle.Visuals;
using Game.Gameplay.Battle.Weapons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.Gameplay.Battle.Assembly
{
    public sealed partial class BattleSceneController
    {
        private bool CanContinueMinePlacementInput()
        {
            // The sphere's throw already spent the action; the early trigger is the follow-up press.
            if (_battleState.Turn.ActiveTeam == BattleTeamId.Player
                && _inventory?.Selected?.WeaponId == "weapon.teleport"
                && _simulation.CanTriggerTeleportEarly())
            {
                return true;
            }

            if (_battleState.Turn.ActiveTeam != BattleTeamId.Player
                || _inventory == null
                || HasActiveMotion
                || HasActiveWeaponEffect
                || HasPendingGroundAction
                || _simulation.IsAttackCommitted)
            {
                return false;
            }

            return (_inventory.Selected?.WeaponId == "weapon.mine"
                    && _inventory.CanFireSelected)
                || (_inventory.Selected?.WeaponId == "weapon.hopping-grenade"
                    && _simulation.CanContinueHoppingGrenade())
                || (_inventory.Selected?.WeaponId == "weapon.teleport"
                    && _simulation.CanTriggerTeleportEarly());
        }

        private void UpdateControlledProjectileInput()
        {
            if (_controlledUiHorizontalInput != 0 || _ropeVerticalInput != 0)
            {
                _controlledPointerActive = false;
                _simulation.SetControlledProjectileInput(new Vector2(_controlledUiHorizontalInput, -_ropeVerticalInput));
                return;
            }

            var keyboardDirection = Vector2.zero;
            if (Keyboard.current != null)
            {
                keyboardDirection = new Vector2(
                    (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1f : 0f)
                    - (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? 1f : 0f),
                    (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed ? 1f : 0f)
                    - (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed ? 1f : 0f));
            }

            if (keyboardDirection.sqrMagnitude > 0.01f)
            {
                _controlledPointerActive = false;
                _simulation.SetControlledProjectileInput(keyboardDirection);
                return;
            }

            // Owner decision 2026-10-08: a drone is piloted with the on-screen buttons (or the keyboard) only; a
            // finger on the arena moves and zooms the camera instead.
            _controlledPointerActive = false;
            _simulation.SetControlledProjectileInput(Vector2.zero);
        }

        private bool TryReadPointer(
            out bool pressed,
            out bool pressStarted,
            out bool released,
            out Vector2 screenPosition,
            out bool pointerOverUi)
        {
            pressed = false;
            pressStarted = false;
            released = false;
            screenPosition = default;
            pointerOverUi = false;

            if (_camera == null)
            {
                return false;
            }

            if (Touchscreen.current != null)
            {
                var touch = Touchscreen.current.primaryTouch;
                pressed = touch.press.isPressed;
                pressStarted = touch.press.wasPressedThisFrame;
                released = touch.press.wasReleasedThisFrame;
                screenPosition = touch.position.ReadValue();
                pointerOverUi = IsPointerOverUi((int)touch.touchId.ReadValue());
                return true;
            }

            if (Mouse.current != null)
            {
                var button = Mouse.current.leftButton;
                pressed = button.isPressed;
                pressStarted = button.wasPressedThisFrame;
                released = button.wasReleasedThisFrame;
                screenPosition = Mouse.current.position.ReadValue();
                pointerOverUi = IsPointerOverUi();
                return true;
            }

            return false;
        }

        private Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return _camera.ScreenToWorldPoint(screenPosition);
        }

        private void SetUiHorizontalInput(int direction)
        {
            // While the turn transfer is choosing, left/right browse the allies instead of walking.
            if (_simulation != null && _simulation.HasTurnTransferSelection)
            {
                _simulation.SelectTurnTransferTarget(NormalizeDirection(direction));
                return;
            }

            if (_simulation != null && _simulation.HasActiveControlledProjectile)
            {
                _controlledUiHorizontalInput = NormalizeDirection(direction);
                return;
            }

            if (IsTitaniumBarrierPlacementActive())
            {
                _controlledUiHorizontalInput = NormalizeDirection(direction);
                return;
            }

            SetUiWalkInput(direction);
        }

        private void StopUiHorizontalInput(int direction)
        {
            if (_simulation != null && _simulation.HasActiveControlledProjectile)
            {
                if (_controlledUiHorizontalInput == NormalizeDirection(direction))
                {
                    _controlledUiHorizontalInput = 0;
                }

                return;
            }

            if (IsTitaniumBarrierPlacementActive())
            {
                if (_controlledUiHorizontalInput == NormalizeDirection(direction))
                {
                    _controlledUiHorizontalInput = 0;
                }

                return;
            }

            StopUiWalkInput(direction);
        }

        private void SetUiWalkInput(int direction)
        {
            if (_firstBattleGuide != null
                && _firstBattleGuide.IsActive
                && !_firstBattleGuide.AllowsWalkDirection(direction))
            {
                _uiWalkInputDirection = 0;
                return;
            }

            if (!CanPlayerAcceptMovementInput())
            {
                _uiWalkInputDirection = 0;
                if (IsMovementBlockedByGuide())
                {
                    _status = GameText.MovementGuideBlocked();
                }

                return;
            }

            _uiWalkInputDirection = NormalizeDirection(direction);
            EmitDuelCommand(BattleCommandKind.WalkIntent, _uiWalkInputDirection);
        }

        private void StopUiWalkInput(int direction)
        {
            if (_uiWalkInputDirection == NormalizeDirection(direction))
            {
                _uiWalkInputDirection = 0;
                EmitDuelCommand(BattleCommandKind.WalkIntent, 0);
            }
        }

        private void SetKeyboardWalkInput(int direction)
        {
            var normalizedDirection = NormalizeDirection(direction);
            _keyboardWalkInputDirection = CanPlayerAcceptMovementInput()
                && (_firstBattleGuide == null || !_firstBattleGuide.IsActive || _firstBattleGuide.AllowsWalkDirection(normalizedDirection))
                    ? normalizedDirection
                    : 0;
        }

        private static int NormalizeDirection(int direction)
        {
            return direction < 0 ? -1 : direction > 0 ? 1 : 0;
        }

        private bool CanPlayerUseMovement()
        {
            return IsBattleReady()
                && IsPlayerControlTurn(_battleState.Turn.ActiveTeam)
                && _simulation.CanUseMovement();
        }

        private bool CanPlayerAcceptMovementInput()
        {
            return IsBattleReady()
                && IsPlayerControlTurn(_battleState.Turn.ActiveTeam)
                && _simulation.CanAcceptMovementInput();
        }

        private static bool IsPlayerControlTurn(BattleTeamId activeTeam)
        {
            return activeTeam == BattleTeamId.Player;
        }

        private bool IsMovementBlockedByGuide()
        {
            return _firstBattleGuide != null && _firstBattleGuide.IsActive;
        }

        private void UpdateWalking()
        {
            if (_simulation != null && (_simulation.IsPlayerRopeAttached || _simulation.HasPlayerBoost))
            {
                UpdateRopeTraversalInput();
                return;
            }
            var walkDirection = _keyboardWalkInputDirection != 0 ? _keyboardWalkInputDirection : _uiWalkInputDirection;
            if (walkDirection == 0)
            {
                // During the two mandatory navigation steps, preserve an accepted
                // intent until at least one fixed simulation tick has moved the unit.
                if (_pendingTutorialWalkDirection == 0)
                {
                    _simulation?.Walk(0, 0f);
                }

                return;
            }

            if (!CanPlayerUseMovement())
            {
                // Preserve the held direction while the motor is airborne. The
                // command is submitted again automatically after landing.
                return;
            }

            var outcome = _simulation.Walk(walkDirection, 0f);
            switch (outcome)
            {
                case BattleWalkOutcome.Moved:
                    ResetAimState();
                    _status = GameText.Moved(DescribeTeam(BattleTeamId.Player));
                    if (_firstBattleGuide != null
                        && (_firstBattleGuide.CurrentPhase == FirstBattleGuideState.GuidePhase.MoveLeft
                            || _firstBattleGuide.CurrentPhase == FirstBattleGuideState.GuidePhase.MoveRight))
                    {
                        if (_pendingTutorialWalkDirection == 0)
                        {
                            _pendingTutorialWalkDirection = walkDirection;
                            _pendingTutorialWalkStartX = GetPresenter(BattleTeamId.Player)?.State.Position.x ?? 0f;
                        }
                    }
                    else
                    {
                        _firstBattleGuide?.ObserveWalk(walkDirection);
                    }
                    break;
                case BattleWalkOutcome.Blocked:
                    _status = GameText.MovementBlocked();
                    ClearWalkInput();
                    break;
                case BattleWalkOutcome.StartedFalling:
                    // Regular physics takes over from here (HasActiveMotion).
                    ResetAimState();
                    break;
            }
        }

        private void UpdateRopeTraversalInput()
        {
            if (_simulation == null)
            {
                return;
            }

            var horizontal = _keyboardWalkInputDirection != 0 ? _keyboardWalkInputDirection : _uiWalkInputDirection;
            if (_simulation.HasPlayerBoost)
            {
                // The shared rope buttons report "up" as -1; the fuel steering uses up = +1.
                _simulation.SetBoostInput(horizontal, -_ropeVerticalInput);
                return;
            }

            if (!_simulation.IsPlayerRopeAttached)
            {
                return;
            }

            _simulation.SetRopeTraversalInput(horizontal, _ropeVerticalInput);
        }

        private void ClearWalkInput()
        {
            EmitDuelCommand(BattleCommandKind.WalkIntent, 0);
            _uiWalkInputDirection = 0;
            _keyboardWalkInputDirection = 0;
            _controlledUiHorizontalInput = 0;
            _ropeVerticalInput = 0;
            _pendingTutorialWalkDirection = 0;
            _simulation?.Walk(0, 0f);
        }

        private void CompletePendingTutorialWalkIfMoved()
        {
            if (_pendingTutorialWalkDirection == 0)
            {
                return;
            }

            var presenter = GetPresenter(BattleTeamId.Player);
            if (presenter == null
                || (presenter.State.Position.x - _pendingTutorialWalkStartX) * _pendingTutorialWalkDirection < FirstBattleNavigationDistance)
            {
                return;
            }

            var completedDirection = _pendingTutorialWalkDirection;
            _pendingTutorialWalkDirection = 0;
            _firstBattleGuide?.ObserveWalk(completedDirection);
            ClearWalkInput();
        }

        private void TryStartJump(int direction)
        {
            EmitDuelCommand(BattleCommandKind.Jump, direction);
            if (_firstBattleGuide != null && _firstBattleGuide.IsActive && !_firstBattleGuide.AllowsJumpDirection(direction))
            {
                return;
            }

            if (!CanPlayerUseMovement())
            {
                if (IsMovementBlockedByGuide())
                {
                    _status = GameText.MovementGuideBlocked();
                }

                return;
            }

            var outcome = _simulation.TryStartJump(direction);
            if (outcome == BattleJumpOutcome.Jumped)
            {
                ResetAimState();
                _status = GameText.Jumped(DescribeTeam(BattleTeamId.Player));
                _firstBattleGuide?.ObserveJump(direction);
            }
        }

        private static bool IsPointerOverUi()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }

        private static bool IsPointerOverUi(int pointerId)
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(pointerId);
        }
    }
}
