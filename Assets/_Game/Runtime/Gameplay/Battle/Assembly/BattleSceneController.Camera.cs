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
        private void BuildCamera()
        {
            _camera = Camera.main;
            if (_camera == null)
            {
                var cameraObject = new GameObject("Main Camera");
                _camera = cameraObject.AddComponent<Camera>();
                cameraObject.tag = "MainCamera";
                cameraObject.AddComponent<AudioListener>();
            }

            _camera.orthographic = true;
            _camera.orthographicSize = 5.6f;
            _camera.transform.position = new Vector3(0f, 1.2f, -10f);
            _camera.backgroundColor = new Color(0.54f, 0.78f, 0.95f);
        }

        private void ApplyArenaCamera(ArenaDefinition arena)
        {
            _camera.transform.position = arena.CameraPosition;
            _minimumCameraSize = arena.CameraSize * CameraZoomInMultiplier;
            _configuredCameraFrame = _visualTheme?.CameraFrame;
            _cameraWorldBounds = _configuredCameraFrame is Rect configuredFrame
                ? ResolveCameraFrame(configuredFrame, _camera.aspect)
                : CalculateCameraWorldBounds(arena);
            RefreshCameraPanBounds();
            _maximumCameraSize = Mathf.Max(
                _minimumCameraSize,
                CalculateCameraSizeToContainBounds(_cameraWorldBounds, _camera.aspect));
            _camera.orthographicSize = Mathf.Clamp(arena.CameraSize, _minimumCameraSize, _maximumCameraSize);
            _hasPreviousPinchDistance = false;
            ClampCameraToArena();
        }

        private void UpdateCameraZoom()
        {
            if (_camera == null || _minimumCameraSize <= 0f || _maximumCameraSize <= 0f)
            {
                return;
            }

            // Steering a controlled projectile (FPV drone) owns the pointer -
            // camera zoom would fight the piloting input. Since 2026-10-08 drones are piloted with the buttons
            // only, so the fingers are free to zoom and pan the camera during the flight.

            if (IsTitaniumBarrierPlacementActive())
            {
                if (TryGetPinchDistance(out var barrierDistance))
                {
                    if (_hasPreviousPinchDistance && _previousPinchDistance > 0.001f)
                        _barrierPlacementScale = _simulation.ResolveTitaniumBarrierScale(
                            _barrierPlacementScale * barrierDistance / _previousPinchDistance, BattleTeamId.Player);
                    _previousPinchDistance = barrierDistance;
                    _hasPreviousPinchDistance = true;
                    _aimDragActive = false;
                    _cameraPanActive = false;
                }
                else _hasPreviousPinchDistance = false;
                return;
            }

            if (TryGetPinchDistance(out var pinchDistance, out var pinchMidpoint))
            {
                if (_hasPreviousPinchDistance)
                {
                    AdjustCameraZoom(-(pinchDistance - _previousPinchDistance) * PinchZoomSensitivity, pinchMidpoint);
                }

                _previousPinchDistance = pinchDistance;
                _hasPreviousPinchDistance = true;
                _aimDragActive = false;
                _cameraPanActive = false;
                return;
            }

            _hasPreviousPinchDistance = false;

            var zoomDelta = 0f;
            if (Keyboard.current != null)
            {
                if (Keyboard.current.equalsKey.wasPressedThisFrame || Keyboard.current.numpadPlusKey.wasPressedThisFrame)
                {
                    zoomDelta -= CameraZoomStep;
                }

                if (Keyboard.current.minusKey.wasPressedThisFrame || Keyboard.current.numpadMinusKey.wasPressedThisFrame)
                {
                    zoomDelta += CameraZoomStep;
                }
            }

            if (zoomDelta != 0f)
            {
                AdjustCameraZoom(zoomDelta, null);
            }

            if (Mouse.current != null && !IsPointerOverUi())
            {
                var scroll = Mouse.current.scroll.ReadValue().y;
                if (Mathf.Abs(scroll) > 0.01f)
                {
                    AdjustCameraZoom(-Mathf.Sign(scroll) * CameraZoomStep, Mouse.current.position.ReadValue());
                }
            }
        }

        private void UpdateCameraPan()
        {
            if (_camera == null
                || _battleState == null
                || _battleState.Turn.ActiveTeam != BattleTeamId.Player
                || _simulation == null
                || _playerWeaponSelected && SelectedWeaponOwnsTheDrag() && !_simulation.HasActiveControlledProjectile
                || _simulation.Result.IsFinished
                || TryGetPinchDistance(out _))
            {
                _cameraPanActive = false;
                return;
            }

            if (!TryReadPointer(out var pressed, out var pressStarted, out var released, out var screenPosition, out var pointerOverUi))
            {
                _cameraPanActive = false;
                return;
            }

            if (pressStarted && !pointerOverUi)
            {
                _cameraPanActive = true;
                _previousCameraPanScreenPosition = screenPosition;
            }

            if (_cameraPanActive && pressed)
            {
                var previousWorld = ScreenToWorld(_previousCameraPanScreenPosition);
                var currentWorld = ScreenToWorld(screenPosition);
                var delta = previousWorld - currentWorld;
                _camera.transform.position += new Vector3(delta.x, delta.y, 0f);
                _previousCameraPanScreenPosition = screenPosition;
                ClampCameraToArena();
            }

            if (released)
            {
                _cameraPanActive = false;
            }
        }

        /// <summary>
        /// A dragged finger aims (or places the beam) only for weapons that use it; with a mine, a sword or a device
        /// selected the finger moves the camera instead.
        /// </summary>
        private bool SelectedWeaponOwnsTheDrag()
        {
            var weapon = _inventory?.Selected;
            return weapon != null
                && (weapon.WeaponId == TitaniumBarrierRules.WeaponId
                    || BattleSimulation.RequiresAim(weapon) && !BattleSimulation.IsMarkedStrikeWeapon(weapon));
        }

        private bool TryGetPinchDistance(out float distance)
        {
            return TryGetPinchDistance(out distance, out _);
        }

        private bool TryGetPinchDistance(out float distance, out Vector2 midpoint)
        {
            distance = 0f;
            midpoint = default;
            if (Touchscreen.current == null || Touchscreen.current.touches.Count < 2)
            {
                return false;
            }

            var firstTouch = Touchscreen.current.touches[0];
            var secondTouch = Touchscreen.current.touches[1];
            if (!firstTouch.press.isPressed || !secondTouch.press.isPressed)
            {
                return false;
            }

            var firstPosition = firstTouch.position.ReadValue();
            var secondPosition = secondTouch.position.ReadValue();
            distance = Vector2.Distance(firstPosition, secondPosition);
            midpoint = (firstPosition + secondPosition) * 0.5f;
            return true;
        }

        // Keeps the world point under focusScreenPosition fixed on screen
        // while the orthographic size changes, so zoom converges on the
        // cursor/pinch midpoint instead of always snapping the view down to
        // the bottom of the map. A null focus (keyboard zoom) zooms on the
        // current camera center.
        private void AdjustCameraZoom(float sizeDelta, Vector2? focusScreenPosition)
        {
            if (_camera == null || Mathf.Approximately(sizeDelta, 0f) || _minimumCameraSize <= 0f || _maximumCameraSize <= 0f)
            {
                return;
            }

            var newSize = Mathf.Clamp(_camera.orthographicSize + sizeDelta, _minimumCameraSize, _maximumCameraSize);
            if (Mathf.Approximately(newSize, _camera.orthographicSize))
            {
                return;
            }

            Vector2? worldFocusBefore = focusScreenPosition.HasValue
                ? ScreenToWorld(focusScreenPosition.Value)
                : null;

            _camera.orthographicSize = newSize;

            if (worldFocusBefore.HasValue)
            {
                var worldFocusAfter = ScreenToWorld(focusScreenPosition.Value);
                var shift = worldFocusBefore.Value - worldFocusAfter;
                _camera.transform.position += new Vector3(shift.x, shift.y, 0f);
            }

            ClampCameraToArena();
        }

        private static Rect CalculateCameraWorldBounds(ArenaDefinition arena)
        {
            var hasBounds = false;
            var bounds = new Rect(arena.CameraPosition.x, arena.CameraPosition.y, 0f, 0f);
            foreach (var block in arena.TerrainBlocks)
            {
                if (!block.Solid)
                {
                    continue;
                }

                var halfSize = block.Size * 0.5f;
                var minimum = block.Position - halfSize;
                var maximum = block.Position + halfSize;
                if (!hasBounds)
                {
                    bounds = Rect.MinMaxRect(minimum.x, minimum.y, maximum.x, maximum.y);
                    hasBounds = true;
                }
                else
                {
                    bounds.xMin = Mathf.Min(bounds.xMin, minimum.x);
                    bounds.yMin = Mathf.Min(bounds.yMin, minimum.y);
                    bounds.xMax = Mathf.Max(bounds.xMax, maximum.x);
                    bounds.yMax = Mathf.Max(bounds.yMax, maximum.y);
                }
            }

            if (!hasBounds)
            {
                return Rect.MinMaxRect(
                    arena.CameraPosition.x - 9f,
                    arena.CameraPosition.y - 5f,
                    arena.CameraPosition.x + 9f,
                    arena.CameraPosition.y + 5f);
            }

            bounds.xMin -= 2f;
            bounds.xMax += 2f;
            bounds.yMin -= 3f;
            bounds.yMax = Mathf.Max(bounds.yMax + 6f, arena.CameraPosition.y + arena.CameraSize);
            return bounds;
        }

        private void ClampCameraToArena()
        {
            if (_camera == null || _cameraWorldBounds.width <= 0f || _cameraWorldBounds.height <= 0f)
            {
                return;
            }

            var fallbackCenter = _currentArena != null
                ? (Vector2)_currentArena.CameraPosition
                : _cameraWorldBounds.center;
            _camera.transform.position = ClampCameraPositionToBounds(
                _camera.transform.position,
                SelectCameraClampBounds(
                    _cameraWorldBounds,
                    _cameraPanBounds,
                    _currentArena != null && _currentArena.LockCameraAtZoomOutLimit,
                    _camera.orthographicSize,
                    _maximumCameraSize),
                _camera.orthographicSize,
                _camera.aspect,
                fallbackCenter);
        }

        private static Vector3 ClampCameraPositionToBounds(
            Vector3 position,
            Rect bounds,
            float halfHeight,
            float aspect,
            Vector2 fallbackCenter)
        {
            halfHeight = Mathf.Max(0f, halfHeight);
            var halfWidth = halfHeight * Mathf.Max(0.01f, aspect);
            position.x = ClampCameraAxis(position.x, bounds.xMin, bounds.xMax, halfWidth, fallbackCenter.x);
            position.y = ClampCameraAxis(position.y, bounds.yMin, bounds.yMax, halfHeight, fallbackCenter.y);
            return position;
        }

        private static float ClampCameraAxis(float value, float minimum, float maximum, float halfExtent, float fallback)
        {
            var minimumCenter = minimum + halfExtent;
            var maximumCenter = maximum - halfExtent;
            return minimumCenter <= maximumCenter
                ? Mathf.Clamp(value, minimumCenter, maximumCenter)
                : Mathf.Clamp(fallback, minimum, maximum);
        }

        private static Rect SelectCameraClampBounds(
            Rect cameraFrame,
            Rect panBounds,
            bool lockCameraAtZoomOutLimit,
            float currentCameraSize,
            float maximumCameraSize)
        {
            return lockCameraAtZoomOutLimit
                   && Mathf.Abs(currentCameraSize - maximumCameraSize) <= 0.001f
                ? cameraFrame
                : panBounds;
        }

        private static float CalculateCameraSizeToContainBounds(Rect bounds, float aspect)
        {
            if (bounds.width <= 0f || bounds.height <= 0f)
            {
                return 0f;
            }

            return Mathf.Max(bounds.height * 0.5f, bounds.width / (2f * Mathf.Max(0.01f, aspect)));
        }

        private static Rect ResolveCameraFrame(Rect configuredFrame, float aspect)
        {
            aspect = Mathf.Max(0.01f, aspect);
            var requiredWidth = configuredFrame.height * aspect;
            if (requiredWidth > configuredFrame.width)
            {
                var horizontalPadding = (requiredWidth - configuredFrame.width) * 0.5f;
                configuredFrame.xMin -= horizontalPadding;
                configuredFrame.xMax += horizontalPadding;
                return configuredFrame;
            }

            var requiredHeight = configuredFrame.width / aspect;
            if (requiredHeight > configuredFrame.height)
            {
                configuredFrame.yMax = configuredFrame.yMin + requiredHeight;
            }

            return configuredFrame;
        }

        private void RefreshCameraPanBounds()
        {
            _cameraPanBounds = ExpandCameraPanBounds(
                _cameraWorldBounds,
                GetCameraTopPanAllowance(),
                _cameraBottomPanAllowance);
        }

        private static Rect ExpandCameraPanBounds(Rect cameraFrame, float topAllowance, float bottomAllowance)
        {
            cameraFrame.yMin -= Mathf.Max(0f, bottomAllowance);
            cameraFrame.yMax += Mathf.Max(0f, topAllowance);
            return cameraFrame;
        }

        private float GetCameraTopPanAllowance()
        {
            return _cameraWorldBounds.height * _cameraTopPanAllowancePercent;
        }

        private void RefreshCameraFrameForViewport()
        {
            if (_camera == null || !_configuredCameraFrame.HasValue)
            {
                return;
            }

            var resolvedFrame = ResolveCameraFrame(_configuredCameraFrame.Value, _camera.aspect);
            if (Mathf.Approximately(resolvedFrame.xMin, _cameraWorldBounds.xMin)
                && Mathf.Approximately(resolvedFrame.yMin, _cameraWorldBounds.yMin)
                && Mathf.Approximately(resolvedFrame.xMax, _cameraWorldBounds.xMax)
                && Mathf.Approximately(resolvedFrame.yMax, _cameraWorldBounds.yMax))
            {
                return;
            }

            _cameraWorldBounds = resolvedFrame;
            RefreshCameraPanBounds();
            _maximumCameraSize = Mathf.Max(
                _minimumCameraSize,
                CalculateCameraSizeToContainBounds(_cameraWorldBounds, _camera.aspect));
            _camera.orthographicSize = Mathf.Min(_camera.orthographicSize, _maximumCameraSize);
            FitStaticBackgroundToCameraFrame(_staticBackgroundRenderer);
            _animatedBackground?.ConfigureCoverage(_cameraPanBounds, _backgroundWorldOffset, 0f);
            UpdateKillZoneVisual();
            ClampCameraToArena();
        }
    }
}
