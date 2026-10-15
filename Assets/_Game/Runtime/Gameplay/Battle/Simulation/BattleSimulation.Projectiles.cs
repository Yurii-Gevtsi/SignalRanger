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
        /// <summary>The hopping grenade's blast is the attack of the turn: it spends the action and lets the turn pass.</summary>
        private void ConcludeHoppingGrenadeShot()
        {
            _hoppingGrenadeProjectileId = -1;
            _activeShotEndsTurn = true;
            _turnService.SpendAction(_state);
        }

        public bool CanContinueHoppingGrenade()
        {
            return _hoppingGrenadeProjectileId >= 0
                && _projectiles.Exists(projectile => projectile.Id == _hoppingGrenadeProjectileId
                    && projectile.HopsRemaining > 0);
        }

        private bool TryHopRestingGrenade(
            BattleUnitState unit,
            WeaponDefinition weapon,
            AimState aim,
            out BattleFireResult result)
        {
            var projectile = _projectiles.Find(value => value.Id == _hoppingGrenadeProjectileId);
            if (projectile == null || projectile.HopsRemaining <= 0)
            {
                result = default;
                return false;
            }

            var direction = aim.Direction.sqrMagnitude > 0.01f
                ? aim.Direction.normalized
                : new Vector2(unit.FacingDirectionX >= 0 ? 1f : -1f, 0f);
            var horizontal = Mathf.Sign(direction.x == 0f ? unit.FacingDirectionX : direction.x);
            var hopVelocity = new Vector2(horizontal * HoppingGrenadeLaunchSpeed, HoppingGrenadeVerticalSpeed);
            if (projectile.IsRestingFuse)
            {
                // The fuse keeps running through the jump: carry the remaining time into flight.
                projectile.SetHopFuse(projectile.RestingFuseRemaining);
                projectile.ResumeFromRestingFuse(hopVelocity);
            }
            else
            {
                projectile.SetVelocity(hopVelocity);
            }

            projectile.ConsumeHop();
            _activeShotWeapon = weapon;
            _activeShotEndsTurn = false;
            ShotFired?.Invoke(unit.TeamId, weapon, false);
            result = new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon, projectile.Position);
            return true;
        }

        private void ResolveProjectileImpact(ProjectileImpact impact, bool suppressTurnAdvance = false,
            BattleUnitState directHit = null, float pelletTerrainCraterRadius = 0f)
        {
            var impactWeapon = ResolveImpactWeapon(impact);
            var impactForResolution = impact;
            var isPrimaryMortar = impactWeapon?.WeaponId == "weapon.mortira"
                && impact.ProjectileId == "projectile.weapon.mortira";
            if ((IsMortarFragment(impact.ProjectileId) || IsMortarCascade(impact.ProjectileId))
                && _weaponsById.TryGetValue("weapon.mortira", out var mortar))
            {
                impactWeapon = CreateMortarFragmentWeapon(mortar);
            }
            else if (IsMissileCluster(impact.ProjectileId)
                && _weaponsById.TryGetValue(MissileWeaponId, out var missile))
            {
                impactWeapon = CreateMissileClusterWeapon(missile, GetMissileClusterCount(impact.ProjectileId));
                impactForResolution = new ProjectileImpact(
                    impact.ProjectileId,
                    impact.OwnerUnitId,
                    impact.Point,
                    impact.Velocity,
                    impactWeapon.Damage,
                    impactWeapon.ImpactRadius);
            }

            var terrainChanged = false;
            if (impactWeapon?.WeaponId == IonShotgunWeaponId)
            {
                if (directHit != null)
                {
                    ApplyDamage(directHit,
                        ResolveOutgoingDamage(impact.OwnerUnitId, impact.Damage),
                        impact.ProjectileId, ResolveTeamForOwner(impact.OwnerUnitId));
                }
                else if (pelletTerrainCraterRadius > 0f)
                {
                    terrainChanged = ApplyTerrainCrater(impact.Point, pelletTerrainCraterRadius,
                        ResolveTeamForOwner(impact.OwnerUnitId));
                }
            }
            else
            {
                terrainChanged = ResolveImpact(impactForResolution, impactWeapon);
            }
            if (impactWeapon?.WeaponId == IncendiaryGrenadeWeaponId)
            {
                SpawnIncendiaryPuddles(impactForResolution, impactWeapon);
            }
            ImpactResolved?.Invoke(new ImpactResolvedEvent(impact.Point, impactWeapon, terrainChanged));
            if (isPrimaryMortar)
            {
                var evolution = ResolveEvolutionFor(ResolveTeamForOwner(impact.OwnerUnitId), impactWeapon);
                SpawnMortarFragments(impact, impactWeapon,
                    evolution.MortarCascadeCount > 0 ? evolution.MortarCascadeCount : 2,
                    evolution.MortarCascadeCount > 0);
            }
            UnitsMoved?.Invoke();

            if (HasProjectilesInFlight)
            {
                return;
            }

            if (suppressTurnAdvance)
            {
                Result = BattleResultEvaluator.Evaluate(_state);
                if (Result.IsFinished)
                {
                    UnitDefeated?.Invoke(new UnitDefeatedEvent(string.Empty, "battle-finished"));
                    BattleFinished?.Invoke(Result, BattleFinishReason.ProjectileImpactResolved);
                }

                _activeShotEndsTurn = true;
                return;
            }

            if (HasActiveWeaponEffect || !_activeShotEndsTurn)
            {
                _activeShotEndsTurn = true;
                return;
            }

            Result = BattleResultEvaluator.Evaluate(_state);
            if (Result.IsFinished)
            {
                UnitDefeated?.Invoke(new UnitDefeatedEvent(string.Empty, "battle-finished"));
                BattleFinished?.Invoke(Result, BattleFinishReason.ProjectileImpactResolved);
                return;
            }

            AdvanceTurnOrStartRetreat(TurnAdvanceReason.ProjectileImpactResolved);
        }

        private sealed class MortarSequence
        {
            public readonly List<int> ProjectileIds = new();
            public float ContactSeconds;
        }

        private void SpawnMortarFragments(ProjectileImpact impact, WeaponDefinition weapon, int count, bool sequential)
        {
            var sequence = sequential ? new MortarSequence() : null;
            var fragmentDamage = Mathf.Max(1, Mathf.CeilToInt(impact.Damage * 0.5f));
            for (var i = 0; i < count; i++)
            {
                var definition = ProjectileDefinition.CreateRuntime(
                    sequential ? MortarCascadeProjectilePrefix + (i + 1) : "projectile.weapon.mortira.fragment",
                    weapon.ProjectileRadius * 0.5f, weapon.GravityScale, 30f,
                    fragmentDamage, impact.ImpactRadius);
                var side = i % 2 == 0 ? -1f : 1f;
                var position = impact.Point + new Vector2(side * (sequential ? 0.03f : 0.12f), 0.2f);
                var velocity = sequential ? new Vector2(side * 0.18f, 0.6f) : new Vector2(side * 3f, 2.5f);
                Spawn(new ProjectileSpawnRequest(definition, impact.OwnerUnitId, position, velocity,
                    definition.Radius * 2f, ProjectileWeaponEffect.GetProjectileColor(weapon.Role)));
                if (sequence != null)
                {
                    var id = _nextProjectileId;
                    sequence.ProjectileIds.Add(id);
                    _mortarSequences.Add(id, sequence);
                }
            }
        }

        private void StepMortarFragment(int index, SimProjectile projectile, MortarSequence sequence, float deltaTime)
        {
            // Waiting shards remain physical: support is queried again after every crater.
            projectile.ReduceLifetime(deltaTime);
            var weapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
            var velocity = projectile.Velocity + _projectileGravityPerScale * projectile.Definition.GravityScale * deltaTime;
            var position = projectile.Position;
            if (TryFindGrounderForProjectile(projectile, weapon, position, out var attractor))
            {
                var direction = attractor.Position - position;
                if (direction.sqrMagnitude > 0.0001f)
                    velocity = Vector2.MoveTowards(velocity,
                        direction.normalized * Mathf.Max(5f, velocity.magnitude), 35f * deltaTime);
            }
            var displacement = velocity * deltaTime;
            var steps = Mathf.Max(1, Mathf.CeilToInt(displacement.magnitude / ProjectileSubStepLength));
            var contact = false;
            for (var step = 0; step < steps; step++)
            {
                var next = position + displacement / steps;
                if (TryFindGrounderForProjectile(projectile, weapon, next, out var grounder)
                    && Vector2.Distance(next, grounder.Position)
                        <= Mathf.Max(0.35f, projectile.Definition.Radius + 0.18f))
                {
                    ResolveGrounderProjectileContact(index, projectile, weapon, grounder, next);
                    return;
                }
                if (_terrain?.Grid.IsSolidWorld(next) == true)
                {
                    velocity = ProjectileBounceRules.ReflectOffTerrain(_terrain.Grid, next, velocity,
                        projectile.Definition.Radius, ProjectileSubStepLength) * ProjectileBounceRules.SpeedRetain;
                    contact = true;
                    break;
                }
                position = next;
            }
            projectile.MoveTo(position);
            projectile.SetVelocity(velocity);
            var radius = Mathf.Max(0.08f, projectile.Definition.Radius);
            contact |= _terrain?.Grid.IsSolidWorld(position + Vector2.down * radius) == true;
            if (sequence.ProjectileIds[0] != projectile.Id) return;
            if (contact) sequence.ContactSeconds += deltaTime;
            if (sequence.ContactSeconds < MortarFragmentContactSeconds && position.y >= _settings.FallDeathY - 2f
                && projectile.RemainingLifetime > 0f) return;
            ResolveProjectileAt(index, position);
        }

        private void ReleaseMortarFragment(int projectileId)
        {
            if (!_mortarSequences.TryGetValue(projectileId, out var sequence)) return;
            if (sequence.ProjectileIds.Count > 0 && sequence.ProjectileIds[0] == projectileId)
                sequence.ContactSeconds = 0f;
            sequence.ProjectileIds.Remove(projectileId);
            _mortarSequences.Remove(projectileId);
        }

        private static WeaponDefinition CreateMortarFragmentWeapon(WeaponDefinition weapon)
        {
            return WeaponDefinition.CreateRuntime(
                "weapon.mortira.fragment",
                weapon.DisplayKey,
                weapon.Role,
                Mathf.Max(1, Mathf.CeilToInt(weapon.Damage * 0.5f)),
                weapon.ImpactRadius,
                weapon.ProjectileRadius * 0.5f,
                weapon.GravityScale,
                3f,
                weapon.MinPower,
                weapon.MaxPower,
                -1,
                weapon.TutorialHintKey,
                CopyTags(weapon.Tags),
                iconId: weapon.IconId);
        }

        private static WeaponDefinition CreateC4EvolutionWeapon(
            WeaponDefinition weapon,
            WeaponEvolutionProfile evolution,
            WeaponUpgradeModifiers modifiers = default)
        {
            var damageMultiplier = (evolution.C4DamageMultiplier > 0f ? evolution.C4DamageMultiplier : 1f) * modifiers.DamageMultiplier;
            var radiusMultiplier = (evolution.C4ImpactRadiusMultiplier > 0f ? evolution.C4ImpactRadiusMultiplier : 1f) * modifiers.ImpactRadiusMultiplier;
            if (Mathf.Approximately(damageMultiplier, 1f) && Mathf.Approximately(radiusMultiplier, 1f))
            {
                return weapon;
            }

            return WeaponDefinition.CreateRuntime(
                weapon.WeaponId,
                weapon.DisplayKey,
                weapon.Role,
                Mathf.Max(1, Mathf.CeilToInt(weapon.Damage * damageMultiplier)),
                weapon.ImpactRadius * radiusMultiplier,
                weapon.ProjectileRadius,
                weapon.GravityScale,
                weapon.TimeoutSeconds,
                weapon.MinPower,
                weapon.MaxPower,
                weapon.AmmoCount,
                weapon.TutorialHintKey,
                CopyTags(weapon.Tags),
                iconId: weapon.IconId);
        }

        private static WeaponDefinition CreateFpvEvolutionWeapon(
            WeaponDefinition weapon,
            WeaponEvolutionProfile evolution,
            WeaponUpgradeModifiers modifiers = default)
        {
            var damageMultiplier = (evolution.FpvDamageMultiplier > 0f ? evolution.FpvDamageMultiplier : 1f) * modifiers.DamageMultiplier;
            var radiusMultiplier = (evolution.FpvImpactRadiusMultiplier > 0f ? evolution.FpvImpactRadiusMultiplier : 1f) * modifiers.ImpactRadiusMultiplier;
            if (Mathf.Approximately(damageMultiplier, 1f) && Mathf.Approximately(radiusMultiplier, 1f))
            {
                return weapon;
            }

            return WeaponDefinition.CreateRuntime(
                weapon.WeaponId,
                weapon.DisplayKey,
                weapon.Role,
                Mathf.Max(1, Mathf.CeilToInt(weapon.Damage * damageMultiplier)),
                weapon.ImpactRadius * radiusMultiplier,
                weapon.ProjectileRadius,
                weapon.GravityScale,
                weapon.TimeoutSeconds,
                weapon.MinPower,
                weapon.MaxPower,
                weapon.AmmoCount,
                weapon.TutorialHintKey,
                CopyTags(weapon.Tags),
                iconId: weapon.IconId);
        }

        private static WeaponDefinition CreateMissileClusterWeapon(WeaponDefinition weapon, int chargeCount)
        {
            var count = Mathf.Max(2, chargeCount);
            return WeaponDefinition.CreateRuntime(
                "weapon.missle.cluster",
                weapon.DisplayKey,
                weapon.Role,
                Mathf.Max(1, Mathf.CeilToInt(weapon.Damage / (float)count)),
                weapon.ImpactRadius * 0.58f,
                weapon.ProjectileRadius * 0.6f,
                weapon.GravityScale,
                3f,
                weapon.MinPower,
                weapon.MaxPower,
                -1,
                weapon.TutorialHintKey,
                CopyTags(weapon.Tags),
                iconId: weapon.IconId);
        }

        private static string[] CopyTags(IReadOnlyList<string> tags)
        {
            if (tags == null || tags.Count == 0)
            {
                return Array.Empty<string>();
            }

            var result = new string[tags.Count];
            for (var i = 0; i < tags.Count; i++)
            {
                result[i] = tags[i];
            }

            return result;
        }

        private static bool IsMortarFragment(string projectileId)
        {
            return projectileId != null
                && projectileId.StartsWith("projectile.weapon.mortira.fragment", StringComparison.Ordinal);
        }

        private static bool IsMortarCascade(string projectileId)
        {
            return projectileId != null
                && projectileId.StartsWith(MortarCascadeProjectilePrefix, StringComparison.Ordinal);
        }


        private static bool IsMissileCluster(string projectileId)
        {
            return projectileId != null
                && projectileId.StartsWith(MissileClusterProjectilePrefix, StringComparison.Ordinal);
        }

        private static int GetMissileClusterCount(string projectileId)
        {
            if (!IsMissileCluster(projectileId))
            {
                return 0;
            }

            return int.TryParse(projectileId.Substring(MissileClusterProjectilePrefix.Length), out var count)
                ? Mathf.Max(0, count)
                : 0;
        }

        public void Spawn(in ProjectileSpawnRequest request)
        {
            if (request.Definition == null)
            {
                return;
            }

            var spawnedWeapon = ResolveWeaponFromProjectileId(request.Definition.ProjectileId);
            var sourceOrigin = spawnedWeapon?.WeaponId == SwapJumpWeaponId
                ? _state.GetUnit(request.OwnerUnitId)?.Position
                : (Vector2?)null;
            var projectile = new SimProjectile(
                ++_nextProjectileId,
                request.Definition,
                request.OwnerUnitId,
                request.Position,
                request.Velocity,
                request.Definition.TimeoutSeconds,
                sourceOrigin: sourceOrigin);
            if (spawnedWeapon?.WeaponId == BurRocketWeaponId)
            {
                var team = ResolveTeamForOwner(request.OwnerUnitId);
                var radiusPurchases = ProgressionFor(team).ResolveUpgradeCount(
                    spawnedWeapon.WeaponId, WeaponUpgradePath.Radius);
                projectile.ConfigureTerrainDrill(
                    WeaponUpgradeChoiceResolver.ResolveDrillRocketTerrainSeconds(radiusPurchases),
                    TerrainMaterial.Titanium);
            }
            else if (spawnedWeapon?.WeaponId == SkyHookWeaponId)
            {
                var launcher = _state.GetUnit(request.OwnerUnitId);
                projectile.SetSkyHookLaunchFacing(launcher != null && launcher.FacingDirectionX < 0 ? -1 : 1);
                projectile.SetSkyHookArming(SkyHookArmingSeconds);
            }
            else if (spawnedWeapon?.WeaponId == HoppingGrenadeWeaponId)
            {
                // One extra jump as standard; every Radius purchase adds another (at most five).
                var jumpPurchases = ProgressionFor(ResolveTeamForOwner(request.OwnerUnitId))
                    .ResolveUpgradeCount(spawnedWeapon.WeaponId, WeaponUpgradePath.Radius);
                projectile.ConfigureHops(1 + Mathf.Clamp(jumpPurchases, 0, 4));
            }
            else if (spawnedWeapon?.WeaponId == RailRifleWeaponId)
            {
                // The shared penetration budget P0 is one second of travel inside obstacles; the
                // purchased terrain_pierce steps add 10% of it each.
                var railSteps = ResolveEvolutionFor(ResolveTeamForOwner(request.OwnerUnitId), spawnedWeapon).RailPierceSteps;
                projectile.ConfigureTerrainDrill(
                    RailBasePierceSeconds * (1f + railSteps * RailPierceStepPercent), TerrainMaterial.Titanium);
            }
            else if (spawnedWeapon?.WeaponId == MissileWeaponId
                && !IsMissileCluster(request.Definition.ProjectileId))
            {
                var evolution = ResolveEvolutionFor(ResolveTeamForOwner(request.OwnerUnitId), spawnedWeapon);
                projectile.ConfigureMissileEvolution(
                    evolution.MissileSplitChargeCount,
                    Mathf.Max(MissileBaseObstacleBypasses, evolution.MissileObstacleBypassCount));
            }

            _projectiles.Add(projectile);
            ProjectileSpawned?.Invoke(projectile);
        }

        public void Schedule(in ProjectileSpawnRequest request, float delaySeconds)
        {
            if (request.Definition == null)
            {
                return;
            }

            if (delaySeconds <= 0f)
            {
                Spawn(request);
                return;
            }

            _scheduledProjectileSpawns.Add(new ScheduledProjectileSpawn(request, delaySeconds));
        }

        private void StepScheduledProjectileSpawns(float deltaTime)
        {
            for (var i = _scheduledProjectileSpawns.Count - 1; i >= 0; i--)
            {
                var scheduled = _scheduledProjectileSpawns[i];
                scheduled.RemainingDelay -= deltaTime;
                if (scheduled.RemainingDelay > 0f)
                {
                    continue;
                }

                _scheduledProjectileSpawns.RemoveAt(i);
                Spawn(scheduled.Request);
            }
        }

        private void StepProjectiles(float deltaTime)
        {
            for (var i = _projectiles.Count - 1; i >= 0; i--)
            {
                var projectile = _projectiles[i];
                if (_mortarSequences.TryGetValue(projectile.Id, out var mortarSequence))
                {
                    StepMortarFragment(i, projectile, mortarSequence, deltaTime);
                    continue;
                }
                var steppedWeaponId = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId)?.WeaponId;
                if (steppedWeaponId == RailRifleWeaponId)
                {
                    StepRailBullet(i, projectile, deltaTime);
                    continue;
                }
                if (steppedWeaponId == SkyHookWeaponId)
                {
                    StepSkyHook(i, projectile, deltaTime);
                    continue;
                }
                if (projectile.Definition.ProjectileId == BomberCopterProjectileId
                    || projectile.Definition.ProjectileId == CarrierCopterProjectileId)
                {
                    StepBomberCopter(i, projectile, deltaTime);
                    continue;
                }
                if (_guardianCopterShots.TryGetValue(projectile.Id, out var targetCopterId))
                {
                    StepGuardianCopterShot(i, projectile, targetCopterId, deltaTime);
                    continue;
                }
                if (!string.IsNullOrEmpty(projectile.GuaranteedTargetUnitId))
                {
                    StepGuaranteedTargetProjectile(i, projectile, deltaTime);
                    continue;
                }

                var projectileWeapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
                var isDrillRocket = projectileWeapon?.WeaponId == BurRocketWeaponId;
                var isControlledDrone = projectile.Id == _controlledProjectileId;
                var isLaserFenceDeploy = projectile.Definition.ProjectileId == LaserFenceDeployProjectileId;
                var grenadeEvolution = projectileWeapon?.WeaponId == GrenadeWeaponId
                    ? ResolveEvolutionFor(ResolveTeamForOwner(projectile.OwnerUnitId), projectileWeapon)
                    : WeaponEvolutionProfile.Base;
                var isStickyGrenade = projectileWeapon?.WeaponId == StickyGrenadeWeaponId
                    || grenadeEvolution.GrenadeSticks;
                var isHoppingGrenade = projectileWeapon?.WeaponId == HoppingGrenadeWeaponId;
                var isAttachmentDevice = IsAttachmentDevice(projectileWeapon);
                var isInstantTouchTeleport = projectileWeapon?.WeaponId == TeleportWeaponId
                    && ResolveEvolutionFor(ResolveTeamForOwner(projectile.OwnerUnitId), projectileWeapon)
                        .TeleportInstantTouch;

                if (projectile.IsRestingFuse)
                {
                    UpdateStickyGrenadeAttachment(projectile);
                    if (grenadeEvolution.GrenadeGravityPullRadius > 0f)
                    {
                        ApplyGravityGrenadePull(
                            projectile,
                            grenadeEvolution.GrenadeGravityPullRadius,
                            grenadeEvolution.GrenadePullAcceleration,
                            deltaTime);
                    }

                    projectile.ReduceRestingFuse(deltaTime);
                    if (projectile.RestingFuseRemaining <= 0f)
                    {
                        ResolveProjectileAt(i, projectile.Position);
                    }

                    continue;
                }

                if (isHoppingGrenade && projectile.HasHopFuse)
                {
                    projectile.ReduceHopFuse(deltaTime);
                    if (projectile.HopFuseRemaining <= 0f)
                    {
                        ResolveProjectileAt(i, projectile.Position);
                        continue;
                    }
                }

                var isMissile = projectileWeapon?.WeaponId == MissileWeaponId
                    && !IsMissileCluster(projectile.Definition.ProjectileId);
                if (!isMissile && !(isHoppingGrenade && projectile.HasHopFuse))
                {
                    projectile.ReduceLifetime(deltaTime);
                    if (projectile.RemainingLifetime <= 0f)
                    {
                        if (isLaserFenceDeploy)
                        {
                            DiscardProjectile(i, projectile);
                            continue;
                        }

                        ResolveProjectileAt(i, projectile.Position);
                        continue;
                    }
                }

                // The homing missile mirrors the reference: a short ballistic
                // free-flight phase, then gravity switches off and it steers
                // toward the target at constant speed.
                var isHomingMissile = isMissile;
                var isBallisticSplitMissile = isHomingMissile && projectile.MissileSplitChargeCount > 0;
                var homingActive = isHomingMissile;

                var velocity = isControlledDrone
                    ? _controlledProjectileInput * Mathf.Max(0.1f, projectileWeapon?.MaxPower ?? 1f)
                        + projectile.ExternalVelocity
                    : projectile.Velocity;
                if (!isControlledDrone && !homingActive)
                {
                    // Gravity also keeps acting inside terrain for the drill
                    // rocket, so its tunnels curve like in the reference.
                    velocity += _projectileGravityPerScale * projectile.Definition.GravityScale * deltaTime;
                }

                if (grenadeEvolution.GrenadeMagneticSeekRadius > 0f
                    && TryFindMagneticGrenadeTarget(
                        projectile,
                        grenadeEvolution.GrenadeMagneticSeekRadius,
                        out var magneticTarget))
                {
                    velocity = SteerGrenadeToward(velocity, projectile.Position, magneticTarget, deltaTime);
                }

                if (homingActive)
                {
                    velocity = isBallisticSplitMissile
                        ? ApplyBallisticMissileGuidance(projectile, velocity, deltaTime)
                        : ApplyProjectileSteering(projectile, velocity, deltaTime);
                }

                if (TryFindGrounderForProjectile(projectile, projectileWeapon,
                        projectile.Position, out var attractor))
                {
                    // Inside the magnet's range the carrier is taken over completely and flies straight in.
                    var toGrounder = attractor.Position - projectile.Position;
                    if (toGrounder.sqrMagnitude > 0.0001f)
                    {
                        velocity = toGrounder.normalized * Mathf.Max(GrounderCaptureSpeed, velocity.magnitude);
                    }
                }
                projectile.SetVelocity(velocity);
                if (isControlledDrone)
                {
                    projectile.DampenExternalVelocity(deltaTime, GravityGrenadeExternalVelocityDamping);
                }

                var position = projectile.Position;
                var displacement = velocity * deltaTime;
                var stepCount = Mathf.Max(1, Mathf.CeilToInt(displacement.magnitude / ProjectileSubStepLength));
                var step = displacement / stepCount;

                for (var stepIndex = 0; stepIndex < stepCount; stepIndex++)
                {
                    position += step;

                    if (TryFindForceFieldForProjectile(projectile, projectileWeapon, position, out var forceField))
                    {
                        // Absorbed whole: no blast and nothing reaches the droids; the device takes the charge's damage.
                        projectile.MoveTo(position);
                        ForceFieldAbsorbed?.Invoke(forceField.Id, position);
                        DamageForceField(forceField, projectile.Definition.Damage);
                        DiscardProjectile(i, projectile);
                        break;
                    }

                    if (TryFindGrounderForProjectile(projectile, projectileWeapon,
                            position, out var grounder))
                    {
                        if (Vector2.Distance(position, grounder.Position)
                            <= Mathf.Max(0.35f, projectile.Definition.Radius + 0.18f))
                        {
                            ResolveGrounderProjectileContact(i, projectile, projectileWeapon,
                                grounder, position);
                            break;
                        }

                        // Captured: neither ground nor a droid on the way stops it before the magnet.
                        projectile.MoveTo(position);
                        continue;
                    }

                    if (position.y < _settings.FallDeathY - 2f)
                    {
                        if (isLaserFenceDeploy)
                        {
                            DiscardProjectile(i, projectile);
                            break;
                        }

                        ResolveProjectileAt(i, position);
                        break;
                    }

                    // The owner is only shielded until the projectile clears
                    // their hitbox once; after that a clipping shot hurts them
                    // too (reference behaviour).
                    if (!projectile.HasClearedOwner
                        && !IsTouchingUnit(position, projectile.Definition.Radius, projectile.OwnerUnitId))
                    {
                        projectile.MarkClearedOwner();
                    }

                    var ignoredUnitId = projectile.HasClearedOwner ? null : projectile.OwnerUnitId;

                    // A hull is wrecked by a real hit: a charge that goes off on contact goes off on the hull. A
                    // grenade that only rolls or bounces past it does nothing; its later blast may still wreck it.
                    if (!isLaserFenceDeploy && _activeLaserFenceNodes.Count > 0
                        && DetonatesOnContact(projectileWeapon, projectile)
                        && TryWreckLaserFenceNodeTouchedBy(position, projectile.Definition.Radius))
                    {
                        ResolveProjectileAt(i, position);
                        break;
                    }

                    if ((isStickyGrenade || isAttachmentDevice)
                        && TryFindHitUnit(
                            position,
                            projectile.Definition.Radius,
                            ignoredUnitId,
                            out var stuckUnit,
                            out var stickyHitPoint))
                    {
                        projectile.MoveTo(stickyHitPoint);
                        projectile.StartRestingFuse(stickyHitPoint, StickyGrenadeFuseSeconds);
                        projectile.AttachToUnit(stuckUnit.UnitId, stuckUnit.Position);
                        break;
                    }

                    if (isBallisticSplitMissile
                        && projectile.IsBallisticMissileDescending
                        && TryFindTargetFor(projectile.OwnerUnitId, out var ballisticTarget)
                        && (ballisticTarget.Position - position).sqrMagnitude
                            <= BallisticMissileSplitDistance * BallisticMissileSplitDistance)
                    {
                        SplitBallisticMissile(i, projectile, projectileWeapon);
                        break;
                    }

                    if (isDrillRocket
                        && TryHitUnit(position, projectile.Definition.Radius, ignoredUnitId, out var drillHitPoint))
                    {
                        ResolveProjectileAt(i, drillHitPoint);
                        break;
                    }

                    var touchesTerrain = _terrain != null && _terrain.Grid.IsSolidWorld(position);
                    var traversesDrillableTerrain = isDrillRocket
                        && IsInsideDrillableTerrain(projectile, position, velocity, touchesTerrain);
                    if (traversesDrillableTerrain)
                    {
                        if (!projectile.IsTraversingTerrain)
                        {
                            projectile.EnterTerrain(DrillRocketTerrainSpeedRetain);
                        }

                        projectile.ConsumeTerrainDrillTime(deltaTime / stepCount);
                        // Carving uploads the destructible texture to the GPU;
                        // spacing the craters out keeps the tunnel continuous
                        // (radius >> spacing) without a hitch every sub-step.
                        if (projectile.ShouldCarveTunnelAt(position, DrillCarveSpacing))
                        {
                            ApplyTerrainCrater(
                                position,
                                DrillRocketTunnelRadius,
                                ResolveTeamForOwner(projectile.OwnerUnitId),
                                projectile.TerrainDrillPenetration);
                        }

                        projectile.MoveTo(position);
                        if (projectile.TerrainDrillTimeRemaining <= 0f)
                        {
                            ResolveProjectileAt(i, position);
                            break;
                        }

                        continue;
                    }

                    if (touchesTerrain)
                    {
                        if (isLaserFenceDeploy)
                        {
                            ResolveProjectileAt(i, position - step);
                            break;
                        }

                        if (isInstantTouchTeleport)
                        {
                            // The first contact with anything triggers at the last free point.
                            ResolveProjectileAt(i, position - step, hitTerrain: true);
                            break;
                        }

                        if (isStickyGrenade)
                        {
                            projectile.StartRestingFuse(position - step, StickyGrenadeFuseSeconds);
                            break;
                        }

                        if (isHoppingGrenade)
                        {
                            // Owner decision 2026-10-08: the first contact with ground blasts it; the extra hops are
                            // pressed while it is still in the air.
                            ConcludeHoppingGrenadeShot();
                            ResolveProjectileAt(i, position - step, hitTerrain: true);
                            break;
                        }

                        if (isAttachmentDevice)
                        {
                            // Attachment devices are only allowed to affect a
                            // nearby enemy, never terrain or empty space.
                            DiscardProjectile(i, projectile);
                            break;
                        }

                        if (TryBounceProjectile(projectile, position - step, position, velocity))
                        {
                            break;
                        }

                        if (projectileWeapon?.WeaponId is GrenadeWeaponId or CryoGrenadeWeaponId)
                        {
                            projectile.StartRestingFuse(position - step, projectile.RemainingLifetime);
                            break;
                        }

                        // Resolve at the last free sub-step point, matching the
                        // reference which explodes at the pre-collision position:
                        // craters bite the surface and radial damage is not
                        // attenuated by originating inside solid ground.
                        ResolveProjectileAt(i, position - step, hitTerrain: true);
                        break;
                    }

                    if (isDrillRocket && projectile.IsTraversingTerrain)
                    {
                        projectile.ExitTerrain();
                    }

                    var ignoresUnitsInFlight = projectileWeapon?.WeaponId == GrenadeWeaponId
                        || projectileWeapon?.WeaponId == CryoGrenadeWeaponId
                        || (projectileWeapon?.WeaponId == TeleportWeaponId && !isInstantTouchTeleport)
                        || projectileWeapon?.WeaponId == SwapJumpWeaponId
                        || projectile.Definition.ProjectileId == MineDeployProjectileId
                        || isLaserFenceDeploy;
                    if (!isDrillRocket
                        && !ignoresUnitsInFlight
                        && TryFindHitUnit(position, projectile.Definition.Radius, ignoredUnitId,
                            out var directHit, out var hitPoint,
                            // A bomb explodes on hostile droids only; it falls past its own side.
                            projectile.Definition.ProjectileId == BomberBombProjectileId
                                ? ResolveTeamForOwner(projectile.OwnerUnitId) : BattleTeamId.None))
                    {
                        if (isHoppingGrenade)
                        {
                            ConcludeHoppingGrenadeShot();
                        }

                        ResolveProjectileAt(i, hitPoint, directHit: directHit);
                        break;
                    }

                    if (stepIndex == stepCount - 1)
                    {
                        projectile.MoveTo(position);
                    }
                }
            }
        }

        /// <summary>
        /// Straight bullet that tunnels through ground and droids. Time spent inside an obstacle or a
        /// body spends the same penetration budget; each droid is hurt once per pass.
        /// </summary>
        /// <summary>
        /// Each following droid on the rail bullet's path takes 25% of the base less:
        /// 100% / 75% / 50% / 25%, and never below 25%.
        /// </summary>
        public static float ResolveRailPierceDamageFactor(int pierceOrdinal)
        {
            return Mathf.Max(RailPierceMinimumDamageFactor, 1f - (Mathf.Max(1, pierceOrdinal) - 1) * RailPierceDamageStep);
        }

        private void StepRailBullet(int projectileIndex, SimProjectile projectile, float deltaTime)
        {
            projectile.ReduceLifetime(deltaTime);
            if (projectile.RemainingLifetime <= 0f)
            {
                ConcludeProjectileShot(projectileIndex, projectile);
                return;
            }

            var weapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
            var sourceTeam = ResolveTeamForOwner(projectile.OwnerUnitId);
            var radius = Mathf.Max(0.05f, projectile.Definition.Radius);
            var position = projectile.Position;
            var displacement = projectile.Velocity * deltaTime;
            var stepCount = Mathf.Max(1, Mathf.CeilToInt(displacement.magnitude / ProjectileSubStepLength));
            var step = displacement / stepCount;
            var stepSeconds = deltaTime / stepCount;
            var carved = false;
            for (var stepIndex = 0; stepIndex < stepCount; stepIndex++)
            {
                position += step;
                if (position.y < _settings.FallDeathY - 2f
                    || (_terrain != null && (position.x < _terrain.Grid.WorldMinX - 2f || position.x > _terrain.Grid.WorldMaxX + 2f
                        || position.y > _terrain.Grid.WorldMaxY + 4f)))
                {
                    projectile.MoveTo(position);
                    ConcludeProjectileShot(projectileIndex, projectile);
                    return;
                }

                if (!projectile.HasClearedOwner && !IsTouchingUnit(position, radius, projectile.OwnerUnitId))
                {
                    projectile.MarkClearedOwner();
                }

                if (TryFindGrounderForProjectile(projectile, weapon, position, out var grounder)
                    && Vector2.Distance(position, grounder.Position) <= Mathf.Max(0.35f, radius + 0.18f))
                {
                    projectile.MoveTo(position);
                    DamageGrounder(grounder, 1);
                    ConcludeProjectileShot(projectileIndex, projectile);
                    return;
                }

                if (_activeLaserFenceNodes.Count > 0)
                {
                    TryWreckLaserFenceNodeTouchedBy(position, radius);
                }

                var insideBody = false;
                foreach (var team in _state.Teams)
                {
                    foreach (var unit in team.Units)
                    {
                        if (unit.IsDefeated
                            || (unit.UnitId == projectile.OwnerUnitId && !projectile.HasClearedOwner)
                            || !IsTouchingUnitHitbox(position, radius, unit))
                        {
                            continue;
                        }

                        insideBody = true;
                        if (projectile.TryMarkPierced(unit.UnitId))
                        {
                            var falloff = ResolveRailPierceDamageFactor(projectile.PiercedCount);
                            ApplyDamage(unit, Mathf.RoundToInt(
                                    ResolveOutgoingDamage(projectile.OwnerUnitId, projectile.Definition.Damage) * falloff),
                                projectile.Definition.ProjectileId, sourceTeam);
                        }
                    }
                }

                // The look-ahead keeps the bullet counted as inside ground while it moves along the
                // channel it has just carved.
                var heading = projectile.Velocity.sqrMagnitude > 0.0001f ? projectile.Velocity.normalized : Vector2.right;
                var insideGround = _terrain != null
                    && (_terrain.Grid.IsSolidWorld(position)
                        || _terrain.Grid.IsSolidWorld(position + heading * (RailTunnelRadius * 1.3f)));
                if (insideGround && projectile.ShouldCarveTunnelAt(position, DrillCarveSpacing))
                {
                    carved |= ApplyTerrainCrater(position, RailTunnelRadius, sourceTeam, TerrainMaterial.Titanium);
                }

                if (insideGround || insideBody)
                {
                    projectile.ConsumeTerrainDrillTime(stepSeconds);
                    if (projectile.TerrainDrillTimeRemaining <= 0f)
                    {
                        projectile.MoveTo(position);
                        if (carved)
                        {
                            WakeUnitsWithoutSupport();
                        }

                        ConcludeProjectileShot(projectileIndex, projectile);
                        return;
                    }
                }
            }

            projectile.MoveTo(position);
            if (carved)
            {
                WakeUnitsWithoutSupport();
            }
        }

        /// <summary>A bullet that simply runs out has no impact to end the shot, so it concludes it itself.</summary>
        private void ConcludeProjectileShot(int projectileIndex, SimProjectile projectile)
        {
            DiscardProjectile(projectileIndex, projectile);
            if (HasProjectilesInFlight || HasActiveWeaponEffect || !_activeShotEndsTurn)
            {
                return;
            }

            Result = BattleResultEvaluator.Evaluate(_state);
            if (Result.IsFinished)
            {
                UnitDefeated?.Invoke(new UnitDefeatedEvent(string.Empty, "battle-finished"));
                BattleFinished?.Invoke(Result, BattleFinishReason.ProjectileImpactResolved);
                return;
            }

            AdvanceTurnOrStartRetreat(TurnAdvanceReason.ProjectileImpactResolved);
        }

        private void StepGuaranteedTargetProjectile(int projectileIndex, SimProjectile projectile, float deltaTime)
        {
            // A dedicated magnet wins over the guaranteed hit: the bullet is a physical carrier.
            if (TryFindGrounderForProjectile(projectile,
                    ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId), projectile.Position, out var shield))
            {
                DamageGrounder(shield, 1);
                DiscardProjectile(projectileIndex, projectile);
                return;
            }

            var target = _state.GetUnit(projectile.GuaranteedTargetUnitId);
            if (target == null || target.IsDefeated)
            {
                DiscardProjectile(projectileIndex, projectile);
                return;
            }

            projectile.ReduceLifetime(deltaTime);
            var targetPoint = target.Position;
            var offset = targetPoint - projectile.Position;
            var distance = offset.magnitude;
            var speed = Mathf.Max(0.1f, projectile.Velocity.magnitude);
            if (distance <= speed * deltaTime + projectile.Definition.Radius || projectile.RemainingLifetime <= 0f)
            {
                ResolveGuardianDroneShot(projectileIndex, projectile, target);
                return;
            }

            var velocity = offset / distance * speed;
            projectile.SetVelocity(velocity);
            projectile.MoveTo(projectile.Position + velocity * deltaTime);
        }

        private bool IsInsideDrillableTerrain(
            SimProjectile projectile,
            Vector2 position,
            Vector2 velocity,
            bool centerIsSolid)
        {
            if (_terrain == null || (!centerIsSolid && !projectile.IsTraversingTerrain))
            {
                return false;
            }

            if (centerIsSolid)
            {
                return _terrain.Grid.GetMaterialWorld(position) <= projectile.TerrainDrillPenetration;
            }

            var direction = velocity.sqrMagnitude > 0.01f ? velocity.normalized : Vector2.right;
            var normal = new Vector2(-direction.y, direction.x);
            var sampleDistance = DrillRocketTunnelRadius * 1.15f;
            return IsDrillableTerrainAt(position + direction * sampleDistance, projectile.TerrainDrillPenetration)
                || IsDrillableTerrainAt(position + normal * sampleDistance, projectile.TerrainDrillPenetration)
                || IsDrillableTerrainAt(position - normal * sampleDistance, projectile.TerrainDrillPenetration);
        }

        private bool IsDrillableTerrainAt(Vector2 position, TerrainMaterial penetration)
        {
            return _terrain != null
                && _terrain.Grid.IsSolidWorld(position)
                && _terrain.Grid.GetMaterialWorld(position) <= penetration;
        }

        private void ResolveProjectileAt(int projectileIndex, Vector2 point,
            bool interceptedMine = false, BattleUnitState directHit = null,
            bool hitTerrain = false)
        {
            var projectile = _projectiles[projectileIndex];
            _projectiles.RemoveAt(projectileIndex);
            ReleaseMortarFragment(projectile.Id);

            var impactDamage = projectile.Definition.Damage;
            var impactWeapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
            if (interceptedMine && impactWeapon != null)
            {
                impactDamage = impactWeapon.Damage;
            }
            if (impactWeapon?.WeaponId == TitaniumDiscWeaponId && projectile.BounceCount > 0)
            {
                impactDamage = Mathf.RoundToInt(
                    impactDamage * (1f + TitaniumDiscDamageBonusPerBounce * projectile.BounceCount));
            }

            var impact = new ProjectileImpact(
                projectile.Definition.ProjectileId,
                projectile.OwnerUnitId,
                point,
                projectile.Velocity,
                impactDamage,
                projectile.Definition.ImpactRadius,
                projectile.Definition.KnockbackMultiplier,
                projectile.SourceOrigin);

            ProjectileExploded?.Invoke(projectile.Id, impact);
            if (projectile.Id == _controlledProjectileId)
            {
                _controlledProjectileId = -1;
                _controlledProjectileInput = Vector2.zero;
            }
            if (projectile.Id == _hoppingGrenadeProjectileId)
            {
                _hoppingGrenadeProjectileId = -1;
                _activeShotEndsTurn = true;
            }

            if (projectile.Definition.ProjectileId == MineDeployProjectileId && !interceptedMine)
            {
                LandMine(projectile, point);
                return;
            }

            if (projectile.Definition.ProjectileId == LaserFenceDeployProjectileId)
            {
                LandLaserFenceNode(projectile, point);
                return;
            }

            if (projectile.Definition.ProjectileId == GuardianDroneDeployProjectileId)
            {
                LandGuardianDrone(projectile, point);
                return;
            }

            if (impactWeapon?.WeaponId == VirusWeaponId)
            {
                if (!string.IsNullOrWhiteSpace(projectile.AttachedUnitId))
                {
                    InfectWithVirus(projectile.AttachedUnitId, projectile.OwnerUnitId, impactWeapon);
                }

                ImpactResolved?.Invoke(new ImpactResolvedEvent(point, impactWeapon, false));
                UnitsMoved?.Invoke();
                AdvanceTurnOrStartRetreat(TurnAdvanceReason.ProjectileImpactResolved);
                return;
            }

            if (impactWeapon?.WeaponId == RepairJammerWeaponId)
            {
                if (!string.IsNullOrWhiteSpace(projectile.AttachedUnitId))
                {
                    var ownerTeam = ResolveTeamForOwner(projectile.OwnerUnitId);
                    var duration = RepairJammerBaseDurationTurns
                        + ResolveEvolutionFor(ownerTeam, impactWeapon).RepairJammerDurationBonusTurns;
                    _repairJammerTurns[projectile.AttachedUnitId] = duration;
                }

                ImpactResolved?.Invoke(new ImpactResolvedEvent(point, impactWeapon, false));
                UnitsMoved?.Invoke();
                AdvanceTurnOrStartRetreat(TurnAdvanceReason.ProjectileImpactResolved);
                return;
            }

            ResolveProjectileImpact(impact, projectile.SuppressesTurnAdvanceOnImpact, directHit,
                hitTerrain ? projectile.Definition.Radius * 2f : 0f);
        }

        private void DiscardProjectile(int projectileIndex, SimProjectile projectile)
        {
            _projectiles.RemoveAt(projectileIndex);
            ReleaseMortarFragment(projectile.Id);
            if (projectile.Id == _hoppingGrenadeProjectileId)
            {
                _hoppingGrenadeProjectileId = -1;
                _activeShotEndsTurn = true;
            }
            ProjectileExploded?.Invoke(projectile.Id, new ProjectileImpact(
                projectile.Definition.ProjectileId,
                projectile.OwnerUnitId,
                projectile.Position,
                projectile.Velocity,
                0,
                0f));

            if (ResolveTeamForOwner(projectile.OwnerUnitId) == BattleTeamId.Opponent
                && _state.Turn.ActiveUnitId == projectile.OwnerUnitId
                && !_state.Turn.ActionSpent)
            {
                CompleteInstantAction();
            }
        }

        private void SplitBallisticMissile(
            int projectileIndex,
            SimProjectile projectile,
            WeaponDefinition weapon)
        {
            var chargeCount = Mathf.Max(2, projectile.MissileSplitChargeCount);
            _projectiles.RemoveAt(projectileIndex);
            var damage = Mathf.Max(1, Mathf.CeilToInt(weapon.Damage / (float)chargeCount));
            var impactRadius = weapon.ImpactRadius * 0.58f;
            var definition = ProjectileDefinition.CreateRuntime(
                MissileClusterProjectilePrefix + chargeCount,
                weapon.ProjectileRadius * 0.6f,
                weapon.GravityScale,
                3f,
                damage,
                impactRadius);

            for (var index = 0; index < chargeCount; index++)
            {
                var t = chargeCount > 1 ? index / (float)(chargeCount - 1) : 0.5f;
                var angle = Mathf.Lerp(-90f - MissileClusterSpreadDegrees, -90f + MissileClusterSpreadDegrees, t)
                    * Mathf.Deg2Rad;
                var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                Spawn(new ProjectileSpawnRequest(
                    definition,
                    projectile.OwnerUnitId,
                    projectile.Position + direction * 0.08f,
                    direction * MissileClusterSpeed,
                    definition.Radius * 2f,
                    ProjectileWeaponEffect.GetProjectileColor(weapon.Role)));
            }
        }

        private Vector2 ApplyBallisticMissileGuidance(SimProjectile projectile, Vector2 velocity, float deltaTime)
        {
            if (velocity.sqrMagnitude < 0.01f || !TryFindTargetFor(projectile.OwnerUnitId, out var target))
            {
                return velocity;
            }

            var speed = velocity.magnitude;
            if (projectile.IsBallisticMissileDescending)
            {
                var horizontal = Mathf.MoveTowards(velocity.x, target.Position.x - projectile.Position.x, BallisticMissileHorizontalSteerSpeed * deltaTime);
                return new Vector2(horizontal, -Mathf.Max(speed, MissileClusterSpeed));
            }

            var apexY = target.Position.y + 12f;
            if (projectile.Position.y >= apexY)
            {
                projectile.BeginBallisticMissileDescent();
                projectile.MoveTo(new Vector2(target.Position.x, apexY));
                return Vector2.down * Mathf.Max(speed, MissileClusterSpeed);
            }

            return Vector2.up * Mathf.Max(speed, MissileClusterSpeed);
        }

        private Vector2 ApplyProjectileSteering(SimProjectile projectile, Vector2 velocity, float deltaTime)
        {
            if (velocity.sqrMagnitude < 0.01f)
            {
                return velocity;
            }

            if (!TryFindTargetFor(projectile.OwnerUnitId, out var target))
            {
                return velocity;
            }

            var guidancePoint = ResolveMissileGuidancePoint(projectile, target.Position);
            var desired = guidancePoint - projectile.Position;
            if (desired.sqrMagnitude < 0.01f)
            {
                return velocity;
            }

            var speed = velocity.magnitude;
            var currentAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            var desiredAngle = Mathf.Atan2(desired.y, desired.x) * Mathf.Rad2Deg;
            var steeredAngle = Mathf.MoveTowardsAngle(currentAngle, desiredAngle, MissileSteerDegreesPerSecond * deltaTime);
            var angleRad = steeredAngle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * speed;
        }

        private Vector2 ResolveMissileGuidancePoint(SimProjectile projectile, Vector2 targetPoint)
        {
            if (projectile.HasMissileWaypoint)
            {
                if ((projectile.MissileWaypoint - projectile.Position).sqrMagnitude
                    > MissileDetourWaypointArrivalDistance * MissileDetourWaypointArrivalDistance)
                {
                    return projectile.MissileWaypoint;
                }

                projectile.ClearMissileWaypoint();
            }

            if (projectile.MissileObstacleBypassesRemaining > 0
                && TryFindTerrainOnLine(projectile.Position, targetPoint, out var obstaclePoint)
                && TryBuildMissileDetourWaypoint(projectile.Position, targetPoint, obstaclePoint, out var waypoint))
            {
                projectile.TrySetMissileWaypoint(waypoint);
                return waypoint;
            }

            return targetPoint;
        }

        private bool TryFindTerrainOnLine(Vector2 start, Vector2 end, out Vector2 hitPoint)
        {
            hitPoint = default;
            if (_terrain == null)
            {
                return false;
            }

            var delta = end - start;
            var distance = delta.magnitude;
            if (distance <= MissileDetourProbeStep)
            {
                return false;
            }

            var steps = Mathf.CeilToInt(distance / MissileDetourProbeStep);
            var step = delta / steps;
            for (var index = 1; index <= steps; index++)
            {
                var point = start + step * index;
                if (_terrain.Grid.IsSolidWorld(point))
                {
                    hitPoint = point;
                    return true;
                }
            }

            return false;
        }

        private bool TryBuildMissileDetourWaypoint(
            Vector2 start,
            Vector2 target,
            Vector2 obstaclePoint,
            out Vector2 waypoint)
        {
            waypoint = default;
            if (_terrain == null)
            {
                return false;
            }

            var forward = Mathf.Sign(target.x - start.x);
            if (Mathf.Abs(forward) < 0.01f)
            {
                forward = 1f;
            }

            var clearancePoint = obstaclePoint;
            for (var clearance = MissileDetourClearance;
                 clearance <= 8f;
                 clearance += MissileDetourClearance)
            {
                var candidate = obstaclePoint + Vector2.up * clearance;
                if (_terrain.Grid.IsSolidWorld(candidate))
                {
                    continue;
                }

                clearancePoint = candidate + Vector2.up * MissileDetourClearance;
                waypoint = new Vector2(clearancePoint.x - forward * MissileDetourClearance, clearancePoint.y);
                return !_terrain.Grid.IsSolidWorld(waypoint);
            }

            return false;
        }

        private bool TryBounceProjectile(
            SimProjectile projectile,
            Vector2 safePosition,
            Vector2 collisionPosition,
            Vector2 velocity)
        {
            var weapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
            var terrainBounceLimit = projectile.Definition.TerrainBounceLimit;
            var usesDefaultBounceRules = weapon != null
                && (weapon.WeaponId == TeleportWeaponId || weapon.WeaponId == SwapJumpWeaponId
                    || weapon.WeaponId == GrenadeWeaponId || weapon.WeaponId == CryoGrenadeWeaponId);
            if (!usesDefaultBounceRules && terrainBounceLimit <= 0)
            {
                return false;
            }

            var maxBounces = terrainBounceLimit > 0
                ? terrainBounceLimit
                : ProjectileBounceRules.MaxBounces;
            if (projectile.BounceCount >= maxBounces
                || velocity.magnitude < ProjectileBounceRules.RestingFuseMinSpeed)
            {
                return false;
            }

            var reflected = ProjectileBounceRules.ReflectOffTerrain(
                _terrain?.Grid,
                collisionPosition,
                velocity,
                projectile.Definition.Radius,
                ProjectileSubStepLength);
            projectile.RegisterBounce();
            projectile.MoveTo(safePosition);
            projectile.SetVelocity(reflected * ProjectileBounceRules.SpeedRetain);
            return true;
        }

        private void UpdateStickyGrenadeAttachment(SimProjectile projectile)
        {
            if (!projectile.IsAttachedToUnit)
            {
                return;
            }

            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit.UnitId != projectile.AttachedUnitId)
                    {
                        continue;
                    }

                    if (!unit.IsDefeated)
                    {
                        projectile.FollowAttachedUnit(unit.Position);
                        return;
                    }
                }
            }

            projectile.DetachFromUnit();
        }

        private void ApplyGravityGrenadePull(
            SimProjectile source,
            float radius,
            float acceleration,
            float deltaTime)
        {
            if (radius <= 0f || acceleration <= 0f)
            {
                return;
            }

            var center = source.Position;
            var radiusSquared = radius * radius;
            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit == null || unit.IsDefeated)
                    {
                        continue;
                    }

                    var offset = center - unit.Position;
                    var distanceSquared = offset.sqrMagnitude;
                    if (distanceSquared > radiusSquared || distanceSquared < GravityGrenadeMinimumDistance * GravityGrenadeMinimumDistance)
                    {
                        continue;
                    }

                    var falloff = 1f - Mathf.Sqrt(distanceSquared) / radius;
                    var impulse = offset.normalized * (acceleration * Mathf.SmoothStep(0f, 1f, falloff) * deltaTime);
                    GetMotor(unit.UnitId)?.AddImpulse(impulse);
                }
            }

            foreach (var projectile in _projectiles)
            {
                if (projectile == source || projectile.IsRestingFuse)
                {
                    continue;
                }

                var offset = center - projectile.Position;
                var distanceSquared = offset.sqrMagnitude;
                if (distanceSquared > radiusSquared || distanceSquared < GravityGrenadeMinimumDistance * GravityGrenadeMinimumDistance)
                {
                    continue;
                }

                var falloff = 1f - Mathf.Sqrt(distanceSquared) / radius;
                var pullVelocity = offset.normalized
                    * (acceleration * GravityGrenadeProjectilePullScale * Mathf.SmoothStep(0f, 1f, falloff) * deltaTime);
                if (projectile.Id == _controlledProjectileId)
                {
                    projectile.AddExternalVelocity(pullVelocity);
                }
                else
                {
                    projectile.SetVelocity(projectile.Velocity + pullVelocity);
                }
            }
        }

        private Vector2 SteerGrenadeToward(Vector2 velocity, Vector2 position, Vector2 target, float deltaTime)
        {
            var desired = target - position;
            if (desired.sqrMagnitude < 0.0001f || velocity.sqrMagnitude < 0.0001f)
            {
                return velocity;
            }

            return Vector2.MoveTowards(
                velocity,
                desired.normalized * velocity.magnitude,
                MagneticGrenadeSteerAcceleration * deltaTime);
        }

        private bool TryFindMagneticGrenadeTarget(SimProjectile projectile, float radius, out Vector2 target)
        {
            target = default;
            var bestDistanceSquared = radius * radius;
            var ownerTeam = ResolveTeamForOwner(projectile.OwnerUnitId);
            foreach (var team in _state.Teams)
            {
                if (team.TeamId == ownerTeam)
                {
                    continue;
                }

                foreach (var unit in team.Units)
                {
                    if (unit == null || unit.IsDefeated || unit.UnitId == projectile.OwnerUnitId)
                    {
                        continue;
                    }

                    var distanceSquared = (unit.Position - projectile.Position).sqrMagnitude;
                    if (distanceSquared >= bestDistanceSquared)
                    {
                        continue;
                    }

                    bestDistanceSquared = distanceSquared;
                    target = unit.Position;
                }
            }

            if (_terrain == null)
            {
                return bestDistanceSquared < radius * radius;
            }

            var grid = _terrain.Grid;
            var minimum = grid.WorldToCell(projectile.Position - Vector2.one * radius);
            var maximum = grid.WorldToCell(projectile.Position + Vector2.one * radius);
            for (var y = Mathf.Max(0, minimum.y); y <= Mathf.Min(grid.Height - 1, maximum.y); y++)
            {
                for (var x = Mathf.Max(0, minimum.x); x <= Mathf.Min(grid.Width - 1, maximum.x); x++)
                {
                    var material = grid.GetMaterialCell(x, y);
                    if (!grid.IsSolidCell(x, y)
                        || (material != TerrainMaterial.LightMetal && material != TerrainMaterial.Titanium))
                    {
                        continue;
                    }

                    var point = grid.CellToWorld(x, y);
                    var distanceSquared = (point - projectile.Position).sqrMagnitude;
                    if (distanceSquared >= bestDistanceSquared)
                    {
                        continue;
                    }

                    bestDistanceSquared = distanceSquared;
                    target = point;
                }
            }

            return bestDistanceSquared < radius * radius;
        }

        private bool TryFindTargetFor(string ownerUnitId, out BattleUnitState target)
        {
            target = null;
            if (string.IsNullOrWhiteSpace(ownerUnitId))
            {
                return false;
            }

            var owner = _state.GetUnit(ownerUnitId);
            if (owner == null)
            {
                return false;
            }

            var targetTeam = owner.TeamId == BattleTeamId.Player ? BattleTeamId.Opponent : BattleTeamId.Player;
            var bestDistance = float.PositiveInfinity;
            foreach (var unit in _state.GetTeam(targetTeam).Units)
            {
                if (unit == null || unit.IsDefeated)
                {
                    continue;
                }

                var distance = (unit.Position - owner.Position).sqrMagnitude;
                if (distance >= bestDistance)
                {
                    continue;
                }

                bestDistance = distance;
                target = unit;
            }

            return target != null;
        }

        /// <summary>Box-overlap test against one specific unit (used to detect
        /// when a projectile leaves its owner's hitbox after launch).</summary>
        private bool IsTouchingUnit(Vector2 point, float radius, string unitId)
        {
            if (string.IsNullOrEmpty(unitId))
            {
                return false;
            }

            var unit = _state.GetUnit(unitId);
            if (unit == null || unit.IsDefeated)
            {
                return false;
            }

            return IsTouchingUnitHitbox(point, radius, unit);
        }

        private bool TryHitUnit(Vector2 point, float radius, string ignoredUnitId, out Vector2 hitPoint)
        {
            return TryFindHitUnit(point, radius, ignoredUnitId, out _, out hitPoint);
        }

        private bool TryFindHitUnit(
            Vector2 point,
            float radius,
            string ignoredUnitId,
            out BattleUnitState hitUnit,
            out Vector2 hitPoint,
            BattleTeamId ignoredTeam = BattleTeamId.None)
        {
            hitUnit = null;
            hitPoint = point;
            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit.IsDefeated || unit.UnitId == ignoredUnitId
                        || (ignoredTeam != BattleTeamId.None && unit.TeamId == ignoredTeam))
                    {
                        continue;
                    }

                    if (IsTouchingUnitHitbox(point, radius, unit))
                    {
                        hitUnit = unit;
                        hitPoint = unit.Position + (point - unit.Position) * 0.5f;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsTouchingUnitHitbox(Vector2 point, float radius, BattleUnitState unit)
        {
            var halfHeight = ArchetypeFor(unit.TeamId).BodyHeight * 0.5f;
            var lowerY = unit.Position.y - halfHeight - radius;
            var upperY = unit.Position.y + halfHeight + _settings.UnitCollisionTopPadding + radius;
            return Mathf.Abs(point.x - unit.Position.x) <= _settings.UnitCollisionHalfWidth + radius
                && point.y >= lowerY
                && point.y <= upperY;
        }

        private WeaponDefinition ResolveImpactWeapon(ProjectileImpact impact)
        {
            var weaponFromProjectile = ResolveWeaponFromProjectileId(impact.ProjectileId);
            return weaponFromProjectile ?? _activeShotWeapon ?? _playerInventory.Selected;
        }

        private WeaponDefinition ResolveWeaponFromProjectileId(string projectileId)
        {
            if (projectileId == MineDeployProjectileId)
            {
                return _weaponsById.TryGetValue(MineWeaponId, out var mine) ? mine : null;
            }

            if (projectileId == BomberBombProjectileId)
            {
                return _weaponsById.TryGetValue(BomberWeaponId, out var bomber) ? bomber : null;
            }

            if (projectileId == CarrierFallProjectileId)
            {
                return _weaponsById.TryGetValue(CarrierWeaponId, out var carrierWeapon) ? carrierWeapon : null;
            }

            if (projectileId == LaserFenceDeployProjectileId)
            {
                return _weaponsById.TryGetValue(LaserFenceWeaponId, out var laserFence) ? laserFence : null;
            }

            if (projectileId == GuardianDroneDeployProjectileId || projectileId == GuardianDroneShotProjectileId)
            {
                return _weaponsById.TryGetValue(GuardianDroneWeaponId, out var guardian) ? guardian : null;
            }

            if (projectileId != null
                && (projectileId.StartsWith("projectile.weapon.mortira.fragment", StringComparison.Ordinal)
                    || projectileId.StartsWith(MortarCascadeProjectilePrefix, StringComparison.Ordinal)))
            {
                return _weaponsById.TryGetValue("weapon.mortira", out var mortar) ? mortar : null;
            }

            if (IsMissileCluster(projectileId))
            {
                return _weaponsById.TryGetValue(MissileWeaponId, out var missile) ? missile : null;
            }

            if (!string.IsNullOrWhiteSpace(projectileId)
                && projectileId.StartsWith("projectile.", StringComparison.Ordinal))
            {
                var weaponId = projectileId.Substring("projectile.".Length);
                if (_weaponsById.TryGetValue(weaponId, out var weapon))
                {
                    return weapon;
                }
            }

            return null;
        }

        private sealed class ScheduledProjectileSpawn
        {
            public ScheduledProjectileSpawn(ProjectileSpawnRequest request, float delaySeconds)
            {
                Request = request;
                RemainingDelay = delaySeconds;
            }

            public ProjectileSpawnRequest Request { get; }
            public float RemainingDelay { get; set; }
        }
    }
}
