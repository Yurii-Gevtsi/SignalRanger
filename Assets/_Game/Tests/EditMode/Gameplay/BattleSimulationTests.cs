using System.Collections.Generic;
using System.Linq;
using Game.Data.Progression;
using Game.Data.Save;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Motion;
using Game.Gameplay.Battle.Projectiles;
using Game.Gameplay.Battle.Simulation;
using Game.Gameplay.Battle.Terrain;
using Game.Gameplay.Battle.Units;
using Game.Gameplay.Battle.Weapons;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class BattleSimulationTests
    {
        private sealed class Harness
        {
            public BattleState State;
            public BattleSimulation Simulation;
            public BattleTerrain Terrain;
            public Dictionary<string, UnitMotor> Motors;
        }

        private static CharacterArchetype CreateArchetype(string id, int maxHealth = 100)
        {
            return new CharacterArchetype(id, id, maxHealth, 1f, 3f, 3f, 0.4f, 1.6f, 0.9f, 1.1f, 0.7f);
        }

        private static WeaponDefinition CreateWeapon(
            string id,
            WeaponRole role = WeaponRole.ArcingExplosive,
            int damage = 60,
            float impactRadius = 2.5f,
            int ammo = -1,
            float gravityScale = 1f,
            float timeoutSeconds = 8f)
        {
            return WeaponDefinition.CreateRuntime(
                id, id + ".name", role,
                damage, impactRadius, 0.14f, gravityScale, timeoutSeconds, 6f, 20f, ammo, null, null);
        }

        private static Harness CreateHarness(
            WeaponDefinition playerWeapon,
            WeaponDefinition opponentWeapon = null,
            IReadOnlyDictionary<string, WeaponEvolutionProfile> playerWeaponEvolutions = null,
            Vector2? secondOpponentPosition = null,
            SaveWeaponProgressData playerWeaponProgress = null,
            Vector2? secondPlayerPosition = null, int playerMaxHealth = 100, float terrainDepth = 4f,
            SaveWeaponProgressData opponentWeaponProgress = null)
        {
            var bounds = new Rect(-10f, -terrainDepth - 4f, 20f, terrainDepth + 12f);
            var terrain = BattleTerrain.CreateProcedural(bounds);
            terrain.StampSolidRect(new Vector2(0f, -1f - terrainDepth * 0.5f), new Vector2(20f, terrainDepth));

            var playerArchetype = CreateArchetype("archetype.player", playerMaxHealth);
            var opponentArchetype = CreateArchetype("archetype.opponent");

            var playerUnit = new BattleUnitState("player.test.01", BattleTeamId.Player, new Vector2(-3f, 0f), playerArchetype.MaxHealth, 1);
            var opponentUnit = new BattleUnitState("opponent.test.01", BattleTeamId.Opponent, new Vector2(3f, 0f), opponentArchetype.MaxHealth, -1);
            var secondPlayer = secondPlayerPosition.HasValue
                ? new BattleUnitState("player.test.02", BattleTeamId.Player,
                    secondPlayerPosition.Value, playerArchetype.MaxHealth, 1)
                : null;
            var playerUnits = new List<BattleUnitState> { playerUnit };
            if (secondPlayer != null)
            {
                playerUnits.Add(secondPlayer);
            }
            var playerTeam = new BattleTeamState(BattleTeamId.Player, playerUnits);
            var secondOpponent = secondOpponentPosition.HasValue
                ? new BattleUnitState("opponent.test.02", BattleTeamId.Opponent,
                    secondOpponentPosition.Value, opponentArchetype.MaxHealth, -1)
                : null;
            var opponentUnits = new List<BattleUnitState> { opponentUnit };
            if (secondOpponent != null)
            {
                opponentUnits.Add(secondOpponent);
            }
            var opponentTeam = new BattleTeamState(BattleTeamId.Opponent, opponentUnits);
            var state = new BattleState(new[] { playerTeam, opponentTeam }, new TurnState(BattleTeamId.None, string.Empty, 0, false));

            var profile = new UnitMotionProfile(0.34f, 0.5f, 0.3f, 0.42f, 1.6f);
            var motors = new Dictionary<string, UnitMotor>(System.StringComparer.Ordinal)
            {
                [playerUnit.UnitId] = new UnitMotor(terrain.Grid, profile, playerUnit.Position, -6f),
                [opponentUnit.UnitId] = new UnitMotor(terrain.Grid, profile, opponentUnit.Position, -6f)
            };
            if (secondPlayer != null)
            {
                motors[secondPlayer.UnitId] = new UnitMotor(
                    terrain.Grid, profile, secondPlayer.Position, -6f);
            }
            if (secondOpponent != null)
            {
                motors[secondOpponent.UnitId] = new UnitMotor(
                    terrain.Grid, profile, secondOpponent.Position, -6f);
            }

            var resolvedOpponentWeapon = opponentWeapon ?? playerWeapon;
            var playerInventory = new WeaponInventoryRuntime(new[] { playerWeapon });
            var opponentInventory = new WeaponInventoryRuntime(new[] { resolvedOpponentWeapon });
            var executor = new WeaponExecutor();
            var weaponsById = new Dictionary<string, WeaponDefinition>(System.StringComparer.Ordinal)
            {
                [playerWeapon.WeaponId] = playerWeapon,
                [resolvedOpponentWeapon.WeaponId] = resolvedOpponentWeapon
            };

            var turnService = new TurnService();
            turnService.StartFirstTurn(state, BattleTeamId.Player);

            var settings = new BattleSimulationSettings(
                fallDeathY: -terrainDepth - 2f,
                simulationTickSeconds: 1f / 120f,
                playerTurnDurationSeconds: 30f,
                occupiedRadius: 0.72f,
                unitCollisionHalfWidth: 0.34f,
                knockbackLandingSearchPadding: 0.46f,
                groundRayStartHeight: 3.5f,
                groundRayDistance: 8f);

            var simulation = new BattleSimulation(
                state,
                turnService,
                terrain,
                motors,
                playerInventory,
                opponentInventory,
                executor,
                Physics2D.gravity,
                settings,
                weaponsById,
                playerArchetype,
                opponentArchetype,
                playerWeaponProgress != null
                    ? BattleWeaponProgression.FromResolver(new[] { playerWeapon },
                        weaponId => weaponId == playerWeapon.WeaponId ? playerWeaponProgress : null)
                    : BattleWeaponProgression.FromEvolutions(playerWeaponEvolutions),
                opponentWeaponProgress != null
                    ? BattleWeaponProgression.FromResolver(new[] { resolvedOpponentWeapon },
                        weaponId => weaponId == resolvedOpponentWeapon.WeaponId ? opponentWeaponProgress : null)
                    : null);

            return new Harness
            {
                State = state,
                Simulation = simulation,
                Terrain = terrain,
                Motors = motors
            };
        }

        [Test]
        public void Fire_TicksProjectile_DamagesAndKnocksBackTarget_AndAdvancesTurn()
        {
            var weapon = CreateWeapon("weapon.test.arc", gravityScale: 0f);
            var harness = CreateHarness(weapon);
            var opponent = harness.State.GetUnit("opponent.test.01");
            var opponentMotor = harness.Motors["opponent.test.01"];
            var startHealth = opponent.Health;

            var spawned = false;
            var exploded = false;
            var turnAdvanced = false;
            var turnAdvanceReason = TurnAdvanceReason.PlayerTimerExpired;
            harness.Simulation.ProjectileSpawned += projectile => spawned = true;
            harness.Simulation.ProjectileExploded += (projectileId, impact) => exploded = true;
            harness.Simulation.TurnAdvanced += (team, reason) =>
            {
                turnAdvanced = true;
                turnAdvanceReason = reason;
            };

            var aim = new AimState(new Vector2(-3f, 0f), Vector2.right, 20f, true);
            var fireResult = harness.Simulation.TryFire(aim);

            Assert.AreEqual(BattleFireOutcome.Fired, fireResult.Outcome);
            Assert.IsTrue(spawned, "default weapon fire spawns a simulation projectile");
            Assert.AreEqual(1, harness.Simulation.ProjectilesInFlight);

            for (var i = 0; i < 240 && !exploded; i++)
            {
                harness.Simulation.Tick(1f / 60f);
            }

            Assert.IsTrue(exploded, "projectile resolves inside the simulation");
            Assert.AreEqual(0, harness.Simulation.ProjectilesInFlight);
            Assert.Less(opponent.Health, startHealth, "radial damage reduced the target's health");
            Assert.IsFalse(opponentMotor.IsStable, "explosion knockback pushed the target off its feet");
            Assert.IsFalse(turnAdvanced, "the player keeps a movement-only retreat window before the turn hands over");
            Assert.IsTrue(harness.Simulation.IsPlayerRetreatActive);
            Assert.IsTrue(harness.Simulation.IsAttackCommitted, "weapon switching is blocked during the retreat window");

            for (var i = 0; i < 420 && !turnAdvanced; i++)
            {
                harness.Simulation.Tick(1f / 60f);
            }

            Assert.IsTrue(turnAdvanced, "turn advances once the retreat window runs out");
            Assert.AreEqual(TurnAdvanceReason.ProjectileImpactResolved, turnAdvanceReason);
            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam);
        }

        [Test]
        public void PlasmaSword_HitsOneNearbyEnemyForConfiguredFixedDamage()
        {
            var sword = CreateWeapon("weapon.plasma-sword", WeaponRole.ShortRangeBurst, 45, 0.55f);
            var harness = CreateHarness(sword);
            var opponent = harness.State.GetUnit("opponent.test.01");
            var opponentPosition = new Vector2(-1.5f, 0f);
            opponent.MoveTo(opponentPosition);
            harness.Motors[opponent.UnitId].Teleport(opponentPosition);
            SettleUnits(harness);

            var result = harness.Simulation.TryFire(new AimState(
                new Vector2(-3f, 0f), Vector2.right, 2f, false));

            Assert.That(result.Outcome, Is.EqualTo(BattleFireOutcome.BurstFired));
            Assert.That(result.HitCount, Is.EqualTo(1));
            Assert.That(opponent.Health, Is.EqualTo(55),
                "plasma sword damage is the fixed configured value, not a percentage of target health");
            Assert.That(harness.State.GetUnit("player.test.01").Health, Is.EqualTo(100));
            Assert.That(harness.Motors[opponent.UnitId].Velocity.x, Is.GreaterThan(0f),
                "the sword uses the standard blast knockback on its one target");
        }

        [Test]
        public void PlasmaSword_PurchasedCoreChangesTheActualMeleeDamage()
        {
            var sword = CreateWeapon("weapon.plasma-sword", WeaponRole.ShortRangeBurst, 45, 0.55f);
            var progress = new SaveWeaponProgressData
            {
                weaponId = sword.WeaponId,
                rank = 2,
                upgradeChoices = new List<SaveWeaponUpgradeChoiceData>
                {
                    new SaveWeaponUpgradeChoiceData { rank = 2, path = (int)WeaponUpgradePath.Core }
                }
            };
            var harness = CreateHarness(sword, playerWeaponProgress: progress);
            var opponent = harness.State.GetUnit("opponent.test.01");
            var position = new Vector2(-1.5f, 0f);
            opponent.MoveTo(position);
            harness.Motors[opponent.UnitId].Teleport(position);
            SettleUnits(harness);

            Assert.That(harness.Simulation.TryFire(new AimState(
                new Vector2(-3f, 0f), Vector2.right, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.BurstFired));
            Assert.That(opponent.Health, Is.EqualTo(32));
            Assert.That(harness.Motors[opponent.UnitId].Velocity.x, Is.GreaterThan(0f));
        }

        [TestCase("weapon.impulse", 1f)]
        [TestCase("weapon.impulse", 30f)]
        [TestCase("weapon.granata", 1f)]
        [TestCase("weapon.granata", 30f)]
        [TestCase("weapon.ion-shotgun", 1f)]
        [TestCase("weapon.ion-shotgun", 30f)]
        public void StarterProjectiles_IgnoreInputPowerAndUseSharedLaunchSpeed(
            string weaponId, float inputPower)
        {
            var weapon = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                candidate => candidate.WeaponId == weaponId);
            var harness = CreateHarness(weapon);

            Assert.That(harness.Simulation.TryFire(new AimState(
                new Vector2(-3f, 0f), Vector2.right, inputPower, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            Assert.That(harness.Simulation.Projectiles, Is.Not.Empty);
            foreach (var projectile in harness.Simulation.Projectiles)
            {
                Assert.That(projectile.Velocity.magnitude, Is.EqualTo(12f).Within(0.0001f));
            }
        }

        [TestCase(0, 8f)]
        [TestCase(1, 8.8f)]
        [TestCase(4, 11.2f)]
        public void Rope_LengthFollowsPurchasedRadiusNotRank(int purchases, float expectedLength)
        {
            var rope = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                candidate => candidate.WeaponId == "weapon.rope");
            var progress = new SaveWeaponProgressData
            {
                weaponId = rope.WeaponId,
                rank = 10,
                upgradeChoices = new List<SaveWeaponUpgradeChoiceData>()
            };
            for (var rank = 2; rank <= 10; rank++)
            {
                progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                {
                    rank = rank,
                    path = (int)(rank <= purchases + 1
                        ? WeaponUpgradePath.Radius : WeaponUpgradePath.Aim)
                });
            }

            var harness = CreateHarness(rope, playerWeaponProgress: progress);
            Assert.That(harness.Simulation.TryFire(new AimState(
                new Vector2(-3f, 0f), Vector2.up, 0f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            var field = typeof(BattleSimulation).GetField("_ropeTraversal",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.That(field, Is.Not.Null);
            var traversal = field.GetValue(harness.Simulation) as RopeTraversal;
            Assert.That(traversal, Is.Not.Null);
            Assert.That(traversal.MaximumLength, Is.EqualTo(expectedLength).Within(0.0001f));
            Assert.That(WeaponUpgradeChoiceResolver.ResolveValue(rope, null, progress,
                WeaponUpgradePath.Radius), Is.EqualTo(expectedLength).Within(0.0001f));
        }

        [Test]
        public void UnimplementedUtility_CannotHealOrTeleportThroughGenericFallback()
        {
            foreach (var (id, role) in new[]
            {
                ("weapon.force-field", WeaponRole.DefensiveTool),
                ("weapon.boost", WeaponRole.MovementTool)
            })
            {
                var weapon = CreateWeapon(id, role, ammo: 1);
                var harness = CreateHarness(weapon);
                var player = harness.State.GetUnit("player.test.01");
                player.ApplyDamage(20, "test");
                var beforePosition = player.Position;

                Assert.That(harness.Simulation.CanUsePlayerWeapon(weapon), Is.False, id);
                Assert.That(harness.Simulation.TryFire(new AimState(
                    player.Position, Vector2.right, 10f, true)).Outcome,
                    Is.EqualTo(BattleFireOutcome.Blocked), id);
                Assert.That(player.Health, Is.EqualTo(80), id);
                Assert.That(player.Position, Is.EqualTo(beforePosition), id);
            }
        }

        [Test]
        public void Grounder_PlacesWithoutAimAndDoesNotHealOwner()
        {
            var grounder = CreateWeapon("weapon.grounder", WeaponRole.DefensiveTool,
                damage: 0, ammo: 1);
            var harness = CreateHarness(grounder);
            var player = harness.State.GetUnit("player.test.01");
            player.ApplyDamage(20, "test");
            var deployed = false;
            harness.Simulation.GrounderDeployed += evt => deployed = true;

            var result = harness.Simulation.TryFire(
                new AimState(Vector2.zero, Vector2.zero, 0f, false));

            Assert.That(result.Outcome, Is.EqualTo(BattleFireOutcome.UtilityUsed));
            Assert.That(deployed, Is.True);
            Assert.That(player.Health, Is.EqualTo(80));
            Assert.That(harness.Simulation.CanUsePlayerWeapon(grounder), Is.False);
        }

        [Test]
        public void Grounder_PullsHostilePhysicalShotIntoDevice()
        {
            var grounder = CreateWeapon("weapon.grounder", WeaponRole.DefensiveTool,
                damage: 0, impactRadius: 1.4f, ammo: 1);
            var physicalShot = CreateWeapon("weapon.blaster-caset", WeaponRole.DirectProjectile,
                damage: 24, impactRadius: 0.45f, gravityScale: 0f);
            var harness = CreateHarness(grounder, opponentWeapon: physicalShot);
            var devicePosition = default(Vector2?);
            harness.Simulation.GrounderDeployed += evt => devicePosition = evt.Point;
            Assert.That(harness.Simulation.TryFire(
                new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));
            Assert.That(devicePosition.HasValue, Is.True);

            var player = harness.State.GetUnit("player.test.01");
            player.MoveTo(new Vector2(-8f, 0f));
            harness.Motors[player.UnitId].Teleport(player.Position);
            var shot = ProjectileDefinition.CreateRuntime(
                "projectile.weapon.blaster-caset", 0.1f, 0f, 0.8f, 24, 0.45f);
            var shotOrigin = devicePosition.Value + new Vector2(1.4f, 1f);
            var impactPoint = default(Vector2?);
            harness.Simulation.ProjectileExploded += (id, impact) =>
            {
                if (impact.ProjectileId == shot.ProjectileId)
                {
                    impactPoint = impact.Point;
                }
            };
            harness.Simulation.Spawn(new ProjectileSpawnRequest(
                shot, "opponent.test.01", shotOrigin, Vector2.left * 5f,
                0.2f, Color.white));
            TickMany(harness.Simulation, 120, 1f / 120f);

            Assert.That(impactPoint.HasValue, Is.True);
            Assert.That(Vector2.Distance(impactPoint.Value, devicePosition.Value),
                Is.LessThan(0.55f), "a hostile physical shot must detonate at the magnet");
        }

        [Test]
        public void Grounder_ExpiresAfterTwoOpposingTurns()
        {
            var grounder = CreateWeapon("weapon.grounder", WeaponRole.DefensiveTool,
                damage: 0, ammo: 1);
            var harness = CreateHarness(grounder);
            var removed = 0;
            harness.Simulation.GrounderRemoved += _ => removed++;

            Assert.That(harness.Simulation.TryFire(
                new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));
            Assert.That(removed, Is.Zero);

            harness.Simulation.AdvanceTurnDueToNoUsableWeapon(); // opponent's first turn ends
            Assert.That(removed, Is.Zero);
            harness.Simulation.AdvanceTurnDueToNoUsableWeapon(); // player's turn ends
            Assert.That(removed, Is.Zero);
            harness.Simulation.AdvanceTurnDueToNoUsableWeapon(); // opponent's second turn ends
            Assert.That(removed, Is.EqualTo(1));
        }

        [Test]
        public void Grounder_FourCorePurchasesExtendLifetimeToSixOpposingTurns()
        {
            var grounder = CreateWeapon("weapon.grounder", WeaponRole.DefensiveTool,
                damage: 0, impactRadius: 1.4f, ammo: 1);
            var progress = new SaveWeaponProgressData
            {
                weaponId = grounder.WeaponId,
                rank = 5,
                upgradeChoices = new List<SaveWeaponUpgradeChoiceData>
                {
                    new() { rank = 2, path = (int)WeaponUpgradePath.Core },
                    new() { rank = 3, path = (int)WeaponUpgradePath.Core },
                    new() { rank = 4, path = (int)WeaponUpgradePath.Core },
                    new() { rank = 5, path = (int)WeaponUpgradePath.Core }
                }
            };
            var harness = CreateHarness(grounder, playerWeaponProgress: progress);
            var removed = 0;
            harness.Simulation.GrounderRemoved += _ => removed++;
            Assert.That(harness.Simulation.TryFire(
                new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));

            for (var opponentTurn = 1; opponentTurn <= 6; opponentTurn++)
            {
                harness.Simulation.AdvanceTurnDueToNoUsableWeapon();
                Assert.That(removed, Is.EqualTo(opponentTurn == 6 ? 1 : 0));
                if (opponentTurn < 6)
                {
                    harness.Simulation.AdvanceTurnDueToNoUsableWeapon();
                    Assert.That(removed, Is.Zero);
                }
            }
        }

        [Test]
        public void Grounder_RadiusPurchasesCapturePhysicalShotOutsideBaseRange()
        {
            var grounder = CreateWeapon("weapon.grounder", WeaponRole.DefensiveTool,
                damage: 0, impactRadius: 1.4f, ammo: 1);
            var physicalShot = CreateWeapon("weapon.blaster-caset", WeaponRole.DirectProjectile,
                damage: 24, impactRadius: 0.45f, gravityScale: 0f);
            var progress = new SaveWeaponProgressData
            {
                weaponId = grounder.WeaponId,
                rank = 5,
                upgradeChoices = new List<SaveWeaponUpgradeChoiceData>
                {
                    new() { rank = 2, path = (int)WeaponUpgradePath.Radius },
                    new() { rank = 3, path = (int)WeaponUpgradePath.Radius },
                    new() { rank = 4, path = (int)WeaponUpgradePath.Radius },
                    new() { rank = 5, path = (int)WeaponUpgradePath.Radius }
                }
            };
            var harness = CreateHarness(grounder, opponentWeapon: physicalShot,
                playerWeaponProgress: progress);
            var devicePosition = default(Vector2?);
            harness.Simulation.GrounderDeployed += evt => devicePosition = evt.Point;
            Assert.That(harness.Simulation.TryFire(
                new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));
            Assert.That(devicePosition.HasValue, Is.True);
            var player = harness.State.GetUnit("player.test.01");
            player.MoveTo(new Vector2(-8f, 0f));
            harness.Motors[player.UnitId].Teleport(player.Position);

            var impactPoint = default(Vector2?);
            var shot = ProjectileDefinition.CreateRuntime(
                "projectile.weapon.blaster-caset", 0.1f, 0f, 3f, 24, 0.45f);
            harness.Simulation.ProjectileExploded += (_, impact) =>
            {
                if (impact.ProjectileId == shot.ProjectileId)
                {
                    impactPoint = impact.Point;
                }
            };
            harness.Simulation.Spawn(new ProjectileSpawnRequest(
                shot, "opponent.test.01", devicePosition.Value + Vector2.up * 1.7f,
                Vector2.zero, 0.2f, Color.white));
            TickMany(harness.Simulation, 120, 1f / 120f);

            Assert.That(impactPoint.HasValue, Is.True,
                "the upgraded magnet must pull a stationary physical shot from beyond base N0");
            Assert.That(Vector2.Distance(impactPoint.Value, devicePosition.Value),
                Is.LessThan(0.55f));
        }

        [Test]
        public void Grounder_TwoPhysicalImpactsDestroyDeviceAndBothExplode()
        {
            var grounder = CreateWeapon("weapon.grounder", WeaponRole.DefensiveTool,
                damage: 0, impactRadius: 1.4f, ammo: 1);
            var physicalShot = CreateWeapon("weapon.blaster-caset", WeaponRole.DirectProjectile,
                damage: 24, impactRadius: 0.45f, gravityScale: 0f);
            var harness = CreateHarness(grounder, opponentWeapon: physicalShot);
            var devicePosition = default(Vector2?);
            var removed = 0;
            var impacts = 0;
            harness.Simulation.GrounderDeployed += evt => devicePosition = evt.Point;
            harness.Simulation.GrounderRemoved += _ => removed++;
            harness.Simulation.ProjectileExploded += (_, impact) =>
            {
                if (impact.ProjectileId == "projectile.weapon.blaster-caset")
                {
                    impacts++;
                }
            };
            Assert.That(harness.Simulation.TryFire(
                new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));
            Assert.That(devicePosition.HasValue, Is.True);

            var player = harness.State.GetUnit("player.test.01");
            player.MoveTo(new Vector2(-8f, 0f));
            harness.Motors[player.UnitId].Teleport(player.Position);
            var shot = ProjectileDefinition.CreateRuntime(
                "projectile.weapon.blaster-caset", 0.1f, 0f, 0.8f, 24, 0.45f);
            for (var hit = 0; hit < 2; hit++)
            {
                harness.Simulation.Spawn(new ProjectileSpawnRequest(
                    shot, "opponent.test.01", devicePosition.Value + new Vector2(1.4f, 1f),
                    Vector2.left * 5f, 0.2f, Color.white));
                TickMany(harness.Simulation, 120, 1f / 120f);
                Assert.That(impacts, Is.EqualTo(hit + 1));
                Assert.That(removed, Is.EqualTo(hit == 0 ? 0 : 1));
            }
        }

        [TestCase("weapon.blaster-caset", true)]
        [TestCase("weapon.impulse", false)]
        public void Grounder_IgnoresFriendlyPhysicalAndHostileEnergyShots(
            string shotWeaponId, bool friendly)
        {
            var grounder = CreateWeapon("weapon.grounder", WeaponRole.DefensiveTool,
                damage: 0, impactRadius: 1.4f, ammo: 1);
            var shotWeapon = CreateWeapon(shotWeaponId, WeaponRole.DirectProjectile,
                damage: 24, impactRadius: 0.45f, gravityScale: 0f);
            var harness = CreateHarness(grounder, opponentWeapon: shotWeapon);
            var devicePosition = default(Vector2?);
            var impacts = 0;
            var removed = 0;
            harness.Simulation.GrounderDeployed += evt => devicePosition = evt.Point;
            harness.Simulation.GrounderRemoved += _ => removed++;
            harness.Simulation.ProjectileExploded += (_, impact) =>
            {
                if (impact.ProjectileId == "projectile." + shotWeaponId)
                {
                    impacts++;
                }
            };
            Assert.That(harness.Simulation.TryFire(
                new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));
            Assert.That(devicePosition.HasValue, Is.True);

            var player = harness.State.GetUnit("player.test.01");
            player.MoveTo(new Vector2(-8f, 0f));
            harness.Motors[player.UnitId].Teleport(player.Position);
            var shot = ProjectileDefinition.CreateRuntime(
                "projectile." + shotWeaponId, 0.1f, 0f, 3f, 24, 0.45f);
            harness.Simulation.Spawn(new ProjectileSpawnRequest(
                shot, friendly ? player.UnitId : "opponent.test.01",
                devicePosition.Value + Vector2.up, Vector2.zero, 0.2f, Color.white));
            TickMany(harness.Simulation, 120, 1f / 120f);

            Assert.That(impacts, Is.Zero, "this projectile must not be attracted or detonated by the magnet");
            Assert.That(removed, Is.Zero);
        }

        [TestCase("weapon.rope")]
        [TestCase("weapon.electric-harpoon")]
        public void Grounder_InterceptsHostilePhysicalHookBeforeItAttaches(string hookWeaponId)
        {
            var grounder = CreateWeapon("weapon.grounder", WeaponRole.DefensiveTool,
                damage: 0, impactRadius: 1.4f, ammo: 1);
            var hookWeapon = CreateWeapon(hookWeaponId, WeaponRole.DirectProjectile,
                damage: 20, impactRadius: 0.2f);
            var harness = CreateHarness(grounder, opponentWeapon: hookWeapon);
            var devicePosition = default(Vector2?);
            harness.Simulation.GrounderDeployed += evt => devicePosition = evt.Point;
            Assert.That(harness.Simulation.TryFire(
                new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));
            Assert.That(devicePosition.HasValue, Is.True);

            var origin = devicePosition.Value + Vector2.up * 0.5f;
            Assert.That(harness.Simulation.TryFire(new AimState(
                origin, Vector2.up, 10f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 20, 1f / 120f);

            var hookPosition = hookWeaponId == "weapon.rope"
                ? harness.Simulation.RopeHookPosition
                : harness.Simulation.ElectricHarpoonHookPosition;
            Assert.That(hookPosition, Is.EqualTo(Vector2.zero),
                "a hostile material hook must be intercepted before attaching");
            Assert.That(harness.State.GetUnit("player.test.01").Health, Is.EqualTo(100));
        }

        [Test]
        public void Grounder_DetonatesInterceptedMineInsteadOfDeployingIt()
        {
            var grounder = CreateWeapon("weapon.grounder", WeaponRole.DefensiveTool,
                damage: 0, impactRadius: 1.4f, ammo: 1);
            var mine = CreateWeapon("weapon.mine", WeaponRole.ArcingExplosive,
                damage: 24, impactRadius: 1.1f);
            var harness = CreateHarness(grounder, opponentWeapon: mine);
            var devicePosition = default(Vector2?);
            var deployedMines = 0;
            var impactDamage = -1;
            harness.Simulation.GrounderDeployed += evt => devicePosition = evt.Point;
            harness.Simulation.MineDeployed += _ => deployedMines++;
            harness.Simulation.ProjectileExploded += (_, impact) =>
            {
                if (impact.ProjectileId == "projectile.weapon.mine.deploy")
                {
                    impactDamage = impact.Damage;
                }
            };
            Assert.That(harness.Simulation.TryFire(
                new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));
            Assert.That(devicePosition.HasValue, Is.True);

            var player = harness.State.GetUnit("player.test.01");
            player.MoveTo(new Vector2(-8f, 0f));
            harness.Motors[player.UnitId].Teleport(player.Position);
            var deployShot = ProjectileDefinition.CreateRuntime(
                "projectile.weapon.mine.deploy", 0.1f, 0f, 3f, 0, mine.ImpactRadius);
            harness.Simulation.Spawn(new ProjectileSpawnRequest(
                deployShot, "opponent.test.01", devicePosition.Value + Vector2.up,
                Vector2.zero, 0.2f, Color.white));
            TickMany(harness.Simulation, 120, 1f / 120f);

            Assert.That(impactDamage, Is.EqualTo(mine.Damage));
            Assert.That(deployedMines, Is.Zero);
        }

        [TestCase("weapon.teleport")]
        [TestCase("weapon.swap-jump")]
        public void Grounder_NeutralizesInterceptedTeleportSphere(string sphereWeaponId)
        {
            var grounder = CreateWeapon("weapon.grounder", WeaponRole.DefensiveTool,
                damage: 0, impactRadius: 1.4f, ammo: 1);
            var sphere = CreateWeapon(sphereWeaponId, WeaponRole.MovementTool,
                damage: 0, impactRadius: 1.4f);
            var harness = CreateHarness(grounder, opponentWeapon: sphere);
            var devicePosition = default(Vector2?);
            harness.Simulation.GrounderDeployed += evt => devicePosition = evt.Point;
            Assert.That(harness.Simulation.TryFire(
                new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));
            Assert.That(devicePosition.HasValue, Is.True);

            var opponent = harness.State.GetUnit("opponent.test.01");
            var origin = opponent.Position;
            var deployShot = ProjectileDefinition.CreateRuntime(
                "projectile." + sphereWeaponId, 0.1f, 0f, 3f, 0, sphere.ImpactRadius);
            harness.Simulation.Spawn(new ProjectileSpawnRequest(
                deployShot, opponent.UnitId, devicePosition.Value + Vector2.up,
                Vector2.zero, 0.2f, Color.white));
            TickMany(harness.Simulation, 120, 1f / 120f);

            Assert.That(opponent.Position, Is.EqualTo(origin),
                "an intercepted sphere must not teleport its owner");
        }

        [TestCase("weapon.guardian-drone", "projectile.weapon.guardian-drone.deploy")]
        [TestCase("weapon.laser-fence", "projectile.weapon.laser-fence.deploy")]
        public void Grounder_DestroysInterceptedNonExplosiveDeployable(
            string weaponId, string projectileId)
        {
            var grounder = CreateWeapon("weapon.grounder", WeaponRole.DefensiveTool,
                damage: 0, impactRadius: 1.4f, ammo: 1);
            var deployable = CreateWeapon(weaponId, WeaponRole.DefensiveTool,
                damage: 24, impactRadius: 1.1f);
            var harness = CreateHarness(grounder, opponentWeapon: deployable);
            var devicePosition = default(Vector2?);
            var deployed = 0;
            harness.Simulation.GrounderDeployed += evt => devicePosition = evt.Point;
            harness.Simulation.MineDeployed += _ => deployed++;
            harness.Simulation.LaserFenceNodeDeployed += _ => deployed++;
            Assert.That(harness.Simulation.TryFire(
                new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));
            Assert.That(devicePosition.HasValue, Is.True);

            var deployShot = ProjectileDefinition.CreateRuntime(
                projectileId, 0.1f, 0f, 3f, 0, deployable.ImpactRadius);
            harness.Simulation.Spawn(new ProjectileSpawnRequest(
                deployShot, "opponent.test.01", devicePosition.Value + Vector2.up,
                Vector2.zero, 0.2f, Color.white));
            TickMany(harness.Simulation, 120, 1f / 120f);

            Assert.That(deployed, Is.Zero, "the intercepted carrier must not deploy a trap");
        }

        [Test]
        public void CryoGrenade_ChillsUnitsInsideItsBlast()
        {
            var grenade = CreateWeapon("weapon.cryo-grenade", damage: 20,
                impactRadius: 1.35f, gravityScale: 0f);
            var harness = CreateHarness(grenade);
            var opponent = harness.State.GetUnit("opponent.test.01");
            var aim = new AimState(new Vector2(-3f, 0.6f), Vector2.right, 20f, true);

            Assert.That(harness.Simulation.TryFire(aim).Outcome, Is.EqualTo(BattleFireOutcome.Fired));
            for (var frame = 0; frame < 240 && !harness.Simulation.IsUnitChilled(opponent.UnitId); frame++)
            {
                harness.Simulation.Tick(1f / 60f);
            }

            Assert.That(harness.Simulation.IsUnitChilled(opponent.UnitId), Is.True);
            Assert.That(opponent.Health, Is.LessThan(opponent.MaxHealth));
        }

        [Test]
        public void Projectile_GrazingDroidUpperBody_DetonatesBeforePassingThrough()
        {
            var weapon = CreateWeapon("weapon.test.upper-body", gravityScale: 0f);
            var harness = CreateHarness(weapon);
            var exploded = false;
            harness.Simulation.ProjectileExploded += (projectileId, impact) => exploded = true;

            Assert.AreEqual(
                BattleFireOutcome.Fired,
                harness.Simulation.TryFire(new AimState(new Vector2(-3f, 0.7f), Vector2.right, 20f, true)).Outcome);

            for (var i = 0; i < 240 && !exploded; i++)
            {
                harness.Simulation.Tick(1f / 60f);
            }

            Assert.IsTrue(exploded,
                "a projectile touching the droid's upper body must detonate instead of passing through it");
        }

        [Test]
        public void FallOutOfBounds_DefeatsUnit_AndFinishesBattle()
        {
            var weapon = CreateWeapon("weapon.test.arc2");
            var harness = CreateHarness(weapon);
            var opponent = harness.State.GetUnit("opponent.test.01");
            var opponentMotor = harness.Motors["opponent.test.01"];

            var finished = false;
            var finishedResult = default(BattleResult);
            harness.Simulation.BattleFinished += (result, reason) =>
            {
                finished = true;
                finishedResult = result;
            };

            var outOfBoundsPosition = new Vector2(opponent.Position.x, -7f);
            opponent.MoveTo(outOfBoundsPosition);
            opponentMotor.Teleport(outOfBoundsPosition);
            opponentMotor.AddImpulse(Vector2.down);
            for (var i = 0; i < 240 && !finished; i++)
            {
                harness.Simulation.Tick(1f / 60f);
            }

            Assert.IsTrue(finished, "opponent falling out of bounds ends the battle");
            Assert.AreEqual(BattleOutcome.PlayerWon, finishedResult.Outcome);
            Assert.IsTrue(harness.State.GetUnit("opponent.test.01").IsDefeated);
        }

        [Test]
        public void PlayerTurnTimer_Expiring_AdvancesTurnToOpponent()
        {
            var weapon = CreateWeapon("weapon.test.arc3");
            var harness = CreateHarness(weapon);

            var advanced = false;
            harness.Simulation.TurnAdvanced += (team, reason) =>
            {
                advanced = true;
                Assert.AreEqual(TurnAdvanceReason.PlayerTimerExpired, reason);
                Assert.AreEqual(BattleTeamId.Opponent, team);
            };

            for (var i = 0; i < 30 * 120 + 4 && !advanced; i++)
            {
                harness.Simulation.Tick(1f / 120f);
            }

            Assert.IsTrue(advanced);
            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam);
        }

        [Test]
        public void WalkIntent_MovesPlayerUnit_AndUpdatesFacing()
        {
            var harness = CreateHarness(CreateWeapon("weapon.test.walk"));
            var player = harness.State.GetUnit("player.test.01");
            SettleUnits(harness);
            var startX = player.Position.x;

            Assert.AreEqual(BattleWalkOutcome.Moved, harness.Simulation.Walk(1, 0f));
            for (var i = 0; i < 60; i++)
            {
                harness.Simulation.Walk(1, 0f);
                harness.Simulation.Tick(1f / 60f);
            }

            Assert.Greater(player.Position.x, startX, "walk intent consumed by fixed ticks moves the unit");
            Assert.AreEqual(1, player.FacingDirectionX);
        }

        [Test]
        public void WalkIntent_AccumulatesAcrossFramesShorterThanOneTick()
        {
            // At high framerates a frame delivers less than one fixed tick of
            // time; the accumulator must carry the remainder across frames.
            var harness = CreateHarness(CreateWeapon("weapon.test.walk2"));
            var player = harness.State.GetUnit("player.test.01");
            SettleUnits(harness);
            var startX = player.Position.x;

            for (var i = 0; i < 480; i++)
            {
                harness.Simulation.Walk(-1, 0f);
                harness.Simulation.Tick(1f / 240f);
            }

            Assert.Less(player.Position.x, startX, "sub-tick frames still add up to movement");
            Assert.AreEqual(-1, player.FacingDirectionX);
        }

        [Test]
        public void WalkIntent_MovesOpponentUnit_DuringOpponentTurn()
        {
            var harness = CreateHarness(CreateWeapon("weapon.test.opp-walk", gravityScale: 0f, timeoutSeconds: 0.5f));
            AdvanceToOpponentTurn(harness);
            var opponent = harness.State.GetUnit("opponent.test.01");
            var startX = opponent.Position.x;

            Assert.IsTrue(harness.Simulation.CanUseMovement(),
                "the opponent can move on its own turn through the same API as the player");
            Assert.AreEqual(BattleWalkOutcome.Moved, harness.Simulation.Walk(-1, 0f));
            for (var i = 0; i < 120; i++)
            {
                harness.Simulation.Walk(-1, 0f);
                harness.Simulation.Tick(1f / 120f);
            }

            Assert.Less(opponent.Position.x, startX, "opponent walk intent moves the opponent unit");
            Assert.AreEqual(-1, opponent.FacingDirectionX);
        }

        [Test]
        public void StandardJump_OpponentJumps_DuringOpponentTurn()
        {
            var harness = CreateHarness(CreateWeapon("weapon.test.opp-jump", gravityScale: 0f, timeoutSeconds: 0.5f));
            AdvanceToOpponentTurn(harness);
            var opponent = harness.State.GetUnit("opponent.test.01");
            var startX = opponent.Position.x;

            Assert.AreEqual(BattleJumpOutcome.Jumped, harness.Simulation.TryStartJump(-1));
            for (var i = 0; i < 2400 && harness.Simulation.HasActiveMotion; i++)
            {
                harness.Simulation.Tick(1f / 120f);
            }

            Assert.IsFalse(harness.Simulation.HasActiveMotion, "the opponent jump settles back on the ground");
            Assert.Less(opponent.Position.x, startX, "the opponent jumped to the left");
            Assert.AreEqual(-1, opponent.FacingDirectionX);
        }

        [Test]
        public void JumpAgainstWall_SettlesAndUnblocksInput(
            [Values(0f, 0.04f, 0.08f, 0.12f)] float wallOffset,
            [Values(-1, 1)] int direction)
        {
            var harness = CreateHarness(CreateWeapon("weapon.test.jumpwall"));
            harness.Terrain.StampSolidRect(new Vector2(-2f - wallOffset, 1f), new Vector2(1f, 4f));
            SettleUnits(harness);

            for (var i = 0; i < 600; i++)
            {
                harness.Simulation.Walk(1, 0f);
                harness.Simulation.Tick(1f / 120f);
            }

            Assert.AreEqual(BattleJumpOutcome.Jumped, harness.Simulation.TryStartJump(direction));
            for (var i = 0; i < 2400 && harness.Simulation.HasActiveMotion; i++)
            {
                harness.Simulation.Tick(1f / 120f);
            }

            Assert.IsFalse(harness.Simulation.HasActiveMotion,
                "a jump into an obstacle must always come to rest instead of hanging mid-motion");
            Assert.IsTrue(harness.Simulation.CanUseMovement(),
                "movement input unblocks once the jump resolves");
        }

        [Test]
        public void JumpUnderOverhang_SettlesAndUnblocksInput()
        {
            var harness = CreateHarness(CreateWeapon("weapon.test.jumplip"));
            // Wall ahead with a thin lip sticking out over the player's head.
            harness.Terrain.StampSolidRect(new Vector2(-2f, 1f), new Vector2(1f, 4f));
            harness.Terrain.StampSolidRect(new Vector2(-2.8f, 0.3f), new Vector2(0.8f, 0.12f));
            SettleUnits(harness);

            for (var i = 0; i < 600; i++)
            {
                harness.Simulation.Walk(1, 0f);
                harness.Simulation.Tick(1f / 120f);
            }

            Assert.AreEqual(BattleJumpOutcome.Jumped, harness.Simulation.TryStartJump(1));
            for (var i = 0; i < 2400 && harness.Simulation.HasActiveMotion; i++)
            {
                harness.Simulation.Tick(1f / 120f);
            }

            Assert.IsFalse(harness.Simulation.HasActiveMotion,
                "snagging an overhang corner must not leave the unit unstable forever");
            Assert.IsTrue(harness.Simulation.CanUseMovement());
        }

        [Test]
        public void StandardJump_LeftMirrorsRight()
        {
            var rightDisplacement = RunStandardJump(1);
            var leftDisplacement = RunStandardJump(-1);

            Assert.Greater(rightDisplacement, 0.2f);
            Assert.AreEqual(rightDisplacement, -leftDisplacement, 0.05f, "left jumps mirror right jumps");
        }

        [Test]
        public void StandardJump_TurnsUnitTowardsJumpDirection()
        {
            var harness = CreateHarness(CreateWeapon("weapon.test.jumpface"));
            SettleUnits(harness);
            var unit = harness.State.GetUnit("player.test.01");
            Assert.AreEqual(1, unit.FacingDirectionX);

            Assert.AreEqual(BattleJumpOutcome.Jumped, harness.Simulation.TryStartJump(-1));

            Assert.AreEqual(-1, unit.FacingDirectionX);
        }

        [Test]
        public void StandardJump_WhileAirborne_IsRejected()
        {
            var harness = CreateHarness(CreateWeapon("weapon.test.jumpair"));
            SettleUnits(harness);

            Assert.AreEqual(BattleJumpOutcome.Jumped, harness.Simulation.TryStartJump(1));
            Assert.IsTrue(harness.Simulation.CanAcceptMovementInput(),
                "held movement input remains accepted while the motor is airborne");
            Assert.IsFalse(harness.Simulation.CanUseMovement(),
                "the airborne motor cannot execute another jump until it lands");
            Assert.AreEqual(BattleJumpOutcome.NotAllowed, harness.Simulation.TryStartJump(1));
        }

        [Test]
        public void StandardJump_HorizontalLaunchSpeedStaysBelowRunningSpeed()
        {
            var harness = CreateHarness(CreateWeapon("weapon.test.jumpspeed"));
            SettleUnits(harness);
            var motor = harness.Motors["player.test.01"];

            Assert.AreEqual(BattleJumpOutcome.Jumped, harness.Simulation.TryStartJump(1));

            Assert.LessOrEqual(Mathf.Abs(motor.Velocity.x), motor.Profile.WalkSpeed * 0.9f + 0.001f,
                "the standard jump must remain slower than normal walking");
        }

        [Test]
        public void PlayerTurnTimer_ContinuesWhileJumping()
        {
            var harness = CreateHarness(CreateWeapon("weapon.test.jumptimer"));
            SettleUnits(harness);
            var before = harness.Simulation.MovementTimeRemaining;

            Assert.AreEqual(BattleJumpOutcome.Jumped, harness.Simulation.TryStartJump(1));
            harness.Simulation.Tick(0.1f);
            harness.Simulation.Tick(0.1f);
            harness.Simulation.Tick(0.1f);

            Assert.Less(harness.Simulation.MovementTimeRemaining, before - 0.25f,
                "jump time is part of the real player turn timer");
        }

        private static float RunStandardJump(int direction)
        {
            var harness = CreateHarness(CreateWeapon("weapon.test.angledjump"));
            SettleUnits(harness);
            var unit = harness.State.GetUnit("player.test.01");
            var startX = unit.Position.x;

            Assert.AreEqual(BattleJumpOutcome.Jumped, harness.Simulation.TryStartJump(direction));
            for (var i = 0; i < 2400 && harness.Simulation.HasActiveMotion; i++)
            {
                harness.Simulation.Tick(1f / 120f);
            }

            Assert.IsFalse(harness.Simulation.HasActiveMotion, "the jump settles back on the ground");
            return unit.Position.x - startX;
        }

        /// <summary>
        /// Spends the player's action with a harmless flat shot and ticks until
        /// the turn hands over, so movement can be exercised on the opponent's turn.
        /// </summary>
        private static void AdvanceToOpponentTurn(Harness harness)
        {
            SettleUnits(harness);

            // Fire away from the opponent so the shot resolves by timeout without
            // touching it; the resolved shot advances the turn to the opponent.
            var fired = harness.Simulation.TryFire(new AimState(new Vector2(-3f, 0f), Vector2.left, 6f, true));
            Assert.AreEqual(BattleFireOutcome.Fired, fired.Outcome);
            for (var i = 0; i < 600 && harness.State.Turn.ActiveTeam != BattleTeamId.Opponent; i++)
            {
                harness.Simulation.Tick(1f / 120f);
            }

            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam,
                "the player's shot resolves and hands the turn to the opponent");
            SettleUnits(harness);
        }

        private static void SettleUnits(Harness harness)
        {
            foreach (var motor in harness.Motors.Values)
            {
                motor.WakeIfUnsupported();
            }

            for (var i = 0; i < 1200 && harness.Simulation.HasActiveMotion; i++)
            {
                harness.Simulation.Tick(1f / 120f);
            }

            Assert.IsFalse(harness.Simulation.HasActiveMotion, "units settle on the ground before the scenario starts");
        }

        [Test]
        public void CoverWeapon_Fired_StampsTerrainMask()
        {
            var coverWeapon = CreateWeapon("weapon.test.cover", WeaponRole.CoverTool, damage: 0, impactRadius: 1.5f);
            var harness = CreateHarness(coverWeapon);

            var coverDeployed = false;
            var coverPosition = Vector2.zero;
            harness.Simulation.CoverDeployed += e =>
            {
                coverDeployed = true;
                coverPosition = e.Position;
            };

            var aim = new AimState(new Vector2(-3f, 1f), Vector2.right, 14f, true);
            var result = harness.Simulation.TryFire(aim);

            Assert.AreEqual(BattleFireOutcome.UtilityUsed, result.Outcome);
            Assert.IsTrue(coverDeployed);
            Assert.IsTrue(harness.Terrain.Grid.IsSolidWorld(coverPosition), "cover placement stamped solid into the terrain mask");
            Assert.IsTrue(harness.Simulation.IsAttackCommitted);
            Assert.IsTrue(harness.Simulation.IsPlayerRetreatActive, "cover uses the shared movement-only retreat window");
            Assert.IsTrue(harness.Simulation.CanAcceptMovementInput());
            Assert.AreEqual(BattleFireOutcome.Blocked, harness.Simulation.TryFire(aim).Outcome);
            TickMany(harness.Simulation, 600, 1f / 120f);
            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam);
        }

        [TestCase(0f)]
        [TestCase(90f)]
        public void CanonicalBarrier_FireUsesThePreviewCenter(float angle)
        {
            var weapon = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                item => item.WeaponId == "weapon.titanium-barrier");
            var harness = CreateHarness(weapon);
            harness.Simulation.ConfigureTitaniumBarrier(Resources.Load<Texture2D>("UI/Weapons/titanium_barrier"), 6.2f);
            var requestedCenter = new Vector2(0f, 3f);
            var direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            CoverDeployedEvent deployed = default;
            harness.Simulation.CoverDeployed += value => deployed = value;

            Assert.That(harness.Simulation.TryFire(new AimState(requestedCenter, direction, 0.5f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));
            Assert.That(Vector2.Distance(deployed.Position, requestedCenter), Is.LessThan(0.001f),
                "the confirmed beam must be placed exactly where its preview was");
            Assert.That(harness.Simulation.ProjectilesInFlight, Is.Zero);
            Assert.That(harness.Simulation.IsAttackCommitted, Is.True);
            Assert.That(deployed.Size, Is.EqualTo(harness.Simulation.TitaniumBarrierBaseSize * 0.5f));
            Assert.That(harness.Simulation.ActiveInventory.GetSelectedRemainingAmmo(), Is.EqualTo(1));
            harness.Terrain.Dispose();
        }

        [Test]
        public void CanonicalBarrier_HasOnePurchasedSizeFeatureAtRankFive()
        {
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.titanium-barrier", out var definition), Is.True);
            Assert.That(definition.TryGetEvolutionOption(5, "size-unlock", out _), Is.True);
            Assert.That(definition.Special5PurchaseCount, Is.EqualTo(1));
            Assert.That(definition.Special10PurchaseCount, Is.Zero);
        }

        [TestCase(false, 0, 1f)]
        [TestCase(false, 4, 1f)]
        [TestCase(true, 0, 1.1f)]
        [TestCase(true, 1, 1.2f)]
        [TestCase(true, 2, 1.3f)]
        [TestCase(true, 3, 1.4f)]
        [TestCase(true, 4, 1.5f)]
        public void CanonicalBarrier_SizeLimitRequiresPurchaseAndEachRadiusStep(bool unlocked, int radius, float expected)
        {
            var weapon = System.Array.Find(PrototypeWeaponLibrary.CreateAll(), item => item.WeaponId == TitaniumBarrierRules.WeaponId);
            var progress = new SaveWeaponProgressData { weaponId = weapon.WeaponId, rank = 6 + radius };
            progress.EnsureDefaults();
            for (var i = 0; i < 4; i++) progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                { rank = i + 2, path = (int)WeaponUpgradePath.Loadout });
            for (var i = 0; i < radius; i++) progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                { rank = i + 7, path = (int)WeaponUpgradePath.Radius });
            if (unlocked) progress.evolutionChoices.Add(new SaveWeaponEvolutionChoiceData
                { rank = 5, optionId = TitaniumBarrierRules.SizeFeatureId });
            var harness = CreateHarness(weapon, playerWeaponProgress: progress);
            harness.Simulation.ConfigureTitaniumBarrier(Resources.Load<Texture2D>("UI/Weapons/titanium_barrier"), 6.2f);
            CoverDeployedEvent deployed = default;
            harness.Simulation.CoverDeployed += value => deployed = value;
            Assert.That(harness.Simulation.TryFire(new AimState(new Vector2(0f, 3f), Vector2.right, 9f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));
            Assert.That(deployed.Size.x / harness.Simulation.TitaniumBarrierBaseSize.x, Is.EqualTo(expected).Within(0.001f));
            Assert.That(WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var definition), Is.True);
            Assert.That(WeaponUpgradeChoiceResolver.ResolveValue(weapon, definition, progress, WeaponUpgradePath.Radius),
                Is.EqualTo(weapon.ImpactRadius * expected).Within(0.001f));
            harness.Terrain.Dispose();
        }

        [TestCase(0f)]
        [TestCase(45f)]
        [TestCase(90f)]
        [TestCase(180f)]
        public void CanonicalBarrier_CollisionFollowsSpriteAndCarvingSurvivesRefresh(float angle)
        {
            var weapon = System.Array.Find(PrototypeWeaponLibrary.CreateAll(), item => item.WeaponId == TitaniumBarrierRules.WeaponId);
            var harness = CreateHarness(weapon);
            harness.Simulation.ConfigureTitaniumBarrier(Resources.Load<Texture2D>("UI/Weapons/titanium_barrier"), 6.2f);
            var center = new Vector2(0f, 3f);
            var axis = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            CoverDeployedEvent deployed = default;
            harness.Simulation.CoverDeployed += value => deployed = value;
            Assert.That(harness.Simulation.TryFire(new AimState(center, axis, 1f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));
            var surface = deployed.Surface;
            Assert.That(harness.Terrain.Grid.IsSolidWorld(center), Is.True);
            var emptyCorner = center + axis * (-surface.Size.x * 0.4f)
                + new Vector2(-axis.y, axis.x) * (surface.Size.y * 0.4f);
            Assert.That(harness.Terrain.Grid.IsSolidWorld(emptyCorner), Is.False, "transparent art must not be an invisible box");
            Assert.That(harness.Terrain.Grid.Raycast(center - axis * 2f, center + axis * 2f, out _), Is.True,
                "shots collide with the placed beam");
            harness.Terrain.RefreshMovingSpriteSurfaces();
            Assert.That(harness.Terrain.Grid.IsSolidWorld(center), Is.True);
            Assert.That(harness.Terrain.ApplyCrater(center, 0.13f), Is.True);
            Assert.That(surface.Sprite.texture.GetPixel(surface.Sprite.texture.width / 2,
                surface.Sprite.texture.height / 2).a, Is.Zero);
            Assert.That(harness.Terrain.Grid.IsSolidWorld(center), Is.False);
            harness.Terrain.RefreshMovingSpriteSurfaces();
            Assert.That(harness.Terrain.Grid.IsSolidWorld(center), Is.False, "refresh must not resurrect a carved beam");
            Assert.That(surface.Center, Is.EqualTo(center), "beam remains stationary");
            harness.Terrain.Dispose();
        }

        [Test]
        public void CanonicalBarrier_InvalidPlacementDoesNotConsumeAmmoOrAction()
        {
            var weapon = System.Array.Find(PrototypeWeaponLibrary.CreateAll(), item => item.WeaponId == TitaniumBarrierRules.WeaponId);
            var harness = CreateHarness(weapon);
            harness.Simulation.ConfigureTitaniumBarrier(Resources.Load<Texture2D>("UI/Weapons/titanium_barrier"), 6.2f);
            foreach (var center in new[] { new Vector2(0f, -3f), new Vector2(30f, 3f), new Vector2(-3f, 0f) })
                Assert.That(harness.Simulation.TryFire(new AimState(center, Vector2.right, 1f, true)).Outcome,
                    Is.EqualTo(BattleFireOutcome.Blocked));
            Assert.That(harness.Simulation.ActiveInventory.GetSelectedRemainingAmmo(), Is.EqualTo(2));
            Assert.That(harness.Simulation.IsAttackCommitted, Is.False);
            harness.Terrain.Dispose();
        }

        [Test]
        public void RepairKit_UsesWithoutAimAndKeepsPlayerTurn()
        {
            var repair = CreateWeapon("weapon.heal", WeaponRole.DefensiveTool, damage: 34, ammo: 1);
            var harness = CreateHarness(repair);
            var player = harness.State.GetUnit("player.test.01");
            player.ApplyDamage(50, "test.damage");

            var result = harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.zero, 0f, false));

            Assert.AreEqual(BattleFireOutcome.UtilityUsed, result.Outcome);
            Assert.AreEqual(75, player.Health);
            Assert.AreEqual(BattleTeamId.Player, harness.State.Turn.ActiveTeam);
            Assert.IsTrue(harness.Simulation.CanAcceptMovementInput());
            Assert.IsFalse(harness.Simulation.ActiveInventory.CanFireSelected);
        }

        [Test]
        public void CanonicalMine_HasFourChargesAndNoLoadoutPurchases()
        {
            var mine = System.Array.Find(PrototypeWeaponLibrary.CreateAll(), w => w.WeaponId == "weapon.mine");
            Assert.That(mine.AmmoCount, Is.EqualTo(4));
            Assert.That(WeaponProgressionCatalog.TryGet(mine.WeaponId, out var definition), Is.True);
            Assert.That(definition.LoadoutUpgradeLimit, Is.Zero);
            Assert.That(mine.ImpactRadius, Is.EqualTo(PrototypeWeaponLibrary.SharedExplosionRadius));
        }

        [Test]
        public void CanonicalRepair_HealsOnlyOwnerFromMaxHealth()
        {
            var repair = System.Array.Find(PrototypeWeaponLibrary.CreateAll(), w => w.WeaponId == "weapon.heal");
            var harness = CreateHarness(repair, secondPlayerPosition: new Vector2(-3.4f, 0f));
            var owner = harness.State.GetUnit("player.test.01");
            var ally = harness.State.GetUnit("player.test.02");
            owner.ApplyDamage(80, "test");
            ally.ApplyDamage(80, "test");
            Assert.That(harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.right, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));
            Assert.That(owner.Health, Is.EqualTo(45));
            Assert.That(ally.Health, Is.EqualTo(20));
        }

        [Test]
        public void CanonicalDisc_HasSharedExplosionRadius()
        {
            var disc = System.Array.Find(PrototypeWeaponLibrary.CreateAll(), w => w.WeaponId == "weapon.titanium-disc");
            Assert.That(disc.ImpactRadius, Is.EqualTo(PrototypeWeaponLibrary.SharedExplosionRadius));
        }

        [TestCase(100, 0, 25)]
        [TestCase(1000, 0, 250)]
        [TestCase(1000, 1, 275)]
        [TestCase(1000, 4, 350)]
        public void CanonicalRepair_CoreScalesHealingFromOwnerMaxHp(int maxHp, int core, int expected)
        {
            var repair = System.Array.Find(PrototypeWeaponLibrary.CreateAll(), w => w.WeaponId == "weapon.heal");
            var harness = CreateHarness(repair, playerWeaponProgress: PurchasedBaseStats(repair.WeaponId, core, 0),
                playerMaxHealth: maxHp);
            var owner = harness.State.GetUnit("player.test.01");
            owner.ApplyDamage(maxHp - 1, "test");
            harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.right, 0f, false));
            Assert.That(owner.Health, Is.EqualTo(1 + expected));
            Assert.That(WeaponUpgradeChoiceResolver.ResolveValue(repair, null,
                PurchasedBaseStats(repair.WeaponId, core, 0), WeaponUpgradePath.Core),
                Is.EqualTo(25f + core * 2.5f));
        }

        [Test]
        public void CanonicalRepair_CapsHealingAndStillGrantsProtectionAtFullHp()
        {
            var repair = System.Array.Find(PrototypeWeaponLibrary.CreateAll(), w => w.WeaponId == "weapon.heal");
            var evolutions = new Dictionary<string, WeaponEvolutionProfile>
            {
                [repair.WeaponId] = new WeaponEvolutionProfile(1, 0f, 1f, repairProtectionTurns: 1)
            };
            var shot = CreateWeapon("weapon.test.protection", damage: 40, impactRadius: 2f, gravityScale: 0f);
            var harness = CreateHarness(repair, shot, playerWeaponEvolutions: evolutions);
            var owner = harness.State.GetUnit("player.test.01");
            harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.right, 0f, false));
            Assert.That(owner.Health, Is.EqualTo(owner.MaxHealth));
            harness.Simulation.Spawn(new ProjectileSpawnRequest(shot.CreateProjectileDefinition(),
                "opponent.test.01", owner.Position, Vector2.zero, 0.2f, Color.white));
            TickMany(harness.Simulation, 1, 1f / 120f);
            Assert.That(owner.Health, Is.EqualTo(80), "The protective upgrade works even when the initial repair restored zero HP.");
        }

        [TestCase(false, false)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void CanonicalMine_ActualDamageInterruptsEitherSideAndEscapeAvoidsIt(bool hitOwner, bool escape)
        {
            var mine = System.Array.Find(PrototypeWeaponLibrary.CreateAll(), w => w.WeaponId == "weapon.mine");
            var harness = CreateHarness(mine);
            SettleUnits(harness);
            harness.State.SetTurn(new TurnState(BattleTeamId.Player, "player.test.01", 2, false));
            Vector2? landed = null;
            var triggered = false;
            var exploded = false;
            harness.Simulation.MineDeployed += value => landed = value.Point;
            harness.Simulation.MineTriggered += _ => triggered = true;
            harness.Simulation.ProjectileExploded += (_, value) => { if (value.Damage > 0) exploded = true; };
            harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.right, 0f, false));
            for (var i = 0; i < 240 && !landed.HasValue; i++) harness.Simulation.Tick(1f / 120f);
            Assert.That(landed.HasValue, Is.True);
            MoveTestUnit(harness, "player.test.01", new Vector2(-8f, -0.49f));
            var victimId = hitOwner ? "player.test.01" : "opponent.test.01";
            var victimTeam = hitOwner ? BattleTeamId.Player : BattleTeamId.Opponent;
            MoveTestUnit(harness, victimId, landed.Value +
                (escape ? new Vector2(1.15f, 0.35f) : Vector2.up * 0.5f));
            harness.State.SetTurn(new TurnState(victimTeam, victimId, 2, false));
            for (var i = 0; i < 300 && !triggered; i++) harness.Simulation.Tick(1f / 120f);
            Assert.That(triggered, Is.True);
            Assert.That(harness.State.Turn.ActiveTeam, Is.EqualTo(victimTeam), "Arming alone must not end the turn.");
            if (escape)
            {
                harness.Simulation.Walk(1, 0f);
                TickMany(harness.Simulation, 105, 1f / 120f);
                harness.Simulation.Walk(0, 0f);
                TickMany(harness.Simulation, 30, 1f / 120f);
            }
            else TickMany(harness.Simulation, 135, 1f / 120f);
            var victim = harness.State.GetUnit(victimId);
            Assert.That(exploded, Is.True);
            if (escape)
            {
                Assert.That(victim.Health, Is.EqualTo(victim.MaxHealth));
                Assert.That(harness.State.Turn.ActiveTeam, Is.EqualTo(victimTeam));
            }
            else
            {
                Assert.That(victim.Health, Is.LessThan(victim.MaxHealth));
                Assert.That(harness.State.Turn.ActiveTeam, Is.Not.EqualTo(victimTeam));
            }
        }

        [Test]
        public void CanonicalMine_AllFourPlacementsLeaveARegularAttackAvailable()
        {
            var mine = System.Array.Find(PrototypeWeaponLibrary.CreateAll(), w => w.WeaponId == "weapon.mine");
            var shot = System.Array.Find(PrototypeWeaponLibrary.CreateAll(), w => w.WeaponId == "weapon.impulse");
            var harness = CreateHarness(mine, shot);
            SettleUnits(harness);
            harness.State.SetTurn(new TurnState(BattleTeamId.Player, "player.test.01", 2, false));
            var inventory = harness.Simulation.ActiveInventory;
            Assert.That(inventory.TryAddTemporaryWeapon(shot, -1), Is.True);
            var deployed = 0;
            harness.Simulation.MineDeployed += _ => deployed++;
            for (var i = 0; i < 4; i++) Assert.That(harness.Simulation.TryFire(
                new AimState(Vector2.zero, Vector2.right, 0f, false)).Outcome, Is.EqualTo(BattleFireOutcome.Fired));
            MoveTestUnit(harness, "player.test.01", new Vector2(-8f, -0.49f));
            for (var i = 0; i < 240 && deployed < 4; i++) harness.Simulation.Tick(1f / 120f);
            Assert.That(deployed, Is.EqualTo(4));
            Assert.That(inventory.GetRemainingAmmo(mine), Is.Zero);
            Assert.That(harness.State.Turn.ActionSpent, Is.False);
            inventory.Select(1);
            Assert.That(harness.Simulation.TryFire(new AimState(
                new Vector2(-8f, 2f), Vector2.up, 1f, true)).Outcome, Is.EqualTo(BattleFireOutcome.Fired));
            harness.Simulation.EndActiveTurn();
            Assert.That(inventory.GetRemainingAmmo(mine), Is.Zero, "The stock is per battle, not per turn.");
        }

        [TestCase(false)]
        [TestCase(true)]
        public void CanonicalMine_PurchasedAlliedSafetyKeepsAlliesSafe(bool explosionImmunity)
        {
            var mine = System.Array.Find(PrototypeWeaponLibrary.CreateAll(), w => w.WeaponId == "weapon.mine");
            var evolution = new WeaponEvolutionProfile(1, 0f, 1f,
                mineTriggersEnemiesOnly: true, mineAlliesImmuneToExplosion: explosionImmunity);
            var harness = CreateHarness(mine, playerWeaponEvolutions:
                new Dictionary<string, WeaponEvolutionProfile> { [mine.WeaponId] = evolution });
            SettleUnits(harness);
            harness.State.SetTurn(new TurnState(BattleTeamId.Player, "player.test.01", 2, false));
            Vector2? landed = null;
            var triggered = false;
            harness.Simulation.MineDeployed += value => landed = value.Point;
            harness.Simulation.MineTriggered += _ => triggered = true;
            harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.right, 0f, false));
            for (var i = 0; i < 240 && !landed.HasValue; i++) harness.Simulation.Tick(1f / 120f);
            Assert.That(landed.HasValue, Is.True);
            MoveTestUnit(harness, "player.test.01", landed.Value + new Vector2(-0.35f, 0.5f));
            TickMany(harness.Simulation, 120, 1f / 120f);
            Assert.That(triggered, Is.False, "ally_safe prevents an allied proximity trigger.");
            if (!explosionImmunity) return;
            MoveTestUnit(harness, "opponent.test.01", landed.Value + new Vector2(0.35f, 0.5f));
            TickMany(harness.Simulation, 375, 1f / 120f);
            Assert.That(triggered, Is.True);
            Assert.That(harness.State.GetUnit("player.test.01").Health, Is.EqualTo(100));
            Assert.That(harness.Motors["player.test.01"].Velocity.x, Is.EqualTo(0f).Within(0.001f));
            Assert.That(harness.State.GetUnit("opponent.test.01").Health, Is.LessThan(100));
        }

        [TestCase(0, 54, 1.55f)]
        [TestCase(1, 81, 1.705f)]
        [TestCase(4, 540, 2.17f)]
        public void CanonicalMine_PurchasedCoreAndRadiusAffectItsActualExplosion(int purchases, int damage, float radius)
        {
            var mine = System.Array.Find(PrototypeWeaponLibrary.CreateAll(), w => w.WeaponId == "weapon.mine");
            var harness = CreateHarness(mine, playerWeaponProgress: PurchasedBaseStats(mine.WeaponId, purchases, purchases));
            SettleUnits(harness);
            harness.State.SetTurn(new TurnState(BattleTeamId.Player, "player.test.01", 2, false));
            Vector2? landed = null;
            ProjectileImpact? impact = null;
            var crater = 0f;
            harness.Simulation.MineDeployed += value => landed = value.Point;
            harness.Simulation.ProjectileExploded += (_, value) => { if (value.Damage > 0) impact = value; };
            harness.Simulation.TerrainCraterApplied += (_, _, value) => crater = value;
            harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.right, 0f, false));
            for (var i = 0; i < 240 && !landed.HasValue; i++) harness.Simulation.Tick(1f / 120f);
            Assert.That(landed.HasValue, Is.True);
            MoveTestUnit(harness, "player.test.01", new Vector2(-8f, -0.49f));
            MoveTestUnit(harness, "opponent.test.01", landed.Value + Vector2.up * 0.5f);
            for (var i = 0; i < 480 && !impact.HasValue; i++) harness.Simulation.Tick(1f / 120f);
            Assert.That(impact.HasValue, Is.True);
            Assert.That(impact.Value.Damage, Is.EqualTo(damage));
            Assert.That(impact.Value.ImpactRadius, Is.EqualTo(radius).Within(0.001f));
            Assert.That(crater, Is.EqualTo(radius * 0.62f).Within(0.001f));
        }

        [TestCase(0, 52)]
        [TestCase(4, 525)]
        public void CanonicalDisc_TwoRealRicochetsAddLinearDamage(int corePurchases, int expectedDamage)
        {
            var disc = System.Array.Find(PrototypeWeaponLibrary.CreateAll(), w => w.WeaponId == "weapon.titanium-disc");
            var harness = CreateHarness(disc, playerWeaponProgress: PurchasedBaseStats(disc.WeaponId, corePurchases, 0));
            harness.Terrain.StampSolidRect(new Vector2(0f, 2f), new Vector2(0.3f, 3f));
            harness.Terrain.StampSolidRect(new Vector2(-6f, 2f), new Vector2(0.3f, 3f));
            ProjectileImpact? impact = null;
            harness.Simulation.ProjectileExploded += (_, value) => impact = value;
            harness.Simulation.TryFire(new AimState(new Vector2(-3f, 2f), Vector2.right, 99f, true));
            var projectile = harness.Simulation.Projectiles[0];
            Assert.That(projectile.Velocity.magnitude, Is.EqualTo(PrototypeWeaponLibrary.SharedProjectileSpeed));
            for (var i = 0; i < 600 && !impact.HasValue; i++) harness.Simulation.Tick(1f / 120f);
            Assert.That(impact.HasValue, Is.True);
            Assert.That(projectile.BounceCount, Is.EqualTo(2));
            Assert.That(impact.Value.Damage, Is.EqualTo(expectedDamage));
        }

        private static SaveWeaponProgressData PurchasedBaseStats(string id, int core, int radius)
        {
            var progress = new SaveWeaponProgressData { weaponId = id, rank = 1 + core + radius,
                upgradeChoices = new List<SaveWeaponUpgradeChoiceData>() };
            for (var i = 0; i < core + radius; i++) progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
            { rank = i + 2, path = (int)(i < core ? WeaponUpgradePath.Core : WeaponUpgradePath.Radius) });
            return progress;
        }

        private static void MoveTestUnit(Harness harness, string id, Vector2 position)
        {
            harness.State.GetUnit(id).MoveTo(position);
            harness.Motors[id].Teleport(position);
        }

        [Test]
        public void C4_DropsWithoutAim_AllowsEscape_ThenExplodesAndEndsTurn()
        {
            var c4 = CreateWeapon("weapon.c4", WeaponRole.ShortRangeBurst, damage: 74, ammo: 1);
            var harness = CreateHarness(c4);
            var player = harness.State.GetUnit("player.test.01");
            SettleUnits(harness);
            var startX = player.Position.x;
            var exploded = false;
            harness.Simulation.ProjectileExploded += (id, impact) => exploded = true;

            var result = harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.zero, 0f, false));

            Assert.AreEqual(BattleFireOutcome.UtilityUsed, result.Outcome);
            Assert.IsTrue(harness.Simulation.HasPendingGroundAction);
            Assert.AreEqual(BattleTeamId.Player, harness.State.Turn.ActiveTeam);
            Assert.AreEqual(BattleWalkOutcome.Moved, harness.Simulation.Walk(-1, 0f));
            TickMany(harness.Simulation, 30, 1f / 120f);
            harness.Simulation.Walk(0, 0f);
            Assert.Less(player.Position.x, startX, "the player can move away while the fuse is burning");
            Assert.IsFalse(exploded);

            TickMany(harness.Simulation, 340, 1f / 120f);
            Assert.IsTrue(exploded);
            Assert.IsFalse(harness.Simulation.HasPendingGroundAction);
            TickMany(harness.Simulation, 380, 1f / 120f);
            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam);
        }

        [Test]
        public void C4Evolutions_ApplyConfiguredDamageRadiusAndKnockback()
        {
            var weapon = CreateWeapon("weapon.c4", WeaponRole.ShortRangeBurst, damage: 74, impactRadius: 1.85f, ammo: 1);
            var enhanced = DetonateC4(weapon, new WeaponEvolutionProfile(
                1,
                0f,
                1f,
                c4DamageMultiplier: 1.15f,
                c4ImpactRadiusMultiplier: 1.3f));
            var shockwave = DetonateC4(weapon, new WeaponEvolutionProfile(
                1,
                0f,
                3f,
                c4ImpactRadiusMultiplier: 0.85f));

            Assert.That(enhanced.Damage, Is.EqualTo(86));
            Assert.That(enhanced.ImpactRadius, Is.EqualTo(2.405f).Within(0.0001f));
            Assert.That(enhanced.KnockbackMultiplier, Is.EqualTo(1.5f).Within(0.0001f));
            Assert.That(shockwave.Damage, Is.EqualTo(74));
            Assert.That(shockwave.ImpactRadius, Is.EqualTo(1.5725f).Within(0.0001f));
            Assert.That(shockwave.KnockbackMultiplier, Is.EqualTo(4.5f).Within(0.0001f));
        }

        [TestCase(0, 65, 1.9375f)]
        [TestCase(1, 98, 2.13125f)]
        [TestCase(4, 650, 2.7125f)]
        public void C4_PurchasedCoreAndRadiusAffectRealExplosionWithoutAim(
            int purchases, int expectedDamage, float expectedRadius)
        {
            var c4 = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                weapon => weapon.WeaponId == "weapon.c4");
            var progress = new SaveWeaponProgressData
            {
                weaponId = c4.WeaponId,
                rank = 1 + purchases * 2,
                upgradeChoices = new List<SaveWeaponUpgradeChoiceData>()
            };
            for (var rank = 2; rank <= progress.rank; rank++)
            {
                progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                {
                    rank = rank,
                    path = (int)(rank % 2 == 0 ? WeaponUpgradePath.Core : WeaponUpgradePath.Radius)
                });
            }

            var harness = CreateHarness(c4, playerWeaponProgress: progress);
            var craterRadius = 0f;
            var explosion = default(ProjectileImpact);
            harness.Simulation.TerrainCraterApplied += (_, _, radius) => craterRadius = radius;
            harness.Simulation.ProjectileExploded += (_, impact) => explosion = impact;
            Assert.That(harness.Simulation.TryFire(new AimState(
                Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));
            TickMany(harness.Simulation, 365, 1f / 120f);

            Assert.That(explosion.Damage, Is.EqualTo(expectedDamage));
            Assert.That(explosion.ImpactRadius, Is.EqualTo(expectedRadius).Within(0.0001f));
            Assert.That(explosion.KnockbackMultiplier, Is.EqualTo(1.5f));
            Assert.That(craterRadius, Is.EqualTo(expectedRadius * 0.62f).Within(0.0001f));
            Assert.That(WeaponUpgradeChoiceResolver.ResolveValue(c4, null, progress,
                WeaponUpgradePath.Radius), Is.EqualTo(expectedRadius).Within(0.0001f));
        }

        private static ProjectileImpact DetonateC4(WeaponDefinition weapon, WeaponEvolutionProfile evolution)
        {
            var harness = CreateHarness(weapon, playerWeaponEvolutions: new Dictionary<string, WeaponEvolutionProfile>
            {
                [weapon.WeaponId] = evolution
            });
            var hasImpact = false;
            var impact = default(ProjectileImpact);
            harness.Simulation.ProjectileExploded += (id, resolvedImpact) =>
            {
                hasImpact = true;
                impact = resolvedImpact;
            };

            Assert.That(
                harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));
            TickMany(harness.Simulation, 365, 1f / 120f);

            Assert.That(hasImpact, Is.True);
            return impact;
        }

        [Test]
        public void Blaster_KeepsBurstAvailableUntilThirdResolvedShot()
        {
            var blaster = CreateWeapon(
                "weapon.blaster-caset",
                WeaponRole.DirectProjectile,
                ammo: 3,
                gravityScale: 0f,
                timeoutSeconds: 0.12f);
            var harness = CreateHarness(blaster);
            var aim = new AimState(new Vector2(-3f, 4f), Vector2.up, 14f, true);

            for (var shot = 1; shot <= 3; shot++)
            {
                Assert.AreEqual(BattleFireOutcome.Fired, harness.Simulation.TryFire(aim).Outcome);
                Assert.AreEqual(shot < 3, harness.Simulation.CanContinueBlasterBurst);
                // The third shot ends the burst: its impact starts the retreat
                // window, so give it three extra seconds to hand the turn over.
                TickMany(harness.Simulation, shot < 3 ? 20 : 400, 1f / 120f);
                Assert.AreEqual(shot < 3 ? BattleTeamId.Player : BattleTeamId.Opponent, harness.State.Turn.ActiveTeam);
            }
        }

        [Test]
        public void Blaster_OutOfAmmoMidBurst_EndsTurnAfterImpact()
        {
            var blaster = CreateWeapon(
                "weapon.blaster-caset",
                WeaponRole.DirectProjectile,
                ammo: 1,
                gravityScale: 0f,
                timeoutSeconds: 0.12f);
            var harness = CreateHarness(blaster);

            Assert.AreEqual(
                BattleFireOutcome.Fired,
                harness.Simulation.TryFire(new AimState(new Vector2(-3f, 4f), Vector2.up, 14f, true)).Outcome);
            Assert.IsFalse(harness.Simulation.CanContinueBlasterBurst);
            TickMany(harness.Simulation, 400, 1f / 120f);
            Assert.AreEqual(
                BattleTeamId.Opponent,
                harness.State.Turn.ActiveTeam,
                "the turn does not hang when the magazine empties mid-burst");
        }

        [TestCase(6)]
        [TestCase(9)]
        public void Blaster_EvolvedQuantityBranchExtendsBurstToConfiguredShotCount(int shotCount)
        {
            var blaster = CreateWeapon(
                "weapon.blaster-caset",
                WeaponRole.DirectProjectile,
                ammo: shotCount,
                gravityScale: 0f,
                timeoutSeconds: 0.12f);
            var evolutions = new Dictionary<string, WeaponEvolutionProfile>
            {
                [blaster.WeaponId] = new WeaponEvolutionProfile(1, 0f, 1f, shotCount)
            };
            var harness = CreateHarness(blaster, playerWeaponEvolutions: evolutions);
            var aim = new AimState(new Vector2(-3f, 4f), Vector2.up, 14f, true);

            for (var shot = 1; shot <= shotCount; shot++)
            {
                Assert.AreEqual(BattleFireOutcome.Fired, harness.Simulation.TryFire(aim).Outcome);
                Assert.AreEqual(shot < shotCount, harness.Simulation.CanContinueBlasterBurst);
                TickMany(harness.Simulation, shot < shotCount ? 20 : 400, 1f / 120f);
            }

            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam);
        }

        [Test]
        public void Blaster_RicochetEvolutionSetsProjectileBounceLimit()
        {
            var blaster = CreateWeapon("weapon.blaster-caset", WeaponRole.DirectProjectile, ammo: 3);
            var evolutions = new Dictionary<string, WeaponEvolutionProfile>
            {
                [blaster.WeaponId] = new WeaponEvolutionProfile(1, 0f, 1f, 3, 2)
            };
            var harness = CreateHarness(blaster, playerWeaponEvolutions: evolutions);

            Assert.AreEqual(
                BattleFireOutcome.Fired,
                harness.Simulation.TryFire(new AimState(new Vector2(-3f, 2f), Vector2.right, 8f, true)).Outcome);
            Assert.That(harness.Simulation.Projectiles[0].Definition.TerrainBounceLimit, Is.EqualTo(2));
        }

        [Test]
        public void Mine_IsAvailableOnFirstTurn_AndKeepsTurnOpenAfterPlacement()
        {
            var mine = CreateWeapon(
                "weapon.mine",
                damage: 54,
                ammo: 2,
                gravityScale: 1.05f,
                timeoutSeconds: 8f);
            var harness = CreateHarness(mine);
            SettleUnits(harness);
            var deployed = false;
            harness.Simulation.MineDeployed += value => deployed = true;

            Assert.IsTrue(harness.Simulation.CanUsePlayerWeapon(mine),
                "the mine is selectable on the first turn");

            Assert.AreEqual(
                BattleFireOutcome.Fired,
                harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome);
            for (var i = 0; i < 600 && !deployed; i++)
            {
                harness.Simulation.Tick(1f / 120f);
            }

            Assert.IsTrue(deployed);
            Assert.AreEqual(
                BattleFireOutcome.Fired,
                harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                "the second available mine can be placed during the same turn");
            for (var i = 0; i < 600 && harness.Simulation.HasProjectilesInFlight; i++)
            {
                harness.Simulation.Tick(1f / 120f);
            }

            Assert.AreEqual(
                BattleFireOutcome.OutOfAmmo,
                harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                "mine placement stops only when the inventory is empty");
            Assert.IsFalse(harness.Simulation.IsAttackCommitted);
        }

        [Test]
        public void MineEvolution_AllowsFourSequentialPlacementsInOneTurn()
        {
            var mine = CreateWeapon(
                "weapon.mine",
                damage: 54,
                ammo: 4,
                gravityScale: 1.05f,
                timeoutSeconds: 8f);
            var harness = CreateHarness(mine, playerWeaponEvolutions: new Dictionary<string, WeaponEvolutionProfile>
            {
                [mine.WeaponId] = new WeaponEvolutionProfile(1, 0f, 1f, mineDeployLimitPerTurn: 4)
            });
            SettleUnits(harness);
            harness.State.SetTurn(new TurnState(
                BattleTeamId.Player,
                "player.test.01",
                2,
                false));
            var deployedCount = 0;
            harness.Simulation.MineDeployed += value => deployedCount++;

            for (var expectedCount = 1; expectedCount <= 4; expectedCount++)
            {
                Assert.That(
                    harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                    Is.EqualTo(BattleFireOutcome.Fired));
                for (var tick = 0; tick < 600 && deployedCount < expectedCount; tick++)
                {
                    harness.Simulation.Tick(1f / 120f);
                }

                Assert.That(deployedCount, Is.EqualTo(expectedCount));
            }

            Assert.That(harness.Simulation.IsAttackCommitted, Is.False);
            Assert.That(
                harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.OutOfAmmo));
        }

        [Test]
        public void CanonicalMortar_BaseWeapon_SpawnsTwoFragmentsWithoutCascade()
        {
            var mortar = CreateWeapon("weapon.mortira", damage: 48, impactRadius: 1.8f,
                gravityScale: 0f, timeoutSeconds: 0.2f);
            var harness = CreateHarness(mortar);
            var fragments = new List<SimProjectile>();
            harness.Simulation.ProjectileSpawned += p =>
            {
                if (p.Definition.ProjectileId != "projectile.weapon.mortira") fragments.Add(p);
            };
            Assert.That(harness.Simulation.TryFire(new AimState(new Vector2(0f, 4f), Vector2.up, 6f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 30, 1f / 120f);
            Assert.That(fragments.Count, Is.EqualTo(2));
            Assert.That(fragments[0].Velocity.x, Is.LessThan(0f));
            Assert.That(fragments[1].Velocity.x, Is.GreaterThan(0f));
            foreach (var fragment in fragments)
            {
                Assert.That(fragment.Definition.ProjectileId, Is.EqualTo("projectile.weapon.mortira.fragment"));
                Assert.That(fragment.Definition.Damage, Is.EqualTo(24));
                Assert.That(fragment.Definition.ImpactRadius, Is.EqualTo(1.8f));
                Assert.That(fragment.Definition.Radius, Is.EqualTo(mortar.ProjectileRadius * 0.5f));
            }
        }

        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 4)]
        [TestCase(4, 5)]
        [TestCase(5, 6)]
        public void CanonicalMortar_PaidChargesRemainPhysicalAndDigSequentially(int purchases, int expected)
        {
            var mortar = CreateWeapon("weapon.mortira", damage:48, impactRadius:1.55f);
            var progress = new SaveWeaponProgressData { weaponId=mortar.WeaponId, rank=5+purchases };
            progress.EnsureDefaults();
            for (var i=2;i<=5;i++) progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                { rank=i, path=(int)(i<=3 ? WeaponUpgradePath.Loadout : WeaponUpgradePath.Aim) });
            progress.evolutionChoices.Add(new SaveWeaponEvolutionChoiceData { rank=5, optionId="cascade-mortar" });
            progress.specialUpgrades.Add(new SaveWeaponSpecialUpgradeData { rank=5, purchaseCount=purchases });
            var harness = CreateHarness(mortar, playerWeaponProgress:progress, terrainDepth:24f);
            var fragments = new List<SimProjectile>();
            var impactPoints = new List<Vector2>();
            var impactTimes = new List<float>();
            var time=0f;
            harness.Simulation.ProjectileSpawned += p =>
            {
                if (p.Definition.ProjectileId.StartsWith("projectile.weapon.mortira.cascade.")) fragments.Add(p);
            };
            harness.Simulation.ImpactResolved += impact =>
            {
                if (impact.Weapon.WeaponId == "weapon.mortira.fragment")
                { impactPoints.Add(impact.Point); impactTimes.Add(time); }
            };
            Assert.That(harness.Simulation.TryFire(new AimState(new Vector2(0f, 0.5f), Vector2.down, 6f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            for(var i=0;i<3600;i++) { time+=1f/120f; harness.Simulation.Tick(1f/120f); }
            Assert.That(fragments.Count, Is.EqualTo(expected));
            Assert.That(impactPoints.Count, Is.EqualTo(expected));
            for(var i=0;i<expected;i++)
            {
                Assert.That(fragments[i].Definition.Damage, Is.EqualTo(24));
                Assert.That(fragments[i].Definition.ImpactRadius, Is.EqualTo(1.55f));
                Assert.That(harness.Terrain.Grid.IsSolidWorld(impactPoints[i]), Is.False);
                if(i>0)
                {
                    Assert.That(impactTimes[i]-impactTimes[i-1], Is.GreaterThan(0.7f));
                    Assert.That(impactPoints[i].y, Is.LessThan(impactPoints[i-1].y-0.3f),
                        "next physical fragment must descend into newly destroyed support");
                }
            }
            Assert.That(harness.Simulation.ProjectilesInFlight, Is.Zero);
            Assert.That(harness.State.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Opponent));
        }

        private static SaveWeaponProgressData CreateStickyToxicProgress(int steps)
        {
            var progress = new SaveWeaponProgressData { weaponId = "weapon.sticky-grenade", rank = 5 + steps };
            progress.EnsureDefaults();
            for (var i = 2; i <= 5; i++) progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                { rank = i, path = (int)(i <= 3 ? WeaponUpgradePath.Loadout : WeaponUpgradePath.Aim) });
            if (steps > 0)
            {
                progress.evolutionChoices.Add(new SaveWeaponEvolutionChoiceData { rank = 5, optionId = "toxic-release" });
                progress.specialUpgrades.Add(new SaveWeaponSpecialUpgradeData { rank = 5, purchaseCount = steps });
            }
            return progress;
        }

        private static Harness FireStickyAtOpponent(SaveWeaponProgressData progress, out int startHealth,
            Vector2? allyPosition = null)
        {
            var sticky = CreateWeapon("weapon.sticky-grenade", damage: 40, impactRadius: 2.5f, gravityScale: 0f,
                timeoutSeconds: 8f);
            var harness = CreateHarness(sticky, playerWeaponProgress: progress, secondPlayerPosition: allyPosition);
            startHealth = harness.State.GetUnit("opponent.test.01").Health;
            Assert.That(harness.Simulation.TryFire(new AimState(new Vector2(-3f, 0.4f), Vector2.right, 6f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            return harness;
        }

        [Test]
        public void StickyGrenade_BaseSticksToTheTargetAndExplodesAfterTheFuseWithoutPoison()
        {
            var harness = FireStickyAtOpponent(null, out var startHealth);
            var opponent = harness.State.GetUnit("opponent.test.01");
            TickMany(harness.Simulation, 60, 1f / 120f);
            Assert.That(opponent.Health, Is.EqualTo(startHealth), "the fuse is still running");
            Assert.That(harness.Simulation.ProjectilesInFlight, Is.EqualTo(1));
            TickMany(harness.Simulation, 600, 1f / 120f);
            Assert.That(opponent.Health, Is.LessThan(startHealth));
            Assert.That(harness.Simulation.IsUnitPoisoned(opponent.UnitId), Is.False);
            Assert.That(harness.State.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Opponent));
        }

        [Test]
        public void StickyGrenade_ToxicReleaseAppliesTheSharedTwoTurnPoisonCycleToEveryDroidInTheBlast()
        {
            var ally = new Vector2(4f, 0f);
            var harness = FireStickyAtOpponent(CreateStickyToxicProgress(1), out var startHealth, ally);
            var opponent = harness.State.GetUnit("opponent.test.01");
            var friend = harness.State.GetUnit("player.test.02");
            var friendStart = friend.Health;
            TickMany(harness.Simulation, 720, 1f / 120f);

            var direct = startHealth - opponent.Health;
            Assert.That(direct, Is.GreaterThan(0));
            Assert.That(harness.Simulation.IsUnitPoisoned(opponent.UnitId), Is.True);
            Assert.That(harness.Simulation.IsUnitPoisoned(friend.UnitId), Is.True,
                "an ally inside the primary radius is poisoned too");
            Assert.That(friend.Health, Is.LessThan(friendStart));

            var tick = Mathf.Max(1, Mathf.RoundToInt((startHealth - opponent.Health) * 0.2f));
            var afterBlast = opponent.Health;
            harness.Simulation.AdvanceTurnDueToNoUsableWeapon();   // opponent turn ends: first tick
            Assert.That(opponent.Health, Is.EqualTo(afterBlast - tick));
            harness.Simulation.AdvanceTurnDueToNoUsableWeapon();   // player turn ends: no opponent tick
            Assert.That(opponent.Health, Is.EqualTo(afterBlast - tick));
            Assert.That(harness.Simulation.IsUnitPoisoned(opponent.UnitId), Is.True);
            harness.Simulation.AdvanceTurnDueToNoUsableWeapon();   // opponent turn ends: second tick, status removed
            Assert.That(opponent.Health, Is.EqualTo(afterBlast - tick * 2));
            Assert.That(harness.Simulation.IsUnitPoisoned(opponent.UnitId), Is.False);
        }

        [Test]
        public void StickyGrenade_ToxicReleaseIsOneFeatureStoredThroughThePaidServiceAndSurvivesJson()
        {
            const string id = "weapon.sticky-grenade";
            var save = SaveData.CreateNew();
            save.inventory.unlockedWeaponIds.Add(id);
            var economy = new Game.Economy.EconomyService(save);
            economy.Grant(new Game.Data.Progression.ProgressionReward(300000, 100000, 10000, "test"), "test", string.Empty,
                System.Guid.NewGuid().ToString());
            var progress = save.inventory.GetOrCreateWeaponProgress(id);
            progress.blueprints = 10000;
            var service = new Game.Progression.WeaponUpgradeService(save, economy);
            Assert.That(service.TryPurchaseEvolution(id, 5, "toxic-release").Applied, Is.False, "needs rank five");
            for (var i = 0; i < 2; i++) Assert.That(service.TryUpgrade(id, WeaponUpgradePath.Loadout).Applied, Is.True);
            Assert.That(service.TryUpgrade(id, WeaponUpgradePath.Loadout).Applied, Is.False);
            for (var i = 0; i < 2; i++) Assert.That(service.TryUpgrade(id, WeaponUpgradePath.Aim).Applied, Is.True);
            Assert.That(service.TryPurchaseEvolution(id, 5, "toxic-release").Applied, Is.True);
            Assert.That(service.TryPurchaseEvolution(id, 5, "invented-second-option").Applied, Is.False);

            var restored = JsonUtility.FromJson<SaveData>(JsonUtility.ToJson(save));
            restored.EnsureDefaults();
            var restoredProgress = restored.inventory.GetOrCreateWeaponProgress(id);
            var weapon = PrototypeWeaponLibrary.CreateAll().Single(w => w.WeaponId == id);
            Assert.That(WeaponEvolutionRuntimeResolver.Resolve(weapon, restoredProgress).StickyToxicRelease, Is.True);
            Assert.That(WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
                { weaponId = id, rank = 5 }).StickyToxicRelease, Is.False);

            WeaponProgressionCatalog.TryGet(id, out var definition);
            Assert.That(definition.GetEvolutionOptions(5).Select(o => o.Id), Is.EquivalentTo(new[] { "toxic-release" }));
            Assert.That(WeaponEvolutionIconResolver.ResolveCurrentIconResource(weapon.IconId, definition, restoredProgress),
                Is.EqualTo("UI/Weapons/sticky_grenade_5A"));
            Assert.That(WeaponEvolutionIconResolver.ResolveCurrentIconResource(weapon.IconId, definition, new SaveWeaponProgressData
                { weaponId = id, rank = 1 }), Is.EqualTo("UI/Weapons/sticky_grenade"));
            Assert.That(Resources.Load<Texture2D>("UI/Weapons/sticky_grenade"), Is.Not.Null);
            Assert.That(Resources.Load<Texture2D>("UI/Weapons/sticky_grenade_5A"), Is.Not.Null);
        }

        [Test]
        public void CanonicalMortar_HostileSequentialShardsAreInterceptedByTheMagnetAndTheQueueClears()
        {
            var grounder = CreateWeapon("weapon.grounder", WeaponRole.DefensiveTool,
                damage: 0, impactRadius: 4f, ammo: 1);
            var mortar = CreateWeapon("weapon.mortira", damage: 48, impactRadius: 1.2f, timeoutSeconds: 3f);
            var progress = new SaveWeaponProgressData { weaponId = mortar.WeaponId, rank = 8 };
            progress.EnsureDefaults();
            progress.evolutionChoices.Add(new SaveWeaponEvolutionChoiceData { rank = 5, optionId = "cascade-mortar" });
            progress.specialUpgrades.Add(new SaveWeaponSpecialUpgradeData { rank = 5, purchaseCount = 3 });
            var harness = CreateHarness(grounder, opponentWeapon: mortar, opponentWeaponProgress: progress);
            var devicePosition = default(Vector2?);
            harness.Simulation.GrounderDeployed += evt => devicePosition = evt.Point;
            Assert.That(harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));
            Assert.That(harness.State.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Opponent),
                "placing the magnet hands the turn to the opponent");

            var shardImpacts = new List<Vector2>();
            var trace = new List<string>();
            harness.Simulation.ProjectileSpawned += p => trace.Add("spawn:" + p.Definition.ProjectileId);
            harness.Simulation.ImpactResolved += impact =>
            {
                trace.Add("impact:" + impact.Weapon?.WeaponId + "@" + impact.Point);
                if (impact.Weapon.WeaponId == "weapon.mortira.fragment") shardImpacts.Add(impact.Point);
            };
            var origin = devicePosition.Value + new Vector2(2.5f, 1f);
            Assert.That(harness.Simulation.TryFire(new AimState(origin, Vector2.left, 6f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 1200, 1f / 120f);

            Assert.That(shardImpacts, Is.Not.Empty,
                "paid shards must still resolve near the magnet; trace=" + string.Join(" | ", trace)
                + " grounder=" + devicePosition + " inFlight=" + harness.Simulation.ProjectilesInFlight
                + " shards=" + string.Join(";", harness.Simulation.Projectiles.Select(p =>
                    p.Definition.ProjectileId + "@" + p.Position + " v=" + p.Velocity + " life=" + p.RemainingLifetime)));
            Assert.That(harness.Simulation.ProjectilesInFlight, Is.Zero, "no orphaned shard may remain queued");
        }

        [Test]
        public void CanonicalMortar_RadiusPurchasesUseTheSharedTenPercentStep()
        {
            var mortar = CreateWeapon("weapon.mortira", impactRadius: 1.8f);
            var progress = new SaveWeaponProgressData { weaponId = mortar.WeaponId, rank = 3 };
            progress.EnsureDefaults();
            for (var i = 2; i <= 3; i++) progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                { rank = i, path = (int)WeaponUpgradePath.Radius });
            Assert.That(WeaponUpgradeChoiceResolver.ResolveImpactRadiusPercent(mortar, progress), Is.EqualTo(20));
            WeaponProgressionCatalog.TryGet(mortar.WeaponId, out var definition);
            Assert.That(WeaponUpgradeChoiceResolver.ResolveValue(mortar, definition, progress, WeaponUpgradePath.Radius),
                Is.EqualTo(2.16f).Within(0.001f));
        }

        [Test]
        public void CanonicalMortar_HasOnlyOneFivePurchaseFeatureAndTheExistingSkin()
        {
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.mortira", out var definition), Is.True);
            Assert.That(definition.GetEvolutionOptions(5).Count, Is.EqualTo(1));
            Assert.That(definition.GetEvolutionOptions(5)[0].Id, Is.EqualTo("cascade-mortar"));
            Assert.That(definition.GetSpecialPurchaseLimit(5), Is.EqualTo(5));
            Assert.That(definition.GetEvolutionOptions(10), Is.Empty);
            Assert.That(Resources.Load<Texture2D>("UI/Weapons/mortira_shot"), Is.Not.Null);
            Assert.That(WeaponEvolutionIconResolver.ResolveEvolutionIconResource("mortira",5,WeaponUpgradeBranch.A),
                Is.EqualTo("UI/Weapons/mortira_5_10_B"));
        }

        [Test]
        public void Teleport_RepositionsWhenProjectileStopsWithoutExtraDelay()
        {
            var teleport = CreateWeapon(
                "weapon.teleport",
                WeaponRole.MovementTool,
                damage: 0,
                gravityScale: 0f,
                timeoutSeconds: 0.15f);
            var harness = CreateHarness(teleport);
            var player = harness.State.GetUnit("player.test.01");
            var startX = player.Position.x;

            Assert.AreEqual(
                BattleFireOutcome.Fired,
                harness.Simulation.TryFire(new AimState(new Vector2(-3f, 1f), Vector2.right, 8f, true)).Outcome);
            TickMany(harness.Simulation, 25, 1f / 120f);

            Assert.Greater(player.Position.x, startX + 0.5f);
            TickMany(harness.Simulation, 400, 1f / 120f);
            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam);
        }

        [Test]
        public void Teleport_IntoTightSpot_CarvesArrivalPocketInsteadOfClimbingOnTop()
        {
            var teleport = CreateWeapon(
                "weapon.teleport",
                WeaponRole.MovementTool,
                damage: 0,
                gravityScale: 0f,
                timeoutSeconds: 0.55f);
            var harness = CreateHarness(teleport);
            // Roof over the floor: the 1 m gap underneath is too low for the
            // 1.1 m body, so the old surface snap would climb onto the roof.
            harness.Terrain.StampSolidRect(new Vector2(2.5f, 0.5f), new Vector2(3f, 1f));
            var player = harness.State.GetUnit("player.test.01");

            Assert.AreEqual(
                BattleFireOutcome.Fired,
                harness.Simulation.TryFire(new AimState(new Vector2(-3f, -0.2f), Vector2.right, 8f, true)).Outcome);
            TickMany(harness.Simulation, 90, 1f / 120f);

            Assert.Less(player.Position.y, 0.5f, "the unit materialises inside the gap, not on top of the roof");
            Assert.Greater(player.Position.x, 0.5f, "the unit arrived at the teleport landing spot");
            Assert.IsFalse(
                harness.Terrain.Grid.IsSolidWorld(player.Position + Vector2.up * 0.6f),
                "arrival blasts a pocket so the body fits");
        }

        private static Harness CreateCryoHarness(Vector2? ally = null, int damage = 40)
        {
            var cryo = CreateWeapon("weapon.cryo-grenade", WeaponRole.ArcingExplosive, damage: damage, impactRadius: 2.5f,
                gravityScale: 0f, timeoutSeconds: 8f);
            return CreateHarness(cryo, secondPlayerPosition: ally);
        }

        private static void ThrowAtOpponent(Harness harness)
        {
            Assert.That(harness.Simulation.TryFire(new AimState(new Vector2(-2.4f, 0.1f), Vector2.right, 12f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 1200, 1f / 120f);
        }

        [Test]
        public void CryoGrenade_AppliesTheSharedTwoTurnColdCycleToEveryDroidInTheBlast()
        {
            var harness = CreateCryoHarness(ally: new Vector2(4f, 0f));
            var enemy = harness.State.GetUnit("opponent.test.01");
            var friend = harness.State.GetUnit("player.test.02");
            var enemyStart = enemy.Health;
            ThrowAtOpponent(harness);
            var direct = enemyStart - enemy.Health;
            Assert.That(direct, Is.GreaterThan(0));
            Assert.That(harness.Simulation.IsUnitChilled(enemy.UnitId), Is.True);
            Assert.That(harness.Simulation.IsUnitChilled(friend.UnitId), Is.True, "allies in the primary radius are chilled");
            Assert.That(harness.Simulation.IsUnitFrozen(enemy.UnitId), Is.False, "the base cold cycle adds no movement ban");

            var tick = Mathf.Max(1, Mathf.RoundToInt(direct * 0.2f));
            var afterBlast = enemy.Health;
            Assert.That(harness.State.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Opponent));
            harness.Simulation.AdvanceTurnDueToNoUsableWeapon();   // victim's first turn ends: tick
            Assert.That(enemy.Health, Is.EqualTo(afterBlast - tick));
            harness.Simulation.AdvanceTurnDueToNoUsableWeapon();   // other side
            Assert.That(enemy.Health, Is.EqualTo(afterBlast - tick));
            harness.Simulation.AdvanceTurnDueToNoUsableWeapon();   // victim's second turn ends: tick and removal
            Assert.That(enemy.Health, Is.EqualTo(afterBlast - tick * 2));
            Assert.That(harness.Simulation.IsUnitChilled(enemy.UnitId), Is.False);
        }

        [Test]
        public void CryoGrenade_RepeatHitRefreshesToTwoTurnsAndOnlyBoostsItsOwnDirectDamage()
        {
            var harness = CreateCryoHarness(damage: 20);
            var enemy = harness.State.GetUnit("opponent.test.01");
            var start = enemy.Health;
            ThrowAtOpponent(harness);
            var first = start - enemy.Health;
            harness.Simulation.AdvanceTurnDueToNoUsableWeapon();   // victim turn ends: tick one, one turn of cold left
            var beforeSecond = enemy.Health;
            // The first blast knocked the droid back; restore its spot so both hits have equal falloff.
            enemy.MoveTo(new Vector2(3f, 0f));
            harness.Motors[enemy.UnitId].Teleport(enemy.Position);
            Assert.That(harness.State.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Player));
            ThrowAtOpponent(harness);
            var second = beforeSecond - enemy.Health;
            Assert.That(second, Is.EqualTo(Mathf.CeilToInt(first * 1.25f)).Within(1), "+25% to the direct damage only");

            var tick = Mathf.Max(1, Mathf.RoundToInt(second * 0.2f));
            var afterSecond = enemy.Health;
            harness.Simulation.AdvanceTurnDueToNoUsableWeapon();
            Assert.That(enemy.Health, Is.EqualTo(afterSecond - tick), "one tick of the new hit");
            harness.Simulation.AdvanceTurnDueToNoUsableWeapon();
            harness.Simulation.AdvanceTurnDueToNoUsableWeapon();
            Assert.That(enemy.Health, Is.EqualTo(afterSecond - tick * 2), "two ticks in total, not three");
            Assert.That(harness.Simulation.IsUnitChilled(enemy.UnitId), Is.False);
        }

        [Test]
        public void CryoGrenade_UsesFourTthPathsHasNoFeatureAndKeepsItsOwnBody()
        {
            Assert.That(WeaponProgressionCatalog.TryGet("weapon.cryo-grenade", out var definition), Is.True);
            Assert.That(new[] { definition.CoreUpgradeLimit, definition.AimUpgradeLimit,
                definition.LoadoutUpgradeLimit, definition.RadiusUpgradeLimit }, Is.EqualTo(new[] { 4, 4, 2, 4 }));
            Assert.That(definition.HasEvolutionOptions, Is.False);
            Assert.That(WeaponEvolutionIconResolver.ResolveCurrentIconResource("cryo_grenade", definition,
                new SaveWeaponProgressData { weaponId = "weapon.cryo-grenade", rank = 15 }),
                Is.EqualTo("UI/Weapons/cryo_grenade"));
            Assert.That(Resources.Load<Texture2D>("UI/Weapons/cryo_grenade"), Is.Not.Null);
        }

        private static Harness CreateRailHarness(SaveWeaponProgressData progress = null,
            Vector2? secondOpponent = null)
        {
            var rail = CreateWeapon("weapon.rail-rifle", WeaponRole.DirectProjectile, damage: 52, impactRadius: 0.35f,
                ammo: 2, gravityScale: 0f, timeoutSeconds: 5f);
            return CreateHarness(rail, playerWeaponProgress: progress, secondOpponentPosition: secondOpponent);
        }

        private static SaveWeaponProgressData CreateRailProgress(int pierceSteps)
        {
            var progress = new SaveWeaponProgressData { weaponId = "weapon.rail-rifle", rank = 5 + pierceSteps };
            progress.EnsureDefaults();
            for (var rank = 2; rank <= 5; rank++)
                progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                    { rank = rank, path = (int)(rank % 2 == 0 ? WeaponUpgradePath.Core : WeaponUpgradePath.Loadout) });
            progress.evolutionChoices.Add(new SaveWeaponEvolutionChoiceData { rank = 5, optionId = "terrain-pierce" });
            progress.specialUpgrades.Add(new SaveWeaponSpecialUpgradeData { rank = 5, purchaseCount = pierceSteps });
            return progress;
        }

        [Test]
        public void RailRifle_PassesThroughEveryDroidOnTheLineAndHurtsEachOnce()
        {
            var harness = CreateRailHarness(secondOpponent: new Vector2(5f, 0f));
            var first = harness.State.GetUnit("opponent.test.01");
            var second = harness.State.GetUnit("opponent.test.02");
            var firstStart = first.Health;
            var secondStart = second.Health;
            var hits = new Dictionary<string, int>();
            harness.Simulation.DamageApplied += (_, evt) =>
                hits[evt.TargetUnitId] = hits.TryGetValue(evt.TargetUnitId, out var n) ? n + 1 : 1;
            Assert.That(harness.Simulation.TryFire(new AimState(new Vector2(-2.4f, 0.1f), Vector2.right, 12f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 360, 1f / 120f);
            Assert.That(firstStart - first.Health, Is.EqualTo(52));
            Assert.That(secondStart - second.Health, Is.EqualTo(52), "the bullet kept flying after the first droid");
            Assert.That(hits["opponent.test.01"], Is.EqualTo(1));
            Assert.That(hits["opponent.test.02"], Is.EqualTo(1));
            Assert.That(harness.State.GetUnit("player.test.01").Health, Is.EqualTo(100), "the shooter is untouched");
            Assert.That(harness.Simulation.ProjectilesInFlight, Is.Zero);
            TickMany(harness.Simulation, 3600, 1f / 120f); // the retreat window ends
            Assert.That(harness.State.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Opponent));
        }

        [Test]
        public void RailRifle_TunnelsANarrowChannelThroughGroundAndHitsTheDroidBehind()
        {
            var harness = CreateRailHarness();
            harness.Terrain.StampSolidRect(new Vector2(1f, 1f), new Vector2(1f, 4f)); // x 0.5..1.5, y -1..3
            var opponent = harness.State.GetUnit("opponent.test.01");
            var start = opponent.Health;
            Assert.That(harness.Terrain.Grid.IsSolidWorld(new Vector2(1f, 0.1f)), Is.True);
            Assert.That(harness.Simulation.TryFire(new AimState(new Vector2(-2.4f, 0.1f), Vector2.right, 12f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 360, 1f / 120f);
            Assert.That(harness.Terrain.Grid.IsSolidWorld(new Vector2(1f, 0.1f)), Is.False, "channel carved");
            Assert.That(harness.Terrain.Grid.IsSolidWorld(new Vector2(1f, 1.6f)), Is.True, "the channel is narrow");
            Assert.That(start - opponent.Health, Is.EqualTo(52), "reached the droid behind the wall");
        }

        [Test]
        public void RailRifle_StopsWhenThePenetrationBudgetIsSpentAndEachPierceStepAddsTenPercent()
        {
            float TunnelEnd(SaveWeaponProgressData progress)
            {
                var harness = CreateRailHarness(progress);
                // A long slab above the droids so only the ground budget matters (speed 6 u/s, budget 1 s).
                harness.Terrain.StampSolidRect(new Vector2(4.5f, 2f), new Vector2(11f, 1f)); // x -1..10, y 1.5..2.5
                Assert.That(harness.Simulation.TryFire(new AimState(new Vector2(-1.4f, 2f), Vector2.right, 6f, true)).Outcome,
                    Is.EqualTo(BattleFireOutcome.Fired));
                TickMany(harness.Simulation, 600, 1f / 120f);
                Assert.That(harness.Simulation.ProjectilesInFlight, Is.Zero);
                var x = -1f;
                while (x < 10f && !harness.Terrain.Grid.IsSolidWorld(new Vector2(x, 2f))) x += 0.05f;
                return x;
            }

            var baseEnd = TunnelEnd(null);
            var steppedEnd = TunnelEnd(CreateRailProgress(4));
            Assert.That(baseEnd, Is.EqualTo(5f).Within(0.3f), "6 units of ground at 6 u/s for one second");
            Assert.That(steppedEnd - baseEnd, Is.EqualTo(2.4f).Within(0.3f), "four steps add 40% of the budget");
        }

        [Test]
        public void RailRifle_FeatureIsFourPaidStepsAndSurvivesJson()
        {
            const string id = "weapon.rail-rifle";
            var save = SaveData.CreateNew();
            save.inventory.unlockedWeaponIds.Add(id);
            var economy = new Game.Economy.EconomyService(save);
            economy.Grant(new Game.Data.Progression.ProgressionReward(900000, 900000, 10000, "test"), "test",
                string.Empty, System.Guid.NewGuid().ToString());
            var progress = save.inventory.GetOrCreateWeaponProgress(id);
            progress.blueprints = 10000;
            var service = new Game.Progression.WeaponUpgradeService(save, economy);
            Assert.That(service.TryUpgrade(id, WeaponUpgradePath.Aim).Applied, Is.False, "no Aim steps");
            Assert.That(service.TryUpgrade(id, WeaponUpgradePath.Radius).Applied, Is.False, "no Radius steps");
            for (var i = 0; i < 2; i++) Assert.That(service.TryUpgrade(id, WeaponUpgradePath.Core).Applied, Is.True);
            for (var i = 0; i < 2; i++) Assert.That(service.TryUpgrade(id, WeaponUpgradePath.Loadout).Applied, Is.True);
            for (var i = 0; i < 4; i++)
                Assert.That(service.TryPurchaseEvolution(id, 5, "terrain-pierce").Applied, Is.True, $"step {i + 1}");
            Assert.That(service.TryPurchaseEvolution(id, 5, "terrain-pierce").Applied, Is.False, "only four steps");
            for (var i = 0; i < 2; i++) Assert.That(service.TryUpgrade(id, WeaponUpgradePath.Core).Applied, Is.True);
            for (var i = 0; i < 2; i++) Assert.That(service.TryUpgrade(id, WeaponUpgradePath.Loadout).Applied, Is.True);
            Assert.That(service.TryUpgrade(id, WeaponUpgradePath.Core).Applied, Is.False, "core limit is four");

            var restored = JsonUtility.FromJson<SaveData>(JsonUtility.ToJson(save));
            restored.EnsureDefaults();
            var weapon = PrototypeWeaponLibrary.CreateAll().Single(w => w.WeaponId == id);
            Assert.That(WeaponEvolutionRuntimeResolver.Resolve(weapon, restored.inventory.GetOrCreateWeaponProgress(id))
                .RailPierceSteps, Is.EqualTo(4));
            Assert.That(weapon.MinPower, Is.EqualTo(PrototypeWeaponLibrary.SharedProjectileSpeed), "shared base speed");
            Assert.That(Resources.Load<Texture2D>("UI/Weapons/blaster_shot"), Is.Not.Null);
        }

        private static SaveWeaponProgressData CreateTeleportProgress(bool earlyTrigger)
        {
            var progress = new SaveWeaponProgressData { weaponId = "weapon.teleport", rank = earlyTrigger ? 11 : 6 };
            progress.EnsureDefaults();
            var last = earlyTrigger ? 10 : 5;
            for (var rank = 2; rank <= last; rank++)
            {
                if (rank == 6) continue; // rank six is the paid instant_touch step
                progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                    { rank = rank, path = (int)(rank % 2 == 0 ? WeaponUpgradePath.Aim : WeaponUpgradePath.Loadout) });
            }
            progress.evolutionChoices.Add(new SaveWeaponEvolutionChoiceData { rank = 5, optionId = "instant-touch" });
            progress.specialUpgrades.Add(new SaveWeaponSpecialUpgradeData { rank = 5, purchaseCount = 1 });
            if (earlyTrigger)
            {
                progress.evolutionChoices.Add(new SaveWeaponEvolutionChoiceData { rank = 10, optionId = "early-trigger" });
                progress.specialUpgrades.Add(new SaveWeaponSpecialUpgradeData { rank = 10, purchaseCount = 1 });
            }
            return progress;
        }

        private static Harness CreateTeleportHarness(SaveWeaponProgressData progress, float gravity = 1f)
        {
            var sphere = CreateWeapon("weapon.teleport", WeaponRole.MovementTool, damage: 0, impactRadius: 2.5f,
                ammo: 2, gravityScale: gravity, timeoutSeconds: 8f);
            return CreateHarness(sphere, playerWeaponProgress: progress);
        }

        [Test]
        public void TeleportSphere_BaseBouncesOnFirstContactWhileInstantTouchTeleportsAtOnce()
        {
            var baseHarness = CreateTeleportHarness(null);
            Assert.That(baseHarness.Simulation.TryFire(new AimState(new Vector2(-3f, 1.5f), Vector2.down, 12f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(baseHarness.Simulation, 30, 1f / 120f);
            Assert.That(baseHarness.Simulation.ProjectilesInFlight, Is.EqualTo(1), "base sphere bounces on");

            var harness = CreateTeleportHarness(CreateTeleportProgress(false));
            var owner = harness.State.GetUnit("player.test.01");
            Assert.That(harness.Simulation.TryFire(new AimState(new Vector2(1.5f, 1.5f), Vector2.down, 12f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 30, 1f / 120f);
            Assert.That(harness.Simulation.ProjectilesInFlight, Is.Zero, "first contact consumed the sphere");
            Assert.That(owner.Position.x, Is.EqualTo(1.5f).Within(0.4f));
            Assert.That(harness.Terrain.Grid.IsSolidWorld(owner.Position), Is.False);
        }

        [Test]
        public void TeleportSphere_InstantTouchOnACeilingLeavesTheDroidOnTheFreeSideAndItFalls()
        {
            var harness = CreateTeleportHarness(CreateTeleportProgress(false));
            harness.Terrain.StampSolidRect(new Vector2(-3f, 3f), new Vector2(6f, 1f)); // underside at y = 2.5
            var owner = harness.State.GetUnit("player.test.01");
            Assert.That(harness.Simulation.TryFire(new AimState(new Vector2(-3f, 1.4f), Vector2.up, 12f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 30, 1f / 120f);
            Assert.That(harness.Simulation.ProjectilesInFlight, Is.Zero);
            Assert.That(owner.Position.y, Is.GreaterThan(1.2f), "arrived at the ceiling contact");
            Assert.That(owner.Position.y, Is.LessThan(2.5f), "free side below the ceiling, not on top");
            Assert.That(harness.Terrain.Grid.IsSolidWorld(owner.Position + Vector2.up * 0.5f), Is.False);
            var startY = owner.Position.y;
            TickMany(harness.Simulation, 360, 1f / 120f);
            Assert.That(owner.Position.y, Is.LessThan(startY - 0.5f), "unsupported droid falls");
        }

        [Test]
        public void TeleportSphere_InstantTouchOnAWallDoesNotJumpOntoTheObstacle()
        {
            var harness = CreateTeleportHarness(CreateTeleportProgress(false));
            harness.Terrain.StampSolidRect(new Vector2(0.5f, 1f), new Vector2(1f, 4f)); // x 0..1, y -1..3
            var owner = harness.State.GetUnit("player.test.01");
            Assert.That(harness.Simulation.TryFire(new AimState(new Vector2(-3f, 0.5f), Vector2.right, 12f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 60, 1f / 120f);
            Assert.That(owner.Position.x, Is.LessThan(0.1f), "free side of the wall");
            Assert.That(owner.Position.x, Is.GreaterThan(-1.2f));
            Assert.That(owner.Position.y, Is.LessThan(2f), "not carried to the top of the obstacle");
            Assert.That(harness.Terrain.Grid.IsSolidWorld(owner.Position), Is.False);
        }

        [Test]
        public void TeleportSphere_EarlyTriggerSecondPressTeleportsToTheCurrentSpherePointWithoutASecondCharge()
        {
            var harness = CreateTeleportHarness(CreateTeleportProgress(true), gravity: 0f);
            var owner = harness.State.GetUnit("player.test.01");
            var aim = new AimState(new Vector2(-3f, 2.5f), Vector2.right, 12f, true);
            Assert.That(harness.Simulation.CanTriggerTeleportEarly(), Is.False);
            Assert.That(harness.Simulation.TryFire(aim).Outcome, Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 30, 1f / 120f);
            Assert.That(harness.Simulation.CanTriggerTeleportEarly(), Is.True);
            var spherePoint = harness.Simulation.Projectiles[0].Position;
            var ammoBefore = harness.Simulation.ActiveInventory.GetSelectedRemainingAmmo();

            Assert.That(harness.Simulation.TryFire(aim).Outcome, Is.EqualTo(BattleFireOutcome.Fired));
            Assert.That(Vector2.Distance(owner.Position, spherePoint), Is.LessThan(0.05f));
            Assert.That(harness.Simulation.ProjectilesInFlight, Is.Zero);
            Assert.That(harness.Simulation.CanTriggerTeleportEarly(), Is.False);
            Assert.That(harness.Simulation.ActiveInventory.GetSelectedRemainingAmmo(), Is.EqualTo(ammoBefore));
            TickMany(harness.Simulation, 600, 1f / 120f);
            Assert.That(harness.State.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Opponent));
        }

        [Test]
        public void TeleportSphere_WithoutTheEarlyTriggerSecondPressIsBlockedAndContactStillWorks()
        {
            var harness = CreateTeleportHarness(CreateTeleportProgress(false), gravity: 0f);
            var aim = new AimState(new Vector2(-3f, 2.5f), Vector2.right, 12f, true);
            Assert.That(harness.Simulation.TryFire(aim).Outcome, Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 30, 1f / 120f);
            Assert.That(harness.Simulation.CanTriggerTeleportEarly(), Is.False);
            Assert.That(harness.Simulation.TryFire(aim).Outcome, Is.EqualTo(BattleFireOutcome.Blocked));
            Assert.That(harness.Simulation.ProjectilesInFlight, Is.EqualTo(1));
        }

        [Test]
        public void TeleportSphere_FeaturesAreBoughtThroughThePaidServiceKeepTheSphereBodyAndSurviveJson()
        {
            const string id = "weapon.teleport";
            var save = SaveData.CreateNew();
            save.inventory.unlockedWeaponIds.Add(id);
            var economy = new Game.Economy.EconomyService(save);
            economy.Grant(new Game.Data.Progression.ProgressionReward(300000, 100000, 10000, "test"), "test",
                string.Empty, System.Guid.NewGuid().ToString());
            var progress = save.inventory.GetOrCreateWeaponProgress(id);
            progress.blueprints = 10000;
            var service = new Game.Progression.WeaponUpgradeService(save, economy);
            Assert.That(service.TryUpgrade(id, WeaponUpgradePath.Core).Applied, Is.False, "no Core steps");
            Assert.That(service.TryUpgrade(id, WeaponUpgradePath.Radius).Applied, Is.False, "no Radius steps");
            Assert.That(service.TryPurchaseEvolution(id, 5, "instant-touch").Applied, Is.False, "needs rank five");
            for (var i = 0; i < 2; i++) Assert.That(service.TryUpgrade(id, WeaponUpgradePath.Aim).Applied, Is.True);
            for (var i = 0; i < 2; i++) Assert.That(service.TryUpgrade(id, WeaponUpgradePath.Loadout).Applied, Is.True);
            Assert.That(service.TryPurchaseEvolution(id, 10, "early-trigger").Applied, Is.False, "needs rank ten");
            Assert.That(service.TryPurchaseEvolution(id, 5, "instant-touch").Applied, Is.True);
            Assert.That(service.TryPurchaseEvolution(id, 5, "instant-touch").Applied, Is.False, "one step only");
            for (var i = 0; i < 2; i++) Assert.That(service.TryUpgrade(id, WeaponUpgradePath.Aim).Applied, Is.True);
            for (var i = 0; i < 2; i++) Assert.That(service.TryUpgrade(id, WeaponUpgradePath.Loadout).Applied, Is.True);
            Assert.That(service.TryUpgrade(id, WeaponUpgradePath.Aim).Applied, Is.False, "aim limit is four");
            var early = service.TryPurchaseEvolution(id, 10, "early-trigger");
            Assert.That(early.Applied, Is.True, $"rank={progress.rank} result={early.Rank} level={early.RequiredCommanderLevel} credits={early.Cost.Credits} bp={early.Cost.Blueprints} have={save.economy.softCurrency}/{progress.blueprints}");

            var restored = JsonUtility.FromJson<SaveData>(JsonUtility.ToJson(save));
            restored.EnsureDefaults();
            var restoredProgress = restored.inventory.GetOrCreateWeaponProgress(id);
            var weapon = PrototypeWeaponLibrary.CreateAll().Single(w => w.WeaponId == id);
            var profile = WeaponEvolutionRuntimeResolver.Resolve(weapon, restoredProgress);
            Assert.That(profile.TeleportInstantTouch, Is.True);
            Assert.That(profile.TeleportEarlyTrigger, Is.True);
            Assert.That(profile.TeleportExchangesEnemy, Is.False, "no legacy exchange behaviour");

            WeaponProgressionCatalog.TryGet(id, out var definition);
            Assert.That(WeaponEvolutionIconResolver.ResolveCurrentIconResource(weapon.IconId, definition, restoredProgress),
                Is.EqualTo("UI/Weapons/teleport"));
            Assert.That(Resources.Load<Texture2D>("UI/Weapons/teleport"), Is.Not.Null);
        }

        [Test]
        public void SwapJump_WithoutNearbyEnemy_RepositionsOwnerOnly()
        {
            var swapJump = CreateWeapon(
                "weapon.swap-jump",
                WeaponRole.MovementTool,
                damage: 0,
                gravityScale: 0f,
                timeoutSeconds: 0.15f);
            var harness = CreateHarness(swapJump);
            var player = harness.State.GetUnit("player.test.01");
            var opponent = harness.State.GetUnit("opponent.test.01");
            var origin = player.Position;
            var opponentOrigin = opponent.Position;

            Assert.That(harness.Simulation.TryFire(new AimState(
                origin, Vector2.right, 8f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 120, 1f / 120f);

            Assert.That(player.Position.x, Is.GreaterThan(origin.x + 0.4f));
            Assert.That(opponent.Position, Is.EqualTo(opponentOrigin));
        }

        [Test]
        public void SwapJump_ExchangesClosestEnemyUsingThrowOrigin()
        {
            var swapJump = CreateWeapon(
                "weapon.swap-jump",
                WeaponRole.MovementTool,
                damage: 0,
                gravityScale: 0f,
                timeoutSeconds: 0.15f);
            var harness = CreateHarness(swapJump, secondOpponentPosition: new Vector2(-0.3f, 0f));
            var player = harness.State.GetUnit("player.test.01");
            var opponent = harness.State.GetUnit("opponent.test.01");
            var fartherOpponent = harness.State.GetUnit("opponent.test.02");
            var fartherOrigin = fartherOpponent.Position;
            var origin = player.Position;
            var opponentNearLanding = new Vector2(-1.5f, 0f);
            opponent.MoveTo(opponentNearLanding);
            harness.Motors[opponent.UnitId].Teleport(opponentNearLanding);
            var sphereLanding = default(Vector2?);
            harness.Simulation.ProjectileExploded += (id, impact) =>
            {
                if (impact.OwnerUnitId == player.UnitId)
                {
                    sphereLanding = impact.Point;
                }
            };

            Assert.That(harness.Simulation.TryFire(new AimState(
                origin, Vector2.right, 8f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            var ownerPositionAfterThrow = new Vector2(-4f, 0f);
            player.MoveTo(ownerPositionAfterThrow);
            harness.Motors[player.UnitId].Teleport(ownerPositionAfterThrow);
            TickMany(harness.Simulation, 120, 1f / 120f);

            Assert.That(sphereLanding.HasValue, Is.True);
            Assert.That(player.Position.x, Is.EqualTo(sphereLanding.Value.x).Within(0.05f),
                "the owner must arrive at the sphere, not at the enemy's old position");
            Assert.That(player.Position.x, Is.GreaterThan(origin.x + 0.4f));
            Assert.That(opponent.Position.x, Is.EqualTo(origin.x).Within(0.05f));
            Assert.That(opponent.Position.y, Is.EqualTo(origin.y).Within(0.05f));
            Assert.That(fartherOpponent.Position, Is.EqualTo(fartherOrigin),
                "the farther enemy inside the exchange radius must stay in place");
        }

        [TestCase(-1.2f)]
        [TestCase(1.5f)]
        public void IonShotgun_EachContactDealsFullPelletDamageWithoutExplosion(float targetX)
        {
            var shotgun = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                weapon => weapon.WeaponId == "weapon.ion-shotgun");
            var harness = CreateHarness(shotgun);
            var target = harness.State.GetUnit("opponent.test.01");
            target.MoveTo(new Vector2(targetX, 0f));
            harness.Motors[target.UnitId].Teleport(target.Position);
            var pelletHits = 0;
            var craterCount = 0;
            harness.Simulation.DamageApplied += (_, damage) =>
            {
                if (damage.TargetUnitId == target.UnitId)
                {
                    pelletHits++;
                }
            };
            harness.Simulation.TerrainCraterApplied += (_, _, _) => craterCount++;

            Assert.That(harness.Simulation.TryFire(new AimState(
                new Vector2(-3f, 0f), Vector2.right, 8f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.AimRequired));
            Assert.That(harness.Simulation.TryFire(new AimState(
                new Vector2(-3f, 0f), Vector2.right, 8f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            Assert.That(harness.Simulation.Projectiles, Has.Count.EqualTo(5));
            var spawnedDamage = 0;
            foreach (var pellet in harness.Simulation.Projectiles)
            {
                spawnedDamage += pellet.Definition.Damage;
            }
            Assert.That(spawnedDamage, Is.EqualTo(shotgun.Damage));

            TickMany(harness.Simulation, 240, 1f / 120f);

            Assert.That(target.Health, Is.EqualTo(target.MaxHealth - shotgun.Damage));
            Assert.That(pelletHits, Is.EqualTo(5));
            Assert.That(craterCount, Is.Zero);
        }

        [TestCase(0, 0.1f)]
        [TestCase(4, 0.132f)]
        public void IonShotgun_RadiusPurchasesExpandPelletContactWithoutChangingPelletCount(
            int purchases, float expectedRadius)
        {
            var shotgun = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                weapon => weapon.WeaponId == "weapon.ion-shotgun");
            var progress = new SaveWeaponProgressData
            {
                weaponId = shotgun.WeaponId,
                rank = 1 + purchases,
                upgradeChoices = new List<SaveWeaponUpgradeChoiceData>()
            };
            for (var rank = 2; rank <= progress.rank; rank++)
            {
                progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                {
                    rank = rank,
                    path = (int)WeaponUpgradePath.Radius
                });
            }

            var harness = CreateHarness(shotgun, playerWeaponProgress: progress);
            Assert.That(harness.Simulation.TryFire(new AimState(
                new Vector2(-3f, 0f), Vector2.right, 8f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            Assert.That(harness.Simulation.Projectiles, Has.Count.EqualTo(5));
            foreach (var pellet in harness.Simulation.Projectiles)
            {
                Assert.That(pellet.Definition.Radius, Is.EqualTo(expectedRadius).Within(0.0001f));
            }
            Assert.That(WeaponUpgradeChoiceResolver.ResolveValue(shotgun, null,
                progress, WeaponUpgradePath.Radius), Is.EqualTo(expectedRadius).Within(0.0001f));
        }

        [TestCase(0, 38)]
        [TestCase(4, 380)]
        public void IonShotgun_CorePurchasesIncreaseTheFivePelletsTotalDamage(
            int purchases, int expectedDamage)
        {
            var shotgun = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                weapon => weapon.WeaponId == "weapon.ion-shotgun");
            var progress = new SaveWeaponProgressData
            {
                weaponId = shotgun.WeaponId,
                rank = 1 + purchases,
                upgradeChoices = new List<SaveWeaponUpgradeChoiceData>()
            };
            for (var rank = 2; rank <= progress.rank; rank++)
            {
                progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                {
                    rank = rank,
                    path = (int)WeaponUpgradePath.Core
                });
            }

            var harness = CreateHarness(shotgun, playerWeaponProgress: progress);
            Assert.That(harness.Simulation.TryFire(new AimState(
                new Vector2(-3f, 0f), Vector2.right, 8f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            Assert.That(harness.Simulation.Projectiles, Has.Count.EqualTo(5));
            var totalDamage = 0;
            foreach (var pellet in harness.Simulation.Projectiles)
            {
                totalDamage += pellet.Definition.Damage;
            }

            Assert.That(totalDamage, Is.EqualTo(expectedDamage));
            Assert.That(WeaponUpgradeChoiceResolver.ResolveValue(shotgun, null,
                progress, WeaponUpgradePath.Core), Is.EqualTo(expectedDamage).Within(0.5f));
        }

        [TestCase(0, 0.2f)]
        [TestCase(4, 0.264f)]
        public void IonShotgun_TerrainContactCarvesCraterTwicePelletRadius(
            int purchases, float expectedCraterRadius)
        {
            var shotgun = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                weapon => weapon.WeaponId == "weapon.ion-shotgun");
            var progress = new SaveWeaponProgressData
            {
                weaponId = shotgun.WeaponId,
                rank = 1 + purchases,
                upgradeChoices = new List<SaveWeaponUpgradeChoiceData>()
            };
            for (var rank = 2; rank <= progress.rank; rank++)
            {
                progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                {
                    rank = rank,
                    path = (int)WeaponUpgradePath.Radius
                });
            }

            var harness = CreateHarness(shotgun, playerWeaponProgress: progress);
            var target = harness.State.GetUnit("opponent.test.01");
            var targetHealth = target.Health;
            var craterRadii = new List<float>();
            harness.Simulation.TerrainCraterApplied += (_, _, radius) => craterRadii.Add(radius);

            Assert.That(harness.Simulation.TryFire(new AimState(
                new Vector2(-3f, 0f), Vector2.down, 8f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 240, 1f / 120f);

            Assert.That(craterRadii, Is.Not.Empty);
            foreach (var radius in craterRadii)
            {
                Assert.That(radius, Is.EqualTo(expectedCraterRadius).Within(0.0001f));
            }
            Assert.That(target.Health, Is.EqualTo(targetHealth));
        }

        [Test]
        public void Grenade_FuseCountsFromThrow_AndExplodesWhileResting()
        {
            var grenade = CreateWeapon(
                "weapon.granata",
                gravityScale: 1.1f,
                timeoutSeconds: 3f);
            var harness = CreateHarness(grenade);
            var exploded = false;
            harness.Simulation.ProjectileExploded += (id, impact) => exploded = true;

            Assert.AreEqual(
                BattleFireOutcome.Fired,
                harness.Simulation.TryFire(new AimState(new Vector2(-3f, 0f), Vector2.down, 6f, true)).Outcome);
            for (var i = 0;
                 i < 600
                 && harness.Simulation.Projectiles.Count > 0
                 && !harness.Simulation.Projectiles[0].IsRestingFuse;
                 i++)
            {
                harness.Simulation.Tick(1f / 120f);
            }

            Assert.AreEqual(1, harness.Simulation.Projectiles.Count);
            var projectile = harness.Simulation.Projectiles[0];
            Assert.IsTrue(projectile.IsRestingFuse);
            Assert.IsFalse(exploded);
            Assert.LessOrEqual(
                projectile.RestingFuseRemaining,
                3f,
                "the ground fuse continues the from-throw timer instead of restarting");

            TickMany(harness.Simulation, 100, 1f / 120f);
            Assert.IsFalse(exploded, "the resting grenade waits out the remainder of its fuse");
            TickMany(harness.Simulation, 300, 1f / 120f);
            Assert.IsTrue(exploded, "the grenade explodes once the from-throw fuse expires");
            TickMany(harness.Simulation, 400, 1f / 120f);
            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam);
        }

        [TestCase(0, 1.55f)]
        [TestCase(1, 1.705f)]
        [TestCase(4, 2.17f)]
        public void Grenade_RadiusPurchasesAffectTheActualBlast(int purchases, float expectedRadius)
        {
            var grenade = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                weapon => weapon.WeaponId == "weapon.granata");
            var progress = new SaveWeaponProgressData
            {
                weaponId = grenade.WeaponId,
                rank = 1 + purchases,
                upgradeChoices = new List<SaveWeaponUpgradeChoiceData>()
            };
            for (var rank = 2; rank <= progress.rank; rank++)
            {
                progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                {
                    rank = rank,
                    path = (int)WeaponUpgradePath.Radius
                });
            }

            var harness = CreateHarness(grenade, playerWeaponProgress: progress);
            Assert.That(harness.Simulation.TryFire(new AimState(
                new Vector2(-3f, 0f), Vector2.right, 8f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.AimRequired));
            Assert.That(harness.Simulation.TryFire(new AimState(
                new Vector2(-3f, 0f), Vector2.right, 8f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            Assert.That(harness.Simulation.Projectiles, Has.Count.EqualTo(1));
            Assert.That(harness.Simulation.Projectiles[0].Definition.ImpactRadius,
                Is.EqualTo(expectedRadius).Within(0.0001f));
            Assert.That(WeaponUpgradeChoiceResolver.ResolveValue(grenade,
                null, progress, WeaponUpgradePath.Radius),
                Is.EqualTo(expectedRadius).Within(0.0001f));
        }

        [Test]
        public void Grenade_TargetPullSteersTowardEnemyWithoutSelectingACloserAlly()
        {
            var grenade = CreateWeapon("weapon.granata", gravityScale: 1.1f);
            var progress = new SaveWeaponProgressData
            {
                weaponId = grenade.WeaponId,
                rank = 5,
                evolutionChoices = new List<SaveWeaponEvolutionChoiceData>
                {
                    new() { rank = 5, optionId = "target-pull" }
                }
            };
            var harness = CreateHarness(grenade, playerWeaponProgress: progress,
                secondPlayerPosition: new Vector2(-1.9f, 0.55f));
            var opponent = harness.State.GetUnit("opponent.test.01");
            opponent.MoveTo(new Vector2(-1.35f, 0.55f));
            harness.Motors[opponent.UnitId].Teleport(opponent.Position);

            Assert.That(harness.Simulation.TryFire(new AimState(
                new Vector2(-3f, 0f), Vector2.right, 8f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            var projectile = harness.Simulation.Projectiles[0];
            TickMany(harness.Simulation, 1, 1f / 120f);

            Assert.That(projectile.Velocity.x, Is.GreaterThan(0f),
                "seeking must keep the grenade heading toward the enemy, not the nearby ally");
            Assert.That(projectile.Velocity.y, Is.GreaterThan(-0.01f),
                "the purchased target pull should bend the throw toward the raised enemy");
        }

        [Test]
        public void Grenade_GravityPullAtRestAttractsAFreeProjectileBeforeDetonation()
        {
            var grenade = CreateWeapon("weapon.granata", gravityScale: 1.1f, timeoutSeconds: 3f);
            var progress = new SaveWeaponProgressData
            {
                weaponId = grenade.WeaponId,
                rank = 10,
                evolutionChoices = new List<SaveWeaponEvolutionChoiceData>
                {
                    new() { rank = 5, optionId = "target-pull" },
                    new() { rank = 10, optionId = "gravity-pull" }
                }
            };
            var harness = CreateHarness(grenade, playerWeaponProgress: progress);
            Assert.That(harness.Simulation.TryFire(new AimState(
                new Vector2(-3f, 0f), Vector2.down, 6f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            for (var i = 0; i < 600 && !harness.Simulation.Projectiles[0].IsRestingFuse; i++)
            {
                harness.Simulation.Tick(1f / 120f);
            }

            var restingGrenade = harness.Simulation.Projectiles[0];
            Assert.That(restingGrenade.IsRestingFuse, Is.True);
            var freeShot = ProjectileDefinition.CreateRuntime(
                "projectile.test.gravity-grenade-target", 0.1f, 0f, 3f, 0, 0.1f);
            harness.Simulation.Spawn(new ProjectileSpawnRequest(
                freeShot, "opponent.test.01", restingGrenade.Position + new Vector2(1f, 0.4f),
                Vector2.zero, 0.2f, Color.white));
            var target = harness.Simulation.Projectiles[1];
            TickMany(harness.Simulation, 2, 1f / 120f);

            Assert.That(target.Velocity.x, Is.LessThan(0f),
                "gravity pull should attract an unanchored physical shot while the fuse counts down");
            Assert.That(restingGrenade.IsRestingFuse, Is.True);
        }

        [Test]
        public void HoppingGrenade_CanJumpAgainAfterLandingWithoutConsumingAnotherItem()
        {
            var grenade = CreateWeapon(
                "weapon.hopping-grenade",
                gravityScale: 1.1f,
                timeoutSeconds: 3f,
                ammo: 1);
            var harness = CreateHarness(grenade);

            Assert.AreEqual(
                BattleFireOutcome.Fired,
                harness.Simulation.TryFire(new AimState(new Vector2(-3f, 0f), Vector2.down, 6f, true)).Outcome);
            for (var i = 0; i < 600 && !harness.Simulation.CanContinueHoppingGrenade(); i++)
            {
                harness.Simulation.Tick(1f / 120f);
            }

            Assert.IsTrue(harness.Simulation.CanContinueHoppingGrenade());
            Assert.AreEqual(1, harness.Simulation.Projectiles.Count);
            var projectile = harness.Simulation.Projectiles[0];
            Assert.IsTrue(projectile.IsRestingFuse);

            Assert.AreEqual(
                BattleFireOutcome.Fired,
                harness.Simulation.TryFire(new AimState(projectile.Position, Vector2.right, 1f, true)).Outcome);
            Assert.IsFalse(projectile.IsRestingFuse);
            Assert.Greater(projectile.Velocity.y, 0f);
        }

        [Test]
        public void IncendiaryGrenade_ImpactSpawnsFourLocalFirePuddles()
        {
            var grenade = CreateWeapon(
                "weapon.incendiary-grenade",
                gravityScale: 1.1f,
                timeoutSeconds: 3f);
            var harness = CreateHarness(grenade);
            var blobs = new List<FireBlobEvent>();
            harness.Simulation.FireBlobSpawned += blobs.Add;

            Assert.AreEqual(
                BattleFireOutcome.Fired,
                harness.Simulation.TryFire(new AimState(new Vector2(-3f, 0f), Vector2.down, 6f, true)).Outcome);
            TickMany(harness.Simulation, 600, 1f / 120f);

            Assert.AreEqual(4, blobs.Count);
            Assert.That(blobs, Has.All.Matches<FireBlobEvent>(blob =>
                Vector2.Distance(blob.Origin, blob.ImpactPoint) <= 0.56f));
        }

        [Test]
        public void Mine_IsThrownBeforeItBecomesDeployed()
        {
            var mine = CreateWeapon(
                "weapon.mine",
                damage: 54,
                ammo: 1,
                gravityScale: 1.05f,
                timeoutSeconds: 8f);
            var harness = CreateHarness(mine);
            SettleUnits(harness);
            var deployed = false;
            harness.Simulation.MineDeployed += value => deployed = true;

            Assert.AreEqual(
                BattleFireOutcome.Fired,
                harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome);
            Assert.AreEqual(1, harness.Simulation.ProjectilesInFlight);
            Assert.IsFalse(deployed);

            for (var i = 0; i < 360 && !deployed; i++)
            {
                harness.Simulation.Tick(1f / 120f);
            }

            Assert.IsTrue(deployed);
            Assert.AreEqual(0, harness.Simulation.ProjectilesInFlight);
            Assert.AreEqual(BattleTeamId.Player, harness.State.Turn.ActiveTeam);
        }

        [Test]
        public void Fpv_HoversWithoutInput_AndMovesOnlyWhileDirected()
        {
            var fpv = CreateWeapon(
                "weapon.fpv-drone",
                WeaponRole.DirectProjectile,
                damage: 58,
                ammo: 1,
                gravityScale: 0f,
                timeoutSeconds: 8f);
            var harness = CreateHarness(fpv);

            Assert.IsFalse(BattleSimulation.RequiresAim(fpv));
            Assert.AreEqual(
                BattleFireOutcome.Fired,
                harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome);
            Assert.IsTrue(harness.Simulation.HasActiveControlledProjectile);
            var spawnPoint = harness.Simulation.Projectiles[0].Position;

            TickMany(harness.Simulation, 30, 1f / 120f);
            Assert.That(harness.Simulation.Projectiles[0].Position, Is.EqualTo(spawnPoint));

            harness.Simulation.SetControlledProjectileInput(Vector2.up);
            TickMany(harness.Simulation, 10, 1f / 120f);
            var movedPoint = harness.Simulation.Projectiles[0].Position;
            Assert.Greater(movedPoint.y, spawnPoint.y + 1f);

            harness.Simulation.SetControlledProjectileInput(Vector2.zero);
            TickMany(harness.Simulation, 10, 1f / 120f);
            Assert.That(harness.Simulation.Projectiles[0].Position, Is.EqualTo(movedPoint));
        }

        [Test]
        public void DrillRocket_CarriesRemainingDrillTimeBetweenTerrainSections()
        {
            var drillRocket = CreateWeapon(
                "weapon.bur-rocket",
                gravityScale: 0f,
                timeoutSeconds: 8f);
            var harness = CreateHarness(drillRocket);
            harness.Terrain.StampSolidRect(new Vector2(-0.75f, 4f), new Vector2(0.5f, 1.5f));
            harness.Terrain.StampSolidRect(new Vector2(5.25f, 4f), new Vector2(9.5f, 1.5f));

            // Slow shot (3 m/s) so the 1 s drill budget genuinely runs out inside
            // the long second section instead of the rocket tunnelling through.
            var result = harness.Simulation.TryFire(new AimState(new Vector2(-3f, 4f), Vector2.right, 3f, true));
            Assert.AreEqual(BattleFireOutcome.Fired, result.Outcome);
            Assert.That(
                harness.Simulation.Projectiles[0].TerrainDrillTimeRemaining,
                Is.EqualTo(1f).Within(0.0001f));

            TickMany(harness.Simulation, 120, 1f / 120f);
            Assert.AreEqual(1, harness.Simulation.ProjectilesInFlight, "the rocket exits the first terrain section without exploding");
            Assert.That(harness.Simulation.Projectiles[0].Velocity.magnitude, Is.EqualTo(3f).Within(0.01f));
            Assert.Less(harness.Simulation.Projectiles[0].TerrainDrillTimeRemaining, 1f);
            Assert.Greater(harness.Simulation.Projectiles[0].TerrainDrillTimeRemaining, 0.5f);

            TickMany(harness.Simulation, 600, 1f / 120f);
            Assert.AreEqual(0, harness.Simulation.ProjectilesInFlight, "the rocket explodes after consuming one cumulative second inside terrain");
        }

        [TestCase(TerrainMaterial.Soil)]
        [TestCase(TerrainMaterial.LightMetal)]
        [TestCase(TerrainMaterial.Titanium)]
        public void CanonicalDrillRocket_UsesSharedShotAndDrillsEveryTerrain(TerrainMaterial material)
        {
            var rocket = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                weapon => weapon.WeaponId == "weapon.bur-rocket");
            var harness = CreateHarness(rocket);
            harness.Terrain.StampSolidRect(new Vector2(-0.75f, 4f),
                new Vector2(0.5f, 1.5f), material);

            Assert.That(harness.Simulation.TryFire(new AimState(
                new Vector2(-3f, 4f), Vector2.right, 1f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            var projectile = harness.Simulation.Projectiles[0];
            Assert.That(projectile.Velocity.magnitude,
                Is.EqualTo(PrototypeWeaponLibrary.SharedProjectileSpeed).Within(0.001f));
            Assert.That(projectile.Definition.ImpactRadius,
                Is.EqualTo(PrototypeWeaponLibrary.SharedExplosionRadius).Within(0.001f));
            Assert.That(projectile.TerrainDrillPenetration, Is.EqualTo(TerrainMaterial.Titanium));
            TickMany(harness.Simulation, 50, 1f / 120f);
            Assert.That(projectile.Position.x, Is.GreaterThan(-0.4f));
            Assert.That(harness.Terrain.Grid.IsSolidWorld(new Vector2(-0.75f, 4f)), Is.False);
        }

        [TestCase(0, 1f)]
        [TestCase(1, 1.1f)]
        [TestCase(4, 1.4f)]
        public void CanonicalDrillRocket_RadiusPurchasesExtendGroundTimeOnly(int purchases, float expectedSeconds)
        {
            var rocket = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                weapon => weapon.WeaponId == "weapon.bur-rocket");
            var progress = PurchasedBaseStats(rocket.WeaponId, 0, purchases);
            var harness = CreateHarness(rocket, playerWeaponProgress: progress);

            Assert.That(harness.Simulation.TryFire(new AimState(
                new Vector2(-3f, 4f), Vector2.right, 99f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            var projectile = harness.Simulation.Projectiles[0];
            Assert.That(projectile.TerrainDrillTimeRemaining,
                Is.EqualTo(expectedSeconds).Within(0.001f));
            Assert.That(projectile.Definition.ImpactRadius,
                Is.EqualTo(PrototypeWeaponLibrary.SharedExplosionRadius).Within(0.001f));
            Assert.That(WeaponProgressionCatalog.TryGet(rocket.WeaponId, out var progression), Is.True);
            Assert.That(WeaponUpgradeChoiceResolver.ResolveValue(rocket,
                progression, progress, WeaponUpgradePath.Radius),
                Is.EqualTo(expectedSeconds).Within(0.001f));
            TickMany(harness.Simulation, 30, 1f / 120f);
            Assert.That(projectile.TerrainDrillTimeRemaining,
                Is.EqualTo(expectedSeconds).Within(0.001f), "flight in air must not consume drilling range");
        }

        [Test]
        public void DrillRocket_PassesThroughLightMetalWithoutEvolution()
        {
            var drillRocket = CreateWeapon(
                "weapon.bur-rocket",
                gravityScale: 0f,
                timeoutSeconds: 8f);
            var harness = CreateHarness(drillRocket);
            harness.Terrain.StampSolidRect(
                new Vector2(-0.75f, 4f),
                new Vector2(0.5f, 1.5f),
                TerrainMaterial.LightMetal);

            Assert.AreEqual(
                BattleFireOutcome.Fired,
                harness.Simulation.TryFire(new AimState(new Vector2(-3f, 4f), Vector2.right, 3f, true)).Outcome);
            TickMany(harness.Simulation, 100, 1f / 120f);

            Assert.AreEqual(1, harness.Simulation.ProjectilesInFlight);
            Assert.Greater(harness.Simulation.Projectiles[0].Position.x, -0.5f);
            Assert.That(
                harness.Simulation.Projectiles[0].TerrainDrillPenetration,
                Is.EqualTo(TerrainMaterial.Titanium));
        }

        [TestCase(2)]
        [TestCase(3)]
        public void HomingMissile_BallisticEvolution_SplitsIntoConfiguredCharges(int chargeCount)
        {
            var missile = CreateWeapon(
                "weapon.missle",
                damage: 52,
                impactRadius: 1.4f,
                gravityScale: 1f,
                timeoutSeconds: 8f);
            var evolutions = new Dictionary<string, WeaponEvolutionProfile>
            {
                [missile.WeaponId] = new WeaponEvolutionProfile(
                    1,
                    0f,
                    1f,
                    missileSplitChargeCount: chargeCount)
            };
            var harness = CreateHarness(missile, playerWeaponEvolutions: evolutions);
            var clusterCharges = new List<SimProjectile>();
            harness.Simulation.ProjectileSpawned += projectile =>
            {
                if (projectile.Definition.ProjectileId.StartsWith(
                        "projectile.weapon.missle.cluster.",
                        System.StringComparison.Ordinal))
                {
                    clusterCharges.Add(projectile);
                }
            };

            Assert.AreEqual(
                BattleFireOutcome.Fired,
                harness.Simulation.TryFire(new AimState(new Vector2(-3f, 1f), Vector2.up, 10f, true)).Outcome);
            TickMany(harness.Simulation, 720, 1f / 120f);

            Assert.AreEqual(chargeCount, clusterCharges.Count);
            Assert.AreEqual(Mathf.CeilToInt(missile.Damage / (float)chargeCount), clusterCharges[0].Definition.Damage);
            Assert.That(
                clusterCharges[0].Definition.ImpactRadius,
                Is.EqualTo(missile.ImpactRadius * 0.58f).Within(0.001f));
        }

        [Test]
        public void HomingMissile_ManeuverEvolution_CreatesDetourAroundTerrainObstacle()
        {
            var missile = CreateWeapon(
                "weapon.missle",
                gravityScale: 0f,
                timeoutSeconds: 8f);
            var evolutions = new Dictionary<string, WeaponEvolutionProfile>
            {
                [missile.WeaponId] = new WeaponEvolutionProfile(
                    1,
                    0f,
                    1f,
                    missileObstacleBypassCount: 1)
            };
            var harness = CreateHarness(missile, playerWeaponEvolutions: evolutions);
            harness.Terrain.StampSolidRect(new Vector2(0f, 1f), new Vector2(1f, 4f));

            Assert.AreEqual(
                BattleFireOutcome.Fired,
                harness.Simulation.TryFire(new AimState(new Vector2(-3f, 0f), Vector2.up, 6f, true)).Outcome);
            TickMany(harness.Simulation, 70, 1f / 120f);

            Assert.AreEqual(1, harness.Simulation.ProjectilesInFlight);
            Assert.AreEqual(0, harness.Simulation.Projectiles[0].MissileObstacleBypassesRemaining);
            Assert.That(harness.Simulation.Projectiles[0].HasMissileWaypoint, Is.True);
        }

        [Test]
        public void ProjectileTimeout_ExplodesAndAdvancesTurn()
        {
            var weapon = CreateWeapon("weapon.test.timeout", gravityScale: 0f, timeoutSeconds: 0.12f);
            var harness = CreateHarness(weapon);

            var exploded = false;
            var advanced = false;
            harness.Simulation.ProjectileExploded += (projectileId, impact) => exploded = true;
            harness.Simulation.TurnAdvanced += (team, reason) => advanced = true;

            var result = harness.Simulation.TryFire(new AimState(new Vector2(-3f, 4f), Vector2.up, 6f, true));
            Assert.AreEqual(BattleFireOutcome.Fired, result.Outcome);

            for (var i = 0; i < 300 && !advanced; i++)
            {
                harness.Simulation.Tick(1f / 60f);
            }

            Assert.IsTrue(exploded, "projectile timeout resolves without a scene object");
            Assert.IsTrue(advanced, "timeout of the last projectile advances the turn (after the retreat window)");
            Assert.AreEqual(0, harness.Simulation.ProjectilesInFlight);
        }

        [Test]
        public void ProjectileIgnoresOwnerDuringLaunch()
        {
            var weapon = CreateWeapon("weapon.test.owner-skip", gravityScale: 0f);
            var harness = CreateHarness(weapon);

            var exploded = false;
            harness.Simulation.ProjectileExploded += (projectileId, impact) => exploded = true;

            var result = harness.Simulation.TryFire(new AimState(new Vector2(-3f, 0f), Vector2.right, 1f, true));
            Assert.AreEqual(BattleFireOutcome.Fired, result.Outcome);

            harness.Simulation.Tick(1f / 120f);

            Assert.IsFalse(exploded, "projectile does not collide with its owner at launch");
            Assert.AreEqual(1, harness.Simulation.ProjectilesInFlight);
        }

        [Test]
        public void ExplosionNearOwner_DamagesOwnerAtNormalStrength()
        {
            var weapon = CreateWeapon("weapon.test.self-damage", damage: 60, impactRadius: 2.5f, gravityScale: 0f);
            var harness = CreateHarness(weapon);
            var player = harness.State.GetUnit("player.test.01");

            var result = harness.Simulation.TryFire(new AimState(new Vector2(-3f, -0.85f), Vector2.down, 6f, true));
            Assert.AreEqual(BattleFireOutcome.Fired, result.Outcome);

            // Replay the same blast with another owner: radial falloff must be
            // identical, without an additional self-damage discount.
            var hostileHarness = CreateHarness(weapon);
            var projectile = harness.Simulation.Projectiles[0];
            hostileHarness.Simulation.Spawn(new ProjectileSpawnRequest(
                projectile.Definition, "opponent.test.01", projectile.Position,
                projectile.Velocity, 0.2f, Color.white));

            for (var i = 0; i < 120 && harness.Simulation.ProjectilesInFlight > 0; i++)
            {
                harness.Simulation.Tick(1f / 120f);
                hostileHarness.Simulation.Tick(1f / 120f);
            }

            Assert.Less(player.Health, player.MaxHealth);
            Assert.AreEqual(hostileHarness.State.GetUnit("player.test.01").Health, player.Health,
                "The same blast must deal equal damage whether the victim owns it or not.");
        }

        [Test]
        public void DrillLaser_DoesNotDamageOwner()
        {
            var drill = CreateWeapon("weapon.bur", WeaponRole.ShortRangeBurst, damage: 18, impactRadius: 0.7f, ammo: 1, gravityScale: 0f);
            var harness = CreateHarness(drill);
            var player = harness.State.GetUnit("player.test.01");
            var startHealth = player.Health;

            var result = harness.Simulation.TryFire(new AimState(player.Position, Vector2.right, 3f, true));
            Assert.AreEqual(BattleFireOutcome.Fired, result.Outcome);

            for (var i = 0; i < 420; i++)
            {
                harness.Simulation.Tick(1f / 120f);
            }

            Assert.AreEqual(startHealth, player.Health);
        }

        [TestCase(0, 3.25f)]
        [TestCase(1, 3.575f)]
        [TestCase(4, 4.55f)]
        public void DrillLaser_RadiusPurchasesIncreaseBattleAndWorkshopRange(int purchases, float expectedRange)
        {
            var drill = CreateWeapon("weapon.bur", WeaponRole.ShortRangeBurst, ammo: -1, gravityScale: 0f);
            var progress = new SaveWeaponProgressData
            {
                weaponId = drill.WeaponId,
                rank = 1 + purchases,
                upgradeChoices = new List<SaveWeaponUpgradeChoiceData>()
            };
            for (var rank = 2; rank <= progress.rank; rank++)
            {
                progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                {
                    rank = rank,
                    path = (int)WeaponUpgradePath.Radius
                });
            }

            var harness = CreateHarness(drill, playerWeaponProgress: progress);
            var reportedRange = 0f;
            harness.Simulation.LinearWeaponEffectStarted += effect => reportedRange = effect.Range;

            Assert.That(harness.Simulation.TryFire(new AimState(
                new Vector2(-3f, 0f), Vector2.right, 3f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.AimRequired));

            Assert.That(
                harness.Simulation.TryFire(new AimState(new Vector2(-3f, 0f), Vector2.right, 3f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));

            Assert.That(reportedRange, Is.EqualTo(expectedRange).Within(0.0001f));
            Assert.That(harness.Simulation.ProjectilesInFlight, Is.Zero,
                "the drill laser is a beam and does not spawn a flying shell");
            Assert.That(WeaponUpgradeChoiceResolver.ResolveValue(drill,
                null, progress, WeaponUpgradePath.Radius),
                Is.EqualTo(expectedRange).Within(0.0001f));
        }

        [TestCase(TerrainMaterial.Soil)]
        [TestCase(TerrainMaterial.LightMetal)]
        [TestCase(TerrainMaterial.Titanium)]
        public void DrillLaser_CarvesEveryDestructibleMaterial(TerrainMaterial material)
        {
            var drill = CreateWeapon("weapon.bur", WeaponRole.ShortRangeBurst, ammo: -1, gravityScale: 0f);
            var harness = CreateHarness(drill);
            var sample = new Vector2(-1.5f, 0f);
            harness.Terrain.StampSolidRect(sample, new Vector2(0.5f, 0.5f), material);
            Assert.That(harness.Terrain.Grid.IsSolidWorld(sample), Is.True);

            Assert.That(harness.Simulation.TryFire(new AimState(
                new Vector2(-3f, 0f), Vector2.right, 3f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 180, 1f / 120f);

            Assert.That(harness.Terrain.Grid.IsSolidWorld(sample), Is.False,
                $"the beam should carve {material} inside its range");
        }

        [Test]
        public void DrillLaser_StarterDefinitionHasUnlimitedAmmo()
        {
            var drill = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                weapon => weapon.WeaponId == "weapon.bur");
            Assert.That(drill, Is.Not.Null);
            Assert.That(drill.HasLimitedAmmo, Is.False);
        }

        [Test]
        public void CommandScript_ReplaysWithMatchingHashes()
        {
            var first = RunDeterministicScript();
            var second = RunDeterministicScript();

            Assert.AreEqual(first.Count, second.Count);
            for (var i = 0; i < first.Count; i++)
            {
                Assert.AreEqual(first[i], second[i], $"hash mismatch after fixed tick {i}");
            }
        }

        [Test]
        public void SameTotalTickTime_WithDifferentChunks_ProducesSameFinalState()
        {
            var weapon = CreateWeapon("weapon.test.chunked", gravityScale: 0f);
            var fixedHarness = CreateHarness(weapon);
            var chunkedHarness = CreateHarness(weapon);

            fixedHarness.Simulation.Walk(1, 0f);
            TickMany(fixedHarness.Simulation, 60, 1f / 120f);
            fixedHarness.Simulation.Walk(0, 0f);
            fixedHarness.Simulation.TryStartJump(1);
            TickMany(fixedHarness.Simulation, 180, 1f / 120f);
            fixedHarness.Simulation.TryFire(new AimState(new Vector2(-3f, 0f), Vector2.right, 20f, true));
            TickMany(fixedHarness.Simulation, 120, 1f / 120f);

            chunkedHarness.Simulation.Walk(1, 0f);
            TickMany(chunkedHarness.Simulation, 30, 1f / 60f);
            chunkedHarness.Simulation.Walk(0, 0f);
            chunkedHarness.Simulation.TryStartJump(1);
            TickMany(chunkedHarness.Simulation, 90, 1f / 60f);
            chunkedHarness.Simulation.TryFire(new AimState(new Vector2(-3f, 0f), Vector2.right, 20f, true));
            TickMany(chunkedHarness.Simulation, 60, 1f / 60f);

            Assert.AreEqual(HashSimulation(fixedHarness.Simulation), HashSimulation(chunkedHarness.Simulation));
        }

        [Test]
        public void ToxicEvolutionResolver_ReturnsConfiguredArmorWeakenAndMovementDamageValues()
        {
            var weapon = CreateWeapon("weapon.toxic", WeaponRole.DirectProjectile, 30, 1.2f, gravityScale: 0f);

            var branchA5 = WeaponEvolutionRuntimeResolver.Resolve(weapon,
                new SaveWeaponProgressData { weaponId = weapon.WeaponId, rank = 5, branch = (int)WeaponUpgradeBranch.A });
            var branchA10 = WeaponEvolutionRuntimeResolver.Resolve(weapon,
                new SaveWeaponProgressData { weaponId = weapon.WeaponId, rank = 10, branch = (int)WeaponUpgradeBranch.A });
            var branchB5 = WeaponEvolutionRuntimeResolver.Resolve(weapon,
                new SaveWeaponProgressData { weaponId = weapon.WeaponId, rank = 5, branch = (int)WeaponUpgradeBranch.B });
            var branchB10 = WeaponEvolutionRuntimeResolver.Resolve(weapon,
                new SaveWeaponProgressData { weaponId = weapon.WeaponId, rank = 10, branch = (int)WeaponUpgradeBranch.B });

            Assert.Greater(branchA10.ToxicArmorWeakenPercent, branchA5.ToxicArmorWeakenPercent,
                "10A weakens armor more than 5A");
            Assert.GreaterOrEqual(branchA10.ToxicArmorWeakenTurns, branchA5.ToxicArmorWeakenTurns,
                "10A holds the armor weaken at least as long as 5A");
            Assert.Greater(branchB10.ToxicMovementDamagePerSecond, branchB5.ToxicMovementDamagePerSecond,
                "10B hits harder per second of movement than 5B");
            Assert.Greater(branchB10.ToxicDurationBonusTurns, branchB5.ToxicDurationBonusTurns,
                "10B keeps the poison active a turn longer than 5B");
            Assert.AreEqual(0, branchA5.ToxicMovementDamagePerSecond, "branch A does not add movement damage");
            Assert.AreEqual(0, branchB5.ToxicArmorWeakenPercent, "branch B does not weaken armor");
        }

        [Test]
        public void ToxicWeapon_AppliesTwoEndOfVictimTurnTicksEqualToTwentyPercentOfImpactDamage()
        {
            var weapon = CreateWeapon("weapon.toxic", WeaponRole.DirectProjectile, 30, 1.2f, gravityScale: 0f);
            var harness = CreateHarness(weapon);
            var opponent = harness.State.GetUnit("opponent.test.01");

            var exploded = false;
            var turnAdvanced = false;
            harness.Simulation.ProjectileExploded += (id, impact) => exploded = true;
            harness.Simulation.TurnAdvanced += (team, reason) => turnAdvanced = true;

            harness.Simulation.TryFire(new AimState(new Vector2(-3f, 0f), Vector2.right, 20f, true));
            for (var i = 0; i < 240 && !exploded; i++)
            {
                harness.Simulation.Tick(1f / 60f);
            }
            Assert.IsTrue(exploded, "the toxic projectile resolves inside the simulation");

            var healthAfterImpact = opponent.Health;
            var directDamage = opponent.MaxHealth - healthAfterImpact;
            var expectedTickDamage = Mathf.RoundToInt(directDamage * 0.2f);
            Assert.IsTrue(harness.Simulation.IsUnitPoisoned(opponent.UnitId));

            for (var i = 0; i < 420 && !turnAdvanced; i++)
            {
                harness.Simulation.Tick(1f / 60f);
            }

            Assert.IsTrue(turnAdvanced);
            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam);
            Assert.AreEqual(healthAfterImpact, opponent.Health,
                "a toxic tick waits until the poisoned unit finishes its turn");

            harness.Simulation.EndActiveTurn();
            Assert.AreEqual(healthAfterImpact - expectedTickDamage, opponent.Health,
                "the first toxic tick is 20% of the direct impact damage");
            Assert.IsTrue(harness.Simulation.IsUnitPoisoned(opponent.UnitId));

            harness.Simulation.EndActiveTurn();
            harness.Simulation.EndActiveTurn();
            Assert.AreEqual(healthAfterImpact - expectedTickDamage * 2, opponent.Health,
                "the second and final toxic tick occurs at the end of the victim's following turn");
            Assert.IsFalse(harness.Simulation.IsUnitPoisoned(opponent.UnitId));
        }

        [Test]
        public void ToxicWeapon_StatusDurationStaysAtTwoVictimTurnsDespiteLegacyDurationEvolution()
        {
            var weapon = CreateWeapon("weapon.toxic", WeaponRole.DirectProjectile, 30, 1.2f, gravityScale: 0f);
            var evolutions = new Dictionary<string, WeaponEvolutionProfile>
            {
                [weapon.WeaponId] = new WeaponEvolutionProfile(1, 0f, 1f, toxicDurationBonusTurns: 3)
            };
            var harness = CreateHarness(weapon, playerWeaponEvolutions: evolutions);
            var opponent = harness.State.GetUnit("opponent.test.01");
            var exploded = false;
            harness.Simulation.ProjectileExploded += (id, impact) => exploded = true;

            harness.Simulation.TryFire(new AimState(new Vector2(-3f, 0f), Vector2.right, 20f, true));
            for (var i = 0; i < 240 && !exploded; i++)
            {
                harness.Simulation.Tick(1f / 60f);
            }

            Assert.IsTrue(exploded);
            for (var victimTurn = 0; victimTurn < 2; victimTurn++)
            {
                harness.Simulation.EndActiveTurn();
                harness.Simulation.EndActiveTurn();
            }

            Assert.IsFalse(harness.Simulation.IsUnitPoisoned(opponent.UnitId),
                "the elemental poison always expires after two completed turns of its victim");
        }

        [Test]
        public void ToxicWeaponBranchB_DealsExtraDamage_WhilePoisonedTargetMoves()
        {
            var weapon = CreateWeapon("weapon.toxic", WeaponRole.DirectProjectile, 30, 1.2f, gravityScale: 0f);
            var evolutions = new Dictionary<string, WeaponEvolutionProfile>
            {
                [weapon.WeaponId] = new WeaponEvolutionProfile(1, 0f, 1f,
                    toxicMovementDamagePerSecond: 6f, toxicDurationBonusTurns: 0)
            };
            var harness = CreateHarness(weapon, playerWeaponEvolutions: evolutions);
            var opponent = harness.State.GetUnit("opponent.test.01");

            var exploded = false;
            var turnAdvanced = false;
            harness.Simulation.ProjectileExploded += (id, impact) => exploded = true;
            harness.Simulation.TurnAdvanced += (team, reason) => turnAdvanced = true;

            harness.Simulation.TryFire(new AimState(new Vector2(-3f, 0f), Vector2.right, 20f, true));
            for (var i = 0; i < 240 && !exploded; i++)
            {
                harness.Simulation.Tick(1f / 60f);
            }
            Assert.IsTrue(exploded);

            for (var i = 0; i < 420 && !turnAdvanced; i++)
            {
                harness.Simulation.Tick(1f / 60f);
            }

            Assert.IsTrue(turnAdvanced);
            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam,
                "the poisoned opponent is now the active unit and can move itself");

            var healthBeforeMovement = opponent.Health;
            harness.Simulation.Walk(1, 0f);
            TickMany(harness.Simulation, 600, 1f / 60f);

            Assert.Less(opponent.Health, healthBeforeMovement,
                "moving while poisoned by the mobile toxin (branch B) deals extra damage on top of the turn tick");
        }

        [Test]
        public void LaserFence_DeploysTwoGroundNodesFromRealCombatShots()
        {
            var weapon = CreateWeapon("weapon.laser-fence", WeaponRole.ArcingExplosive,
                damage: 42, impactRadius: 1.2f, ammo: 4, gravityScale: 0f);
            var harness = CreateHarness(weapon);
            var nodes = new List<LaserFenceNodeDeployedEvent>();
            harness.Simulation.LaserFenceNodeDeployed += nodes.Add;

            Assert.That(harness.Simulation.TryFire(
                new AimState(new Vector2(-3f, 0f), Vector2.down, 4f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 180, 1f / 60f);

            Assert.That(harness.Simulation.TryFire(
                new AimState(new Vector2(-3f, 0f), new Vector2(1f, -0.2f), 4f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 240, 1f / 60f);

            Assert.That(nodes, Has.Count.EqualTo(2));
            Assert.That(Vector2.Distance(nodes[0].Point, nodes[1].Point), Is.LessThanOrEqualTo(5.4f));
        }

        [Test]
        public void OrbitalRocket_DefersImpactUntilOpponentTurnEnds()
        {
            var rocket = CreateWeapon("weapon.orbital-rocket", WeaponRole.ArcingExplosive,
                damage: 65, impactRadius: 2.1f, gravityScale: 1.1f);
            var harness = CreateHarness(rocket);
            var targetUnit = harness.State.GetUnit("opponent.test.01");
            var target = targetUnit.Position;
            var armTarget = typeof(BattleSimulation).GetMethod(
                "TrySetOrbitalTarget",
                new[] { typeof(Vector2) });

            Assert.IsNotNull(armTarget,
                "Orbital rocket requires a public target-marking action before its fire confirmation.");
            Assert.IsTrue((bool)armTarget.Invoke(harness.Simulation, new object[] { target }));
            Assert.That(harness.Simulation.TryFire(new AimState(target, Vector2.up, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));
            Assert.That(targetUnit.Health, Is.EqualTo(targetUnit.MaxHealth),
                "confirming the strike must not hit during the player turn");
            Assert.That(harness.State.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Opponent));

            var advanceTurn = typeof(BattleSimulation).GetMethod(
                "AdvanceTurnNow",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.IsNotNull(advanceTurn, "test fixture must be able to complete the otherwise AI-owned opponent turn");
            advanceTurn.Invoke(harness.Simulation, new object[] { TurnAdvanceReason.InstantActionCompleted });

            Assert.That(harness.Simulation.HasProjectilesInFlight, Is.True,
                "the delayed strike must visibly fall after the opponent turn instead of resolving instantly");
            Assert.That(targetUnit.Health, Is.EqualTo(targetUnit.MaxHealth));
            TickMany(harness.Simulation, 360, 1f / 60f);

            Assert.That(targetUnit.Health, Is.LessThan(targetUnit.MaxHealth),
                "the falling rocket must resolve damage only after the opponent turn");
            Assert.That(harness.State.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Player),
                "the deferred impact must leave the newly opened player turn available");
        }

        [Test]
        public void GuardianDrone_DeploysAndGuaranteedShotEndsTriggeredTurn()
        {
            var guardian = CreateWeapon("weapon.guardian-drone", WeaponRole.DefensiveTool,
                damage: 42, impactRadius: 1.6f, gravityScale: 0f);
            var harness = CreateHarness(guardian);
            var opponent = harness.State.GetUnit("opponent.test.01");
            var spawnedShot = false;
            harness.Simulation.ProjectileSpawned += projectile =>
                spawnedShot |= projectile.Definition.ProjectileId == "projectile.weapon.guardian-drone.shot";

            Assert.That(harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            harness.Simulation.EndActiveTurn();
            var triggerPosition = new Vector2(-1.8f, 0f);
            opponent.MoveTo(triggerPosition);
            harness.Motors[opponent.UnitId].Teleport(triggerPosition);
            var healthBefore = opponent.Health;
            TickMany(harness.Simulation, 240, 1f / 60f);

            Assert.That(spawnedShot, Is.True, "guardian must rise and fire its own fast shot");
            Assert.That(opponent.Health, Is.LessThan(healthBefore), "the triggered target cannot evade the guardian shot");
            Assert.That(harness.State.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Player),
                "guardian fire ends the triggered enemy turn");
        }

        [Test]
        public void LaserFence_EnemyCrossingLinkTakesBaseDamageOnceAndLosesTheActiveTurn()
        {
            var weapon = CreateWeapon("weapon.laser-fence", WeaponRole.ArcingExplosive,
                damage: 42, impactRadius: 1.2f, ammo: 4, gravityScale: 0f);
            var harness = CreateHarness(weapon);
            var nodes = new List<LaserFenceNodeDeployedEvent>();
            harness.Simulation.LaserFenceNodeDeployed += nodes.Add;

            Assert.That(harness.Simulation.TryFire(
                new AimState(new Vector2(-3f, 0f), Vector2.down, 4f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 180, 1f / 60f);
            Assert.That(harness.Simulation.TryFire(
                new AimState(new Vector2(-3f, 0f), new Vector2(1f, -0.2f), 4f, true)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 240, 1f / 60f);
            Assert.That(nodes, Has.Count.EqualTo(2));

            var midpoint = (nodes[0].Point + nodes[1].Point) * 0.5f;
            var opponent = harness.State.GetUnit("opponent.test.01");
            opponent.MoveTo(midpoint);
            harness.Motors[opponent.UnitId].Teleport(midpoint);
            harness.Simulation.EndActiveTurn();

            var healthBefore = opponent.Health;
            var turnAdvanced = false;
            harness.Simulation.TurnAdvanced += (team, reason) => turnAdvanced = team == BattleTeamId.Player;
            TickMany(harness.Simulation, 2, 1f / 120f);

            Assert.That(opponent.Health, Is.EqualTo(healthBefore - weapon.Damage));
            Assert.That(turnAdvanced, Is.True);
            Assert.That(harness.State.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Player));

            TickMany(harness.Simulation, 2, 1f / 120f);
            Assert.That(opponent.Health, Is.EqualTo(healthBefore - weapon.Damage),
                "a triggered laser-fence link cannot deal damage a second time");
        }

        [Test]
        public void RepairJammer_NearbyEnemyIsAttachedWithoutDirectDamage()
        {
            var jammer = CreateWeapon("weapon.repair-jammer", WeaponRole.DirectProjectile,
                damage: 42, impactRadius: 1.2f, ammo: 1, gravityScale: 0f);
            var harness = CreateHarness(jammer);
            var opponent = harness.State.GetUnit("opponent.test.01");
            var nearbyPosition = new Vector2(-1.4f, 0f);
            opponent.MoveTo(nearbyPosition);
            harness.Motors[opponent.UnitId].Teleport(nearbyPosition);
            SettleUnits(harness);

            var healthBefore = opponent.Health;
            Assert.That(harness.Simulation.TryFire(new AimState(
                new Vector2(-3f, 0f), Vector2.right, 1f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));

            TickMany(harness.Simulation, 120, 1f / 60f);
            Assert.That(opponent.Health, Is.EqualTo(healthBefore),
                "repair jammer only applies its repair block; it does not explode for direct damage");
        }

        [Test]
        public void RepairJammer_BlocksTheAffectedEnemyHealingForTwoOwnTurns()
        {
            var jammer = CreateWeapon("weapon.repair-jammer", WeaponRole.DirectProjectile,
                damage: 42, impactRadius: 1.2f, ammo: 1, gravityScale: 0f);
            var heal = CreateWeapon("weapon.test.heal", WeaponRole.DefensiveTool,
                damage: 30, impactRadius: 2f, ammo: 2, gravityScale: 0f);
            var harness = CreateHarness(jammer, heal);
            var opponent = harness.State.GetUnit("opponent.test.01");
            var nearbyPosition = new Vector2(-1.4f, 0f);
            opponent.MoveTo(nearbyPosition);
            harness.Motors[opponent.UnitId].Teleport(nearbyPosition);
            opponent.ApplyDamage(40, "test");
            SettleUnits(harness);

            Assert.That(harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 600, 1f / 60f);
            Assert.That(harness.State.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Opponent));

            Assert.That(harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));
            Assert.That(opponent.Health, Is.EqualTo(60));
        }

        [Test]
        public void RepairJammer_BlocksTimedRegenerationUntilItsBaseDurationExpires()
        {
            var repairKit = CreateWeapon("weapon.repair-kit", WeaponRole.DefensiveTool,
                damage: 20, impactRadius: 2f, ammo: 1, gravityScale: 0f);
            var jammer = CreateWeapon("weapon.repair-jammer", WeaponRole.DirectProjectile,
                damage: 42, impactRadius: 1.2f, ammo: 1, gravityScale: 0f);
            var playerEvolutions = new Dictionary<string, WeaponEvolutionProfile>
            {
                [repairKit.WeaponId] = new WeaponEvolutionProfile(1, 0f, 1f,
                    repairRegenerationTurns: 3)
            };
            var harness = CreateHarness(repairKit, jammer, playerEvolutions);
            var player = harness.State.GetUnit("player.test.01");
            var opponent = harness.State.GetUnit("opponent.test.01");
            var nearbyPosition = new Vector2(-1.4f, 0f);
            opponent.MoveTo(nearbyPosition);
            harness.Motors[opponent.UnitId].Teleport(nearbyPosition);
            player.ApplyDamage(40, "test");
            SettleUnits(harness);

            Assert.That(harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.UtilityUsed));
            player.ApplyDamage(20, "test");
            harness.Simulation.EndActiveTurn();
            Assert.That(harness.State.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Opponent));

            Assert.That(harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 600, 1f / 60f);

            Assert.That(player.Health, Is.EqualTo(60),
                "the first regeneration check must be blocked when the jammer attaches");
            harness.Simulation.EndActiveTurn();
            harness.Simulation.EndActiveTurn();
            Assert.That(player.Health, Is.EqualTo(60),
                "the second completed target turn must still block regeneration");

            harness.Simulation.EndActiveTurn();
            harness.Simulation.EndActiveTurn();
            Assert.That(player.Health, Is.EqualTo(80),
                "regeneration resumes after the two target-turn jammer duration expires");
        }

        [Test]
        public void RepairJammer_TwoDurationUpgradesKeepTheTargetJammedForFourOwnTurns()
        {
            var jammer = CreateWeapon("weapon.repair-jammer", WeaponRole.DirectProjectile,
                damage: 42, impactRadius: 1.2f, ammo: 1, gravityScale: 0f);
            var evolutions = new Dictionary<string, WeaponEvolutionProfile>
            {
                [jammer.WeaponId] = new WeaponEvolutionProfile(1, 0f, 1f,
                    repairJammerDurationBonusTurns: 2)
            };
            var harness = CreateHarness(jammer, playerWeaponEvolutions: evolutions);
            var opponent = harness.State.GetUnit("opponent.test.01");
            var nearbyPosition = new Vector2(-1.4f, 0f);
            opponent.MoveTo(nearbyPosition);
            harness.Motors[opponent.UnitId].Teleport(nearbyPosition);
            SettleUnits(harness);

            Assert.That(harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                Is.EqualTo(BattleFireOutcome.Fired));
            TickMany(harness.Simulation, 600, 1f / 60f);

            for (var targetTurn = 0; targetTurn < 4; targetTurn++)
            {
                Assert.That(harness.State.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Opponent));
                Assert.That(harness.Simulation.IsUnitRepairJammed(opponent.UnitId), Is.True);
                harness.Simulation.EndActiveTurn();
                if (targetTurn < 3)
                {
                    harness.Simulation.EndActiveTurn();
                }
            }

            Assert.That(harness.Simulation.IsUnitRepairJammed(opponent.UnitId), Is.False);
        }

        private static List<int> RunDeterministicScript()
        {
            var weapon = CreateWeapon("weapon.test.determinism", gravityScale: 0f);
            var harness = CreateHarness(weapon);
            var hashes = new List<int>();

            for (var tick = 0; tick < 420; tick++)
            {
                if (tick == 0)
                {
                    harness.Simulation.Walk(1, 0f);
                }
                else if (tick == 48)
                {
                    harness.Simulation.Walk(0, 0f);
                }
                else if (tick == 72)
                {
                    harness.Simulation.TryStartJump(1);
                }
                else if (tick == 260)
                {
                    harness.Simulation.TryFire(new AimState(new Vector2(-3f, 0f), Vector2.right, 20f, true));
                }

                harness.Simulation.Tick(1f / 120f);
                hashes.Add(HashSimulation(harness.Simulation));
            }

            return hashes;
        }

        private static void TickMany(BattleSimulation simulation, int count, float deltaTime)
        {
            for (var i = 0; i < count; i++)
            {
                simulation.Tick(deltaTime);
            }
        }

        private static int HashSimulation(BattleSimulation simulation)
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 31 + simulation.Result.Outcome.GetHashCode();
                hash = hash * 31 + simulation.State.Turn.ActiveTeam.GetHashCode();
                hash = hash * 31 + simulation.State.Turn.TurnNumber.GetHashCode();
                hash = hash * 31 + simulation.State.Turn.ActionSpent.GetHashCode();
                hash = hash * 31 + simulation.MovementTimeRemaining.GetHashCode();

                foreach (var team in simulation.State.Teams)
                {
                    hash = hash * 31 + team.TeamId.GetHashCode();
                    foreach (var unit in team.Units)
                    {
                        hash = hash * 31 + unit.UnitId.GetHashCode();
                        hash = hash * 31 + unit.Health.GetHashCode();
                        hash = hash * 31 + unit.Position.x.GetHashCode();
                        hash = hash * 31 + unit.Position.y.GetHashCode();
                        hash = hash * 31 + unit.FacingDirectionX.GetHashCode();
                    }
                }

                foreach (var projectile in simulation.Projectiles)
                {
                    hash = hash * 31 + projectile.Id.GetHashCode();
                    hash = hash * 31 + projectile.Definition.ProjectileId.GetHashCode();
                    hash = hash * 31 + projectile.Position.x.GetHashCode();
                    hash = hash * 31 + projectile.Position.y.GetHashCode();
                    hash = hash * 31 + projectile.Velocity.x.GetHashCode();
                    hash = hash * 31 + projectile.Velocity.y.GetHashCode();
                    hash = hash * 31 + projectile.RemainingLifetime.GetHashCode();
                    hash = hash * 31 + projectile.TerrainDrillTimeRemaining.GetHashCode();
                    hash = hash * 31 + projectile.IsTraversingTerrain.GetHashCode();
                }

                return hash;
            }
        }
    }
}
