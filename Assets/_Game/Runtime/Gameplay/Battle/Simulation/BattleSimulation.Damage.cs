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
        private void ApplyLinearDamageAndTerrain(ActiveTimedEffect effect, float range, int damage, float craterRadius, bool canHitOwner)
        {
            var endpoint = ResolveLineEndpoint(effect.Origin, effect.Direction, range);
            if (_terrain != null && craterRadius > 0f)
            {
                ApplyTerrainCrater(endpoint, craterRadius, effect.TeamId);
            }

            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit == null || unit.IsDefeated || (!canHitOwner && unit.UnitId == effect.OwnerUnitId))
                    {
                        continue;
                    }

                    var distance = DistanceToSegment(unit.Position, effect.Origin, endpoint);
                    if (distance > 0.7f)
                    {
                        continue;
                    }

                    if (damage > 0)
                    {
                        ApplyDamage(unit, damage, effect.Weapon.WeaponId, effect.TeamId);
                    }
                }
            }

            UnitsMoved?.Invoke();
        }

        /// <summary>
        /// Burns a channel along the whole beam. Crater centers are spaced so
        /// the farthest crater's edge lands exactly on the beam tip: terrain
        /// never crumbles ahead of the laser, and destruction stops together
        /// with the beam (the beam is drawn through walls at its full length,
        /// so everything it overlaps must burn).
        /// </summary>
        private void ApplyLinearTerrain(ActiveTimedEffect effect, float beamLength, float craterRadius)
        {
            if (_terrain == null)
            {
                return;
            }

            var carveEnd = beamLength - craterRadius;
            for (var distance = 0.45f; distance <= carveEnd; distance += craterRadius)
            {
                ApplyTerrainCrater(
                    effect.Origin + effect.Direction * distance,
                    craterRadius,
                    effect.TeamId,
                    effect.DrillPenetration);
            }

            UnitsMoved?.Invoke();
        }

        private Vector2 ResolveLineEndpoint(Vector2 origin, Vector2 direction, float range)
        {
            var endpoint = origin + direction.normalized * range;
            if (_terrain != null && _terrain.Grid.Raycast(origin, endpoint, out var hitPoint))
            {
                return hitPoint;
            }

            return endpoint;
        }

        private void ApplyPointDamage(
            Vector2 point,
            WeaponDefinition weapon,
            string ownerUnitId,
            float radius,
            int damage,
            bool applyLavaOverlay = false,
            int attackId = 0)
        {
            var sourceTeam = ResolveTeamForOwner(ownerUnitId);
            var fireEvolution = applyLavaOverlay ? ResolveEvolutionFor(sourceTeam, weapon) : default;
            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit == null || unit.IsDefeated)
                    {
                        continue;
                    }

                    if (Vector2.Distance(unit.Position, point) > radius)
                    {
                        continue;
                    }

                    var resolvedDamage = damage;
                    if (applyLavaOverlay
                        && ApplyBurn(unit, sourceTeam, fireEvolution, out var burnWasAlreadyActive)
                        && burnWasAlreadyActive)
                    {
                        resolvedDamage = Mathf.CeilToInt(
                            resolvedDamage * (1f + fireEvolution.FireGunChainDamagePercent / 100f));
                    }

                    StreamHit streamHit = null;
                    if (applyLavaOverlay && attackId > 0)
                    {
                        var key = (attackId, unit.UnitId);
                        if (!_streamHits.TryGetValue(key, out streamHit))
                        {
                            // The first drop of this attack on this droid: a fire status left by an EARLIER attack boosts
                            // this whole attack's direct damage (shared +25%), later drops of the same attack do not stack it.
                            streamHit = new StreamHit
                            {
                                Boosted = _fireEffects.TryGetValue(unit.UnitId, out var earlier) && earlier.AttackId != attackId
                            };
                            _streamHits[key] = streamHit;
                        }

                        if (streamHit.Boosted)
                        {
                            resolvedDamage = Mathf.CeilToInt(
                                resolvedDamage * (1f + ResolveRepeatBonusPercent(sourceTeam, FireGunWeaponId) / 100f));
                        }
                    }

                    var healthBefore = unit.Health;
                    ApplyDamage(unit, resolvedDamage, weapon.WeaponId, sourceTeam);
                    if (applyLavaOverlay)
                    {
                        _lavaOverlayRemainingSeconds[unit.UnitId] = LavaOverlayDurationSeconds;
                        if (streamHit != null)
                        {
                            // The shared fire status; its tick is 20% of everything this attack dealt to the droid.
                            streamHit.Total += Mathf.Max(0, healthBefore - unit.Health);
                            var streamStatusTurns = ResolveElementStatusTurns(sourceTeam, weapon.WeaponId);
                            ApplyElementalStatus(_fireEffects, unit, sourceTeam, streamHit.Total, attackId, streamStatusTurns);
                            // The burning movement lock: 1 turn, 2 with the second step, +1 per Radius purchase.
                            if (fireEvolution.ElementControlSteps > 0)
                            {
                                ApplyDisorientation(unit.UnitId, fireEvolution.ElementControlSteps
                                    + streamStatusTurns - ElementalBaseDurationTurns);
                            }
                        }
                    }
                }
            }

            Result = BattleResultEvaluator.Evaluate(_state);
            if (Result.IsFinished)
            {
                BattleFinished?.Invoke(Result, BattleFinishReason.ProjectileImpactResolved);
            }
        }

        private static float DistanceToSegment(Vector2 point, Vector2 segmentStart, Vector2 segmentEnd)
        {
            var segment = segmentEnd - segmentStart;
            var lengthSq = segment.sqrMagnitude;
            if (lengthSq <= 0.0001f)
            {
                return Vector2.Distance(point, segmentStart);
            }

            var t = Mathf.Clamp01(Vector2.Dot(point - segmentStart, segment) / lengthSq);
            return Vector2.Distance(point, segmentStart + segment * t);
        }

        private void RestoreFriendlyHealth(Vector2 impactPoint, string ownerUnitId, WeaponDefinition impactWeapon)
        {
            if (string.IsNullOrWhiteSpace(ownerUnitId))
            {
                return;
            }

            var owner = _state.GetUnit(ownerUnitId);
            if (owner == null || owner.IsDefeated || IsUnitRepairJammed(owner.UnitId)) return;
            var restored = owner.RestoreHealth(ResolveRepairHealAmount(owner, impactWeapon));
            ApplyRepairAftereffect(owner, owner.TeamId, impactWeapon);
            HealthRestored?.Invoke(new HealthRestoredEvent(owner.Position, restored, owner.TeamId));
        }

        private int ResolveRepairHealAmount(BattleUnitState unit, WeaponDefinition weapon)
        {
            if (weapon.WeaponId != "weapon.heal") return weapon.Damage;
            var purchases = ProgressionFor(unit.TeamId).ResolveUpgradeCount(weapon.WeaponId, WeaponUpgradePath.Core);
            return Mathf.RoundToInt(unit.MaxHealth * WeaponUpgradeChoiceResolver.ResolveRepairHealPercent(purchases) / 100f);
        }

        private bool ResolveImpact(ProjectileImpact impact, WeaponDefinition impactWeapon)
        {
            if (impactWeapon == null)
            {
                return false;
            }

            switch (impactWeapon.Role)
            {
                case WeaponRole.CoverTool:
                    DeployCover(impact.Point, impactWeapon);
                    return false;
                case WeaponRole.DefensiveTool:
                    RestoreFriendlyHealth(impact.Point, impact.OwnerUnitId, impactWeapon);
                    return false;
                case WeaponRole.MovementTool when impactWeapon.WeaponId == CarrierWeaponId:
                    // The carrier's end-of-flight blast is an ordinary blast; it must never move its owner there.
                    return ResolveDamageImpact(impact, impactWeapon);
                case WeaponRole.MovementTool:
                    RepositionOwner(impact.Point, impact.OwnerUnitId, impactWeapon, impact.SourceOrigin);
                    return false;
                default:
                    return ResolveDamageImpact(impact, impactWeapon);
            }
        }

        private bool ResolveDamageImpact(
            ProjectileImpact impact,
            WeaponDefinition impactWeapon,
            BattleTeamId ignoredTeam = BattleTeamId.None)
        {
            var terrainChanged = ApplyTerrainDestruction(
                impact.Point,
                impactWeapon,
                ResolveTeamForOwner(impact.OwnerUnitId), impact.ImpactRadius);

            var allUnits = new List<BattleUnitState>();
            foreach (var team in _state.Teams)
            {
                if (team.TeamId == ignoredTeam)
                {
                    continue;
                }

                allUnits.AddRange(team.Units);
            }

            var sourceTeam = ResolveTeamForOwner(impact.OwnerUnitId);
            var effectiveDamage = ResolveOutgoingDamage(impact.OwnerUnitId, impact.Damage);

            var appliesPoison = impactWeapon.WeaponId == ToxicWeaponId;
            var poisonEvolution = appliesPoison ? ResolveEvolutionFor(sourceTeam, impactWeapon) : default;
            if (impactWeapon.WeaponId == StickyGrenadeWeaponId)
            {
                // The purchased toxic release turns the blast into the shared poison cycle.
                appliesPoison = ResolveEvolutionFor(sourceTeam, impactWeapon).StickyToxicRelease;
            }
            var spiderElement = impactWeapon.WeaponId == SpiderMineWeaponId
                ? ResolveEvolutionFor(sourceTeam, impactWeapon).SpiderElement
                : SpiderElement.None;
            if (spiderElement == SpiderElement.Poison)
            {
                // The chosen element runs the shared cycle of the matching grenade.
                appliesPoison = true;
                poisonEvolution = ResolveEvolutionFor(sourceTeam, impactWeapon);
            }
            var appliesEmpStun = impactWeapon.WeaponId == EmpWeaponId;
            var empEvolution = appliesEmpStun ? ResolveEvolutionFor(sourceTeam, impactWeapon) : default;
            // The cannon's base is the same shared cold as the cryo grenade's (and the cold spider's).
            var appliesChill = impactWeapon.WeaponId == CryoGrenadeWeaponId || spiderElement == SpiderElement.Cold
                || impactWeapon.WeaponId == FreezeWeaponId;
            var appliesFire = impactWeapon.WeaponId == IncendiaryGrenadeWeaponId || spiderElement == SpiderElement.Fire;
            var appliesElectric = impactWeapon.WeaponId == EmpWeaponId;
            var appliesFreeze = impactWeapon.WeaponId == FreezeWeaponId;
            var freezeEvolution = appliesFreeze ? ResolveEvolutionFor(sourceTeam, impactWeapon) : default;
            // The element cannons' Radius purchases lengthen their status (and their purchased controls) by a turn each.
            var statusTurns = ResolveElementStatusTurns(sourceTeam, impactWeapon.WeaponId);
            var cannonExtraTurns = statusTurns - ElementalBaseDurationTurns;
            var movementPoisonPerSecond = impactWeapon.WeaponId == ToxicWeaponId
                ? ResolveMovementPoisonPerSecond(sourceTeam, impactWeapon, poisonEvolution)
                : 0f;

            const float selfDamageMultiplier = 1f;
            var settings = new RadialDamageSettings(effectiveDamage, impact.ImpactRadius, selfDamageMultiplier);
            var damageEvents = DamageResolver.ResolveRadialDamage(
                allUnits,
                impact.Point,
                impact.OwnerUnitId,
                impact.ProjectileId,
                settings,
                (unit, amount) => ResolvePoisonRepeatBonus(appliesPoison, unit,
                    ResolveElementalRepeatBonus(
                        _electricEffects, appliesElectric, unit,
                        ResolveElementalRepeatBonus(
                            _fireEffects, appliesFire, unit,
                            ResolveElementalRepeatBonus(
                                _chillEffects, appliesChill, unit,
                                ResolveFreezeChainDamage(ResolveIncomingDamage(unit, amount), unit, appliesFreeze, freezeEvolution),
                                ResolveRepeatBonusPercent(sourceTeam, FreezeWeaponId)),
                            ResolveRepeatBonusPercent(sourceTeam, FireGunWeaponId)),
                        ElementalRepeatBonusPercent),
                    ResolveRepeatBonusPercent(sourceTeam, ToxicWeaponId)));
            foreach (var damageEvent in damageEvents)
            {
                DamageApplied?.Invoke(sourceTeam, damageEvent);
                var target = FindUnit(damageEvent.TargetUnitId);
                if (appliesPoison)
                {
                    ApplyPoison(target, sourceTeam, damageEvent.Damage, poisonEvolution, statusTurns, movementPoisonPerSecond);
                }

                if (appliesChill)
                {
                    ApplyElementalStatus(_chillEffects, target, sourceTeam, damageEvent.Damage, turns: statusTurns);
                    // The full freeze of the cryo cannon: 1 turn, 2 with the second step, +1 per Radius purchase.
                    if (appliesFreeze && freezeEvolution.ElementControlSteps > 0)
                    {
                        ApplyFreeze(target?.UnitId, freezeEvolution.ElementControlSteps + cannonExtraTurns);
                    }
                }

                if (appliesFire)
                {
                    ApplyElementalStatus(_fireEffects, target, sourceTeam, damageEvent.Damage, turns: statusTurns);
                }

                if (appliesElectric)
                {
                    ApplyElementalStatus(_electricEffects, target, sourceTeam, damageEvent.Damage, turns: statusTurns);
                    // The aim jam (1 → 2 turns) and the movement lock (2 → 3 turns), each +1 per Radius purchase.
                    if (empEvolution.ElementControlSteps > 0)
                    {
                        ApplyAimJam(target?.UnitId, empEvolution.ElementControlSteps + cannonExtraTurns);
                    }

                    if (empEvolution.ElementSecondFeatureSteps > 0)
                    {
                        ApplyDisorientation(target?.UnitId, 1 + empEvolution.ElementSecondFeatureSteps + cannonExtraTurns);
                    }
                }

                if (appliesEmpStun)
                {
                    ApplyElectrifiedOverlay(target?.UnitId);
                    if (empEvolution.EmpStunTurns > 0)
                    {
                        ApplyEmpStun(target?.UnitId, empEvolution.EmpStunTurns);
                        if (empEvolution.EmpStunBlocksMovement)
                        {
                            ApplyDisorientation(target?.UnitId, empEvolution.EmpStunTurns);
                        }
                    }

                    if (empEvolution.EmpWeakenDamagePercent > 0)
                    {
                        ApplyEmpWeaken(target?.UnitId, empEvolution.EmpWeakenDamagePercent, empEvolution.EmpWeakenTurns);
                    }
                }

                // The old movement-blocking freeze only survives in a legacy saved branch; the base cannon no longer
                // blocks anything (when the full freeze opens is an open owner question).
                if (appliesFreeze && freezeEvolution.CryoFreezeTurns > 0f)
                {
                    var freezeTurns = freezeEvolution.CryoFreezeTurns > 0f
                        ? Mathf.RoundToInt(freezeEvolution.CryoFreezeTurns)
                        : CryoBaseFreezeTurns;
                    ApplyFreeze(target?.UnitId, freezeTurns);
                    if (target != null)
                    {
                        _lastFreezeApplicationTurn[target.UnitId] = _turnSequence;
                    }
                }
            }
            DamageLaserFenceNodesInBlast(impact.Point, impact.ImpactRadius, effectiveDamage);
            DamagePlacedDevicesInBlast(impact.Point, impact.ImpactRadius, effectiveDamage);
            ApplyExplosionKnockback(
                impact.Point,
                impact.Velocity,
                impactWeapon,
                terrainChanged,
                impact.KnockbackMultiplier,
                ignoredTeam);
            return terrainChanged;
        }

        /// <summary>Shots that go off when they touch something, as opposed to thrown bodies that bounce, roll or rest.</summary>
        private static bool DetonatesOnContact(WeaponDefinition weapon, SimProjectile projectile)
        {
            if (projectile.Definition.ProjectileId == BomberBombProjectileId)
            {
                return true;
            }

            return weapon?.WeaponId is "weapon.impulse" or EmpWeaponId or FreezeWeaponId or ToxicWeaponId
                or "weapon.ion-shotgun" or BlasterWeaponId or "weapon.missle" or OrbitalRocketWeaponId
                or NuclearChargeWeaponId;
        }

        private void ApplyRepairAftereffect(
            BattleUnitState unit,
            BattleTeamId teamId,
            WeaponDefinition repairWeapon)
        {
            var evolution = ResolveEvolutionFor(teamId, repairWeapon);
            if (evolution.RepairProtectionTurns <= 0 && evolution.RepairRegenerationTurns <= 0)
            {
                return;
            }

            _repairEffects[unit.UnitId] = new ActiveRepairEffect(
                unit.UnitId,
                teamId,
                ResolveRepairHealAmount(unit, repairWeapon),
                evolution.RepairProtectionTurns,
                evolution.RepairRegenerationTurns);
        }

        /// <summary>
        /// Cryo's B branch: a cryo hit landing on a target this attacker already
        /// froze on their previous turn (still within <see cref="CryoChainWindowTurnSequences"/>
        /// turn-sequence steps) deals bonus damage - rewards refreezing the same
        /// target instead of a persistent multi-turn debuff.
        /// </summary>
        private int ResolveFreezeChainDamage(int amount, BattleUnitState unit, bool appliesFreeze, WeaponEvolutionProfile freezeEvolution)
        {
            if (!appliesFreeze || amount <= 0 || unit == null || freezeEvolution.CryoChainDamagePercent <= 0)
            {
                return amount;
            }

            if (_lastFreezeApplicationTurn.TryGetValue(unit.UnitId, out var lastTurn)
                && _turnSequence - lastTurn <= CryoChainWindowTurnSequences)
            {
                return Mathf.CeilToInt(amount * (1f + freezeEvolution.CryoChainDamagePercent / 100f));
            }

            return amount;
        }

        private int ResolveIncomingDamage(BattleUnitState unit, int amount)
        {
            if (unit == null || amount <= 0)
            {
                return 0;
            }

            var resolvedAmount = amount;
            if (_repairEffects.TryGetValue(unit.UnitId, out var effect))
            {
                if (effect.ProtectionTurnsRemaining > 0)
                {
                    resolvedAmount = Mathf.CeilToInt(resolvedAmount * ProtectiveRepairDamageMultiplier);
                }

                if (resolvedAmount > 0)
                {
                    effect.DamageTakenSinceLastCheck = true;
                }
            }

            if (_poisonEffects.TryGetValue(unit.UnitId, out var poison)
                && poison.ArmorWeakenTurnsRemaining > 0
                && poison.ArmorWeakenPercent > 0)
            {
                resolvedAmount = Mathf.CeilToInt(resolvedAmount * (1f + poison.ArmorWeakenPercent / 100f));
            }

            return Mathf.Max(0, resolvedAmount);
        }

        private void ApplyDamage(
            BattleUnitState target,
            int amount,
            string sourceId,
            BattleTeamId sourceTeam,
            bool canBeProtected = true)
        {
            if (target == null || target.IsDefeated || amount <= 0)
            {
                return;
            }

            var resolvedAmount = canBeProtected ? ResolveIncomingDamage(target, amount) : amount;
            if (resolvedAmount <= 0)
            {
                return;
            }

            var damageEvent = target.ApplyDamage(resolvedAmount, sourceId);
            DamageApplied?.Invoke(sourceTeam, damageEvent);
        }

        private int ResolveOutgoingDamage(string ownerUnitId, int damage)
        {
            if (_empWeakens.TryGetValue(ownerUnitId, out var empWeaken)
                && empWeaken.DamagePercent > 0)
            {
                return Mathf.Max(0, Mathf.FloorToInt(
                    damage * (1f - empWeaken.DamagePercent / 100f)));
            }

            return damage;
        }

        private bool ApplyTerrainDestruction(Vector2 impactPoint, WeaponDefinition impactWeapon,
            BattleTeamId sourceTeam, float radiusOverride = -1f)
        {
            if (_terrain == null || impactWeapon == null)
            {
                return false;
            }

            var blastRadius = radiusOverride > 0f ? radiusOverride : impactWeapon.ImpactRadius;
            var craterRadius = impactWeapon.WeaponId == C4WeaponId
                ? blastRadius * 0.62f
                : Mathf.Clamp(blastRadius * 0.62f, 0.38f, 1.55f);
            return ApplyTerrainCrater(impactPoint, craterRadius, sourceTeam);
        }

        private bool ApplyTerrainCrater(
            Vector2 point,
            float radius,
            BattleTeamId sourceTeam,
            TerrainMaterial maximumMaterial = TerrainMaterial.Titanium)
        {
            if (_terrain == null)
            {
                return false;
            }

            var destroyedCellCount = _terrain.ApplyCraterAndGetDestroyedCellCount(point, radius, maximumMaterial);
            if (destroyedCellCount <= 0)
            {
                return false;
            }

            TerrainCellsDestroyed?.Invoke(sourceTeam, destroyedCellCount);
            TerrainCraterApplied?.Invoke(sourceTeam, point, radius);
            return true;
        }

        private BattleTeamId ResolveTeamForOwner(string ownerUnitId)
        {
            if (string.IsNullOrWhiteSpace(ownerUnitId))
            {
                return BattleTeamId.None;
            }

            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit != null && unit.UnitId == ownerUnitId)
                    {
                        return team.TeamId;
                    }
                }
            }

            return BattleTeamId.None;
        }

        private void ApplyExplosionKnockback(
            Vector2 impactPoint,
            Vector2 impactVelocity,
            WeaponDefinition impactWeapon,
            bool terrainChanged,
            float knockbackMultiplier = 1f,
            BattleTeamId ignoredTeam = BattleTeamId.None,
            BattleUnitState onlyTarget = null,
            float radiusOverride = -1f)
        {
            if (impactWeapon == null)
            {
                return;
            }

            var blastRadius = radiusOverride > 0f ? radiusOverride : impactWeapon.ImpactRadius;
            var radius = Mathf.Max(0.65f, blastRadius + _settings.KnockbackLandingSearchPadding);
            var radiusScale = Mathf.InverseLerp(0.75f, 2.4f, blastRadius);
            var incoming = impactVelocity.sqrMagnitude > 0.01f ? impactVelocity.normalized : Vector2.zero;
            var downwardImpact01 = Mathf.InverseLerp(-0.15f, -0.85f, incoming.y);

            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (team.TeamId == ignoredTeam
                        || unit == null
                        || unit.IsDefeated
                        || (onlyTarget != null && unit != onlyTarget)
                        || Mathf.Abs(unit.Position.x - impactPoint.x) > radius)
                    {
                        continue;
                    }

                    var offset = unit.Position - impactPoint;
                    var distance = offset.magnitude;
                    if (distance > radius)
                    {
                        continue;
                    }

                    var falloff = Mathf.Clamp01(1f - (distance / radius));
                    falloff = Mathf.SmoothStep(0f, 1f, falloff);
                    if (falloff <= 0.03f && !terrainChanged)
                    {
                        continue;
                    }

                    var radial = offset.sqrMagnitude > 0.0001f ? offset.normalized : Vector2.up;
                    var side = Mathf.Abs(radial.x) > 0.08f
                        ? Mathf.Sign(radial.x)
                        : (Mathf.Abs(incoming.x) > 0.08f ? Mathf.Sign(incoming.x) : unit.FacingDirectionX);
                    var horizontalSpeed = Mathf.Lerp(1.05f, 3.65f, falloff) * Mathf.Lerp(0.86f, 1.16f, radiusScale);
                    var verticalFromBlast = Mathf.Clamp(radial.y, -0.52f, Mathf.Lerp(0.72f, 0.22f, downwardImpact01));
                    var verticalSpeed = verticalFromBlast
                        * Mathf.Lerp(0.65f, 2.55f, falloff)
                        * Mathf.Lerp(0.88f, 1.08f, radiusScale);
                    if (verticalSpeed > 0f)
                    {
                        verticalSpeed *= Mathf.Lerp(1f, 0.28f, downwardImpact01);
                    }

                    var velocity = new Vector2(side * horizontalSpeed, verticalSpeed);
                    if (incoming.sqrMagnitude > 0.01f)
                    {
                        velocity.x += incoming.x * Mathf.Lerp(0.08f, 0.45f, falloff);
                    }

                    velocity *= Mathf.Max(0f, knockbackMultiplier);

                    if (velocity.sqrMagnitude < 0.02f)
                    {
                        continue;
                    }

                    GetMotor(unit.UnitId)?.AddImpulse(velocity);
                }
            }

            if (!terrainChanged)
            {
                return;
            }

            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (!unit.IsDefeated)
                    {
                        GetMotor(unit.UnitId)?.WakeIfUnsupported();
                    }
                }
            }
        }
    }
}
