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
        /// <summary>
        /// Whether a player's arsenal item can be selected and used in the current
        /// turn. The HUD uses the same rule as TryFire so unavailable weapons stay
        /// visible without looking selectable.
        /// </summary>
        public bool CanUsePlayerWeapon(WeaponDefinition weapon)
        {
            if (weapon == null
                || !IsBattleMechanicAvailable(weapon)
                || weapon.HasLimitedAmmo && _playerInventory.GetRemainingAmmo(weapon) <= 0)
            {
                return false;
            }

            // The EMP aim jam leaves only weapons that need no aiming (mine, sword, devices).
            if (_state.Turn.ActiveTeam == BattleTeamId.Player && !CanActiveUnitUseWeapon(weapon))
            {
                return false;
            }

            // The turn transfer needs a living ally to receive the turn.
            if (weapon.WeaponId == TurnTransferWeaponId && !HasLivingAllyOfActiveUnit())
            {
                return false;
            }

            // A multi-shot cassette blaster owns the rest of the turn once its
            // first round has been fired. Other weapons remain selectable after
            // any number of mine placements.
            return !CanContinueBlasterBurst || weapon.WeaponId == BlasterWeaponId;
        }

        public static bool IsBattleMechanicAvailable(WeaponDefinition weapon)
        {
            if (weapon == null)
            {
                return false;
            }

            return weapon.WeaponId switch
            {
                _ => true
            };
        }

        public static bool RequiresAim(WeaponDefinition weapon)
        {
            if (weapon == null || weapon.Role == WeaponRole.DefensiveTool)
            {
                return false;
            }

            return weapon.WeaponId != PlasmaSwordWeaponId
                && weapon.WeaponId != MineWeaponId
                && weapon.WeaponId != SpiderMineWeaponId
                && weapon.WeaponId != C4WeaponId
                && weapon.WeaponId != RepairJammerWeaponId
                && !IsMarkedStrikeWeapon(weapon)
                && weapon.WeaponId != BoostWeaponId
                && weapon.WeaponId != BomberWeaponId
                && weapon.WeaponId != SelfDestructWeaponId
                && weapon.WeaponId != CarrierWeaponId
                && weapon.WeaponId != VirusWeaponId
                && weapon.WeaponId != SkyHookWeaponId
                && weapon.WeaponId != FpvWeaponId;
        }

        /// <summary>The orbital rocket and the nuclear charge share one scenario: mark, confirm, strike after the enemy turn.</summary>
        public static bool IsMarkedStrikeWeapon(WeaponDefinition weapon)
        {
            return weapon?.WeaponId == OrbitalRocketWeaponId || weapon?.WeaponId == NuclearChargeWeaponId;
        }

        /// <summary>
        /// Arms the player-selected orbital rocket at an arena point. Marking is
        /// intentionally free: ammo and the action are committed only by the
        /// subsequent fire-button confirmation.
        /// </summary>
        public bool TrySetOrbitalTarget(Vector2 target)
        {
            if (Result.IsFinished || HasProjectilesInFlight || HasActiveMotion || HasActiveWeaponEffect
                || HasPendingGroundAction || _state.Turn.ActionSpent
                || _state.Turn.ActiveTeam != BattleTeamId.Player
                || !IsMarkedStrikeWeapon(_playerInventory.Selected)
                || !CanUsePlayerWeapon(_playerInventory.Selected))
            {
                return false;
            }

            _orbitalTargetMarker = target;
            return true;
        }

        /// <summary>Discards an unconfirmed orbital marker when the player changes intent.</summary>
        public void ClearOrbitalTarget()
        {
            _orbitalTargetMarker = null;
        }

        /// <summary>
        /// Whether player movement input may stay active. Unlike
        /// <see cref="CanUseMovement"/>, this remains true while the unit is in
        /// player-controlled motion so a held movement button is not released mid-jump or
        /// while dropping from a ledge.
        /// </summary>
        /// <summary>
        /// False while the active unit is EMP-stunned (cannot fire this turn). The bot
        /// consults this before planning a shot so it falls back to
        /// <see cref="AdvanceTurnDueToNoUsableWeapon"/> instead of repeatedly attempting
        /// a <see cref="TryFire"/> that would just be blocked.
        /// </summary>
        public bool CanActiveUnitFire()
        {
            return !IsActiveUnitEmpStunned();
        }

        public BattleFireResult TryFire(AimState aim)
        {
            if (_copter != null && _copter.OwnerUnitId == _state.Turn.ActiveUnitId)
            {
                return _copter.IsCarrier ? PressCarrier() : DropBomberBomb();
            }

            if (_turnTransfer != null && _turnTransfer.OwnerUnitId == _state.Turn.ActiveUnitId)
            {
                return ConfirmTurnTransfer();
            }

            if (_boost != null && _boost.OwnerUnitId == _state.Turn.ActiveUnitId)
            {
                // A second press cuts the fuel; the droid falls unless it is supported.
                var boostWeapon = _boost.Weapon;
                EndBoost();
                return new BattleFireResult(BattleFireOutcome.Fired, _state.Turn.ActiveTeam, boostWeapon);
            }

            if (_ropeTraversal != null && _ropeTraversal.IsAttached
                && _ropeTraversal.OwnerUnitId == _state.Turn.ActiveUnitId)
            {
                _ropeTraversal.Detach(GetMotor(_ropeTraversal.OwnerUnitId));
                _ropeTraversal = null;
                UnitsMoved?.Invoke();
                return new BattleFireResult(BattleFireOutcome.Fired, _state.Turn.ActiveTeam, _weaponsById[RopeWeaponId]);
            }

            if (TryTriggerTeleportSphereEarly(out var earlyTeleportResult))
            {
                return earlyTeleportResult;
            }

            var canQueuePlacementWhileProjectileIsInFlight = _state.Turn.ActiveTeam == BattleTeamId.Player
                && ((_playerInventory.Selected?.WeaponId == MineWeaponId
                        && _minesDeployedThisTurn > 0
                        && CanContinueMineDeploymentThisTurn())
                    || (_playerInventory.Selected?.WeaponId == LaserFenceWeaponId
                        && _laserFenceNodesDeployedThisTurn > 0
                        && CanContinueLaserFenceDeploymentThisTurn())
                    || CanContinueHoppingGrenade());
            if (Result.IsFinished || HasActiveMotion || HasActiveWeaponEffect
                || HasProjectilesInFlight && !canQueuePlacementWhileProjectileIsInFlight
                || HasPendingGroundAction || _state.Turn.ActionSpent)
            {
                return new BattleFireResult(BattleFireOutcome.Blocked, BattleTeamId.None, null);
            }

            var activeUnitId = _state.Turn.ActiveUnitId;
            if (string.IsNullOrEmpty(activeUnitId))
            {
                return new BattleFireResult(BattleFireOutcome.NoActiveUnit, BattleTeamId.None, null);
            }

            var unit = _state.GetUnit(activeUnitId);

            if (IsActiveUnitEmpStunned())
            {
                return new BattleFireResult(BattleFireOutcome.Blocked, unit.TeamId, null);
            }

            if (unit.TeamId == BattleTeamId.Player && !_playerMovement.HasTimeRemaining)
            {
                EndPlayerTurnFromTimer();
                return new BattleFireResult(BattleFireOutcome.TurnTimerExpired, unit.TeamId, null);
            }

            var activeInventory = GetInventoryFor(unit.TeamId);
            var weapon = activeInventory.Selected;
            if (weapon == null)
            {
                return new BattleFireResult(BattleFireOutcome.NoWeaponSelected, unit.TeamId, null);
            }

            if (!IsBattleMechanicAvailable(weapon))
            {
                return new BattleFireResult(BattleFireOutcome.Blocked, unit.TeamId, weapon);
            }

            if (!CanActiveUnitUseWeapon(weapon))
            {
                return new BattleFireResult(BattleFireOutcome.Blocked, unit.TeamId, weapon);
            }

            if (IsMarkedStrikeWeapon(weapon))
            {
                return ConfirmOrbitalStrike(unit, activeInventory, weapon);
            }

            if (!aim.HasInput && RequiresAim(weapon))
            {
                return new BattleFireResult(BattleFireOutcome.AimRequired, unit.TeamId, weapon);
            }

            var effectiveAim = weapon.WeaponId == SkyHookWeaponId
                ? CreateSkyHookAim(unit)
                : aim.HasInput ? aim : CreateImmediateActionAim(unit);

            if (weapon.WeaponId == HoppingGrenadeWeaponId
                && TryHopRestingGrenade(unit, weapon, effectiveAim, out var hopResult))
            {
                return hopResult;
            }

            if (weapon.WeaponId == BoostWeaponId)
            {
                return StartBoost(unit, activeInventory, weapon);
            }

            if (weapon.WeaponId == TurnTransferWeaponId)
            {
                return BeginTurnTransfer(unit, activeInventory, weapon);
            }

            if (weapon.WeaponId == RopeWeaponId)
            {
                var rank = ResolveRankFor(unit.TeamId, weapon);
                var radiusPurchases = ProgressionFor(unit.TeamId).ResolveUpgradeCount(
                    weapon.WeaponId, WeaponUpgradePath.Radius);
                var aimPurchases = ProgressionFor(unit.TeamId).ResolveUpgradeCount(
                    weapon.WeaponId, WeaponUpgradePath.Aim);
                _ropeTraversal = new RopeTraversal(
                    unit.UnitId,
                    effectiveAim.Origin,
                    effectiveAim.Direction,
                    WeaponUpgradeChoiceResolver.ResolveRopeLength(radiusPurchases),
                    RopeHookSpeed,
                    WeaponUpgradeChoiceResolver.ResolveRopeAimSectorDegrees(aimPurchases),
                    RopeBaseSwingAcceleration + (rank - 1) * 0.65f);
                ShotFired?.Invoke(unit.TeamId, weapon, false);
                UnitsMoved?.Invoke();
                return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon);
            }

            if (weapon.WeaponId == ElectricHarpoonWeaponId)
            {
                return StartElectricHarpoon(unit, activeInventory, weapon, effectiveAim);
            }

            if (weapon.WeaponId == BlasterWeaponId)
            {
                return UseBlasterShot(unit, activeInventory, weapon, effectiveAim);
            }

            if (weapon.WeaponId == MineWeaponId)
            {
                return DeployMine(unit, activeInventory, weapon);
            }

            if (weapon.WeaponId == GuardianDroneWeaponId)
            {
                return DeployGuardianDrone(unit, activeInventory, weapon);
            }

            if (weapon.WeaponId == GrounderWeaponId)
            {
                return DeployGrounder(unit, activeInventory, weapon);
            }

            if (weapon.WeaponId == ForceFieldWeaponId)
            {
                return DeployForceField(unit, activeInventory, weapon);
            }

            if (weapon.WeaponId == SpiderMineWeaponId)
            {
                return DeploySpiderMine(unit, activeInventory, weapon);
            }

            if (weapon.WeaponId == LaserFenceWeaponId)
            {
                return DeployLaserFenceNode(unit, activeInventory, weapon, effectiveAim);
            }

            if (weapon.WeaponId == C4WeaponId)
            {
                return DeployC4(unit, activeInventory, weapon);
            }

            if (weapon.WeaponId == BomberWeaponId || weapon.WeaponId == CarrierWeaponId)
            {
                return LaunchBomber(unit, activeInventory, weapon);
            }

            if (weapon.WeaponId == SelfDestructWeaponId)
            {
                return ArmSelfDestruct(unit, activeInventory, weapon);
            }

            if (IsControlledDroneWeapon(weapon))
            {
                return LaunchControlledDrone(unit, activeInventory, weapon);
            }

            if (weapon.WeaponId == BurWeaponId)
            {
                return StartDrillLaser(unit, activeInventory, weapon, effectiveAim);
            }

            if (weapon.WeaponId == FireGunWeaponId)
            {
                return StartFireStream(unit, activeInventory, weapon, effectiveAim);
            }

            if (IsAttachmentDevice(weapon))
            {
                return LaunchAttachmentDevice(unit, activeInventory, weapon);
            }

            if (weapon.Role == WeaponRole.ShortRangeBurst)
            {
                return UseCloseRangeBurst(unit, activeInventory, weapon, effectiveAim);
            }

            if (IsInstantUtility(weapon.Role))
            {
                if (weapon.WeaponId == TitaniumBarrierRules.WeaponId)
                    return PlaceTitaniumBarrier(unit, activeInventory, weapon, effectiveAim);
                return UseInstantUtility(unit, activeInventory, weapon, effectiveAim);
            }

            var projectilesBeforeFire = _projectiles.Count;
            // The pulled power stays inside the weapon's own range, whatever the input.
            var launchSpeed = Mathf.Approximately(weapon.MinPower, weapon.MaxPower)
                ? weapon.MinPower
                : Mathf.Clamp(effectiveAim.Power, weapon.MinPower, weapon.MaxPower);
            var context = new WeaponFireContext(
                weapon,
                unit.UnitId,
                effectiveAim.Origin,
                effectiveAim.Direction,
                launchSpeed,
                this,
                ResolveEvolutionFor(unit.TeamId, weapon),
                ResolveModifiersFor(unit.TeamId, weapon));
            var fireResult = _weaponExecutor.TryFireSelected(activeInventory, context);
            if (fireResult != WeaponFireResult.Fired)
            {
                var outcome = fireResult == WeaponFireResult.OutOfAmmo ? BattleFireOutcome.OutOfAmmo : BattleFireOutcome.NoWeaponSelected;
                return new BattleFireResult(outcome, unit.TeamId, weapon);
            }

            _activeShotWeapon = weapon;
            var isHoppingGrenade = weapon.WeaponId == HoppingGrenadeWeaponId;
            _activeShotEndsTurn = !isHoppingGrenade;
            if (_activeShotEndsTurn)
            {
                _turnService.SpendAction(_state);
            }
            if (isHoppingGrenade && _projectiles.Count > projectilesBeforeFire)
            {
                _hoppingGrenadeProjectileId = _projectiles[_projectiles.Count - 1].Id;
            }
            _teleportSphereProjectileId = -1;
            if (weapon.WeaponId == TeleportWeaponId
                && ResolveEvolutionFor(unit.TeamId, weapon).TeleportEarlyTrigger
                && _projectiles.Count > projectilesBeforeFire)
            {
                _teleportSphereProjectileId = _projectiles[_projectiles.Count - 1].Id;
            }
            ShotFired?.Invoke(unit.TeamId, weapon, _activeShotEndsTurn);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon);
        }

        private BattleFireResult ConfirmOrbitalStrike(
            BattleUnitState unit,
            WeaponInventoryRuntime inventory,
            WeaponDefinition weapon)
        {
            if (!_orbitalTargetMarker.HasValue)
            {
                return new BattleFireResult(BattleFireOutcome.AimRequired, unit.TeamId, weapon);
            }

            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            _pendingOrbitalStrike = new PendingOrbitalStrike(unit.TeamId, unit.UnitId, weapon, _orbitalTargetMarker.Value);
            _orbitalTargetMarker = null;
            _activeShotWeapon = weapon;
            _activeShotEndsTurn = false;
            _turnService.SpendAction(_state);
            ShotFired?.Invoke(unit.TeamId, weapon, true);
            AdvanceTurnNow(TurnAdvanceReason.InstantActionCompleted);
            return new BattleFireResult(BattleFireOutcome.UtilityUsed, unit.TeamId, weapon);
        }

        /// <summary>
        /// Rank 5/10 branch mechanics for whichever side is firing. Both sides are
        /// looked up the same way: a raid or duel opponent is developed exactly as
        /// far as its own <see cref="BattleWeaponProgression"/> says.
        /// </summary>
        private WeaponEvolutionProfile ResolveEvolutionFor(BattleTeamId teamId, WeaponDefinition weapon)
        {
            return weapon == null
                ? WeaponEvolutionProfile.Base
                : ProgressionFor(teamId).ResolveEvolution(weapon.WeaponId);
        }

        /// <summary>
        /// Flat % Damage/ImpactRadius bonuses from ranks 2-4 and 6-9, resolved per
        /// side exactly like <see cref="ResolveEvolutionFor"/>.
        /// </summary>
        private WeaponUpgradeModifiers ResolveModifiersFor(BattleTeamId teamId, WeaponDefinition weapon)
        {
            return weapon == null
                ? default
                : ProgressionFor(teamId).ResolveModifiers(weapon.WeaponId);
        }

        private int ResolveRankFor(BattleTeamId teamId, WeaponDefinition weapon)
        {
            return weapon == null
                ? 1
                : Mathf.Max(1, (teamId == BattleTeamId.Player ? _playerProgression : _opponentProgression)
                    .ResolveRank(weapon.WeaponId));
        }

        private BattleWeaponProgression ProgressionFor(BattleTeamId teamId)
        {
            return teamId == BattleTeamId.Player ? _playerProgression : _opponentProgression;
        }

        private static AimState CreateImmediateActionAim(BattleUnitState unit)
        {
            var direction = unit.FacingDirectionX >= 0 ? Vector2.right : Vector2.left;
            return new AimState(unit.Position + Vector2.up * 0.12f, direction, 0f, true);
        }

        private void LaunchPendingOrbitalStrikeIfDue()
        {
            if (Result.IsFinished)
            {
                // The battle ended first: nothing may fall on the result screen.
                _pendingOrbitalStrike = null;
                return;
            }

            if (_pendingOrbitalStrike == null
                || _state.Turn.ActiveTeam != _pendingOrbitalStrike.OwnerTeam)
            {
                return;
            }

            var strike = _pendingOrbitalStrike;
            _pendingOrbitalStrike = null;
            var target = strike.Target;
            var terrainTop = _terrain != null
                ? _terrain.Grid.WorldMaxY - 0.25f
                : target.y + OrbitalRocketMinimumSpawnHeight;
            var spawnY = Mathf.Max(target.y + OrbitalRocketMinimumSpawnHeight, terrainTop);
            var definition = strike.Weapon.CreateProjectileDefinition(
                strike.Weapon.Damage,
                knockbackMultiplier: 1f,
                terrainBounceLimit: 0,
                timeoutSecondsOverride: Mathf.Max(2f, strike.Weapon.TimeoutSeconds));
            var projectile = new SimProjectile(
                ++_nextProjectileId,
                definition,
                strike.OwnerUnitId,
                new Vector2(target.x, spawnY),
                Vector2.down * OrbitalRocketFallSpeed,
                definition.TimeoutSeconds,
                suppressTurnAdvanceOnImpact: true);
            _projectiles.Add(projectile);
            ProjectileSpawned?.Invoke(projectile);
        }

        private static bool IsInstantUtility(WeaponRole role)
        {
            return role == WeaponRole.CoverTool || role == WeaponRole.DefensiveTool;
        }

        private BattleFireResult UseBlasterShot(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon, AimState aim)
        {
            var shotsPerTurn = GetBlasterShotsPerTurn(unit.TeamId);
            if (_blasterShotsThisTurn >= shotsPerTurn || !inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var direction = aim.Direction.sqrMagnitude > 0.01f ? aim.Direction.normalized : Vector2.right;
            var evolution = ResolveEvolutionFor(unit.TeamId, weapon);
            var request = new ProjectileSpawnRequest(
                weapon.CreateProjectileDefinition(weapon.Damage, 1f, evolution.TerrainBounceLimit),
                unit.UnitId,
                aim.Origin + direction * 0.45f,
                direction * aim.Power,
                weapon.ProjectileRadius * 2f,
                ProjectileWeaponEffect.GetProjectileColor(weapon.Role));

            _activeShotWeapon = weapon;
            _blasterShotsThisTurn++;
            // The turn ends on the final shot, when the magazine runs dry
            // mid-burst (otherwise the turn would never resolve), and always for
            // the bot - it fires a single planned shot per turn.
            _activeShotEndsTurn = _blasterShotsThisTurn >= shotsPerTurn
                || !inventory.CanFireSelected
                || unit.TeamId == BattleTeamId.Opponent;
            if (_activeShotEndsTurn)
            {
                _turnService.SpendAction(_state);
            }

            Spawn(request);
            ShotFired?.Invoke(unit.TeamId, weapon, true);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon);
        }

        private int GetBlasterShotsPerTurn(BattleTeamId teamId)
        {
            if (teamId != BattleTeamId.Player || _playerInventory.Selected?.WeaponId != BlasterWeaponId)
            {
                return BaseBlasterShotsPerTurn;
            }

            return Mathf.Max(
                BaseBlasterShotsPerTurn,
                ResolveEvolutionFor(teamId, _playerInventory.Selected).BlasterBurstShotCount);
        }

        private BattleFireResult StartDrillLaser(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon, AimState aim)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var direction = aim.Direction.sqrMagnitude > 0.01f ? aim.Direction.normalized : Vector2.right;
            var rangeUpgrades = ProgressionFor(unit.TeamId).ResolveUpgradeCount(
                weapon.WeaponId, WeaponUpgradePath.Radius);
            var drillRange = WeaponUpgradeChoiceResolver.ResolveDrillLaserRange(rangeUpgrades);
            // A beam is cut at the boundary of a hostile force field.
            drillRange = ClipBeamAtHostileForceFields(unit.TeamId, aim.Origin, direction, drillRange);
            _activeShotWeapon = weapon;
            _activeShotEndsTurn = true;
            _turnService.SpendAction(_state);
            _activeTimedEffects.Add(ActiveTimedEffect.Drill(
                unit.UnitId,
                unit.TeamId,
                weapon,
                aim.Origin,
                direction,
                drillRange,
                TerrainMaterial.Titanium));
            LinearWeaponEffectStarted?.Invoke(new LinearWeaponEffectEvent(
                weapon,
                unit.TeamId,
                aim.Origin,
                direction,
                drillRange,
                DrillVisualGrowthSeconds,
                DrillDurationSeconds));
            ShotFired?.Invoke(unit.TeamId, weapon, true);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon, aim.Origin + direction * drillRange);
        }

        private BattleFireResult StartFireStream(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon, AimState aim)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var direction = aim.Direction.sqrMagnitude > 0.01f ? aim.Direction.normalized : Vector2.right;
            _activeShotWeapon = weapon;
            _activeShotEndsTurn = true;
            _turnService.SpendAction(_state);
            var evolution = ResolveEvolutionFor(unit.TeamId, weapon);
            var streamDuration = FireStreamDurationSeconds * evolution.FireGunStreamDurationMultiplier;
            var stream = ActiveTimedEffect.FireStream(unit.UnitId, unit.TeamId, weapon, aim.Origin, direction, streamDuration);
            stream.StreamId = ++_nextProjectileId;
            // The whole stream is one attack: Core steps scale its single damage budget.
            stream.TotalDamage = Mathf.RoundToInt(weapon.Damage * ResolveModifiersFor(unit.TeamId, weapon).DamageMultiplier);
            _activeTimedEffects.Add(stream);
            ShotFired?.Invoke(unit.TeamId, weapon, true);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon, aim.Origin + direction * FireStreamRange);
        }

        private BattleFireResult UseCloseRangeBurst(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon, AimState aim)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            _activeShotWeapon = weapon;
            _turnService.SpendAction(_state);

            var origin = aim.Origin;
            var direction = aim.Direction.sqrMagnitude > 0.01f ? aim.Direction.normalized : Vector2.right;
            if (weapon.WeaponId == PlasmaSwordWeaponId)
            {
                SwordSwingStarted?.Invoke(new SwordSwingEvent(weapon, unit.TeamId, origin, direction));
                var swordRange = Mathf.Max(1.75f, weapon.ImpactRadius * 1.75f);
                BattleUnitState nearestTarget = null;
                var nearestDistance = float.PositiveInfinity;
                foreach (var team in _state.Teams)
                {
                    if (team.TeamId == unit.TeamId)
                    {
                        continue;
                    }

                    foreach (var target in team.Units)
                    {
                        if (target == null || target.IsDefeated)
                        {
                            continue;
                        }

                        var toTarget = target.Position - origin;
                        var distance = toTarget.magnitude;
                        if (distance > swordRange || distance >= nearestDistance
                            || Vector2.Dot(direction, toTarget.normalized) < 0.25f)
                        {
                            continue;
                        }

                        nearestTarget = target;
                        nearestDistance = distance;
                    }
                }

                if (nearestTarget != null)
                {
                    ApplyExplosionKnockback(nearestTarget.Position - direction * 0.15f,
                        direction, weapon, false, ignoredTeam: unit.TeamId,
                        onlyTarget: nearestTarget,
                        radiusOverride: PrototypeWeaponLibrary.SharedExplosionRadius);
                    var modifiers = ResolveModifiersFor(unit.TeamId, weapon);
                    ApplyDamage(nearestTarget,
                        Mathf.RoundToInt(weapon.Damage * modifiers.DamageMultiplier),
                        weapon.WeaponId, unit.TeamId);
                }

                ShotFired?.Invoke(unit.TeamId, weapon, true);
                CompleteInstantAction();
                return new BattleFireResult(BattleFireOutcome.BurstFired, unit.TeamId, weapon,
                    nearestTarget?.Position ?? origin + direction * swordRange,
                    nearestTarget == null ? 0 : 1);
            }
            var range = Mathf.Max(1.2f, weapon.ImpactRadius * 1.75f);
            var hitCount = 0;

            foreach (var team in _state.Teams)
            {
                foreach (var target in team.Units)
                {
                    if (target == null || target.IsDefeated)
                    {
                        continue;
                    }

                    var toUnit = target.Position - origin;
                    if (toUnit.magnitude > range || Vector2.Dot(direction, toUnit.normalized) < 0.25f)
                    {
                        continue;
                    }

                    ApplyDamage(target, weapon.Damage, weapon.WeaponId, unit.TeamId);
                    hitCount++;
                }
            }

            var burstPoint = origin + direction * Mathf.Min(range, 0.95f);
            var terrainChanged = ApplyTerrainDestruction(burstPoint, weapon, unit.TeamId);
            ApplyExplosionKnockback(burstPoint, direction * 8f, weapon, terrainChanged);
            ShotFired?.Invoke(unit.TeamId, weapon, true);
            CompleteInstantAction();
            return new BattleFireResult(BattleFireOutcome.BurstFired, unit.TeamId, weapon, burstPoint, hitCount);
        }

        private BattleFireResult UseInstantUtility(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon, AimState aim)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            _activeShotWeapon = weapon;

            if (weapon.Role == WeaponRole.CoverTool)
            {
                _turnService.SpendAction(_state);
                DeployCover(GetCoverPlacementPoint(aim, weapon), weapon, aim.Direction);
            }
            else if (weapon.Role == WeaponRole.DefensiveTool)
            {
                RestoreFriendlyHealth(unit.Position, unit.UnitId, weapon);
                ShotFired?.Invoke(unit.TeamId, weapon, false);
                // Healing is a completed action too: it enters the universal
                // movement-only retreat window instead of leaving a free turn.
                CompleteInstantAction();
                return new BattleFireResult(BattleFireOutcome.UtilityUsed, unit.TeamId, weapon);
            }

            ShotFired?.Invoke(unit.TeamId, weapon, false);
            CompleteInstantAction();
            return new BattleFireResult(BattleFireOutcome.UtilityUsed, unit.TeamId, weapon);
        }

        private static Vector2 GetCoverPlacementPoint(AimState aim, WeaponDefinition weapon)
        {
            var aimSettings = weapon.CreateAimSettings(AimSettings.Default);
            var power01 = Mathf.InverseLerp(aimSettings.MinPower, aimSettings.MaxPower, aim.Power);
            var placementDistance = Mathf.Lerp(1.25f, 3.1f, power01);
            return aim.Origin + aim.Direction.normalized * placementDistance;
        }

        private void CompleteInstantAction()
        {
            UnitsMoved?.Invoke();
            Result = BattleResultEvaluator.Evaluate(_state);
            if (Result.IsFinished)
            {
                BattleFinished?.Invoke(Result, BattleFinishReason.InstantActionCompleted);
                return;
            }

            AdvanceTurnOrStartRetreat(TurnAdvanceReason.InstantActionCompleted);
        }

        private void StepTimedWeaponEffects(float deltaTime)
        {
            if (Result.IsFinished)
            {
                return;
            }

            for (var i = _activeTimedEffects.Count - 1; i >= 0; i--)
            {
                var effect = _activeTimedEffects[i];
                effect.Elapsed += deltaTime;

                if (effect.Kind == ActiveTimedEffectKind.Drill)
                {
                    effect.TerrainTickAccumulator += deltaTime;
                    while (effect.TerrainTickAccumulator >= DrillTerrainTickSeconds)
                    {
                        effect.TerrainTickAccumulator -= DrillTerrainTickSeconds;
                        // Track the beam visual exactly (same 0.25->max growth
                        // curve), so destruction never outruns what is drawn.
                        var terrainRange01 = Mathf.Clamp01(effect.Elapsed / DrillVisualGrowthSeconds);
                        var terrainRange = Mathf.Lerp(0.25f, effect.DrillRange, terrainRange01);
                        ApplyLinearTerrain(effect, terrainRange, 0.6f);
                    }

                    effect.TickAccumulator += deltaTime;
                    while (effect.TickAccumulator >= DrillTickSeconds)
                    {
                        effect.TickAccumulator -= DrillTickSeconds;
                        var range01 = Mathf.Clamp01(effect.Elapsed / DrillVisualGrowthSeconds);
                        var range = Mathf.Lerp(0.25f, effect.DrillRange, range01);
                        ApplyLinearDamageAndTerrain(effect, range, Mathf.Max(1, Mathf.CeilToInt(effect.Weapon.Damage / 3f)), 0f, false);
                    }

                    if (effect.Elapsed < DrillDurationSeconds)
                    {
                        continue;
                    }

                    _activeTimedEffects.RemoveAt(i);
                    CompleteInstantAction();
                    continue;
                }

                if (effect.Kind == ActiveTimedEffectKind.FireStream)
                {
                    effect.TickAccumulator += deltaTime;
                    while (effect.TickAccumulator >= FireStreamSpawnIntervalSeconds)
                    {
                        effect.TickAccumulator -= FireStreamSpawnIntervalSeconds;
                        SpawnFireBlob(effect);
                    }

                    if (effect.Elapsed < effect.StreamDurationSeconds)
                    {
                        continue;
                    }

                    _activeTimedEffects.RemoveAt(i);
                    CompleteInstantAction();
                }
            }
        }

        private void StepFirePuddles(float deltaTime)
        {
            if (Result.IsFinished)
            {
                return;
            }

            if (_activeFirePuddles.Count == 0 && _streamHits.Count > 0
                && !_activeTimedEffects.Exists(effect => effect.Kind == ActiveTimedEffectKind.FireStream))
            {
                _streamHits.Clear();
            }

            for (var i = _activeFirePuddles.Count - 1; i >= 0; i--)
            {
                var puddle = _activeFirePuddles[i];
                puddle.DelayRemaining = Mathf.Max(0f, puddle.DelayRemaining - deltaTime);
                if (puddle.DelayRemaining > 0f)
                {
                    continue;
                }

                puddle.Elapsed += deltaTime;
                puddle.TickAccumulator += deltaTime;
                puddle.DamageTickAccumulator += deltaTime;
                var startPoint = puddle.Point;
                var hasSupport = true;
                while (puddle.TickAccumulator >= FirePuddleTickSeconds)
                {
                    puddle.TickAccumulator -= FirePuddleTickSeconds;
                    hasSupport &= TryDropFirePuddleToSurface(puddle);
                    puddle.Point += Vector2.down * 0.045f;
                    // Small scorch bites: about a third of the flame sprite's
                    // area per bite - the fire chews through slowly instead of
                    // demolishing the ground. A deterministic sideways wobble
                    // (hashed from puddle id + burn time) keeps the burn holes
                    // ragged instead of drilling perfectly vertical shafts.
                    var wobble = Mathf.Sin(puddle.Id * 12.9898f + puddle.Elapsed * 9.7f) * 0.09f;
                    var terrainChanged = ApplyTerrainCrater(
                        puddle.Point + new Vector2(wobble, -0.08f),
                        FirePuddleTerrainBurnRadius,
                        ResolveTeamForOwner(puddle.OwnerUnitId));
                    if (terrainChanged)
                    {
                        // The fire just burned through its own floor - chase the
                        // new surface below instead of burning in mid-air.
                        hasSupport &= TryDropFirePuddleToSurface(puddle);
                    }

                    UnitsMoved?.Invoke();
                }

                while (puddle.DamageTickAccumulator >= FirePuddleDamageTickSeconds)
                {
                    puddle.DamageTickAccumulator -= FirePuddleDamageTickSeconds;
                    var tickDamage = GetFirePuddleDamagePerTick(puddle);
                    if (tickDamage > 0)
                    {
                        ApplyPointDamage(
                            puddle.Point,
                            puddle.Weapon,
                            puddle.OwnerUnitId,
                            FirePuddleDamageRadius,
                            tickDamage,
                            applyLavaOverlay: true,
                            attackId: puddle.StreamId);
                    }

                    UnitsMoved?.Invoke();
                }

                if (!hasSupport || puddle.Point.y < _settings.FallDeathY)
                {
                    // Nothing left underneath anywhere - the fire drains out of
                    // the world instead of hovering.
                    _activeFirePuddles.RemoveAt(i);
                    FirePuddleExtinguished?.Invoke(puddle.Id);
                    continue;
                }

                if ((puddle.Point - startPoint).sqrMagnitude > 0.0004f)
                {
                    FirePuddleMoved?.Invoke(puddle.Id, puddle.Point);
                }

                if (puddle.Elapsed >= puddle.LifetimeSeconds)
                {
                    _activeFirePuddles.RemoveAt(i);
                    FirePuddleExtinguished?.Invoke(puddle.Id);
                }
            }
        }

        /// <summary>
        /// Keeps a burning puddle glued to the terrain surface. Returns false
        /// when no surface exists anywhere below - the puddle has nothing to
        /// burn on and should be removed.
        /// </summary>
        private bool TryDropFirePuddleToSurface(ActiveFirePuddle puddle)
        {
            if (_terrain == null)
            {
                return true;
            }

            var probePoint = puddle.Point + Vector2.down * 0.12f;
            if (_terrain.Grid.IsSolidWorld(probePoint))
            {
                return true;
            }

            if (_terrain.Grid.TryFindSurfaceBelow(
                    puddle.Point.x,
                    puddle.Point.y - 0.08f,
                    FirePuddleDropSearchDistance,
                    out var surfaceY))
            {
                if (puddle.Point.y - surfaceY > 0.12f)
                {
                    puddle.Point = new Vector2(puddle.Point.x, surfaceY + 0.08f);
                }

                return true;
            }

            return false;
        }

        private static int GetFirePuddleDamagePerTick(ActiveFirePuddle puddle)
        {
            return puddle.ConsumeDamageForTick();
        }

        private void SpawnIncendiaryPuddles(ProjectileImpact impact, WeaponDefinition weapon)
        {
            var ownerTeam = ResolveTeamForOwner(impact.OwnerUnitId);
            // The ground flames are presentation only: the blast and the shared fire status carry the
            // damage, so the puddles neither hurt nor ignite anyone who walks in later.
            var totalDamage = 0;
            var baseDamage = totalDamage / IncendiaryPuddleCount;
            var remainder = totalDamage % IncendiaryPuddleCount;

            // The drops shoot out of the blast at once, sideways and down, and reach the crater's ground within a
            // fraction of a second: no rise and no pause in the air before they fall.
            var gravity = _projectileGravityPerScale * IncendiaryDropGravityScale;
            for (var index = 0; index < IncendiaryPuddleCount; index++)
            {
                // Owner decision 2026-10-09: twice as many, smaller drops that fall nearly straight down in a tight bunch.
                var side = index % 2 == 0 ? -1f : 1f;
                var reach = 0.04f + (index / 2) * 0.05f;
                var initialVelocity = new Vector2(side * IncendiaryDropSpeed * reach, -IncendiaryDropSpeed * 0.6f);
                var origin = impact.Point + Vector2.up * 0.05f;
                TraceFireBlobTrajectory(impact.OwnerUnitId, origin, initialVelocity, gravity,
                    out var endpoint, out var travelSeconds);
                var puddleId = ++_nextProjectileId;
                FireBlobSpawned?.Invoke(new FireBlobEvent(
                    puddleId,
                    weapon,
                    ownerTeam,
                    origin,
                    initialVelocity,
                    gravity,
                    endpoint,
                    travelSeconds,
                    IncendiaryPuddleLifetimeSeconds));
                _activeFirePuddles.Add(new ActiveFirePuddle(
                    puddleId,
                    impact.OwnerUnitId,
                    weapon,
                    endpoint,
                    travelSeconds,
                    IncendiaryPuddleLifetimeSeconds,
                    baseDamage + (index < remainder ? 1 : 0)));
            }
        }

        private void SpawnFireBlob(ActiveTimedEffect effect)
        {
            var puddleId = ++_nextProjectileId;
            // Reference flame thrower ramps projectile power over the burst and
            // wobbles the angle slightly, so the fire lays down a strip instead
            // of pooling in a single point. Deterministic per blob index.
            var blobIndex = effect.SpawnedBlobs++;
            var totalBlobs = Mathf.Max(2, Mathf.RoundToInt(effect.StreamDurationSeconds / FireStreamSpawnIntervalSeconds));
            var ramp01 = Mathf.Clamp01(blobIndex / (float)(totalBlobs - 1));
            var speed = FireBlobSpeed * Mathf.Lerp(0.65f, 1.35f, ramp01);
            var jitterRadians = ((blobIndex % 3) - 1) * 2.5f * Mathf.Deg2Rad;
            var cos = Mathf.Cos(jitterRadians);
            var sin = Mathf.Sin(jitterRadians);
            var direction = new Vector2(
                effect.Direction.x * cos - effect.Direction.y * sin,
                effect.Direction.x * sin + effect.Direction.y * cos);
            var initialVelocity = direction * speed;
            var gravity = _projectileGravityPerScale * FireBlobGravityScale;
            TraceFireBlobTrajectory(
                effect.OwnerUnitId,
                effect.Origin,
                initialVelocity,
                gravity,
                out var endpoint,
                out var travelSeconds);
            var evolution = ResolveEvolutionFor(effect.TeamId, effect.Weapon);
            var puddleLifetime = FirePuddleLifetimeSeconds * evolution.FireGunBurnDurationMultiplier;
            FireBlobSpawned?.Invoke(new FireBlobEvent(
                puddleId,
                effect.Weapon,
                effect.TeamId,
                effect.Origin,
                initialVelocity,
                gravity,
                endpoint,
                travelSeconds,
                puddleLifetime));
            var baseDamageBudget = effect.TotalDamage / totalBlobs;
            var remainingDamageBudget = effect.TotalDamage % totalBlobs;
            var damageBudget = baseDamageBudget + (blobIndex < remainingDamageBudget ? 1 : 0);
            _activeFirePuddles.Add(new ActiveFirePuddle(
                puddleId,
                effect.OwnerUnitId,
                effect.Weapon,
                endpoint,
                travelSeconds,
                puddleLifetime,
                damageBudget,
                effect.StreamId));
        }

        private void TraceFireBlobTrajectory(
            string ownerUnitId,
            Vector2 origin,
            Vector2 initialVelocity,
            Vector2 gravity,
            out Vector2 impactPoint,
            out float travelSeconds)
        {
            const float traceStepSeconds = 1f / 60f;
            var position = origin;
            var velocity = initialVelocity;
            travelSeconds = 0f;

            while (travelSeconds < FireBlobMaxTravelSeconds)
            {
                velocity += gravity * traceStepSeconds;
                var next = position + velocity * traceStepSeconds;
                travelSeconds += traceStepSeconds;

                if (_terrain != null && _terrain.Grid.Raycast(position, next, out impactPoint))
                {
                    return;
                }

                if (TryHitUnit(next, 0.08f, ownerUnitId, out impactPoint))
                {
                    return;
                }

                position = next;
                if (position.y < _settings.FallDeathY)
                {
                    break;
                }
            }

            impactPoint = position;
        }

        private sealed class StreamHit
        {
            public bool Boosted { get; set; }
            public int Total { get; set; }
        }

        private sealed class PendingOrbitalStrike
        {
            public PendingOrbitalStrike(BattleTeamId ownerTeam, string ownerUnitId, WeaponDefinition weapon, Vector2 target)
            {
                OwnerTeam = ownerTeam;
                OwnerUnitId = ownerUnitId;
                Weapon = weapon;
                Target = target;
            }

            public BattleTeamId OwnerTeam { get; }
            public string OwnerUnitId { get; }
            public WeaponDefinition Weapon { get; }
            public Vector2 Target { get; }
        }

        private enum ActiveTimedEffectKind
        {
            Drill,
            FireStream
        }

        private sealed class ActiveTimedEffect
        {
            private ActiveTimedEffect(
                ActiveTimedEffectKind kind,
                string ownerUnitId,
                BattleTeamId teamId,
                WeaponDefinition weapon,
                Vector2 origin,
                Vector2 direction,
                Vector2 targetPoint,
                float drillRange = 0f,
                TerrainMaterial drillPenetration = TerrainMaterial.Soil)
            {
                Kind = kind;
                OwnerUnitId = ownerUnitId;
                TeamId = teamId;
                Weapon = weapon;
                Origin = origin;
                Direction = direction.sqrMagnitude > 0.01f ? direction.normalized : Vector2.right;
                TargetPoint = targetPoint;
                DrillRange = Mathf.Max(0.25f, drillRange);
                DrillPenetration = drillPenetration;
            }

            public ActiveTimedEffectKind Kind { get; }
            public string OwnerUnitId { get; }
            public BattleTeamId TeamId { get; }
            public WeaponDefinition Weapon { get; }
            public Vector2 Origin { get; }
            public Vector2 Direction { get; }
            public Vector2 TargetPoint { get; }
            public float DrillRange { get; }
            public TerrainMaterial DrillPenetration { get; }
            public float Elapsed { get; set; }
            public float TickAccumulator { get; set; }
            public float TerrainTickAccumulator { get; set; }
            public int SpawnedBlobs { get; set; }
            public float StreamDurationSeconds { get; set; } = FireStreamDurationSeconds;

            public static ActiveTimedEffect Drill(
                string ownerUnitId,
                BattleTeamId teamId,
                WeaponDefinition weapon,
                Vector2 origin,
                Vector2 direction,
                float drillRange,
                TerrainMaterial drillPenetration)
            {
                return new ActiveTimedEffect(
                    ActiveTimedEffectKind.Drill,
                    ownerUnitId,
                    teamId,
                    weapon,
                    origin,
                    direction,
                    Vector2.zero,
                    drillRange,
                    drillPenetration);
            }

            public int StreamId { get; set; }
            public int TotalDamage { get; set; }

            public static ActiveTimedEffect FireStream(
                string ownerUnitId,
                BattleTeamId teamId,
                WeaponDefinition weapon,
                Vector2 origin,
                Vector2 direction,
                float streamDurationSeconds = FireStreamDurationSeconds)
            {
                return new ActiveTimedEffect(ActiveTimedEffectKind.FireStream, ownerUnitId, teamId, weapon, origin, direction, Vector2.zero)
                {
                    StreamDurationSeconds = streamDurationSeconds > 0f ? streamDurationSeconds : FireStreamDurationSeconds
                };
            }

        }

        private sealed class ActiveFirePuddle
        {
            public ActiveFirePuddle(
                int id,
                string ownerUnitId,
                WeaponDefinition weapon,
                Vector2 point,
                float delaySeconds,
                float lifetimeSeconds = FirePuddleLifetimeSeconds,
                int damageBudget = 0,
                int streamId = 0)
            {
                StreamId = streamId;
                Id = id;
                OwnerUnitId = ownerUnitId;
                Weapon = weapon;
                Point = point;
                DelayRemaining = Mathf.Max(0f, delaySeconds);
                LifetimeSeconds = lifetimeSeconds > 0f ? lifetimeSeconds : FirePuddleLifetimeSeconds;
                DamageBudget = Mathf.Max(0, damageBudget);
            }

            public int StreamId { get; }
            public int Id { get; }
            public string OwnerUnitId { get; }
            public WeaponDefinition Weapon { get; }
            public Vector2 Point { get; set; }
            public float DelayRemaining { get; set; }
            public float LifetimeSeconds { get; }
            public float Elapsed { get; set; }
            public float TickAccumulator { get; set; }
            public float DamageTickAccumulator { get; set; }
            public int DamageBudget { get; }
            public int DamageTicksProcessed { get; private set; }
            public int DamageApplied { get; private set; }

            public int ConsumeDamageForTick()
            {
                var totalTicks = Mathf.Max(1, Mathf.CeilToInt(LifetimeSeconds / FirePuddleDamageTickSeconds));
                DamageTicksProcessed = Mathf.Min(totalTicks, DamageTicksProcessed + 1);
                var cumulativeBudget = Mathf.CeilToInt(
                    DamageBudget * DamageTicksProcessed / (float)totalTicks);
                var targetDamage = DamageTicksProcessed >= totalTicks
                    ? DamageBudget
                    : cumulativeBudget;
                var tickDamage = Mathf.Max(0, targetDamage - DamageApplied);
                DamageApplied += tickDamage;
                return tickDamage;
            }
        }
    }
}
