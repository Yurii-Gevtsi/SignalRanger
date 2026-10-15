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
        public void ConfigureTitaniumBarrier(Texture2D texture, float referenceCameraSize)
        {
            _titaniumBarrierTexture = texture;
            TitaniumBarrierBaseSize = texture != null
                ? TitaniumBarrierRules.BaseSize(referenceCameraSize, (float)texture.width / texture.height)
                : Vector2.zero;
        }

        public float ResolveTitaniumBarrierScale(float requested, BattleTeamId team)
        {
            var progression = ProgressionFor(team);
            return TitaniumBarrierRules.ClampScale(requested,
                progression.HasPurchasedEvolution(TitaniumBarrierRules.WeaponId, 5, TitaniumBarrierRules.SizeFeatureId),
                progression.ResolveUpgradeCount(TitaniumBarrierRules.WeaponId, WeaponUpgradePath.Radius));
        }

        private void StepLaserFenceLinks()
        {
            foreach (var node in _activeLaserFenceNodes)
            {
                if (node.FallPending && !node.IsCarried)
                {
                    node.FallPending = false;
                    if (TryFindGroundObjectLanding(node.Position, out var landing))
                    {
                        node.MoveTo(landing);
                        GroundObjectRepositioned?.Invoke(new GroundObjectRepositionedEvent(node.Id, landing));
                    }
                }
            }

            if (_activeLaserFenceNodes.Count < 2 || Result.IsFinished)
            {
                _currentLaserFenceLinks.Clear();
                return;
            }

            var deployedNodes = new List<LaserFenceNodeDeployedEvent>(_activeLaserFenceNodes.Count);
            for (var index = 0; index < _activeLaserFenceNodes.Count; index++)
            {
                var node = _activeLaserFenceNodes[index];
                deployedNodes.Add(new LaserFenceNodeDeployedEvent(
                    node.Id,
                    node.OwnerUnitId,
                    node.Position,
                    node.Weapon,
                    node.TeamId));
            }

            var links = LaserFenceLinkResolver.Resolve(deployedNodes, GetLaserFenceLinkRange);
            _currentLaserFenceLinks = links;
            for (var linkIndex = 0; linkIndex < links.Count; linkIndex++)
            {
                var link = links[linkIndex];
                var linkId = GetLaserFenceLinkId(link.FirstNodeId, link.SecondNodeId);
                if (_spentLaserFenceLinks.Contains(linkId)
                    || !TryGetLaserFenceNode(link.FirstNodeId, out var firstNode)
                    || !TryGetLaserFenceNode(link.SecondNodeId, out var secondNode))
                {
                    continue;
                }

                for (var teamIndex = 0; teamIndex < _state.Teams.Count; teamIndex++)
                {
                    var team = _state.Teams[teamIndex];
                    if (team.TeamId == firstNode.TeamId)
                    {
                        continue;
                    }

                    for (var unitIndex = 0; unitIndex < team.Units.Count; unitIndex++)
                    {
                        var unit = team.Units[unitIndex];
                        if (unit.IsDefeated
                            || !IsUnitBodyCrossingLaserFenceLink(unit, firstNode.Position, secondNode.Position))
                        {
                            continue;
                        }

                        _spentLaserFenceLinks.Add(linkId);
                        LaserFenceLinkSpent?.Invoke(link.FirstNodeId, link.SecondNodeId);
                        ApplyDamage(unit, firstNode.Weapon.Damage, LaserFenceWeaponId, firstNode.TeamId);
                        if (unit.UnitId == _state.Turn.ActiveUnitId)
                        {
                            AdvanceTurnNow(TurnAdvanceReason.LaserFenceTriggered);
                        }

                        return;
                    }
                }
            }
        }

        /// <summary>Maximum beam length of this owner's hulls: 5.4, +10% of it per Radius purchase.</summary>
        public float GetLaserFenceLinkRange(string ownerUnitId)
        {
            var radiusPurchases = ProgressionFor(ResolveTeamForOwner(ownerUnitId))
                .ResolveUpgradeCount(LaserFenceWeaponId, WeaponUpgradePath.Radius);
            return LaserFenceLinkRange * (1f + Mathf.Clamp(radiusPurchases, 0, 4) * LaserFenceLinkRangeStep);
        }

        private bool TryGetLaserFenceNode(int nodeId, out ActiveLaserFenceNode node)
        {
            for (var index = 0; index < _activeLaserFenceNodes.Count; index++)
            {
                if (_activeLaserFenceNodes[index].Id == nodeId)
                {
                    node = _activeLaserFenceNodes[index];
                    return true;
                }
            }

            node = null;
            return false;
        }

        private static ulong GetLaserFenceLinkId(int firstNodeId, int secondNodeId)
        {
            var low = (uint)Mathf.Min(firstNodeId, secondNodeId);
            var high = (uint)Mathf.Max(firstNodeId, secondNodeId);
            return ((ulong)high << 32) | low;
        }

        /// <summary>
        /// The beam runs near the ground while a droid's position is the centre of its body: the beam hits a droid
        /// when it passes through the body (with a small margin), wherever along its height.
        /// </summary>
        private bool IsUnitBodyCrossingLaserFenceLink(BattleUnitState unit, Vector2 first, Vector2 second)
        {
            var halfWidth = _settings.UnitCollisionHalfWidth + LaserFenceBodyMargin;
            var halfHeight = ArchetypeFor(unit.TeamId).BodyHeight * 0.5f + LaserFenceBodyMargin;
            var length = Vector2.Distance(first, second);
            var samples = Mathf.Max(2, Mathf.CeilToInt(length / 0.05f));
            for (var index = 0; index <= samples; index++)
            {
                var point = Vector2.Lerp(first, second, index / (float)samples);
                if (Mathf.Abs(point.x - unit.Position.x) <= halfWidth && Mathf.Abs(point.y - unit.Position.y) <= halfHeight)
                {
                    return true;
                }
            }

            return IsPointWithinLaserFenceLink(unit.Position, first, second);
        }

        private static bool IsPointWithinLaserFenceLink(Vector2 point, Vector2 first, Vector2 second)
        {
            var link = second - first;
            var linkLengthSquared = link.sqrMagnitude;
            if (linkLengthSquared <= 0.0001f)
            {
                return (point - first).sqrMagnitude <= LaserFenceContactRadius * LaserFenceContactRadius;
            }

            var projection = Mathf.Clamp01(Vector2.Dot(point - first, link) / linkLengthSquared);
            var nearestPoint = first + link * projection;
            return (point - nearestPoint).sqrMagnitude <= LaserFenceContactRadius * LaserFenceContactRadius;
        }

        private bool CanContinueMineDeploymentThisTurn()
        {
            if (_minesDeployedThisTurn == 0)
            {
                return true;
            }

            if (!_weaponsById.TryGetValue(MineWeaponId, out var mineWeapon))
            {
                return false;
            }

            var activeTeam = _state.Turn.ActiveTeam;
            var inventory = GetInventoryFor(activeTeam);
            return inventory.GetRemainingAmmo(mineWeapon) > 0;
        }

        private bool CanContinueLaserFenceDeploymentThisTurn()
        {
            if (_laserFenceNodesDeployedThisTurn >= MaxLaserFenceNodesPerTurn
                || !_weaponsById.TryGetValue(LaserFenceWeaponId, out var laserFence))
            {
                return false;
            }

            return GetInventoryFor(_state.Turn.ActiveTeam).GetRemainingAmmo(laserFence) > 0;
        }

        private BattleFireResult DeployMine(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var facing = unit.FacingDirectionX >= 0 ? 1f : -1f;
            var origin = unit.Position + new Vector2(facing * 0.28f, 0.46f);
            var definition = ProjectileDefinition.CreateRuntime(
                MineDeployProjectileId,
                weapon.ProjectileRadius,
                weapon.GravityScale,
                weapon.TimeoutSeconds,
                0,
                weapon.ImpactRadius);
            Spawn(new ProjectileSpawnRequest(
                definition,
                unit.UnitId,
                origin,
                new Vector2(facing * MineThrowHorizontalSpeed, MineThrowVerticalSpeed),
                weapon.ProjectileRadius * 2f,
                ProjectileWeaponEffect.GetProjectileColor(weapon.Role)));
            _activeShotWeapon = weapon;
            _activeShotEndsTurn = false;
            _minesDeployedThisTurn++;
            ShotFired?.Invoke(unit.TeamId, weapon, false);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon, origin);
        }

        private BattleFireResult DeployGuardianDrone(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var facing = unit.FacingDirectionX >= 0 ? 1f : -1f;
            var origin = unit.Position + new Vector2(facing * 0.28f, 0.46f);
            var definition = ProjectileDefinition.CreateRuntime(
                GuardianDroneDeployProjectileId,
                weapon.ProjectileRadius,
                // A placed trap must fall even when a compact runtime test
                // definition omits gravity; the grenade arc is not its aim mode.
                Mathf.Max(1f, weapon.GravityScale),
                weapon.TimeoutSeconds,
                0,
                weapon.ImpactRadius);
            Spawn(new ProjectileSpawnRequest(
                definition,
                unit.UnitId,
                origin,
                new Vector2(facing * MineThrowHorizontalSpeed, MineThrowVerticalSpeed),
                weapon.ProjectileRadius * 2f,
                ProjectileWeaponEffect.GetProjectileColor(weapon.Role)));
            _activeShotWeapon = weapon;
            _activeShotEndsTurn = false;
            ShotFired?.Invoke(unit.TeamId, weapon, false);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon, origin);
        }

        private BattleFireResult DeployGrounder(
            BattleUnitState unit,
            WeaponInventoryRuntime inventory,
            WeaponDefinition weapon)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            // The placed device owns its later interception behavior. Placement itself
            // is an action, not a defensive heal through the generic utility path.
            var facing = unit.FacingDirectionX >= 0 ? 1f : -1f;
            var position = ResolveGroundDeployPoint(
                unit.Position + new Vector2(facing * 0.46f, 0.12f), unit.TeamId);
            var coreUpgrades = Mathf.Clamp(
                ProgressionFor(unit.TeamId).ResolveUpgradeCount(weapon.WeaponId, WeaponUpgradePath.Core), 0, 4);
            var radiusUpgrades = Mathf.Clamp(
                ProgressionFor(unit.TeamId).ResolveUpgradeCount(weapon.WeaponId, WeaponUpgradePath.Radius), 0, 4);
            var grounder = new ActiveGrounder(
                ++_nextProjectileId, unit.TeamId, position, weapon,
                remainingTurns: 2 + coreUpgrades, health: unit.MaxHealth,
                detectionRadius: weapon.ImpactRadius * PlacedDeviceRadiusMultiplier * (1f + radiusUpgrades * 0.10f));
            _activeGrounders.Add(grounder);
            GrounderDeployed?.Invoke(new GrounderDeployedEvent(
                grounder.Id, grounder.Position, weapon, unit.TeamId));
            PlacedDeviceHealthChanged?.Invoke(grounder.Id, grounder.Health, grounder.MaxHealth);
            // Owner decision 2026-10-09: setting the magnet down does not end the turn.
            _activeShotWeapon = weapon;
            _activeShotEndsTurn = false;
            ShotFired?.Invoke(unit.TeamId, weapon, false);
            UnitsMoved?.Invoke();
            return new BattleFireResult(BattleFireOutcome.UtilityUsed, unit.TeamId, weapon, position);
        }

        private BattleFireResult DeployForceField(
            BattleUnitState unit,
            WeaponInventoryRuntime inventory,
            WeaponDefinition weapon)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            // Owner decision 2026-10-08: the device is set on the ground beside the droid and the sphere surrounds the
            // device: the shared nearby distance N0, +10% of it per Radius purchase.
            var progression = ProgressionFor(unit.TeamId);
            var coreUpgrades = Mathf.Clamp(progression.ResolveUpgradeCount(weapon.WeaponId, WeaponUpgradePath.Core), 0, 4);
            var radiusUpgrades = Mathf.Clamp(progression.ResolveUpgradeCount(weapon.WeaponId, WeaponUpgradePath.Radius), 0, 4);
            var facing = unit.FacingDirectionX >= 0 ? 1f : -1f;
            var devicePoint = ResolveGroundDeployPoint(unit.Position + new Vector2(facing * 0.46f, 0.12f), unit.TeamId);
            var field = new ActiveForceField(
                ++_nextProjectileId, unit.TeamId, devicePoint, weapon,
                remainingTurns: 2 + coreUpgrades,
                radius: weapon.ImpactRadius * PlacedDeviceRadiusMultiplier * (1f + radiusUpgrades * 0.10f),
                health: unit.MaxHealth);
            _activeForceFields.Add(field);
            ForceFieldDeployed?.Invoke(new ForceFieldDeployedEvent(
                field.Id, field.Position, field.Radius, weapon, unit.TeamId));
            PlacedDeviceHealthChanged?.Invoke(field.Id, field.Health, field.MaxHealth);
            // Like the magnet, it is set down on the ground and the turn goes on.
            _activeShotWeapon = weapon;
            _activeShotEndsTurn = false;
            ShotFired?.Invoke(unit.TeamId, weapon, false);
            UnitsMoved?.Invoke();
            return new BattleFireResult(BattleFireOutcome.UtilityUsed, unit.TeamId, weapon, devicePoint);
        }

        private BattleFireResult DeployLaserFenceNode(
            BattleUnitState unit,
            WeaponInventoryRuntime inventory,
            WeaponDefinition weapon,
            AimState aim)
        {
            if (_laserFenceNodesDeployedThisTurn >= MaxLaserFenceNodesPerTurn)
            {
                return new BattleFireResult(BattleFireOutcome.Blocked, unit.TeamId, weapon);
            }

            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var direction = aim.Direction.sqrMagnitude > 0.01f ? aim.Direction.normalized : Vector2.right;
            var definition = ProjectileDefinition.CreateRuntime(
                LaserFenceDeployProjectileId,
                weapon.ProjectileRadius,
                LaserFenceGravityScale,
                weapon.TimeoutSeconds,
                0,
                weapon.ImpactRadius);
            var origin = aim.Origin + direction * 0.45f;
            Spawn(new ProjectileSpawnRequest(
                definition,
                unit.UnitId,
                origin,
                direction * aim.Power,
                weapon.ProjectileRadius * 2f,
                ProjectileWeaponEffect.GetProjectileColor(weapon.Role)));

            _activeShotWeapon = weapon;
            _activeShotEndsTurn = false;
            _laserFenceNodesDeployedThisTurn++;
            ShotFired?.Invoke(unit.TeamId, weapon, false);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon, origin);
        }

        private BattleFireResult DeploySpiderMine(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var facing = unit.FacingDirectionX >= 0 ? 1 : -1;
            var placement = ResolveGroundDeployPoint(unit.Position + new Vector2(facing * 0.46f, 0.12f), unit.TeamId);
            var mine = new ActiveMine(
                ++_nextProjectileId,
                unit.UnitId,
                unit.TeamId,
                weapon,
                placement + Vector2.up * 0.04f,
                triggersEnemiesOnly: true,
                alliesImmuneToExplosion: true,
                isSpider: true,
                crawlDirection: facing);
            _activeMines.Add(mine);
            MineDeployed?.Invoke(new MineDeployedEvent(mine.Id, mine.Position, weapon, unit.TeamId));
            // Releasing the spider is the attack of the turn: no other weapon afterwards, and the turn
            // passes once the crawler has blown up.
            _activeShotWeapon = weapon;
            _activeShotEndsTurn = true;
            _turnService.SpendAction(_state);
            ShotFired?.Invoke(unit.TeamId, weapon, true);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon, mine.Position);
        }

        private BattleFireResult DeployC4(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var facing = unit.FacingDirectionX >= 0 ? 1f : -1f;
            var origin = unit.Position + new Vector2(facing * 0.42f, 0.52f);
            var deployPoint = ResolveGroundDeployPoint(
                unit.Position + new Vector2(facing * C4DeployDistance, 0.08f),
                unit.TeamId);
            // In an enclosed pocket the surface scan starts above the roof and
            // "teleports" the charge on top of it. Keep it at the unit's feet.
            var feetY = unit.Position.y - ArchetypeFor(unit.TeamId).BodyHeight * 0.5f;
            if (deployPoint.y > feetY + 0.6f)
            {
                deployPoint = new Vector2(unit.Position.x + facing * 0.3f, feetY + 0.06f);
            }
            var evolution = ResolveEvolutionFor(unit.TeamId, weapon);
            var evolvedWeapon = CreateC4EvolutionWeapon(weapon, evolution, ResolveModifiersFor(unit.TeamId, weapon));
            var charge = new ActiveCharge(
                ++_nextProjectileId,
                unit.UnitId,
                unit.TeamId,
                evolvedWeapon,
                deployPoint,
                C4FuseSeconds,
                1.5f * evolution.KnockbackMultiplier);
            _activeCharges.Add(charge);
            var dropDistance = Mathf.Max(0f, origin.y - deployPoint.y);
            var dropDuration = Mathf.Clamp(C4DropDurationSeconds + dropDistance * 0.08f, C4DropDurationSeconds, 0.75f);
            ChargeDeployed?.Invoke(new ChargeDeployedEvent(
                charge.Id,
                origin,
                deployPoint,
                dropDuration,
                evolvedWeapon,
                unit.TeamId));
            ShotFired?.Invoke(unit.TeamId, weapon, false);
            return new BattleFireResult(BattleFireOutcome.UtilityUsed, unit.TeamId, weapon, deployPoint);
        }

        private void StepMines(float deltaTime)
        {
            if (Result.IsFinished)
            {
                return;
            }

            for (var i = _activeMines.Count - 1; i >= 0; i--)
            {
                var mine = _activeMines[i];
                if (mine.IsCarried)
                {
                    // Held by the carrier: it cannot trigger, and its armed state is kept as it was.
                    continue;
                }

                if (mine.IsSpider)
                {
                    StepSpiderMine(mine, deltaTime);
                }
                else
                {
                    DropGroundObjectIfUnsupported(mine, mine.Id);
                    mine.ArmTimeRemaining = Mathf.Max(0f, mine.ArmTimeRemaining - deltaTime);
                }

                if (mine.ArmTimeRemaining > 0f)
                {
                    continue;
                }

                if (mine.IsGuardian)
                {
                    StepGuardianDrone(mine, deltaTime, i);
                    continue;
                }

                if (!mine.Triggered && !mine.IsSpider && IsMineTriggerTargetInside(mine))
                {
                    mine.Triggered = true;
                    mine.TriggerTimeRemaining = MineTriggerDelaySeconds;
                    MineTriggered?.Invoke(mine.Id);
                }

                if (!mine.Triggered)
                {
                    continue;
                }

                mine.TriggerTimeRemaining = Mathf.Max(0f, mine.TriggerTimeRemaining - deltaTime);
                if (mine.TriggerTimeRemaining > 0f)
                {
                    continue;
                }

                _activeMines.RemoveAt(i);
                ResolveMineExplosion(mine);
            }
        }

        private void StepGuardianDrone(ActiveMine guardian, float deltaTime, int guardianIndex)
        {
            if (!guardian.Triggered && TryFindMineTriggerTarget(guardian, out var target))
            {
                guardian.Triggered = true;
                guardian.TriggerTargetUnitId = target.UnitId;
                guardian.TriggerTimeRemaining = GuardianDroneRiseSeconds;
                MineTriggered?.Invoke(guardian.Id);
            }
            else if (!guardian.Triggered && TryFindHostileCopterNear(guardian.TeamId, guardian.Position,
                         ResolveMineTriggerRange(guardian), out var copterProjectileId))
            {
                // A hostile copter flying into range is shot down like a droid would be.
                guardian.Triggered = true;
                guardian.TriggerCopterProjectileId = copterProjectileId;
                guardian.TriggerTimeRemaining = GuardianDroneRiseSeconds;
                MineTriggered?.Invoke(guardian.Id);
            }

            if (!guardian.Triggered)
            {
                return;
            }

            guardian.TriggerTimeRemaining = Mathf.Max(0f, guardian.TriggerTimeRemaining - deltaTime);
            if (guardian.TriggerTimeRemaining > 0f)
            {
                return;
            }

            _activeMines.RemoveAt(guardianIndex);
            if (guardian.TriggerCopterProjectileId >= 0)
            {
                SpawnGuardianDroneCopterShot(guardian);
                return;
            }

            SpawnGuardianDroneShot(guardian);
        }

        private bool TryFindHostileCopterNear(BattleTeamId team, Vector2 point, float range, out int copterProjectileId)
        {
            copterProjectileId = -1;
            if (_copter == null || ResolveTeamForOwner(_copter.OwnerUnitId) == team)
            {
                return false;
            }

            var copter = _projectiles.Find(value => value.Id == _copter.CopterProjectileId);
            if (copter == null || Vector2.Distance(copter.Position, point) > range)
            {
                return false;
            }

            copterProjectileId = copter.Id;
            return true;
        }

        private void SpawnGuardianDroneCopterShot(ActiveMine guardian)
        {
            var copter = _projectiles.Find(value => value.Id == guardian.TriggerCopterProjectileId);
            if (copter == null)
            {
                return;
            }

            var origin = guardian.Position + Vector2.up * GuardianDroneRiseHeight;
            var direction = (copter.Position - origin).sqrMagnitude > 0.0001f
                ? (copter.Position - origin).normalized
                : Vector2.up;
            var definition = ProjectileDefinition.CreateRuntime(
                GuardianDroneShotProjectileId,
                Mathf.Max(0.04f, guardian.Weapon.ProjectileRadius * 0.38f),
                0f,
                2f,
                0,
                0f);
            var shot = new SimProjectile(
                ++_nextProjectileId,
                definition,
                guardian.OwnerUnitId,
                origin,
                direction * GuardianDroneShotSpeed,
                definition.TimeoutSeconds,
                suppressTurnAdvanceOnImpact: true);
            _projectiles.Add(shot);
            _guardianCopterShots[shot.Id] = copter.Id;
            ProjectileSpawned?.Invoke(shot);
        }

        /// <summary>The guardian's bullet homes on the copter; the copter is brought down on its own step.</summary>
        private void StepGuardianCopterShot(int projectileIndex, SimProjectile projectile, int copterProjectileId, float deltaTime)
        {
            var copter = _projectiles.Find(value => value.Id == copterProjectileId);
            projectile.ReduceLifetime(deltaTime);
            if (copter == null || projectile.RemainingLifetime <= 0f)
            {
                _guardianCopterShots.Remove(projectile.Id);
                DiscardProjectile(projectileIndex, projectile);
                return;
            }

            var offset = copter.Position - projectile.Position;
            var distance = offset.magnitude;
            var speed = Mathf.Max(0.1f, projectile.Velocity.magnitude);
            if (distance <= speed * deltaTime + projectile.Definition.Radius + 0.2f)
            {
                _guardianCopterShots.Remove(projectile.Id);
                _projectiles.RemoveAt(projectileIndex);
                ProjectileExploded?.Invoke(projectile.Id, new ProjectileImpact(
                    projectile.Definition.ProjectileId, projectile.OwnerUnitId, copter.Position, projectile.Velocity, 0, 0f));
                _downedCopterProjectileIds.Add(copterProjectileId);
                return;
            }

            var velocity = offset / distance * speed;
            projectile.SetVelocity(velocity);
            projectile.MoveTo(projectile.Position + velocity * deltaTime);
        }

        /// <summary>A hostile laser beam the copter flies into is spent on it and brings the copter down.</summary>
        private bool TrySpendHostileLaserBeamOnCopter(SimProjectile copter)
        {
            if (_currentLaserFenceLinks.Count == 0)
            {
                return false;
            }

            var copterTeam = ResolveTeamForOwner(copter.OwnerUnitId);
            foreach (var link in _currentLaserFenceLinks)
            {
                var linkId = GetLaserFenceLinkId(link.FirstNodeId, link.SecondNodeId);
                if (_spentLaserFenceLinks.Contains(linkId)
                    || !TryGetLaserFenceNode(link.FirstNodeId, out var first)
                    || !TryGetLaserFenceNode(link.SecondNodeId, out var second)
                    || first.TeamId == copterTeam
                    || !IsPointWithinLaserFenceLink(copter.Position, first.Position, second.Position))
                {
                    continue;
                }

                _spentLaserFenceLinks.Add(linkId);
                LaserFenceLinkSpent?.Invoke(link.FirstNodeId, link.SecondNodeId);
                return true;
            }

            return false;
        }

        private void SpawnGuardianDroneShot(ActiveMine guardian)
        {
            var target = _state.GetUnit(guardian.TriggerTargetUnitId);
            if (target == null || target.IsDefeated)
            {
                return;
            }

            var origin = guardian.Position + Vector2.up * GuardianDroneRiseHeight;
            var direction = (target.Position - origin).normalized;
            if (direction.sqrMagnitude < 0.001f)
            {
                direction = Vector2.right;
            }

            var definition = ProjectileDefinition.CreateRuntime(
                GuardianDroneShotProjectileId,
                Mathf.Max(0.04f, guardian.Weapon.ProjectileRadius * 0.38f),
                0f,
                2f,
                guardian.Weapon.Damage,
                0f);
            var projectile = new SimProjectile(
                ++_nextProjectileId,
                definition,
                guardian.OwnerUnitId,
                origin,
                direction * GuardianDroneShotSpeed,
                definition.TimeoutSeconds,
                suppressTurnAdvanceOnImpact: true,
                guaranteedTargetUnitId: target.UnitId);
            _projectiles.Add(projectile);
            ProjectileSpawned?.Invoke(projectile);
        }

        private void StepSpiderMine(ActiveMine spider, float deltaTime)
        {
            if (spider.Triggered)
            {
                return;
            }

            spider.SearchTimeRemaining = Mathf.Max(0f, spider.SearchTimeRemaining - deltaTime);
            if (TryFindSpiderTarget(spider, out var target))
            {
                spider.MoveTo(target.Position);
                spider.Triggered = true;
                spider.TriggerTimeRemaining = SpiderAttachFuseSeconds;
                GroundObjectRepositioned?.Invoke(new GroundObjectRepositionedEvent(spider.Id, spider.Position));
                MineTriggered?.Invoke(spider.Id);
                return;
            }

            var nextX = spider.Position.x + spider.CrawlDirection * SpiderCrawlSpeed * deltaTime;
            var scanTop = spider.Position.y + SpiderClimbHeight;
            var hitsWall = _terrain != null && _terrain.Grid.IsSolidWorld(new Vector2(nextX, scanTop));
            var groundedPosition = Vector2.zero;
            // The crawler stands on its whole body, so a crack narrower than it is bridged instead of swallowing it.
            var hasGround = !hitsWall && _terrain != null
                && TryFindSpiderFooting(nextX, scanTop, out var surfaceY)
                && (groundedPosition = new Vector2(nextX, surfaceY + 0.04f)).x == nextX;
            if (!hasGround)
            {
                // A wall that is too tall, a drop that is too deep or the map edge: the crawler blows up
                // at the point where it can go no further.
                spider.MoveTo(hitsWall ? spider.Position : new Vector2(nextX, spider.Position.y));
                spider.Triggered = true;
                spider.TriggerTimeRemaining = 0f;
                GroundObjectRepositioned?.Invoke(new GroundObjectRepositionedEvent(spider.Id, spider.Position));
                MineTriggered?.Invoke(spider.Id);
                return;
            }

            spider.MoveTo(groundedPosition);
            GroundObjectRepositioned?.Invoke(new GroundObjectRepositionedEvent(spider.Id, spider.Position));
            if (spider.SearchTimeRemaining <= 0f)
            {
                spider.Triggered = true;
                spider.TriggerTimeRemaining = 0f;
                MineTriggered?.Invoke(spider.Id);
            }
        }

        private bool TryFindSpiderFooting(float centerX, float scanTop, out float surfaceY)
        {
            surfaceY = float.MinValue;
            var found = false;
            foreach (var offset in SpiderFootOffsets)
            {
                if (_terrain.Grid.TryFindSurfaceBelow(centerX + offset, scanTop, SpiderClimbHeight + SpiderDropHeight,
                        out var columnSurface)
                    && columnSurface > surfaceY)
                {
                    surfaceY = columnSurface;
                    found = true;
                }
            }

            return found;
        }

        private bool TryFindSpiderTarget(ActiveMine spider, out BattleUnitState target)
        {
            target = null;
            var targetTeam = spider.TeamId == BattleTeamId.Player ? BattleTeamId.Opponent : BattleTeamId.Player;
            foreach (var candidate in _state.GetTeam(targetTeam).Units)
            {
                if (candidate == null || candidate.IsDefeated)
                {
                    continue;
                }

                // The crawler is on the ground and a standing droid's centre is half a body above it, so the
                // droid is reached when it is level with the crawler horizontally and its body spans the height.
                var halfHeight = GetMotor(candidate.UnitId)?.Profile.HalfHeight ?? ArchetypeFor(candidate.TeamId).BodyHeight * 0.5f;
                var offset = candidate.Position - spider.Position;
                if (Mathf.Abs(offset.x) > SpiderAttachDistance
                    || offset.y < -0.1f || offset.y > halfHeight + 0.3f)
                {
                    continue;
                }

                target = candidate;
                return true;
            }

            return false;
        }

        private void StepCharges(float deltaTime)
        {
            for (var i = _activeCharges.Count - 1; i >= 0; i--)
            {
                var charge = _activeCharges[i];
                DropGroundObjectIfUnsupported(charge, charge.Id);
                charge.FuseRemaining = Mathf.Max(0f, charge.FuseRemaining - deltaTime);
                if (charge.FuseRemaining > 0f)
                {
                    continue;
                }

                _activeCharges.RemoveAt(i);
                var impact = new ProjectileImpact(
                    $"projectile.{charge.Weapon.WeaponId}",
                    charge.OwnerUnitId,
                    charge.Position,
                    Vector2.up,
                    charge.Weapon.Damage,
                    charge.Weapon.ImpactRadius,
                    charge.KnockbackMultiplier);
                ProjectileExploded?.Invoke(charge.Id, impact);
                var terrainChanged = ResolveDamageImpact(impact, charge.Weapon);
                ImpactResolved?.Invoke(new ImpactResolvedEvent(impact.Point, charge.Weapon, terrainChanged));
                UnitsMoved?.Invoke();

                Result = BattleResultEvaluator.Evaluate(_state);
                if (Result.IsFinished)
                {
                    BattleFinished?.Invoke(Result, BattleFinishReason.InstantActionCompleted);
                    continue;
                }

                if (_state.Turn.ActiveUnitId == charge.OwnerUnitId && !_state.Turn.ActionSpent)
                {
                    CompleteInstantAction();
                }
            }
        }

        private void ResolveMineExplosion(ActiveMine mine)
        {
            var activeTarget = FindUnit(_state.Turn.ActiveUnitId);
            var healthBefore = activeTarget?.Health ?? 0;
            var modifiers = ResolveModifiersFor(mine.TeamId, mine.Weapon);
            var impact = new ProjectileImpact(
                $"projectile.{mine.Weapon.WeaponId}",
                mine.OwnerUnitId,
                mine.Position,
                Vector2.up,
                Mathf.RoundToInt(mine.Weapon.Damage * modifiers.DamageMultiplier),
                mine.Weapon.ImpactRadius * modifiers.ImpactRadiusMultiplier);

            ProjectileExploded?.Invoke(mine.Id, impact);
            var terrainChanged = ResolveDamageImpact(
                impact,
                mine.Weapon,
                mine.AlliesImmuneToExplosion ? mine.TeamId : BattleTeamId.None);
            ImpactResolved?.Invoke(new ImpactResolvedEvent(impact.Point, mine.Weapon, terrainChanged));
            UnitsMoved?.Invoke();
            Result = BattleResultEvaluator.Evaluate(_state);
            if (Result.IsFinished)
            {
                BattleFinished?.Invoke(Result, BattleFinishReason.ProjectileImpactResolved);
            }
            else if (activeTarget != null && activeTarget.Health < healthBefore)
            {
                // Arming alone is harmless; an actual hit interrupts either side.
                AdvanceTurnNow(TurnAdvanceReason.MineTriggered);
            }
            else if (mine.IsSpider && mine.OwnerUnitId == _state.Turn.ActiveUnitId && _state.Turn.ActionSpent
                && !HasProjectilesInFlight && !HasActiveWeaponEffect)
            {
                // The owner's spider was the attack of this turn; its blast concludes it.
                AdvanceTurnOrStartRetreat(TurnAdvanceReason.ProjectileImpactResolved);
            }
        }

        private BattleFireResult PlaceTitaniumBarrier(BattleUnitState unit, WeaponInventoryRuntime inventory,
            WeaponDefinition weapon, AimState placement)
        {
            if (_terrain == null || _titaniumBarrierTexture == null || !_titaniumBarrierTexture.isReadable
                || !float.IsFinite(placement.Origin.x) || !float.IsFinite(placement.Origin.y)
                || !float.IsFinite(placement.Direction.x) || !float.IsFinite(placement.Direction.y))
                return new BattleFireResult(BattleFireOutcome.Blocked, unit.TeamId, weapon);
            var scale = ResolveTitaniumBarrierScale(placement.Power, unit.TeamId);
            var angle = Mathf.Atan2(placement.Direction.y, placement.Direction.x) * Mathf.Rad2Deg;
            var surface = new PlacedTerrainSpriteSurface(_titaniumBarrierTexture, placement.Origin,
                TitaniumBarrierBaseSize * scale, angle);
            if (!CanPlaceTitaniumBarrier(surface))
            {
                surface.Dispose();
                return new BattleFireResult(BattleFireOutcome.Blocked, unit.TeamId, weapon);
            }
            if (!inventory.TryConsumeSelectedAmmo())
            {
                surface.Dispose();
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }
            _terrain.AddPlacedSurface(surface);
            // The beam becomes part of the ground; placing it neither spends the action nor ends the turn.
            CoverDeployed?.Invoke(new CoverDeployedEvent(surface.Center, surface.Size, weapon, angle, surface));
            ShotFired?.Invoke(unit.TeamId, weapon, false);
            WakeUnitsWithoutSupport();
            UnitsMoved?.Invoke();
            return new BattleFireResult(BattleFireOutcome.UtilityUsed, unit.TeamId, weapon);
        }

        private bool CanPlaceTitaniumBarrier(PlacedTerrainSpriteSurface surface)
        {
            var grid = _terrain.Grid;
            var bounds = surface.WorldBounds;
            if (bounds.xMin < grid.WorldMinX || bounds.xMax > grid.WorldMaxX
                || bounds.yMin < grid.WorldMinY || bounds.yMax > grid.WorldMaxY) return false;
            var min = grid.WorldToCell(bounds.min);
            var max = grid.WorldToCell(bounds.max);
            for (var y = min.y; y <= max.y; y++)
                for (var x = min.x; x <= max.x; x++)
                {
                    var point = grid.CellToWorld(x, y);
                    if (!surface.IsSolidWorld(point)) continue;
                    if (grid.IsSolidCell(x, y)) return false;
                    foreach (var motor in _motors.Values)
                        if (motor.BodyBounds.Contains(point)) return false;
                }
            return true;
        }

        private void DeployCover(Vector2 impactPoint, WeaponDefinition impactWeapon, Vector2 direction = default)
        {
            var width = Mathf.Clamp(impactWeapon.ImpactRadius * 1.2f, 1.15f, 2.1f);
            const float height = 0.6f;
            var position = impactPoint + Vector2.up * (height * 0.5f);
            _terrain?.StampSolidRect(position, new Vector2(width, height));
            var rotationDegrees = direction.sqrMagnitude > 0.0001f
                ? Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg
                : 0f;
            CoverDeployed?.Invoke(new CoverDeployedEvent(position, new Vector2(width, height), impactWeapon, rotationDegrees));
        }

        /// <summary>A blast can wreck a laser-fence hull: its beam then vanishes (a carried hull is dropped first).</summary>
        private void DamageLaserFenceNodesInBlast(Vector2 point, float radius, int damage)
        {
            if (_activeLaserFenceNodes.Count == 0 || damage <= 0 || radius <= 0f)
            {
                return;
            }

            for (var index = _activeLaserFenceNodes.Count - 1; index >= 0; index--)
            {
                var node = _activeLaserFenceNodes[index];
                if (Vector2.Distance(node.Position, point) > radius)
                {
                    continue;
                }

                node.Health -= damage;
                if (node.Health > 0)
                {
                    continue;
                }

                RemoveLaserFenceNodeAt(index);
            }
        }

        /// <summary>A shot that touches a hull wrecks it.</summary>
        private bool TryWreckLaserFenceNodeTouchedBy(Vector2 point, float radius)
        {
            var wrecked = false;
            for (var index = _activeLaserFenceNodes.Count - 1; index >= 0; index--)
            {
                var reach = radius + LaserFenceHullHitRadius;
                if ((_activeLaserFenceNodes[index].Position - point).sqrMagnitude > reach * reach)
                {
                    continue;
                }

                RemoveLaserFenceNodeAt(index);
                wrecked = true;
            }

            return wrecked;
        }

        private void RemoveLaserFenceNodeAt(int index)
        {
            var node = _activeLaserFenceNodes[index];
            if (_copter != null && _copter.CarriedNode == node)
            {
                ReleaseCarrierCargo(_copter);
            }

            _activeLaserFenceNodes.RemoveAt(index);
            _spentLaserFenceLinks.RemoveWhere(linkId => (int)(linkId & 0xFFFFFFFF) == node.Id || (int)(linkId >> 32) == node.Id);
            LaserFenceNodeRemoved?.Invoke(node.Id);
        }

        /// <summary>True when the enemy team cannot see this placed guardian drone (it is not removed from the simulation).</summary>
        public bool IsGuardianConcealedFrom(int guardianId, BattleTeamId observer)
        {
            var guardian = _activeMines.Find(mine => mine.Id == guardianId && mine.IsGuardian);
            return guardian != null && guardian.IsConcealed && guardian.TeamId != observer;
        }

        private void ResolveGuardianDroneShot(int projectileIndex, SimProjectile projectile, BattleUnitState target)
        {
            _projectiles.RemoveAt(projectileIndex);
            var point = target.Position;
            var impact = new ProjectileImpact(
                projectile.Definition.ProjectileId,
                projectile.OwnerUnitId,
                point,
                projectile.Velocity,
                projectile.Definition.Damage,
                0f);
            ProjectileExploded?.Invoke(projectile.Id, impact);
            ApplyDamage(target, projectile.Definition.Damage, GuardianDroneWeaponId, ResolveTeamForOwner(projectile.OwnerUnitId));
            ImpactResolved?.Invoke(new ImpactResolvedEvent(point, ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId), false));
            UnitsMoved?.Invoke();

            // A trap interrupts the turn it has actually damaged, rather than
            // the earlier visual wake-up. The short rise still locks controls
            // through HasActiveWeaponEffect, so the original target cannot evade.
            if (target.UnitId == _state.Turn.ActiveUnitId)
            {
                Result = BattleResultEvaluator.Evaluate(_state);
                if (Result.IsFinished)
                {
                    BattleFinished?.Invoke(Result, BattleFinishReason.ProjectileImpactResolved);
                }
                else
                {
                    AdvanceTurnNow(TurnAdvanceReason.GuardianDroneTriggered);
                }
            }
        }

        private void LandMine(SimProjectile projectile, Vector2 point)
        {
            var weapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
            if (weapon == null)
            {
                return;
            }

            var owner = _state.GetUnit(projectile.OwnerUnitId);
            var teamId = owner?.TeamId ?? BattleTeamId.None;
            var evolution = ResolveEvolutionFor(teamId, weapon);
            var mine = new ActiveMine(
                projectile.Id,
                projectile.OwnerUnitId,
                teamId,
                weapon,
                point + Vector2.up * 0.04f,
                evolution.MineTriggersEnemiesOnly,
                evolution.MineAlliesImmuneToExplosion);
            _activeMines.Add(mine);
            MineDeployed?.Invoke(new MineDeployedEvent(mine.Id, mine.Position, weapon, teamId));
            ImpactResolved?.Invoke(new ImpactResolvedEvent(point, weapon, false));
            UnitsMoved?.Invoke();
            // Mine placement never commits an action. This keeps the
            // turn open for more mines and for a later normal weapon shot.
        }

        private void LandGuardianDrone(SimProjectile projectile, Vector2 point)
        {
            var weapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
            if (weapon == null)
            {
                return;
            }

            var owner = _state.GetUnit(projectile.OwnerUnitId);
            var teamId = owner?.TeamId ?? BattleTeamId.None;
            var guardian = new ActiveMine(
                projectile.Id,
                projectile.OwnerUnitId,
                teamId,
                weapon,
                point + Vector2.up * 0.04f,
                triggersEnemiesOnly: true,
                alliesImmuneToExplosion: true,
                isGuardian: true);
            guardian.IsConcealed = ResolveEvolutionFor(teamId, weapon).GuardianStealth;
            _activeMines.Add(guardian);
            MineDeployed?.Invoke(new MineDeployedEvent(guardian.Id, guardian.Position, weapon, teamId,
                guardian.IsConcealed));
            ImpactResolved?.Invoke(new ImpactResolvedEvent(point, weapon, false));
            UnitsMoved?.Invoke();

            if (teamId == BattleTeamId.Opponent
                && _state.Turn.ActiveUnitId == projectile.OwnerUnitId
                && !_state.Turn.ActionSpent)
            {
                CompleteInstantAction();
            }
        }

        private void LandLaserFenceNode(SimProjectile projectile, Vector2 point)
        {
            var weapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
            if (weapon == null)
            {
                return;
            }

            var owner = _state.GetUnit(projectile.OwnerUnitId);
            var teamId = owner?.TeamId ?? BattleTeamId.None;
            var node = new ActiveLaserFenceNode(
                projectile.Id,
                projectile.OwnerUnitId,
                teamId,
                weapon,
                point + Vector2.up * 0.04f);
            _activeLaserFenceNodes.Add(node);
            LaserFenceNodeDeployed?.Invoke(new LaserFenceNodeDeployedEvent(
                node.Id,
                node.OwnerUnitId,
                node.Position,
                node.Weapon,
                node.TeamId));
            ImpactResolved?.Invoke(new ImpactResolvedEvent(point, weapon, false));
            UnitsMoved?.Invoke();

            if (teamId == BattleTeamId.Opponent
                && _state.Turn.ActiveUnitId == projectile.OwnerUnitId
                && !_state.Turn.ActionSpent)
            {
                CompleteInstantAction();
            }
        }

        private bool IsMineTriggerTargetInside(ActiveMine mine)
        {
            return TryFindMineTriggerTarget(mine, out _);
        }

        /// <summary>
        /// The mine and the guardian drone notice a droid within the shared N0; the guardian's Radius purchases add
        /// 10% of N0 each. Other ground devices keep their own trigger radius.
        /// </summary>
        private float ResolveMineTriggerRange(ActiveMine mine)
        {
            if (mine.Weapon.WeaponId == MineWeaponId)
            {
                return PrototypeWeaponLibrary.SharedNearbyRadius;
            }

            if (mine.Weapon.WeaponId == GuardianDroneWeaponId)
            {
                var radiusPurchases = ProgressionFor(mine.TeamId).ResolveUpgradeCount(GuardianDroneWeaponId, WeaponUpgradePath.Radius);
                return PrototypeWeaponLibrary.SharedNearbyRadius * GuardianDetectionMultiplier
                    * (1f + Mathf.Clamp(radiusPurchases, 0, 4) * 0.10f);
            }

            return mine.Weapon.ImpactRadius;
        }

        private bool TryFindMineTriggerTarget(ActiveMine mine, out BattleUnitState target)
        {
            target = null;
            foreach (var team in _state.Teams)
            {
                if (mine.TriggersEnemiesOnly && team.TeamId == mine.TeamId)
                {
                    continue;
                }

                foreach (var unit in team.Units)
                {
                    if (unit != null
                        && !unit.IsDefeated
                        && Vector2.Distance(unit.Position, mine.Position) <= ResolveMineTriggerRange(mine))
                    {
                        target = unit;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool TryFindGrounderForProjectile(
            SimProjectile projectile,
            WeaponDefinition weapon,
            Vector2 samplePosition,
            out ActiveGrounder selected)
        {
            selected = null;
            if (!IsGrounderPhysicalCarrier(projectile, weapon))
            {
                return false;
            }

            var projectileTeam = ResolveTeamForOwner(projectile.OwnerUnitId);
            var nearestDistance = float.PositiveInfinity;
            foreach (var grounder in _activeGrounders)
            {
                if (grounder.TeamId == projectileTeam || grounder.Health <= 0)
                {
                    continue;
                }

                var distance = Vector2.Distance(samplePosition, grounder.Position);
                if (distance > grounder.DetectionRadius || distance >= nearestDistance)
                {
                    continue;
                }

                selected = grounder;
                nearestDistance = distance;
            }

            return selected != null;
        }

        private void ResolveGrounderProjectileContact(
            int projectileIndex, SimProjectile projectile, WeaponDefinition weapon,
            ActiveGrounder grounder, Vector2 contactPoint)
        {
            var projectileId = projectile.Definition.ProjectileId;
            if (projectileId == MineDeployProjectileId)
            {
                // The mine's blast at the magnet wears it down like any blast in range.
                ResolveProjectileAt(projectileIndex, contactPoint, interceptedMine: true);
                return;
            }

            // A physical carrier without an explosive payload is destroyed at
            // the magnet. Its placement or teleport action must not run.
            if (projectileId == GuardianDroneDeployProjectileId
                || projectileId == LaserFenceDeployProjectileId
                || weapon?.WeaponId is TeleportWeaponId or SwapJumpWeaponId
                    or RepairJammerWeaponId or VirusWeaponId)
            {
                DamageGrounder(grounder, 1);
                projectile.MoveTo(contactPoint);
                DiscardProjectile(projectileIndex, projectile);
                return;
            }

            // The captured shot blasts at the magnet; that blast wears it down like any blast in range.
            ResolveProjectileAt(projectileIndex, contactPoint);
        }

        private bool TryFindGrounderForHook(
            BattleTeamId ownerTeam, Vector2 start, Vector2 end, out ActiveGrounder selected)
        {
            selected = null;
            var nearestDistance = float.PositiveInfinity;
            var segment = end - start;
            var segmentLengthSquared = segment.sqrMagnitude;
            foreach (var grounder in _activeGrounders)
            {
                if (grounder.TeamId == ownerTeam || grounder.Health <= 0)
                {
                    continue;
                }

                var projection = segmentLengthSquared > 0.0001f
                    ? Mathf.Clamp01(Vector2.Dot(grounder.Position - start, segment) / segmentLengthSquared)
                    : 0f;
                var distance = Vector2.Distance(start + segment * projection, grounder.Position);
                if (distance > grounder.DetectionRadius || distance >= nearestDistance)
                {
                    continue;
                }

                selected = grounder;
                nearestDistance = distance;
            }

            return selected != null;
        }

        private static bool IsGrounderPhysicalCarrier(SimProjectile projectile, WeaponDefinition weapon)
        {
            if (projectile == null)
            {
                return false;
            }

            if (projectile.Definition.ProjectileId == MineDeployProjectileId
                || projectile.Definition.ProjectileId == GuardianDroneDeployProjectileId
                || projectile.Definition.ProjectileId == GuardianDroneShotProjectileId
                || projectile.Definition.ProjectileId == LaserFenceDeployProjectileId)
            {
                return true;
            }

            return weapon?.WeaponId is "weapon.granata" or "weapon.blaster-caset"
                or "weapon.ion-shotgun" or "weapon.c4" or "weapon.mine"
                or "weapon.titanium-disc" or "weapon.bur-rocket" or "weapon.mortira"
                or "weapon.sticky-grenade" or "weapon.teleport" or "weapon.rail-rifle"
                or "weapon.cryo-grenade" or "weapon.hopping-grenade"
                or "weapon.incendiary-grenade" or "weapon.repair-jammer"
                or "weapon.missle" or "weapon.orbital-rocket"
                or "weapon.guardian-drone" or "weapon.electric-harpoon"
                or "weapon.swap-jump" or "weapon.sky-hook" or "weapon.spider-mine"
                or "weapon.virus" or "weapon.bomber" or "weapon.ufo-carrier"
                or "weapon.nuclear-charge" or "weapon.laser-fence";
        }

        private void DamageGrounder(ActiveGrounder grounder, int damage)
        {
            if (!_activeGrounders.Contains(grounder))
            {
                return;
            }

            grounder.Health = Mathf.Max(0, grounder.Health - Mathf.Max(0, damage));
            PlacedDeviceHealthChanged?.Invoke(grounder.Id, grounder.Health, grounder.MaxHealth);
            if (grounder.Health > 0)
            {
                return;
            }

            _activeGrounders.Remove(grounder);
            GrounderRemoved?.Invoke(grounder.Id);
        }

        private void DamageForceField(ActiveForceField field, int damage)
        {
            if (!_activeForceFields.Contains(field))
            {
                return;
            }

            field.Health = Mathf.Max(0, field.Health - Mathf.Max(0, damage));
            PlacedDeviceHealthChanged?.Invoke(field.Id, field.Health, field.MaxHealth);
            if (field.Health > 0)
            {
                return;
            }

            _activeForceFields.Remove(field);
            ForceFieldRemoved?.Invoke(field.Id);
        }

        /// <summary>A blast wears placed magnets and force field devices down like droids.</summary>
        private void DamagePlacedDevicesInBlast(Vector2 point, float radius, int damage)
        {
            if (damage <= 0 || radius <= 0f)
            {
                return;
            }

            for (var index = _activeGrounders.Count - 1; index >= 0; index--)
            {
                if (Vector2.Distance(_activeGrounders[index].Position, point) <= radius)
                {
                    DamageGrounder(_activeGrounders[index], damage);
                }
            }

            for (var index = _activeForceFields.Count - 1; index >= 0; index--)
            {
                if (Vector2.Distance(_activeForceFields[index].Position, point) <= radius)
                {
                    DamageForceField(_activeForceFields[index], damage);
                }
            }
        }

        /// <summary>Placed magnets and force field devices are ground objects: they drop when the ground under them goes.</summary>
        private void StepPlacedDevices()
        {
            foreach (var grounder in _activeGrounders)
            {
                if (TryFindGroundObjectLanding(grounder.Position, out var landing))
                {
                    grounder.Position = landing;
                    GroundObjectRepositioned?.Invoke(new GroundObjectRepositionedEvent(grounder.Id, landing));
                }
            }

            foreach (var field in _activeForceFields)
            {
                if (TryFindGroundObjectLanding(field.Position, out var landing))
                {
                    field.Position = landing;
                    GroundObjectRepositioned?.Invoke(new GroundObjectRepositionedEvent(field.Id, landing));
                }
            }
        }

        private static bool IsForceFieldEnergyCarrier(WeaponDefinition weapon)
        {
            // The carrier decides the group, not the weapon alone; these weapons fly an energy charge.
            return weapon?.WeaponId is "weapon.impulse" or "weapon.emp" or "weapon.freeze"
                or "weapon.fire-gun" or "weapon.toxic";
        }

        private bool TryFindForceFieldForProjectile(
            SimProjectile projectile, WeaponDefinition weapon, Vector2 samplePosition, out ActiveForceField selected)
        {
            selected = null;
            if (_activeForceFields.Count == 0 || !IsForceFieldEnergyCarrier(weapon))
            {
                return false;
            }

            var projectileTeam = ResolveTeamForOwner(projectile.OwnerUnitId);
            var nearestDistance = float.PositiveInfinity;
            foreach (var field in _activeForceFields)
            {
                if (field.TeamId == projectileTeam)
                {
                    continue;
                }

                var distance = Vector2.Distance(samplePosition, field.Position);
                if (distance > field.Radius || distance >= nearestDistance)
                {
                    continue;
                }

                selected = field;
                nearestDistance = distance;
            }

            return selected != null;
        }

        /// <summary>
        /// Shortens a beam so that it ends where it first enters a hostile force field. A beam that
        /// starts inside a field is not cut by that field.
        /// </summary>
        private float ClipBeamAtHostileForceFields(
            BattleTeamId ownerTeam, Vector2 origin, Vector2 direction, float range)
        {
            var clipped = range;
            ActiveForceField absorbing = null;
            foreach (var field in _activeForceFields)
            {
                if (field.TeamId == ownerTeam)
                {
                    continue;
                }

                var toCenter = field.Position - origin;
                if (toCenter.magnitude <= field.Radius)
                {
                    continue;
                }

                var along = Vector2.Dot(toCenter, direction);
                var perpendicularSquared = toCenter.sqrMagnitude - along * along;
                var discriminant = field.Radius * field.Radius - perpendicularSquared;
                if (discriminant < 0f)
                {
                    continue;
                }

                var entry = along - Mathf.Sqrt(discriminant);
                if (entry <= 0f || entry >= clipped)
                {
                    continue;
                }

                clipped = entry;
                absorbing = field;
            }

            if (absorbing != null)
            {
                ForceFieldAbsorbed?.Invoke(absorbing.Id, origin + direction * clipped);
            }

            return clipped;
        }

        private void ExpireForceFieldsForEndingTurn()
        {
            for (var index = _activeForceFields.Count - 1; index >= 0; index--)
            {
                var field = _activeForceFields[index];
                if (field.TeamId == _state.Turn.ActiveTeam)
                {
                    continue;
                }

                field.RemainingTurns--;
                if (field.RemainingTurns > 0)
                {
                    continue;
                }

                _activeForceFields.RemoveAt(index);
                ForceFieldRemoved?.Invoke(field.Id);
            }
        }

        private void ExpireGroundersForEndingTurn()
        {
            ExpireForceFieldsForEndingTurn();
            for (var index = _activeGrounders.Count - 1; index >= 0; index--)
            {
                var grounder = _activeGrounders[index];
                if (grounder.TeamId == _state.Turn.ActiveTeam)
                {
                    continue;
                }

                grounder.RemainingTurns--;
                if (grounder.RemainingTurns > 0)
                {
                    continue;
                }

                _activeGrounders.RemoveAt(index);
                GrounderRemoved?.Invoke(grounder.Id);
            }
        }

        private sealed class ActiveForceField
        {
            public ActiveForceField(int id, BattleTeamId teamId, Vector2 position,
                WeaponDefinition weapon, int remainingTurns, float radius, int health = 1)
            {
                Id = id;
                TeamId = teamId;
                Position = position;
                Weapon = weapon;
                RemainingTurns = remainingTurns;
                Radius = radius;
                Health = Mathf.Max(1, health);
                MaxHealth = Health;
            }

            public int Id { get; }
            public BattleTeamId TeamId { get; }
            public Vector2 Position { get; set; }
            public int Health { get; set; }
            public int MaxHealth { get; }
            public WeaponDefinition Weapon { get; }
            public int RemainingTurns { get; set; }
            public float Radius { get; }
        }

        private sealed class ActiveGrounder
        {
            public ActiveGrounder(int id, BattleTeamId teamId, Vector2 position,
                WeaponDefinition weapon, int remainingTurns, int health, float detectionRadius)
            {
                Id = id;
                TeamId = teamId;
                Position = position;
                Weapon = weapon;
                RemainingTurns = remainingTurns;
                Health = health;
                MaxHealth = Mathf.Max(1, health);
                DetectionRadius = detectionRadius;
            }

            public int Id { get; }
            public BattleTeamId TeamId { get; }
            public Vector2 Position { get; set; }
            public int MaxHealth { get; }
            public WeaponDefinition Weapon { get; }
            public int RemainingTurns { get; set; }
            public int Health { get; set; }
            public float DetectionRadius { get; }
        }

        private sealed class ActiveMine
        {
            public ActiveMine(
                int id,
                string ownerUnitId,
                BattleTeamId teamId,
                WeaponDefinition weapon,
                Vector2 position,
                bool triggersEnemiesOnly,
                bool alliesImmuneToExplosion,
                bool isSpider = false,
                int crawlDirection = 1,
                bool isGuardian = false)
            {
                Id = id;
                OwnerUnitId = ownerUnitId;
                TeamId = teamId;
                Weapon = weapon;
                Position = position;
                TriggersEnemiesOnly = triggersEnemiesOnly;
                AlliesImmuneToExplosion = alliesImmuneToExplosion;
                IsSpider = isSpider;
                IsGuardian = isGuardian;
                CrawlDirection = crawlDirection >= 0 ? 1 : -1;
                ArmTimeRemaining = isSpider || isGuardian ? 0f : MineActivationDelaySeconds;
                SearchTimeRemaining = isSpider ? SpiderSearchDurationSeconds : 0f;
            }

            public int Id { get; }
            public string OwnerUnitId { get; }
            public BattleTeamId TeamId { get; }
            public WeaponDefinition Weapon { get; }
            public Vector2 Position { get; private set; }
            public bool TriggersEnemiesOnly { get; }
            public bool AlliesImmuneToExplosion { get; }
            public bool IsSpider { get; }
            public bool IsGuardian { get; }
            public int CrawlDirection { get; }
            public float ArmTimeRemaining { get; set; }
            public float SearchTimeRemaining { get; set; }
            public bool Triggered { get; set; }
            public float TriggerTimeRemaining { get; set; }
            public string TriggerTargetUnitId { get; set; }
            public int TriggerCopterProjectileId { get; set; } = -1;
            public bool IsConcealed { get; set; }
            public bool IsCarried { get; set; }

            public void MoveTo(Vector2 position)
            {
                Position = position;
            }
        }

        private sealed class ActiveLaserFenceNode
        {
            public ActiveLaserFenceNode(
                int id,
                string ownerUnitId,
                BattleTeamId teamId,
                WeaponDefinition weapon,
                Vector2 position)
            {
                Id = id;
                OwnerUnitId = ownerUnitId;
                TeamId = teamId;
                Weapon = weapon;
                Position = position;
            }

            public int Id { get; }
            public string OwnerUnitId { get; }
            public BattleTeamId TeamId { get; }
            public WeaponDefinition Weapon { get; }
            public Vector2 Position { get; private set; }
            public bool IsCarried { get; set; }
            public bool FallPending { get; set; }
            public int Health { get; set; } = LaserFenceNodeHealth;

            public void MoveTo(Vector2 position)
            {
                Position = position;
            }
        }

        private sealed class ActiveCharge
        {
            public ActiveCharge(
                int id,
                string ownerUnitId,
                BattleTeamId teamId,
                WeaponDefinition weapon,
                Vector2 position,
                float fuseSeconds,
                float knockbackMultiplier)
            {
                Id = id;
                OwnerUnitId = ownerUnitId;
                TeamId = teamId;
                Weapon = weapon;
                Position = position;
                FuseRemaining = fuseSeconds;
                KnockbackMultiplier = Mathf.Max(0f, knockbackMultiplier);
            }

            public int Id { get; }
            public string OwnerUnitId { get; }
            public BattleTeamId TeamId { get; }
            public WeaponDefinition Weapon { get; }
            public Vector2 Position { get; private set; }
            public float FuseRemaining { get; set; }
            public float KnockbackMultiplier { get; }

            public void MoveTo(Vector2 position)
            {
                Position = position;
            }
        }
    }
}
