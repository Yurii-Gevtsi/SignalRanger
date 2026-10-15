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
        private void BuildAimRenderers()
        {
            _aimLine = CreateLine("Aim Line", 0.08f, Color.white);
            _trajectoryLine = CreateLine("Trajectory Preview", 0.035f, new Color(1f, 1f, 1f, 0.55f));
            _ropeLine = CreateLine("Rope Tether", 0.028f, new Color(0.88f, 0.82f, 0.68f, 1f));
            _ropeLine.sortingOrder = 31;
            _electricHarpoonLine = CreateLine("Electric Harpoon Tether", 0.035f, new Color(0.16f, 0.85f, 1f, 1f));
            _electricHarpoonLine.sortingOrder = 32;
            _electricHarpoonExtraLine = CreateLine("Electric Harpoon Second Tether", 0.035f, new Color(0.16f, 0.85f, 1f, 1f));
            _electricHarpoonExtraLine.sortingOrder = 32;
            _electricHarpoonExtraLine.enabled = false;
            BuildTrajectoryDots();
        }

        private void BuildTrajectoryDots()
        {
            EnsureTrajectoryDotCapacity(MaxTrajectoryDotCount);
        }

        private void EnsureTrajectoryDotCapacity(int requiredCount)
        {
            requiredCount = Mathf.Clamp(requiredCount, 0, MaxFullTrajectoryDotCount);
            for (var i = _trajectoryDots.Count; i < requiredCount; i++)
            {
                var dotObject = new GameObject($"Trajectory Dot {i + 1}");
                var renderer = dotObject.AddComponent<SpriteRenderer>();
                renderer.sprite = _circleSprite;
                renderer.sortingOrder = 52;
                renderer.color = Color.clear;
                renderer.enabled = false;
                _trajectoryDots.Add(renderer);
            }
        }

        private void UpdateAim()
        {
            if (_firstBattleGuide != null && _firstBattleGuide.IsActive && !_firstBattleGuide.AllowsAimInput)
            {
                _aimDragActive = false;
                return;
            }

            if (!_playerWeaponSelected)
            {
                _aimDragActive = false;
                return;
            }

            if (_inventory?.Selected?.WeaponId == "weapon.titanium-barrier")
            {
                UpdateTitaniumBarrierPlacement();
                return;
            }

            if (BattleSimulation.IsMarkedStrikeWeapon(_inventory?.Selected))
            {
                UpdateOrbitalTargetInput();
                return;
            }

            if (!BattleSimulation.RequiresAim(_inventory?.Selected))
            {
                _aimDragActive = false;
                _aimDragHadValidPull = false;
                return;
            }

            if (TryGetPinchDistance(out _))
            {
                _aimDragActive = false;
                return;
            }

            var activePresenter = GetActivePresenter();
            if (activePresenter == null)
            {
                _aimDragActive = false;
                return;
            }

            if (!TryReadPointer(out var pressed, out var pressStarted, out var released, out var screenPosition, out var pointerOverUi))
            {
                _aimDragActive = false;
                return;
            }

            var origin = (Vector2)activePresenter.View.FirePoint.position;

            // Slingshot: the press anchors anywhere on screen (away from the unit
            // so the finger never covers the arc), and dragging pulls the shot
            // back. The launch aims opposite the pull. Releasing only commits the
            // aim — the shot itself is fired by the Fire button, never on release.
            if (pressStarted && !pointerOverUi)
            {
                _aimDragActive = true;
                _aimDragHadValidPull = false;
                _aimAnchorWorld = ScreenToWorld(screenPosition);
            }

            if (_aimDragActive && pressed)
            {
                var pull = ScreenToWorld(screenPosition) - _aimAnchorWorld;
                var previousAim = _aimState;

                if (_aimDragHadValidPull
                    && UsesSmoothedDirectAim(_inventory.Selected)
                    && pull.magnitude < DirectAimCenterDeadZone)
                {
                    _aimState = new AimState(origin, previousAim.Direction, previousAim.Power, true);
                }
                else if (_aimDragHadValidPull)
                {
                    // Aim is live in this drag: keep tracking the finger everywhere,
                    // including through the dead zone around the anchor — no freeze,
                    // no arc blink (power rests at MinPower on short pulls).
                    _aimState = AimCalculator.ContinueFromPull(origin, pull, _aimSettings, _aimState);
                }
                else
                {
                    var candidate = AimCalculator.CalculateFromPull(origin, pull, _aimSettings);
                    if (candidate.HasInput)
                    {
                        _aimDragHadValidPull = true;
                        _aimState = candidate;
                    }
                    else if (!_aimState.HasInput)
                    {
                        // No committed aim at all — track the unit so the first
                        // valid pull springs from the right origin.
                        _aimState = candidate;
                    }

                    // Otherwise hold the previously committed aim: a fresh tap must
                    // not yank the frozen arc until it pulls past the dead zone.
                }

                if (UsesSmoothedDirectAim(_inventory.Selected)
                    && previousAim.HasInput
                    && _aimState.HasInput)
                {
                    var smoothing = 1f - Mathf.Exp(-DirectAimSmoothingSpeed * Time.deltaTime);
                    var direction = Vector2.Lerp(previousAim.Direction, _aimState.Direction, smoothing).normalized;
                    _aimState = new AimState(origin, direction, _aimState.Power, true);
                }

                _aimState = ClampRopeAimToUpwardSector(_aimState);

                FaceUnitTowardAim(activePresenter);

                if (_aimState.HasInput && !_aimHadInput)
                {
                    ReportTutorial(BattleTutorialBridge.AimStarted);
                    EmitFeedback(BattleFeedbackEventType.AimStarted);
                }

                _aimHadInput = _aimState.HasInput;
                return;
            }

            if (_aimDragActive && released)
            {
                // Freeze the pulled aim so the arc stays on screen for review;
                // the player fires it with the Fire button.
                _aimDragActive = false;
                _firstBattleGuide?.ObserveAimState(_aimState.HasInput);
            }
        }

        private void UpdateOrbitalTargetInput()
        {
            if (!TryReadPointer(out _, out var pressStarted, out _, out var screenPosition, out var pointerOverUi)
                || !pressStarted || pointerOverUi)
            {
                return;
            }

            var isSecondTap = Time.unscaledTime - _orbitalLastTapTime <= OrbitalTargetDoubleTapSeconds
                && (screenPosition - _orbitalLastTapScreenPosition).sqrMagnitude
                    <= OrbitalTargetDoubleTapDistancePixels * OrbitalTargetDoubleTapDistancePixels;
            _orbitalLastTapTime = Time.unscaledTime;
            _orbitalLastTapScreenPosition = screenPosition;
            if (!isSecondTap || _simulation == null)
            {
                return;
            }

            var target = ScreenToWorld(screenPosition);
            if (_simulation.TrySetOrbitalTarget(target))
            {
                ShowOrbitalTargetMarker(target);
            }
        }

        private void ShowOrbitalTargetMarker(Vector2 point)
        {
            ClearOrbitalTargetMarker(clearSimulation: false);
            if (_circleSprite == null)
            {
                return;
            }

            _orbitalTargetMarkerVisual = new GameObject("Orbital Strike Marker");
            _arenaObjects.Add(_orbitalTargetMarkerVisual);
            _orbitalTargetMarkerVisual.transform.position = point;
            // The same red "×" glyph the modal close buttons use, not a plus sign.
            foreach (var rotation in new[] { 45f, -45f })
            {
                var arm = new GameObject("Cross Arm");
                arm.transform.SetParent(_orbitalTargetMarkerVisual.transform, false);
                arm.transform.localScale = new Vector3(0.34f, 0.055f, 1f);
                arm.transform.localRotation = Quaternion.Euler(0f, 0f, rotation);
                var renderer = arm.AddComponent<SpriteRenderer>();
                renderer.sprite = _circleSprite;
                renderer.color = OrbitalMarkerColor;
                renderer.sortingOrder = 34;
            }
        }

        private static bool ShouldClearOrbitalTargetMarkerOnTurnAdvanced(TurnAdvanceReason reason)
        {
            return reason == TurnAdvanceReason.PlayerTimerExpired;
        }

        private void ClearOrbitalTargetMarker(bool clearSimulation = true)
        {
            _orbitalLastTapTime = -10f;
            if (clearSimulation)
            {
                _simulation?.ClearOrbitalTarget();
            }
            if (_orbitalTargetMarkerVisual == null)
            {
                return;
            }

            _arenaObjects.Remove(_orbitalTargetMarkerVisual);
            Destroy(_orbitalTargetMarkerVisual);
            _orbitalTargetMarkerVisual = null;
        }

        private static bool UsesSmoothedDirectAim(WeaponDefinition weapon)
        {
            return weapon?.Role == WeaponRole.DirectProjectile
                && weapon.WeaponId != "weapon.fpv-drone";
        }

        private AimState ClampRopeAimToUpwardSector(in AimState aim)
        {
            if (!aim.HasInput || _inventory?.Selected?.WeaponId != "weapon.rope")
            {
                return aim;
            }

            var angle = Mathf.Atan2(aim.Direction.y, aim.Direction.x);
            var aimPurchases = _playerProgression?.ResolveUpgradeCount(
                "weapon.rope", WeaponUpgradePath.Aim) ?? 0;
            var halfSector = WeaponUpgradeChoiceResolver.ResolveRopeAimSectorDegrees(
                aimPurchases) * 0.5f * Mathf.Deg2Rad;
            var upward = Mathf.PI * 0.5f;
            var constrained = Mathf.Clamp(angle, upward - halfSector, upward + halfSector);
            var direction = new Vector2(Mathf.Cos(constrained), Mathf.Sin(constrained));
            return new AimState(aim.Origin, direction, aim.Power, true);
        }

        private void FaceUnitTowardAim(BattleUnitPresenter presenter)
        {
            if (presenter == null || !_aimState.HasInput)
            {
                return;
            }

            if (Mathf.Abs(_aimState.Direction.x) >= 0.01f)
            {
                presenter.SetFacing(_aimState.Direction.x > 0f ? 1 : -1);
            }

            presenter.SetWeaponAim(_aimState.Direction);
            var origin = presenter.View.WeaponMuzzlePosition - _aimState.Direction * ProjectileMuzzleOffset;
            _aimState = new AimState(
                origin,
                _aimState.Direction,
                _aimState.Power,
                true);
        }

        private void ApplyAimSettingsForActiveTeam()
        {
            var activeInventory = _simulation.ActiveInventory;
            var weapon = activeInventory?.Selected ?? _inventory.Selected;
            _aimSettings = weapon.CreateAimSettings(_baseAimSettings);
        }

        private void DrawAim()
        {
            _aimLine.enabled = false;
        }

        private void DrawTrajectoryPreview()
        {
            if (!_aimState.HasInput)
            {
                HideTrajectoryPreview();
                return;
            }

            var activePresenter = GetActivePresenter();
            if (activePresenter == null)
            {
                HideTrajectoryPreview();
                return;
            }

            if (_inventory.Selected.Role == WeaponRole.DefensiveTool)
            {
                HideTrajectoryPreview();
                return;
            }

            if (_inventory.Selected.Role == WeaponRole.ShortRangeBurst
                && _inventory.Selected.WeaponId != "weapon.bur"
                && _inventory.Selected.WeaponId != "weapon.fire-gun")
            {
                HideTrajectoryPreview();
                return;
            }

            if (_inventory.Selected.Role == WeaponRole.CoverTool)
            {
                DrawCoverPlacementPreview(_aimState.Origin);
                return;
            }

            if (IsInstantUtility(_inventory.Selected.Role))
            {
                HideTrajectoryPreview();
                return;
            }

            DrawBallisticTrajectory(_aimState.Origin, _inventory.Selected);
        }

        private static bool IsInstantUtility(WeaponRole role)
        {
            return role == WeaponRole.CoverTool
                || role == WeaponRole.DefensiveTool;
        }

        private void DrawBallisticTrajectory(Vector2 firePoint, WeaponDefinition weapon)
        {
            _trajectoryLine.enabled = false;

            if (weapon?.WeaponId == "weapon.rail-rifle")
            {
                DrawRailRifleLaser(firePoint, weapon);
                return;
            }

            var power01 = Mathf.Approximately(_aimSettings.MinPower, _aimSettings.MaxPower)
                ? 1f
                : Mathf.InverseLerp(_aimSettings.MinPower, _aimSettings.MaxPower, _aimState.Power);
            var previewLength = ResolveTrajectoryPreviewLength(weapon,
                Mathf.Lerp(MinTrajectoryPreviewLength, MaxTrajectoryPreviewLength, Mathf.Sqrt(power01)));
            var previewEndTime = FindTrajectoryPreviewEndTime(firePoint, weapon, previewLength);
            var upwardAim01 = Mathf.InverseLerp(0.12f, 0.86f, _aimState.Direction.y);
            var weakShot01 = 1f - power01;
            var minimumArcTime = Mathf.Lerp(0.34f, 0.78f, upwardAim01 * weakShot01);
            previewEndTime = Mathf.Max(previewEndTime, minimumArcTime);

            // More dots at higher power. Because the length grows faster than the
            // count, the gap between dots widens with power as well.
            var isLongDirectWeapon = IsLongDirectWeapon(weapon);
            var activeDotCount = isLongDirectWeapon
                ? 10
                : Mathf.Clamp(
                    Mathf.RoundToInt(Mathf.Lerp(MinTrajectoryDotCount, MaxTrajectoryDotCount, power01)),
                    MinTrajectoryDotCount,
                    _trajectoryDots.Count);

            // Training mission: extend the arc all the way to the predicted
            // landing point so a new player sees exactly where the shell falls.
            Vector2? trajectoryEndPoint = null;
            if (!isLongDirectWeapon
                && _impactGuideEnabled
                && TryProbeTrajectoryEnd(firePoint, weapon, out var impactReport))
            {
                var impactArcLength = CalculateTrajectoryArcLength(firePoint, weapon, impactReport.FlightTime);
                activeDotCount = CalculateFullTrajectoryDotCount(impactArcLength);
                EnsureTrajectoryDotCapacity(activeDotCount);
                trajectoryEndPoint = impactReport.EndPoint;
                previewEndTime = Mathf.Max(previewEndTime, impactReport.FlightTime);
            }

            for (var i = 0; i < _trajectoryDots.Count; i++)
            {
                var dot = _trajectoryDots[i];
                if (i >= activeDotCount)
                {
                    dot.enabled = false;
                    continue;
                }

                var t01 = activeDotCount <= 1 ? 0f : (float)i / (activeDotCount - 1);
                var time01 = Mathf.Pow(t01, 0.86f);
                var point = GetTrajectoryPreviewPointAtTime(firePoint, weapon, previewEndTime * time01);
                if (trajectoryEndPoint.HasValue && i == activeDotCount - 1)
                {
                    // Pin the last dot to the probed impact, not the analytic
                    // curve, so the arc ends exactly where the shot will land.
                    point = trajectoryEndPoint.Value;
                }

                var scale = Mathf.Lerp(0.24f, 0.115f, t01);

                dot.enabled = true;
                dot.transform.position = new Vector3(point.x, point.y, -0.18f);
                dot.transform.localScale = new Vector3(scale, scale, 1f);
                dot.color = Color.white;
            }
        }

        private float ResolveTrajectoryPreviewLength(WeaponDefinition weapon, float baseLength)
        {
            if (weapon?.WeaponId != "weapon.granata"
                && weapon?.WeaponId != "weapon.impulse"
                && weapon?.WeaponId != "weapon.titanium-disc")
            {
                return baseLength;
            }

            var aimUpgrades = _playerProgression?.ResolveUpgradeCount(
                weapon.WeaponId, WeaponUpgradePath.Aim) ?? 0;
            return baseLength * WeaponUpgradeChoiceResolver.ResolveAimPreviewMultiplier(aimUpgrades);
        }

        private static bool IsLongDirectWeapon(WeaponDefinition weapon)
        {
            return weapon?.WeaponId == "weapon.blaster-caset"
                || weapon?.WeaponId == "weapon.emp";
        }

        private Vector2 TraceRailLaserEnd(Vector2 start, Vector2 direction)
        {
            var grid = _battleTerrain?.Grid;
            if (grid == null || direction.sqrMagnitude < 0.0001f)
            {
                return start + direction * MaxTrajectoryPreviewLength;
            }

            direction = direction.normalized;
            var step = Mathf.Max(0.02f, Mathf.Min(grid.CellSize.x, grid.CellSize.y));
            var point = start;
            for (var i = 0; i < 20000; i++)
            {
                var next = point + direction * step;
                if (next.x < grid.WorldMinX || next.x > grid.WorldMaxX || next.y < grid.WorldMinY || next.y > grid.WorldMaxY
                    || grid.IsSolidWorld(next))
                {
                    return point;
                }

                point = next;
            }

            return point;
        }

        private void DrawRailRifleLaser(Vector2 firePoint, WeaponDefinition weapon)
        {
            HideTrajectoryDots();
            var muzzle = firePoint + _aimState.Direction * ProjectileMuzzleOffset;
            // A sniper laser: the line runs straight on until it meets ground or leaves the arena.
            var endPoint = TraceRailLaserEnd(muzzle, _aimState.Direction);

            var laserColor = new Color(1f, 0.12f, 0.12f, 0.95f);
            _trajectoryLine.startColor = laserColor;
            _trajectoryLine.endColor = laserColor;
            _trajectoryLine.startWidth = 0.024f;
            _trajectoryLine.endWidth = 0.024f;
            _trajectoryLine.positionCount = 2;
            _trajectoryLine.SetPosition(0, muzzle);
            _trajectoryLine.SetPosition(1, endPoint);
            _trajectoryLine.enabled = true;
        }

        /// <summary>
        /// Probes the exact shot through the same physics the simulation runs.
        /// True when the shell hits terrain/a unit or falls out of the simulated
        /// world. Timeouts have no physical landing point to show.
        /// </summary>
        private bool TryProbeTrajectoryEnd(Vector2 firePoint, WeaponDefinition weapon, out TrajectoryReport report)
        {
            report = default;
            if (_impactGuideProbe == null)
            {
                return false;
            }

            var muzzle = firePoint + _aimState.Direction * ProjectileMuzzleOffset;
            BuildImpactGuideBodies();
            report = _impactGuideProbe.Simulate(
                muzzle,
                _aimState.Direction * _aimState.Power,
                weapon.GravityScale,
                weapon.ProjectileRadius,
                weapon.TimeoutSeconds,
                new TrajectoryObstacles(_impactGuideBodies, _aimState.Target),
                weapon);

            return report.EndKind != TrajectoryEndKind.Timeout;
        }

        private static int CalculateFullTrajectoryDotCount(float arcLength)
        {
            if (!float.IsFinite(arcLength) || arcLength <= 0f)
            {
                return MinTrajectoryDotCount;
            }

            return Mathf.Clamp(
                Mathf.CeilToInt(arcLength / MaxTrajectoryDotSpacing) + 1,
                MinTrajectoryDotCount,
                MaxFullTrajectoryDotCount);
        }

        private void BuildImpactGuideBodies()
        {
            _impactGuideBodies.Clear();
            var activeUnitId = _battleState.Turn.ActiveUnitId;
            foreach (var team in _battleState.Teams)
            {
                var archetype = team.TeamId == BattleTeamId.Player ? _playerArchetype : _opponentArchetype;
                var halfHeight = archetype.BodyHeight * 0.5f;
                foreach (var unit in team.Units)
                {
                    if (unit == null || unit.IsDefeated || unit.UnitId == activeUnitId)
                    {
                        continue;
                    }

                    _impactGuideBodies.Add(new TrajectoryUnitBody(
                        unit.Position,
                        UnitCollisionHalfWidth,
                        halfHeight,
                        isAlly: team.TeamId == BattleTeamId.Player,
                        isTarget: false));
                }
            }
        }

        private float FindTrajectoryPreviewEndTime(Vector2 firePoint, WeaponDefinition weapon, float targetLength)
        {
            const float sampleStepTime = 0.018f;
            const int maxSamples = 120;

            var distance = 0f;
            var previous = GetTrajectoryPreviewPointAtTime(firePoint, weapon, 0f);
            for (var i = 1; i <= maxSamples; i++)
            {
                var t = i * sampleStepTime;
                var current = GetTrajectoryPreviewPointAtTime(firePoint, weapon, t);
                distance += Vector2.Distance(previous, current);
                if (distance >= targetLength)
                {
                    return t;
                }

                previous = current;
            }

            return maxSamples * sampleStepTime;
        }

        private Vector2 GetTrajectoryPreviewPointAtTime(Vector2 firePoint, WeaponDefinition weapon, float time)
        {
            var start = firePoint + (_aimState.Direction * ProjectileMuzzleOffset);
            var velocity = _aimState.Direction * _aimState.Power;
            var gravity = Physics2D.gravity * weapon.GravityScale;
            return start + velocity * time + 0.5f * gravity * (time * time);
        }

        private float CalculateTrajectoryArcLength(Vector2 firePoint, WeaponDefinition weapon, float endTime)
        {
            if (!float.IsFinite(endTime) || endTime <= 0f)
            {
                return 0f;
            }

            const int sampleCount = 48;
            var length = 0f;
            var previous = GetTrajectoryPreviewPointAtTime(firePoint, weapon, 0f);
            for (var i = 1; i <= sampleCount; i++)
            {
                var current = GetTrajectoryPreviewPointAtTime(firePoint, weapon, endTime * i / sampleCount);
                length += Vector2.Distance(previous, current);
                previous = current;
            }

            return length;
        }

        private void DrawCoverPlacementPreview(Vector2 firePoint)
        {
            HideTrajectoryDots();
            if (_inventory.Selected?.WeaponId == "weapon.titanium-barrier")
            {
                _trajectoryLine.enabled = false;
                DrawTitaniumBarrierPlacementPreview(GetCoverPlacementPoint(firePoint));
                return;
            }

            _trajectoryLine.enabled = true;
            var placementColor = new Color(0.35f, 0.85f, 1f, 0.65f);
            _trajectoryLine.startColor = placementColor;
            _trajectoryLine.endColor = placementColor;
            _trajectoryLine.startWidth = 0.035f;
            _trajectoryLine.endWidth = 0.035f;
            _trajectoryLine.positionCount = 2;
            _trajectoryLine.SetPosition(0, firePoint);
            _trajectoryLine.SetPosition(1, GetCoverPlacementPoint(firePoint));
        }

        private bool IsTitaniumBarrierPlacementActive()
        {
            return _playerWeaponSelected
                && _inventory?.Selected?.WeaponId == "weapon.titanium-barrier"
                && _simulation != null
                && _battleState.Turn.ActiveTeam == BattleTeamId.Player
                && !_simulation.IsAttackCommitted;
        }

        private void UpdateTitaniumBarrierPlacement()
        {
            if (!IsTitaniumBarrierPlacementActive())
            {
                HideTrajectoryPreview();
                return;
            }
            var presenter = GetActivePresenter();
            if (presenter == null)
            {
                return;
            }

            if (!_barrierPlacementInitialized)
            {
                var facing = presenter.State.FacingDirectionX >= 0 ? 1f : -1f;
                var point = (Vector2)presenter.View.FirePoint.position + new Vector2(facing * 0.58f, 1.02f);
                for (var attempt = 0; attempt < 5 && _battleTerrain?.Grid.IsSolidWorld(point) == true; attempt++)
                {
                    point += Vector2.up * 0.35f;
                }

                SetTitaniumBarrierPlacement(point, Vector2.right);
                _barrierPlacementInitialized = true;
            }

            var pointNow = GetCoverPlacementPoint(_aimState.Origin);
            var move = new Vector2(_controlledUiHorizontalInput, -_ropeVerticalInput);
            if (move.sqrMagnitude > 0.01f)
            {
                // 50% faster than the former 1.65 (owner decision 2026-10-09). The preview does not slide into ground
                // or into a beam already placed, unless it is already stuck in one and moving out.
                var candidate = pointNow + move.normalized * (2.5f * Time.deltaTime);
                if (IsTitaniumBarrierPreviewClear(candidate, _aimState.Direction)
                    || !IsTitaniumBarrierPreviewClear(pointNow, _aimState.Direction))
                {
                    SetTitaniumBarrierPlacement(candidate, _aimState.Direction);
                    pointNow = candidate;
                }
            }

            // Two touches own resizing; neither finger may rotate the same preview.
            SetTitaniumBarrierPlacement(pointNow, _aimState.Direction);
            if (TryGetPinchDistance(out _)) return;
            if (!TryReadPointer(out var pressed, out _, out _, out var screenPosition, out var pointerOverUi)
                || !pressed || pointerOverUi)
            {
                return;
            }

            var direction = ScreenToWorld(screenPosition) - pointNow;
            if (direction.sqrMagnitude > 0.04f)
            {
                SetTitaniumBarrierPlacement(pointNow, direction.normalized);
            }
        }

        /// <summary>The beam's shape at this spot overlaps neither ground nor a beam placed before (both are solid cells).</summary>
        private bool IsTitaniumBarrierPreviewClear(Vector2 center, Vector2 direction)
        {
            var grid = _battleTerrain?.Grid;
            if (grid == null || _simulation == null)
            {
                return true;
            }

            var axisX = direction.sqrMagnitude > 0.001f ? direction.normalized : Vector2.right;
            var axisY = new Vector2(-axisX.y, axisX.x);
            var size = _simulation.TitaniumBarrierBaseSize * _barrierPlacementScale * 0.8f;
            for (var i = 0; i <= 8; i++)
            {
                for (var j = 0; j <= 2; j++)
                {
                    var point = center + axisX * ((i / 8f - 0.5f) * size.x) + axisY * ((j / 2f - 0.5f) * size.y);
                    if (grid.IsSolidWorld(point))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void SetTitaniumBarrierPlacement(Vector2 point, Vector2 direction)
        {
            var normalizedDirection = direction.sqrMagnitude > 0.001f ? direction.normalized : Vector2.right;
            _aimState = new AimState(
                point,
                normalizedDirection,
                _barrierPlacementScale,
                true);
            DrawTitaniumBarrierPlacementPreview(point);
        }

        private void HideTrajectoryPreview()
        {
            _trajectoryLine.enabled = false;
            HideTrajectoryDots();
            if (_barrierPlacementPreview != null)
            {
                _barrierPlacementPreview.SetActive(false);
            }
        }

        private void DrawTitaniumBarrierPlacementPreview(Vector2 point)
        {
            if (_barrierPlacementPreview == null)
            {
                _barrierPlacementPreview = new GameObject("Titanium Beam Placement Preview");
                _arenaObjects.Add(_barrierPlacementPreview);
                var renderer = _barrierPlacementPreview.AddComponent<SpriteRenderer>();
                renderer.sprite = LoadWeaponIconSprite("titanium_barrier");
                renderer.color = Color.white;
                renderer.sortingOrder = 29;
            }

            _barrierPlacementPreview.SetActive(true);
            _barrierPlacementPreview.transform.position = point;
            // Red where the beam cannot stand (in ground or over another beam).
            _barrierPlacementPreview.GetComponent<SpriteRenderer>().color =
                IsTitaniumBarrierPreviewClear(point, _aimState.Direction) ? Color.white : new Color(1f, 0.35f, 0.35f, 0.85f);
            var previewSprite = _barrierPlacementPreview.GetComponent<SpriteRenderer>().sprite;
            var size = _simulation.TitaniumBarrierBaseSize * _barrierPlacementScale;
            _barrierPlacementPreview.transform.localScale = new Vector3(size.x / previewSprite.bounds.size.x,
                size.y / previewSprite.bounds.size.y, 1f);
            _barrierPlacementPreview.transform.rotation = Quaternion.Euler(
                0f,
                0f,
                Mathf.Atan2(_aimState.Direction.y, _aimState.Direction.x) * Mathf.Rad2Deg);
        }

        private void HideTrajectoryDots()
        {
            for (var i = 0; i < _trajectoryDots.Count; i++)
            {
                _trajectoryDots[i].enabled = false;
            }
        }

        private Vector2 GetCoverPlacementPoint(Vector2 firePoint)
        {
            if (_inventory?.Selected?.WeaponId == TitaniumBarrierRules.WeaponId) return _aimState.Origin;
            var power01 = Mathf.InverseLerp(_aimSettings.MinPower, _aimSettings.MaxPower, _aimState.Power);
            var placementDistance = Mathf.Lerp(1.25f, 3.1f, power01);
            return firePoint + _aimState.Direction.normalized * placementDistance;
        }

        private LineRenderer CreateLine(string objectName, float width, Color color)
        {
            var lineObject = new GameObject(objectName);
            var line = lineObject.AddComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.startColor = color;
            line.endColor = color;
            line.startWidth = width;
            line.endWidth = width;
            line.positionCount = 2;
            line.useWorldSpace = true;
            line.sortingOrder = 50;
            line.enabled = false;
            return line;
        }

        private void CancelAim()
        {
            if (!IsBattleReady() || _simulation.Result.IsFinished || HasProjectilesInFlight || HasActiveMotion || HasActiveWeaponEffect)
            {
                return;
            }

            ResetAimState();
            _status = GameText.AimReset();
        }

        private void ResetAimState()
        {
            _aimState = new AimState(Vector2.zero, Vector2.right, _aimSettings.MinPower, false);
            _aimDragActive = false;
            _aimDragHadValidPull = false;
            _aimHadInput = false;
            if (_aimLine != null)
            {
                _aimLine.enabled = false;
            }

            HideTrajectoryPreview();
            RefreshPlayerWeaponVisual();
        }
    }
}
