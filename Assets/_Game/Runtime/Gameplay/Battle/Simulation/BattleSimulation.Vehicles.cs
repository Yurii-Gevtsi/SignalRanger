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
        public void SetControlledProjectileInput(Vector2 direction)
        {
            _controlledProjectileInput = direction.sqrMagnitude > 0.01f
                ? direction.normalized
                : Vector2.zero;
        }

        private static bool IsControlledDroneWeapon(WeaponDefinition weapon)
        {
            return weapon?.WeaponId == FpvWeaponId
                || weapon?.WeaponId == BomberWeaponId
                || weapon?.WeaponId == CarrierWeaponId;
        }

        private static bool IsAttachmentDevice(WeaponDefinition weapon)
        {
            return weapon?.WeaponId == RepairJammerWeaponId || weapon?.WeaponId == VirusWeaponId;
        }

        private BattleFireResult LaunchBomber(
            BattleUnitState unit,
            WeaponInventoryRuntime inventory,
            WeaponDefinition weapon)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var radiusPurchases = ProgressionFor(unit.TeamId).ResolveUpgradeCount(weapon.WeaponId, WeaponUpgradePath.Radius);
            var isCarrier = weapon.WeaponId == CarrierWeaponId;
            var seconds = WeaponUpgradeChoiceResolver.ResolveBomberSeconds(radiusPurchases);
            var spawnPoint = unit.Position + Vector2.up * 1.05f;
            Spawn(new ProjectileSpawnRequest(
                weapon.CreateProjectileDefinition(0, 1f, 0, seconds),
                unit.UnitId,
                spawnPoint,
                Vector2.zero,
                weapon.ProjectileRadius * 2f,
                ProjectileWeaponEffect.GetProjectileColor(weapon.Role)));
            var copter = _projectiles[_projectiles.Count - 1];
            _controlledProjectileId = copter.Id;
            _controlledProjectileInput = Vector2.zero;
            var bombDamage = Mathf.RoundToInt(weapon.Damage * ResolveModifiersFor(unit.TeamId, weapon).DamageMultiplier);
            _copter = new CopterSession(unit.UnitId, copter.Id, weapon, bombDamage, isCarrier ? 0 : BomberBombCount, isCarrier);
            _activeShotWeapon = weapon;
            _activeShotEndsTurn = true;
            _turnService.SpendAction(_state);
            ShotFired?.Invoke(unit.TeamId, weapon, true);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon, spawnPoint);
        }

        /// <summary>
        /// The carrier's button: it grabs the nearest movable item the copter touches (mines, guardians, laser-fence
        /// nodes — never droids or barriers), and a second press lets it go.
        /// </summary>
        private BattleFireResult PressCarrier()
        {
            var session = _copter;
            var copter = _projectiles.Find(value => value.Id == session.CopterProjectileId);
            if (copter == null)
            {
                return new BattleFireResult(BattleFireOutcome.Blocked, _state.Turn.ActiveTeam, session.Weapon);
            }

            if (session.HasCargo)
            {
                ReleaseCarrierCargo(session);
                return new BattleFireResult(BattleFireOutcome.Fired, _state.Turn.ActiveTeam, session.Weapon, copter.Position);
            }

            ActiveMine bestMine = null;
            ActiveLaserFenceNode bestNode = null;
            CarryablePickup bestPickup = null;
            var bestDistance = CarrierCaptureDistance;
            foreach (var mine in _activeMines)
            {
                var distance = Vector2.Distance(mine.Position, copter.Position);
                if (mine.IsSpider || mine.Triggered || distance > bestDistance)
                {
                    continue;
                }

                bestMine = mine;
                bestNode = null;
                bestPickup = null;
                bestDistance = distance;
            }

            foreach (var node in _activeLaserFenceNodes)
            {
                var distance = Vector2.Distance(node.Position, copter.Position);
                if (distance > bestDistance)
                {
                    continue;
                }

                bestNode = node;
                bestMine = null;
                bestPickup = null;
                bestDistance = distance;
            }

            foreach (var pickup in _carryablePickups)
            {
                var distance = Vector2.Distance(pickup.Position, copter.Position);
                if (distance > bestDistance)
                {
                    continue;
                }

                bestPickup = pickup;
                bestMine = null;
                bestNode = null;
                bestDistance = distance;
            }

            if (bestMine == null && bestNode == null && bestPickup == null)
            {
                return new BattleFireResult(BattleFireOutcome.Blocked, _state.Turn.ActiveTeam, session.Weapon);
            }

            session.CarriedMine = bestMine;
            session.CarriedNode = bestNode;
            session.CarriedPickup = bestPickup;
            if (bestMine != null)
            {
                bestMine.IsCarried = true;
                GroundObjectCarried?.Invoke(bestMine.Id, true);
            }
            else if (bestNode != null)
            {
                bestNode.IsCarried = true;
                GroundObjectCarried?.Invoke(bestNode.Id, true);
            }
            else
            {
                bestPickup.IsCarried = true;
                bestPickup.FallPending = false;
                GroundObjectCarried?.Invoke(bestPickup.Id, true);
            }

            FollowCarrierCargo(session, copter.Position);
            return new BattleFireResult(BattleFireOutcome.Fired, _state.Turn.ActiveTeam, session.Weapon, copter.Position);
        }

        /// <summary>The held item hangs a little above the copter's centre; it keeps its owner and its armed state.</summary>
        private void FollowCarrierCargo(CopterSession session, Vector2 copterPosition)
        {
            var point = copterPosition + Vector2.up * CarrierCargoLift;
            if (session.CarriedMine != null)
            {
                session.CarriedMine.MoveTo(point);
                GroundObjectRepositioned?.Invoke(new GroundObjectRepositionedEvent(session.CarriedMine.Id, point));
            }
            else if (session.CarriedNode != null)
            {
                session.CarriedNode.MoveTo(point);
                GroundObjectRepositioned?.Invoke(new GroundObjectRepositionedEvent(session.CarriedNode.Id, point));
            }
            else if (session.CarriedPickup != null)
            {
                session.CarriedPickup.Position = point;
                GroundObjectRepositioned?.Invoke(new GroundObjectRepositionedEvent(session.CarriedPickup.Id, point));
            }
        }

        /// <summary>The item falls straight to the surface: no bounce and no roll.</summary>
        private void ReleaseCarrierCargo(CopterSession session)
        {
            if (session.CarriedMine != null)
            {
                session.CarriedMine.IsCarried = false;
                GroundObjectCarried?.Invoke(session.CarriedMine.Id, false);
            }
            else if (session.CarriedNode != null)
            {
                var node = session.CarriedNode;
                node.IsCarried = false;
                node.FallPending = true;
                GroundObjectCarried?.Invoke(node.Id, false);
            }
            else if (session.CarriedPickup != null)
            {
                var pickup = session.CarriedPickup;
                pickup.IsCarried = false;
                pickup.FallPending = true;
                GroundObjectCarried?.Invoke(pickup.Id, false);
            }

            session.CarriedMine = null;
            session.CarriedNode = null;
            session.CarriedPickup = null;
        }

        /// <summary>
        /// Medkits and supply boxes that drop during the battle. The scene owns their reward; the simulation
        /// only knows their position, so the carrier can lift them and drop them elsewhere.
        /// </summary>
        public int RegisterCarryablePickup(Vector2 position)
        {
            var pickup = new CarryablePickup(++_nextProjectileId, position);
            _carryablePickups.Add(pickup);
            return pickup.Id;
        }

        /// <summary>The scene collected or removed the pickup; a carrier holding it simply has no cargo any more.</summary>
        public void RemoveCarryablePickup(int pickupId)
        {
            var pickup = _carryablePickups.Find(value => value.Id == pickupId);
            if (pickup == null)
            {
                return;
            }

            if (_copter != null && _copter.CarriedPickup == pickup)
            {
                _copter.CarriedPickup = null;
            }

            _carryablePickups.Remove(pickup);
        }

        /// <summary>The scene settled the pickup somewhere else (it fell after the ground under it was destroyed).</summary>
        public void SetCarryablePickupPosition(int pickupId, Vector2 position)
        {
            var pickup = _carryablePickups.Find(value => value.Id == pickupId);
            if (pickup != null && !pickup.IsCarried)
            {
                pickup.Position = position;
            }
        }

        public bool IsCarryablePickupCarried(int pickupId)
        {
            return _carryablePickups.Exists(value => value.Id == pickupId && value.IsCarried);
        }

        private void StepCarryablePickups()
        {
            foreach (var pickup in _carryablePickups)
            {
                if (!pickup.FallPending || pickup.IsCarried)
                {
                    continue;
                }

                pickup.FallPending = false;
                if (TryFindGroundObjectLanding(pickup.Position, out var landing))
                {
                    pickup.Position = landing;
                    GroundObjectRepositioned?.Invoke(new GroundObjectRepositionedEvent(pickup.Id, landing));
                }
            }
        }

        private sealed class CarryablePickup
        {
            public CarryablePickup(int id, Vector2 position)
            {
                Id = id;
                Position = position;
            }

            public int Id { get; }
            public Vector2 Position { get; set; }
            public bool IsCarried { get; set; }
            public bool FallPending { get; set; }
        }

        /// <summary>Every fire press lets one bomb fall from the copter; there is no delay between bombs.</summary>
        private BattleFireResult DropBomberBomb()
        {
            var session = _copter;
            var copter = _projectiles.Find(value => value.Id == session.CopterProjectileId);
            if (copter == null || session.BombsLeft <= 0)
            {
                return new BattleFireResult(BattleFireOutcome.Blocked, _state.Turn.ActiveTeam, session.Weapon);
            }

            session.BombsLeft--;
            var weapon = session.Weapon;
            var bomb = ProjectileDefinition.CreateRuntime(
                BomberBombProjectileId, 0.1f, 1f, 6f, session.BombDamage, weapon.ImpactRadius, 1f, 0);
            Spawn(new ProjectileSpawnRequest(
                bomb,
                session.OwnerUnitId,
                copter.Position + Vector2.down * 0.35f,
                Vector2.zero,
                0.2f,
                ProjectileWeaponEffect.GetProjectileColor(weapon.Role)));
            return new BattleFireResult(BattleFireOutcome.Fired, _state.Turn.ActiveTeam, weapon, copter.Position);
        }

        private bool IsBomberBlocked(Vector2 center)
        {
            if (_terrain == null)
            {
                return false;
            }

            var grid = _terrain.Grid;
            const float half = BomberBodyHalfSize;
            return center.x - half < grid.WorldMinX || center.x + half > grid.WorldMaxX
                || center.y + half > grid.WorldMaxY
                || grid.IsSolidWorld(center)
                || grid.IsSolidWorld(center + new Vector2(half, 0f)) || grid.IsSolidWorld(center + new Vector2(-half, 0f))
                || grid.IsSolidWorld(center + new Vector2(0f, half)) || grid.IsSolidWorld(center + new Vector2(0f, -half));
        }

        /// <summary>
        /// The controlled copter: steered with the four direction buttons at its own speed, stopped by ground and
        /// walls, never exploding. It ends with the flight window or when the owner dies.
        /// </summary>
        private void StepBomberCopter(int projectileIndex, SimProjectile projectile, float deltaTime)
        {
            projectile.ReduceLifetime(deltaTime);
            var owner = FindUnit(projectile.OwnerUnitId);
            if (projectile.RemainingLifetime <= 0f || owner == null || owner.IsDefeated
                || _downedCopterProjectileIds.Remove(projectile.Id)
                || TrySpendHostileLaserBeamOnCopter(projectile))
            {
                EndBomberCopter(projectileIndex, projectile);
                return;
            }

            var weapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
            var velocity = projectile.Id == _controlledProjectileId
                ? _controlledProjectileInput * Mathf.Max(0.1f, weapon?.MaxPower ?? 1f)
                : Vector2.zero;
            if (_copter != null && _copter.IsCarrier && _copter.CopterProjectileId == projectile.Id && _copter.HasCargo)
            {
                FollowCarrierCargo(_copter, projectile.Position);
            }
            if (TryFindGrounderForProjectile(projectile, weapon, projectile.Position, out var attractor))
            {
                var toGrounder = attractor.Position - projectile.Position;
                if (toGrounder.sqrMagnitude > 0.0001f)
                {
                    velocity = toGrounder.normalized * Mathf.Max(4f, velocity.magnitude);
                }

                if (toGrounder.magnitude <= 0.45f)
                {
                    // Without an explosive payload the copter is simply destroyed at the magnet.
                    DamageGrounder(attractor, 1);
                    EndBomberCopter(projectileIndex, projectile);
                    return;
                }
            }

            projectile.SetVelocity(velocity);
            var step = velocity * deltaTime;
            var position = projectile.Position;
            if (!Mathf.Approximately(step.x, 0f) && !IsBomberBlocked(position + new Vector2(step.x, 0f)))
            {
                position.x += step.x;
            }

            if (!Mathf.Approximately(step.y, 0f) && !IsBomberBlocked(position + new Vector2(0f, step.y)))
            {
                position.y += step.y;
            }

            projectile.MoveTo(position);
            if (_copter != null && _copter.IsCarrier && _copter.CopterProjectileId == projectile.Id && _copter.HasCargo)
            {
                FollowCarrierCargo(_copter, position);
            }
        }

        private void EndBomberCopter(int projectileIndex, SimProjectile projectile)
        {
            var carrier = _copter != null && _copter.IsCarrier ? _copter : null;
            if (carrier != null)
            {
                ReleaseCarrierCargo(carrier);
            }

            _copter = null;
            if (_controlledProjectileId == projectile.Id)
            {
                _controlledProjectileId = -1;
                _controlledProjectileInput = Vector2.zero;
            }

            if (carrier == null)
            {
                ConcludeProjectileShot(projectileIndex, projectile);
                return;
            }

            // The carrier's window is over: it falls and blasts like a mine (damage and R0 of the weapon).
            var ownerId = projectile.OwnerUnitId;
            var position = projectile.Position;
            _projectiles.RemoveAt(projectileIndex);
            ProjectileExploded?.Invoke(projectile.Id, new ProjectileImpact(
                projectile.Definition.ProjectileId, ownerId, position, Vector2.zero, 0, 0f));
            var team = ResolveTeamForOwner(ownerId);
            var damage = Mathf.RoundToInt(carrier.Weapon.Damage * ResolveModifiersFor(team, carrier.Weapon).DamageMultiplier);
            Spawn(new ProjectileSpawnRequest(
                // Owner decision 2026-10-08: it blasts right where it is when the time runs out, it does not fall first.
                ProjectileDefinition.CreateRuntime(CarrierFallProjectileId, 0.2f, 0f, 0.01f, damage, carrier.Weapon.ImpactRadius, 1f, 0),
                ownerId, position, Vector2.zero, 0.4f, ProjectileWeaponEffect.GetProjectileColor(carrier.Weapon.Role)));
        }

        private BattleFireResult LaunchControlledDrone(
            BattleUnitState unit,
            WeaponInventoryRuntime inventory,
            WeaponDefinition weapon)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var evolution = ResolveEvolutionFor(unit.TeamId, weapon);
            var evolvedWeapon = CreateFpvEvolutionWeapon(weapon, evolution, ResolveModifiersFor(unit.TeamId, weapon));
            var controlDuration = weapon.TimeoutSeconds + evolution.FpvControlBonusSeconds;
            var spawnPoint = unit.Position + Vector2.up * 1.05f;
            var definition = evolvedWeapon.CreateProjectileDefinition(evolvedWeapon.Damage, 1f, 0, controlDuration);
            Spawn(new ProjectileSpawnRequest(
                definition,
                unit.UnitId,
                spawnPoint,
                Vector2.zero,
                weapon.ProjectileRadius * 2f,
                ProjectileWeaponEffect.GetProjectileColor(weapon.Role)));
            _controlledProjectileId = _projectiles[_projectiles.Count - 1].Id;
            _controlledProjectileInput = Vector2.zero;
            _activeShotWeapon = weapon;
            _activeShotEndsTurn = true;
            _turnService.SpendAction(_state);
            ShotFired?.Invoke(unit.TeamId, weapon, true);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon, spawnPoint);
        }

        private BattleFireResult LaunchAttachmentDevice(
            BattleUnitState unit,
            WeaponInventoryRuntime inventory,
            WeaponDefinition weapon)
        {
            if (!TryFindTargetFor(unit.UnitId, out var target)
                || (target.Position - unit.Position).sqrMagnitude > AttachmentDeviceRange * AttachmentDeviceRange)
            {
                // The device is not consumed and the turn stays unchanged until a
                // nearby hostile target exists. This avoids firing a visible
                // effect into empty space.
                return new BattleFireResult(BattleFireOutcome.Blocked, unit.TeamId, weapon);
            }

            // One virus per carrier: an infected target refuses a second device without spending it.
            if (weapon.WeaponId == VirusWeaponId && _virusInfections.ContainsKey(target.UnitId))
            {
                return new BattleFireResult(BattleFireOutcome.Blocked, unit.TeamId, weapon);
            }

            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var origin = unit.Position + Vector2.up * 0.38f;
            var direction = (target.Position - origin).normalized;
            // The attached devices deal no damage themselves; the virus' final damage comes from its status.
            var impactDamage = weapon.WeaponId == RepairJammerWeaponId || weapon.WeaponId == VirusWeaponId ? 0 : weapon.Damage;
            var definition = weapon.CreateProjectileDefinition(impactDamage, 0f, 0, StickyGrenadeFuseSeconds);
            Spawn(new ProjectileSpawnRequest(
                definition,
                unit.UnitId,
                origin,
                direction * AttachmentDeviceSpeed,
                weapon.ProjectileRadius * 2f,
                ProjectileWeaponEffect.GetProjectileColor(weapon.Role)));
            _activeShotWeapon = weapon;
            _activeShotEndsTurn = true;
            _turnService.SpendAction(_state);
            ShotFired?.Invoke(unit.TeamId, weapon, true);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon, target.Position);
        }

        private sealed class CopterSession
        {
            public CopterSession(string ownerUnitId, int copterProjectileId, WeaponDefinition weapon, int bombDamage, int bombs,
                bool isCarrier = false)
            {
                OwnerUnitId = ownerUnitId;
                CopterProjectileId = copterProjectileId;
                Weapon = weapon;
                BombDamage = bombDamage;
                BombsLeft = bombs;
                IsCarrier = isCarrier;
            }

            public bool IsCarrier { get; }
            public ActiveMine CarriedMine { get; set; }
            public ActiveLaserFenceNode CarriedNode { get; set; }
            public CarryablePickup CarriedPickup { get; set; }
            public bool HasCargo => CarriedMine != null || CarriedNode != null || CarriedPickup != null;

            public string OwnerUnitId { get; }
            public int CopterProjectileId { get; }
            public WeaponDefinition Weapon { get; }
            public int BombDamage { get; }
            public int BombsLeft { get; set; }
        }
    }
}
