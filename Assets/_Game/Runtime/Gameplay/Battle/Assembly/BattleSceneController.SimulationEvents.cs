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
        private void SubscribeToSimulation(BattleSimulation simulation)
        {
            simulation.ShotFired += OnSimulationShotFired;
            simulation.ImpactResolved += OnSimulationImpactResolved;
            simulation.DamageApplied += OnSimulationDamageApplied;
            simulation.TerrainCellsDestroyed += OnSimulationTerrainCellsDestroyed;
            simulation.TerrainCraterApplied += OnSimulationTerrainCraterApplied;
            simulation.UnitDefeated += OnSimulationUnitDefeated;
            simulation.TurnAdvanced += OnSimulationTurnAdvanced;
            simulation.BattleFinished += OnSimulationBattleFinished;
            simulation.CoverDeployed += OnSimulationCoverDeployed;
            simulation.HealthRestored += OnSimulationHealthRestored;
            simulation.OwnerRepositioned += OnSimulationOwnerRepositioned;
            simulation.ProjectileSpawned += OnSimulationProjectileSpawned;
            simulation.ProjectileExploded += OnSimulationProjectileExploded;
            simulation.MineDeployed += OnSimulationMineDeployed;
            simulation.MineTriggered += OnSimulationMineTriggered;
            simulation.GrounderDeployed += OnSimulationGrounderDeployed;
            simulation.PlacedDeviceHealthChanged += OnSimulationPlacedDeviceHealthChanged;
            simulation.GrounderRemoved += OnSimulationGrounderRemoved;
            simulation.GroundObjectCarried += OnSimulationGroundObjectCarried;
            simulation.LaserFenceLinkSpent += OnSimulationLaserFenceLinkSpent;
            simulation.LaserFenceNodeRemoved += OnSimulationLaserFenceNodeRemoved;
            simulation.ForceFieldDeployed += OnSimulationForceFieldDeployed;
            simulation.ForceFieldRemoved += OnSimulationForceFieldRemoved;
            simulation.ForceFieldAbsorbed += OnSimulationForceFieldAbsorbed;
            simulation.LaserFenceNodeDeployed += OnSimulationLaserFenceNodeDeployed;
            simulation.ChargeDeployed += OnSimulationChargeDeployed;
            simulation.GroundObjectRepositioned += OnSimulationGroundObjectRepositioned;
            simulation.LinearWeaponEffectStarted += OnSimulationLinearWeaponEffectStarted;
            simulation.FireBlobSpawned += OnSimulationFireBlobSpawned;
            simulation.SwordSwingStarted += OnSimulationSwordSwingStarted;
            simulation.FirePuddleMoved += OnSimulationFirePuddleMoved;
            simulation.FirePuddleExtinguished += OnSimulationFirePuddleExtinguished;
            simulation.UnitsMoved += OnSimulationUnitsMoved;
        }

        private void OnSimulationProjectileSpawned(SimProjectile projectile)
        {
            var projectileObject = new GameObject("Production Shell");
            _arenaObjects.Add(projectileObject);
            _projectileVisuals[projectile.Id] = projectileObject;
            projectileObject.transform.position = projectile.Position;

            var visualObject = new GameObject("Visual");
            visualObject.transform.SetParent(projectileObject.transform, false);
            ApplyProjectileVisualTransform(visualObject.transform, projectile);

            var weapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);

            var renderer = visualObject.AddComponent<SpriteRenderer>();
            renderer.sprite = ResolveProjectileSprite(projectile, weapon) ?? _circleSprite;
            renderer.color = renderer.sprite == _circleSprite ? ResolveProjectileVisualColor(weapon) : Color.white;
            renderer.sortingOrder = 30;

            if (weapon?.WeaponId == "weapon.fpv-drone")
            {
                var animator = visualObject.AddComponent<WeaponProjectileSpriteAnimator>();
                animator.SetFrames(LoadWeaponIconSprite("fpv_0"), LoadWeaponIconSprite("fpv_1"),
                    LoadWeaponIconSprite("fpv_2"), LoadWeaponIconSprite("fpv_3"), LoadWeaponIconSprite("fpv_4"));
            }
            else if (weapon?.WeaponId == "weapon.bur-rocket")
            {
                SpawnRocketTrail(visualObject.transform, renderer, true);
            }
            else if (weapon?.WeaponId == "weapon.sky-hook")
            {
                SpawnSkyHookTail(visualObject.transform, renderer);
            }
        }

        private void OnSimulationProjectileExploded(int projectileId, ProjectileImpact impact)
        {
            if (_projectileVisuals.TryGetValue(projectileId, out var projectileObject))
            {
                _projectileVisuals.Remove(projectileId);
                _arenaObjects.Remove(projectileObject);
                Destroy(projectileObject);
            }

            if (_mineVisuals.TryGetValue(projectileId, out var mineObject))
            {
                _mineVisuals.Remove(projectileId);
                _mineOwners.Remove(projectileId);
                _arenaObjects.Remove(mineObject);
                Destroy(mineObject);
            }

            var impactWeapon = ResolveWeaponFromProjectileId(impact.ProjectileId);
            if (impactWeapon?.WeaponId == "weapon.hopping-grenade")
            {
                _hideHeldWeaponForReusableThrow = false;
            }
            if (impactWeapon?.WeaponId == "weapon.teleport")
            {
                SpawnTeleportMarkerVisual(impact.Point);
            }

            if (impactWeapon?.WeaponId != "weapon.teleport"
                && impactWeapon?.WeaponId != "weapon.ion-shotgun"
                // Attaching the virus or the repair jammer is not an explosion.
                && impactWeapon?.WeaponId != "weapon.virus"
                && impactWeapon?.WeaponId != "weapon.repair-jammer"
                && impact.ProjectileId != "projectile.weapon.mine.deploy"
                && impact.ProjectileId != "projectile.weapon.laser-fence.deploy")
            {
                var isSmallMortarCharge = impact.ProjectileId != null
                    && (impact.ProjectileId.StartsWith("projectile.weapon.mortira.fragment", System.StringComparison.Ordinal)
                        || impact.ProjectileId.StartsWith("projectile.weapon.mortira.cascade.", System.StringComparison.Ordinal));
                var isMissileClusterCharge = impact.ProjectileId != null
                    && impact.ProjectileId.StartsWith("projectile.weapon.missle.cluster.", System.StringComparison.Ordinal);
                var flashScale = isSmallMortarCharge || isMissileClusterCharge ? 0.6f : 1.2f;
                SpawnImpactFlash(impact.Point, new Color(1f, 0.74f, 0.18f, 0.85f), flashScale);
            }
            if (!string.IsNullOrWhiteSpace(impact.OwnerUnitId)
                && impact.OwnerUnitId.StartsWith("player.", System.StringComparison.Ordinal))
            {
                _firstBattleGuide?.ObserveProjectileImpact();
                if (IsPlayerTurnShotSpent())
                {
                    // The turn's shot is spent: the next item stays out of the hands and there is nothing to aim.
                    if (_playerWeaponSelected)
                    {
                        _playerWeaponSelected = false;
                        ResetAimState();
                        RefreshPlayerWeaponVisual();
                    }
                }
                else if (ReturnsHeldWeaponAfterFlight(impactWeapon)
                    && _inventory?.Selected?.WeaponId == impactWeapon.WeaponId
                    && _inventory.GetSelectedRemainingAmmo() > 0)
                {
                    _playerWeaponSelected = true;
                    RefreshPlayerWeaponVisual();
                }
            }
        }

        private void OnSimulationMineDeployed(MineDeployedEvent mine)
        {
            var visual = new GameObject("Mine Armed");
            _arenaObjects.Add(visual);
            _mineVisuals[mine.MineId] = visual;
            _mineOwners[mine.MineId] = (mine.Weapon, mine.Team);
            visual.transform.position = mine.Point;
            var renderer = visual.AddComponent<SpriteRenderer>();
            if (mine.Weapon != null && mine.Weapon.WeaponId == "weapon.spider-mine")
            {
                var framePrefix = ResolveMineSpriteId("spider_mine", mine.Weapon, mine.Team) == "spider_mine_5A"
                    ? "spider_mine_5A_frame_"
                    : "spider_mine_frame_";
                var spiderAnimator = visual.AddComponent<WeaponProjectileSpriteAnimator>();
                spiderAnimator.SetFrames(
                    LoadWeaponIconSprite($"{framePrefix}00"),
                    LoadWeaponIconSprite($"{framePrefix}01"),
                    LoadWeaponIconSprite($"{framePrefix}02"),
                    LoadWeaponIconSprite($"{framePrefix}03"));
            }
            else if (mine.Weapon != null && mine.Weapon.WeaponId == "weapon.guardian-drone")
            {
                renderer.sprite = LoadWeaponIconSprite(ResolveMineSpriteId(mine.Weapon.IconId, mine.Weapon, mine.Team));
                // The drone is visible to everyone until the purchased stealth hides it from the
                // enemy; the owner always sees it, and it is revealed again when it fires.
                renderer.enabled = !(mine.ConcealedFromEnemy && mine.Team != BattleTeamId.Player);
            }
            else
            {
                renderer.sprite = LoadWeaponIconSprite(ResolveMineSpriteId("mine_off", mine.Weapon, mine.Team));
            }
            renderer.sortingOrder = 27;
            visual.transform.localScale = Vector3.one * GuardianPlacedVisualScale;
        }

        private void OnSimulationMineTriggered(int mineId)
        {
            if (_mineVisuals.TryGetValue(mineId, out var visual)
                && visual != null
                && visual.TryGetComponent<SpriteRenderer>(out var renderer))
            {
                var (weapon, team) = _mineOwners.TryGetValue(mineId, out var owner) ? owner : (null, BattleTeamId.None);
                if (weapon?.WeaponId == "weapon.guardian-drone")
                {
                    renderer.enabled = true;
                    _mineVisuals.Remove(mineId);
                    _mineOwners.Remove(mineId);
                    _arenaObjects.Remove(visual);
                    Destroy(visual, 0.28f);
                }
                else if (weapon == null || weapon.WeaponId != "weapon.spider-mine")
                {
                    renderer.sprite = LoadWeaponIconSprite(ResolveMineSpriteId("mine_on", weapon, team));
                }
            }
        }

        private void OnSimulationGrounderDeployed(GrounderDeployedEvent grounder)
        {
            var visual = new GameObject("Grounder Placed");
            _arenaObjects.Add(visual);
            _grounderVisuals[grounder.Id] = visual;
            _deviceTeams[grounder.Id] = grounder.Team;
            visual.transform.localScale = Vector3.one * GrounderPlacedVisualScale;
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadWeaponIconSprite(grounder.Weapon?.IconId ?? "grounder");
            renderer.sortingOrder = 27;
            // The magnet's base stands on the ground instead of being half sunk into it.
            visual.transform.position = grounder.Point + Vector2.up * GroundStandingLift(renderer.sprite, GrounderPlacedVisualScale);
        }

        /// <summary>
        /// How far above a ground deploy point (which sits <see cref="GroundDeployPointLift"/> over the surface) a
        /// centre-pivoted device sprite of this world scale must be raised so its bottom edge rests on the surface.
        /// </summary>
        private static float GroundStandingLift(Sprite sprite, float worldScale)
        {
            return sprite == null ? 0f : -sprite.bounds.min.y * worldScale - GroundDeployPointLift;
        }

        private void OnSimulationForceFieldDeployed(ForceFieldDeployedEvent field)
        {
            var visual = new GameObject("Force Field Placed");
            _arenaObjects.Add(visual);
            _forceFieldVisuals[field.Id] = visual;
            _deviceTeams[field.Id] = field.Team;
            visual.transform.position = field.Point;
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadWeaponIconSprite("force_field_sphere");
            renderer.sortingOrder = 26;
            if (renderer.sprite != null)
            {
                // The sphere art spans the field's diameter.
                visual.transform.localScale = Vector3.one * (field.Radius * 2f / renderer.sprite.bounds.size.x);
            }

            // The device itself stands on the ground at the centre of its sphere.
            var device = new GameObject("Force Field Device");
            device.transform.SetParent(visual.transform, false);
            var deviceRenderer = device.AddComponent<SpriteRenderer>();
            deviceRenderer.sprite = LoadWeaponIconSprite(field.Weapon?.IconId ?? "force_field");
            deviceRenderer.sortingOrder = 27;
            var parentScale = Mathf.Max(0.0001f, visual.transform.localScale.x);
            device.transform.localScale = Vector3.one * (GrounderPlacedVisualScale / parentScale);
            // Its bottom rests on the surface; the sphere stays centred on the deploy point around it.
            device.transform.localPosition = Vector3.up
                * (GroundStandingLift(deviceRenderer.sprite, GrounderPlacedVisualScale) / parentScale);
        }

        private void OnSimulationForceFieldRemoved(int fieldId)
        {
            _forceFieldPulseUntil.Remove(fieldId);
            if (!_forceFieldVisuals.TryGetValue(fieldId, out var visual))
            {
                return;
            }

            _forceFieldVisuals.Remove(fieldId);
            _arenaObjects.Remove(visual);
            Destroy(visual);
            RemoveDeviceHealthLabel(fieldId);
        }

        private void OnSimulationForceFieldAbsorbed(int fieldId, Vector2 point)
        {
            _forceFieldPulseUntil[fieldId] = Time.time + 0.3f;
            if (_forceFieldVisuals.TryGetValue(fieldId, out var visual) && visual != null
                && visual.TryGetComponent<SpriteRenderer>(out var renderer))
            {
                renderer.color = new Color(0.7f, 0.95f, 1f, 1f);
            }
        }

        private void UpdateForceFieldPulses()
        {
            if (_forceFieldPulseUntil.Count == 0)
            {
                return;
            }

            List<int> finished = null;
            foreach (var pair in _forceFieldPulseUntil)
            {
                if (Time.time < pair.Value)
                {
                    continue;
                }

                (finished ??= new List<int>()).Add(pair.Key);
            }

            if (finished == null)
            {
                return;
            }

            foreach (var id in finished)
            {
                _forceFieldPulseUntil.Remove(id);
                if (_forceFieldVisuals.TryGetValue(id, out var visual) && visual != null
                    && visual.TryGetComponent<SpriteRenderer>(out var renderer))
                {
                    renderer.color = Color.white;
                }
            }
        }

        private void OnSimulationGrounderRemoved(int grounderId)
        {
            if (!_grounderVisuals.TryGetValue(grounderId, out var visual))
            {
                return;
            }

            _grounderVisuals.Remove(grounderId);
            _arenaObjects.Remove(visual);
            Destroy(visual);
            RemoveDeviceHealthLabel(grounderId);
        }

        /// <summary>A placed magnet or force field shows its health above it, like a droid.</summary>
        private void OnSimulationPlacedDeviceHealthChanged(int deviceId, int health, int maxHealth)
        {
            if (!_deviceHealthLabels.TryGetValue(deviceId, out var label) || label == null)
            {
                var labelObject = new GameObject("Device Health");
                _arenaObjects.Add(labelObject);
                labelObject.transform.localScale = new Vector3(0.11f, 0.11f, 1f);
                label = labelObject.AddComponent<TextMesh>();
                label.alignment = TextAlignment.Center;
                label.anchor = TextAnchor.MiddleCenter;
                label.fontSize = 30;
                label.fontStyle = FontStyle.Bold;
                var font = Resources.Load<Font>("Fonts/Orbitron");
                if (font != null)
                {
                    label.font = font;
                    labelObject.GetComponent<MeshRenderer>().sharedMaterial = font.material;
                }

                labelObject.GetComponent<MeshRenderer>().sortingOrder = 33;
                _deviceHealthLabels[deviceId] = label;
            }

            var team = _deviceTeams.TryGetValue(deviceId, out var owner) ? owner : BattleTeamId.Player;
            label.color = team == BattleTeamId.Player ? new Color(0.06f, 1f, 0.10f, 1f) : new Color(1f, 0.05f, 0.03f, 1f);
            label.text = Mathf.Max(0, health).ToString();
            PlaceDeviceHealthLabel(deviceId);
        }

        private void PlaceDeviceHealthLabel(int deviceId)
        {
            if (!_deviceHealthLabels.TryGetValue(deviceId, out var label) || label == null)
            {
                return;
            }

            SpriteRenderer device = null;
            if (_grounderVisuals.TryGetValue(deviceId, out var grounder) && grounder != null)
            {
                device = grounder.GetComponent<SpriteRenderer>();
            }
            else if (_forceFieldVisuals.TryGetValue(deviceId, out var field) && field != null)
            {
                device = field.transform.Find("Force Field Device")?.GetComponent<SpriteRenderer>();
            }

            if (device != null)
            {
                // Just over the device, below the droid's own number.
                label.transform.position = new Vector3(device.bounds.center.x, device.bounds.max.y + 0.08f, 0f);
            }
        }

        private void RemoveDeviceHealthLabel(int deviceId)
        {
            _deviceTeams.Remove(deviceId);
            if (_deviceHealthLabels.TryGetValue(deviceId, out var label))
            {
                _deviceHealthLabels.Remove(deviceId);
                if (label != null)
                {
                    _arenaObjects.Remove(label.gameObject);
                    Destroy(label.gameObject);
                }
            }
        }

        private void OnSimulationLaserFenceNodeDeployed(LaserFenceNodeDeployedEvent node)
        {
            var visual = new GameObject("Laser Fence Node");
            _arenaObjects.Add(visual);
            _laserFenceNodeVisuals[node.NodeId] = visual;
            visual.transform.position = node.Point;
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadWeaponIconSprite(node.Weapon?.IconId ?? "laser_fence");
            renderer.sortingOrder = 27;
            // Half of the former 0.11: the hulls are small marks on the ground.
            visual.transform.localScale = Vector3.one * 0.055f;

            _laserFenceNodes[node.NodeId] = node;
            RebuildLaserFenceLinks();
        }

        private void OnSimulationLaserFenceLinkSpent(int firstNodeId, int secondNodeId)
        {
            _spentLaserFenceLinks.Add((firstNodeId, secondNodeId));
            RebuildLaserFenceLinks();
        }

        private void OnSimulationLaserFenceNodeRemoved(int nodeId)
        {
            if (_laserFenceNodeVisuals.TryGetValue(nodeId, out var visual))
            {
                _laserFenceNodeVisuals.Remove(nodeId);
                _arenaObjects.Remove(visual);
                if (visual != null)
                {
                    Destroy(visual);
                }
            }

            _laserFenceNodes.Remove(nodeId);
            _spentLaserFenceLinks.RemoveWhere(pair => pair.Item1 == nodeId || pair.Item2 == nodeId);
            RebuildLaserFenceLinks();
        }

        private void RebuildLaserFenceLinks()
        {
            foreach (var line in _laserFenceLinks)
            {
                if (line == null)
                {
                    continue;
                }

                _arenaObjects.Remove(line.gameObject);
                Destroy(line.gameObject);
            }

            _laserFenceLinks.Clear();
            var links = LaserFenceLinkResolver.Resolve(_laserFenceNodes.Values.ToList(),
                owner => _simulation != null ? _simulation.GetLaserFenceLinkRange(owner) : LaserFenceLinkRange);
            foreach (var link in links)
            {
                if (_spentLaserFenceLinks.Contains((link.FirstNodeId, link.SecondNodeId))
                    || _spentLaserFenceLinks.Contains((link.SecondNodeId, link.FirstNodeId)))
                {
                    continue;
                }

                if (!_laserFenceNodes.TryGetValue(link.FirstNodeId, out var first)
                    || !_laserFenceNodes.TryGetValue(link.SecondNodeId, out var second))
                {
                    continue;
                }

                var line = CreateLine("Laser Fence Link", 0.035f, LaserFenceLinkColor);
                line.sortingOrder = 26;
                line.enabled = true;
                line.SetPosition(0, first.Point);
                line.SetPosition(1, second.Point);
                _laserFenceLinks.Add(line);
                _arenaObjects.Add(line.gameObject);
            }
        }

        /// <summary>
        /// Rank/branch-aware mine sprite (armed/triggered), resolved from whichever
        /// side laid it. A raid or duel opponent fielding an evolved mine must look
        /// like it: art that lies about what is on the ground is worse than no art.
        /// </summary>
        private string ResolveMineSpriteId(string baseIconId, WeaponDefinition weapon, BattleTeamId team)
        {
            return ResolveEvolvedIconId(baseIconId, weapon, ProgressionForTeam(team));
        }

        private void OnSimulationChargeDeployed(ChargeDeployedEvent charge)
        {
            var visual = new GameObject("Ground Charge");
            _arenaObjects.Add(visual);
            _mineVisuals[charge.ChargeId] = visual;
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadWeaponIconSprite(charge.Weapon.IconId);
            renderer.sortingOrder = 27;
            // Same visual size as the charge held in the robot's hands
            // (weapon mount scale), so the drop does not "grow" the dynamite.
            visual.transform.localScale = Vector3.one * 0.085f;
            visual.AddComponent<GroundDropVisual>().Play(
                charge.Origin,
                charge.Point,
                charge.DropDurationSeconds);

            if (charge.Team == BattleTeamId.Player)
            {
                _playerWeaponSelected = false;
                RefreshPlayerWeaponVisual();
            }
        }

        private void OnSimulationGroundObjectCarried(int objectId, bool carried)
        {
            if (carried)
            {
                _releasedCargoIds.Remove(objectId);
            }
            else
            {
                _releasedCargoIds.Add(objectId);
            }

            GameObject visual = null;
            var pickup = FindResourcePickupBySimulationId(objectId);
            if (pickup != null)
            {
                pickup.IsCarried = carried;
                visual = pickup.Visual;
            }
            else if (!_mineVisuals.TryGetValue(objectId, out visual) && !_laserFenceNodeVisuals.TryGetValue(objectId, out visual))
            {
                return;
            }

            if (visual != null && visual.TryGetComponent<SpriteRenderer>(out var renderer))
            {
                renderer.sortingOrder = carried ? 31 : 27;
            }
        }

        private void OnSimulationGroundObjectRepositioned(GroundObjectRepositionedEvent groundObject)
        {
            var released = _releasedCargoIds.Remove(groundObject.ObjectId);
            var pickup = FindResourcePickupBySimulationId(groundObject.ObjectId);
            if (pickup != null)
            {
                var from = pickup.Visual != null ? (Vector2)pickup.Visual.transform.position : pickup.Position;
                pickup.Position = groundObject.Point + Vector2.up * pickup.VisualLift;
                if (pickup.Visual != null)
                {
                    MoveGroundObjectVisual(pickup.Visual, from, pickup.Position, released);
                }

                return;
            }

            if (_laserFenceNodeVisuals.TryGetValue(groundObject.ObjectId, out var nodeVisual) && nodeVisual != null
                && _laserFenceNodes.TryGetValue(groundObject.ObjectId, out var node))
            {
                MoveGroundObjectVisual(nodeVisual, nodeVisual.transform.position, groundObject.Point, released);
                _laserFenceNodes[groundObject.ObjectId] = new LaserFenceNodeDeployedEvent(
                    node.NodeId, node.OwnerUnitId, groundObject.Point, node.Weapon, node.Team);
                RebuildLaserFenceLinks();
                return;
            }

            // A magnet or a force field whose ground was blown away drops with it (its art stands on the point).
            if (_grounderVisuals.TryGetValue(groundObject.ObjectId, out var grounderVisual) && grounderVisual != null)
            {
                var renderer = grounderVisual.GetComponent<SpriteRenderer>();
                grounderVisual.transform.position = groundObject.Point
                    + Vector2.up * GroundStandingLift(renderer != null ? renderer.sprite : null, GrounderPlacedVisualScale);
                PlaceDeviceHealthLabel(groundObject.ObjectId);
                return;
            }

            if (_forceFieldVisuals.TryGetValue(groundObject.ObjectId, out var fieldVisual) && fieldVisual != null)
            {
                fieldVisual.transform.position = groundObject.Point;
                PlaceDeviceHealthLabel(groundObject.ObjectId);
                return;
            }

            if (!_mineVisuals.TryGetValue(groundObject.ObjectId, out var visual) || visual == null)
            {
                return;
            }

            if (visual.TryGetComponent<GroundDropVisual>(out var dropVisual))
            {
                Destroy(dropVisual);
            }

            MoveGroundObjectVisual(visual, visual.transform.position, groundObject.Point, released);
        }

        /// <summary>A released cargo falls visibly to its landing point; any other move is applied at once.</summary>
        private static void MoveGroundObjectVisual(GameObject visual, Vector2 from, Vector2 to, bool released)
        {
            if (visual.TryGetComponent<CargoFallVisual>(out var falling))
            {
                Destroy(falling);
            }

            if (released && to.y < from.y - 0.05f)
            {
                visual.AddComponent<CargoFallVisual>().Play(from, to);
                return;
            }

            visual.transform.position = to;
        }

        private void OnSimulationLinearWeaponEffectStarted(LinearWeaponEffectEvent effect)
        {
            var visual = new GameObject("Linear Weapon Effect");
            _arenaObjects.Add(visual);
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadWeaponIconSprite(effect.Weapon.WeaponId == "weapon.bur" ? "bur_lazer" : effect.Weapon.IconId);
            renderer.sortingOrder = 32;
            var animator = visual.AddComponent<LinearWeaponEffectVisual>();
            animator.SetThickness(effect.Weapon.WeaponId == "weapon.bur" ? 1.5f : 1f);
            animator.Play(effect.Origin, effect.Direction, effect.Range, effect.GrowthDurationSeconds);
            Destroy(visual, effect.LifetimeSeconds + 0.08f);
        }

        private void OnSimulationSwordSwingStarted(SwordSwingEvent swing)
        {
            var texture = Resources.Load<Texture2D>("UI/Weapons/plasma_sword");
            if (texture == null || texture.width <= 0 || texture.height <= 0)
            {
                return;
            }

            var visual = new GameObject("Plasma Sword Swing");
            _arenaObjects.Add(visual);
            visual.transform.position = swing.Origin;
            visual.transform.rotation = Quaternion.Euler(
                0f,
                0f,
                Mathf.Atan2(swing.Direction.y, swing.Direction.x) * Mathf.Rad2Deg);

            var hiltRenderer = visual.AddComponent<SpriteRenderer>();
            hiltRenderer.sprite = Sprite.Create(
                texture,
                // Unity imports this art at a different size than the original
                // source. These normalized crop bounds stay inside the texture.
                new Rect(texture.width * 0.0957f, 0f, texture.width * 0.5104f, texture.height * 0.5423f),
                new Vector2(0.53f, 0.71f),
                100f,
                0,
                SpriteMeshType.FullRect);
            hiltRenderer.sortingOrder = 33;
            // Match the selected sword's 2x hand presentation (0.085 * 2), so
            // the animated instance never shrinks when the held one is hidden.
            visual.transform.localScale = Vector3.one * 0.17f;

            var blade = new GameObject("Swinging Blade");
            blade.transform.SetParent(visual.transform, false);
            var bladeRenderer = blade.AddComponent<SpriteRenderer>();
            bladeRenderer.sprite = Sprite.Create(
                texture,
                new Rect(texture.width * 0.4226f, texture.height * 0.2871f, texture.width * 0.5774f, texture.height * 0.7129f),
                new Vector2(0.10f, 0.16f),
                100f,
                0,
                SpriteMeshType.FullRect);
            bladeRenderer.sortingOrder = 34;
            visual.AddComponent<WeaponSwingVisual>().Play(0.42f, 360f / 0.42f, blade.transform);

            if (swing.Team == BattleTeamId.Player)
            {
                _playerWeaponSelected = false;
                RefreshPlayerWeaponVisual();
                StartCoroutine(RestoreSwordWeaponVisualAfterSwing());
            }
        }

        private System.Collections.IEnumerator RestoreSwordWeaponVisualAfterSwing()
        {
            yield return new WaitForSeconds(0.43f);
            if (_inventory?.Selected?.WeaponId == "weapon.plasma-sword" && !IsPlayerTurnShotSpent())
            {
                _playerWeaponSelected = true;
                RefreshPlayerWeaponVisual();
            }
        }

        private void OnSimulationFireBlobSpawned(FireBlobEvent effect)
        {
            var visual = new GameObject("Fire Blob");
            _arenaObjects.Add(visual);
            visual.transform.position = effect.Origin;
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadWeaponIconSprite("fire_shot_1");
            renderer.sortingOrder = 32;
            var animator = visual.AddComponent<WeaponProjectileSpriteAnimator>();
            animator.SetFrames(LoadWeaponIconSprite("fire_shot_1"), LoadWeaponIconSprite("fire_shot_2"),
                LoadWeaponIconSprite("fire_shot_3"), LoadWeaponIconSprite("fire_shot_4"));
            var mover = visual.AddComponent<FireBlobVisual>();
            mover.Play(
                effect.Origin,
                effect.InitialVelocity,
                effect.Gravity,
                effect.ImpactPoint,
                effect.TravelSeconds,
                effect.LifetimeSeconds);
            // Half the former size: the incendiary grenade now throws twice as many smaller drops.
            visual.transform.localScale = Vector3.one * (effect.Weapon?.WeaponId == "weapon.incendiary-grenade" ? 0.036f : 0.072f);
            _firePuddleVisuals[effect.PuddleId] = mover;
            Destroy(visual, effect.TravelSeconds + effect.LifetimeSeconds + 0.25f);
        }

        private void OnSimulationFirePuddleMoved(int puddleId, Vector2 point)
        {
            if (_firePuddleVisuals.TryGetValue(puddleId, out var visual) && visual != null)
            {
                visual.SetBurnPoint(point);
            }
        }

        private void OnSimulationFirePuddleExtinguished(int puddleId)
        {
            if (_firePuddleVisuals.TryGetValue(puddleId, out var visual))
            {
                _firePuddleVisuals.Remove(puddleId);
                if (visual != null)
                {
                    Destroy(visual.gameObject);
                }
            }
        }

        private void SpawnTeleportMarkerVisual(Vector2 point)
        {
            var visual = new GameObject("Teleport Marker");
            _arenaObjects.Add(visual);
            visual.transform.position = point;
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadWeaponIconSprite("teleport");
            renderer.sortingOrder = 31;
            visual.transform.localScale = Vector3.one * 0.22f;
            Destroy(visual, 0.35f);
        }

        private void OnSimulationShotFired(BattleTeamId team, WeaponDefinition weapon, bool setsShotStatus)
        {
            ClearWalkInput();
            if (team == BattleTeamId.Player)
            {
                BattleActivityBridge.ReportShotFired(weapon?.WeaponId);
                RefreshArsenalHudItems();
                ClearSelectedWeaponIfDepleted(weapon);
            }
            if (setsShotStatus)
            {
                _status = GameText.ShotStatus(DescribeTeam(team), DescribeWeapon(weapon));
            }

            // Any weapon whose shot spends the turn's action hides its held skin and the aim afterwards.
            var spentTheTurnsShot = team == BattleTeamId.Player && _battleState.Turn.ActionSpent;
            if (team == BattleTeamId.Player
                && (spentTheTurnsShot
                    || HidesHeldWeaponAfterUse(weapon)
                        && (HidesHeldWeaponDuringFlight(weapon) || _inventory.GetSelectedRemainingAmmo() <= 0)))
            {
                var keepsHoppingControl = weapon?.WeaponId == "weapon.hopping-grenade" && !setsShotStatus;
                // The purchased early trigger needs the same weapon and aim for the second press.
                var keepsSphereControl = weapon?.WeaponId == "weapon.teleport" && _simulation.CanTriggerTeleportEarly();
                var keepsBomberControl = weapon?.WeaponId is "weapon.bomber" or "weapon.ufo-carrier" && _simulation.HasPlayerCopter;
                _hideHeldWeaponForReusableThrow = keepsHoppingControl || keepsSphereControl || keepsBomberControl;
                _playerWeaponSelected = keepsHoppingControl || keepsSphereControl || keepsBomberControl;
                RefreshPlayerWeaponVisual();
            }

            if (team != BattleTeamId.Player
                || (!_simulation.CanContinueBlasterBurst && weapon?.WeaponId != "weapon.hopping-grenade"
                    && !(weapon?.WeaponId == "weapon.teleport" && _simulation.CanTriggerTeleportEarly())))
            {
                ResetAimState();
            }
            else if (weapon?.WeaponId is "weapon.hopping-grenade" or "weapon.teleport")
            {
                // The aim is kept for the follow-up press (a jump, the early trigger) but no longer drawn.
                HideTrajectoryPreview();
            }

            if (team == BattleTeamId.Player && weapon?.WeaponId == TitaniumBarrierRules.WeaponId)
            {
                // The next beam starts over the droid again, not where the reset aim left it.
                _barrierPlacementInitialized = false;
            }
            EmitFeedback(BattleFeedbackEventType.ShotFired);
        }

        private void OnSimulationImpactResolved(ImpactResolvedEvent impact)
        {
            ReportTutorial(BattleTutorialBridge.ProjectileImpact);
            EmitFeedback(BattleFeedbackEventType.ProjectileImpact);
        }

        private void OnSimulationDamageApplied(BattleTeamId sourceTeam, DamageEvent damage)
        {
            if (sourceTeam == BattleTeamId.Player
                && damage.TargetTeam == BattleTeamId.Opponent
                && damage.Damage > 0)
            {
                BattleActivityBridge.ReportPlayerDamageDealt(damage.Damage);
            }
        }

        private void OnSimulationTerrainCellsDestroyed(BattleTeamId sourceTeam, int cells)
        {
            DropUnsupportedSupportPickups();

            if (sourceTeam == BattleTeamId.Player)
            {
                BattleActivityBridge.ReportTerrainDestroyed(cells);
            }
        }

        private void OnSimulationUnitDefeated(UnitDefeatedEvent defeat)
        {
            // Falling out of bounds is the only non-weapon death the simulation reports,
            // and it covers both the map edge and the kill zone barrier.
            if (defeat.Reason == "fall.out-of-bounds"
                && !string.IsNullOrEmpty(defeat.UnitId)
                && defeat.UnitId.StartsWith("opponent.", System.StringComparison.Ordinal))
            {
                _opponentRingOut = true;
            }

            EmitFeedback(BattleFeedbackEventType.UnitDefeated);
        }

        private void OnSimulationTerrainCraterApplied(BattleTeamId sourceTeam, Vector2 center, float radius)
        {
            _turnCraters.Add(new BattleCraterSnapshot(center, radius));
        }

        private void OnSimulationTurnAdvanced(BattleTeamId newActiveTeam, TurnAdvanceReason reason)
        {
            _duel?.PublishTurnBoundary();
            // Craters belong to the turn that cut them; the next turn starts clean.
            _turnCraters.Clear();
            _playerWeaponSelected = false;
            _controlledPointerActive = false;
            if (ShouldClearOrbitalTargetMarkerOnTurnAdvanced(reason))
            {
                ClearOrbitalTargetMarker(clearSimulation: false);
            }

            if (reason == TurnAdvanceReason.PlayerTimerExpired)
            {
                _firstBattleGuide?.ObservePlayerTurnExpired();
            }
            else
            {
                _firstBattleGuide?.ObserveTurnAdvanced(newActiveTeam);
            }

            ApplyAimSettingsForActiveTeam();
            ResetAimState();
            ClearWalkInput();

            var turnText = GameText.TurnStatus(DescribeTeam(newActiveTeam));
            _status = reason == TurnAdvanceReason.InstantActionCompleted ? $"{_status} {turnText}" : turnText;
            TryScheduleSupportDrop();
        }

        private void OnSimulationBattleFinished(BattleResult result, BattleFinishReason reason)
        {
            RecordFinishedBattleActivity(result);
            ReportObjectiveFacts(result);
            _arenaReward = GrantArenaReward(result);
            _resultScreenState = CreateResultScreenState(result, _battleState.Turn.TurnNumber, _arenaReward);
            _status = GameText.ResultPrompt(_resultScreenState.Title);
            if (result.Outcome == BattleOutcome.PlayerWon && reason != BattleFinishReason.InstantActionCompleted)
            {
                ReportTutorial(BattleTutorialBridge.BattleWon);
            }

            _firstBattleGuide?.ObserveBattleFinished();
            EmitFeedback(BattleFeedbackEventType.BattleFinished);
        }

        private void OnSimulationCoverDeployed(CoverDeployedEvent cover)
        {
            if (cover.Weapon?.WeaponId == "weapon.titanium-barrier")
            {
                var visual = new GameObject("Titanium Beam");
                visual.transform.position = cover.Position;
                visual.transform.rotation = Quaternion.Euler(0f, 0f, cover.RotationDegrees);
                var renderer = visual.AddComponent<SpriteRenderer>();
                renderer.sprite = cover.Surface.Sprite;
                renderer.color = Color.white;
                visual.transform.localScale = new Vector3(cover.Size.x / renderer.sprite.bounds.size.x,
                    cover.Size.y / renderer.sprite.bounds.size.y, 1f);
                renderer.sortingOrder = 18;
                _arenaObjects.Add(visual);
                _status = GameText.CoverDeployed(DescribeWeapon(cover.Weapon));
                return;
            }

            var box = CreateBox(
                $"Cover {cover.Weapon.WeaponId}",
                cover.Position,
                cover.Size,
                ProjectileWeaponEffect.GetProjectileColor(cover.Weapon.Role),
                18);
            _arenaObjects.Add(box);
            _status = GameText.CoverDeployed(DescribeWeapon(cover.Weapon));
        }

        private void OnSimulationHealthRestored(HealthRestoredEvent heal)
        {
            SpawnImpactFlash(heal.Point, new Color(0.26f, 0.84f, 0.62f, 0.85f), 1.45f);
            _status = GameText.ShieldPulse(DescribeTeam(heal.Team), heal.Amount);
        }

        private void OnSimulationOwnerRepositioned(string unitId)
        {
            if (string.IsNullOrEmpty(unitId))
            {
                _status = GameText.RepositionFailed();
                return;
            }

            var owner = _battleState.GetUnit(unitId);
            SpawnImpactFlash(owner.Position, new Color(0.66f, 0.42f, 0.96f, 0.8f), 1.05f);
            _status = GameText.Repositioned(DescribeTeam(owner.TeamId));
        }

        private void OnSimulationUnitsMoved()
        {
            TrackPlayerMovementActivity();
            CollectNearbyActivityResources();
            RefreshUnitViews();
            SyncProjectileVisuals();
            RefreshPlayerWeaponVisual();
        }

        private void SpawnImpactFlash(Vector2 position)
        {
            SpawnImpactFlash(position, new Color(1f, 0.74f, 0.18f, 0.85f), 1.2f);
        }

        private void SpawnImpactFlash(Vector2 position, Color color, float scale)
        {
            var flash = new GameObject("Impact Flash");
            _arenaObjects.Add(flash);
            flash.transform.position = new Vector3(position.x, position.y, -0.2f);
            flash.transform.localScale = Vector3.one * scale;

            var renderer = flash.AddComponent<SpriteRenderer>();
            renderer.sprite = _circleSprite;
            renderer.color = color;
            renderer.sortingOrder = 40;

            Destroy(flash, 0.25f);
        }
    }
}
