using System.Collections.Generic;
using Game.Gameplay.Battle.AI.Ballistics;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Motion;
using Game.Gameplay.Battle.Simulation;
using Game.Gameplay.Battle.Terrain;
using Game.Gameplay.Battle.Units;
using Game.Gameplay.Battle.Weapons;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class BotBallisticsTests
    {
        // Mirrors the private constants the simulation integrates projectiles with,
        // so a probe built here resolves shots the same way the real step does.
        private const float TickSeconds = 1f / 120f;
        private const float SubStepLength = 0.04f;
        private const float FallDeathY = -6f;
        private const float MuzzleOffset = 0.45f;
        private const float UnitCollisionHalfWidth = 0.34f;

        // --- Solver: pure math -------------------------------------------------

        [Test]
        public void Solver_LevelTarget_ProducesTwoArcsThatBothLandOnTarget()
        {
            var origin = new Vector2(-3f, 0f);
            var target = new Vector2(3f, 0f);
            const float speed = 12f;
            const float gravity = 9.81f;

            var count = BallisticsSolver.SolveLaunchVelocities(origin, target, speed, gravity, out var low, out var high);

            Assert.AreEqual(2, count, "a level shot above minimum speed has a flat and a lofted arc");
            Assert.AreEqual(speed, low.magnitude, 0.01f, "low arc launches at the requested speed");
            Assert.AreEqual(speed, high.magnitude, 0.01f, "high arc launches at the requested speed");
            AssertArcLandsOnTarget(origin, target, low, gravity);
            AssertArcLandsOnTarget(origin, target, high, gravity);

            var lowAngle = Mathf.Atan2(low.y, low.x) * Mathf.Rad2Deg;
            var highAngle = Mathf.Atan2(high.y, high.x) * Mathf.Rad2Deg;
            Assert.Less(lowAngle, highAngle, "the low arc is flatter than the high arc");
            Assert.Less(lowAngle, 45f);
            Assert.Greater(highAngle, 45f);
        }

        [Test]
        public void Solver_UnreachableTarget_ReturnsNoSolution()
        {
            var count = BallisticsSolver.SolveLaunchVelocities(
                new Vector2(0f, 0f), new Vector2(40f, 0f), 5f, 9.81f, out _, out _);

            Assert.AreEqual(0, count, "a slow shot cannot span a far target");
        }

        [Test]
        public void Solver_NoGravity_ReturnsStraightLineShot()
        {
            var origin = new Vector2(0f, 0f);
            var target = new Vector2(3f, 4f);
            const float speed = 10f;

            var count = BallisticsSolver.SolveLaunchVelocities(origin, target, speed, 0f, out var low, out var high);

            Assert.AreEqual(1, count);
            Assert.AreEqual(low, high, "a gravity-free shot has a single straight-line solution");
            var expected = (target - origin).normalized * speed;
            Assert.AreEqual(expected.x, low.x, 0.001f);
            Assert.AreEqual(expected.y, low.y, 0.001f);
        }

        private static void AssertArcLandsOnTarget(Vector2 origin, Vector2 target, Vector2 velocity, float gravity)
        {
            var delta = target - origin;
            var t = delta.x / velocity.x;
            Assert.Greater(t, 0f, "the arc reaches the target moving forward in time");
            var y = velocity.y * t - 0.5f * gravity * t * t;
            Assert.AreEqual(delta.y, y, 0.01f, "the arc passes through the target height at the target range");
        }

        // --- Probe: mirrors the real projectile step --------------------------

        [Test]
        public void Probe_TerrainShot_LandsWhereTheRealShotLands()
        {
            var harness = CreateHarness(gravityScale: 1f);
            var origin = new Vector2(-3f, 0f);
            var direction = new Vector2(-0.2f, 1f).normalized; // up and away from the opponent
            const float power = 12f;

            // Predict on the intact terrain BEFORE firing: the real shot carves a
            // crater on impact, so probing afterwards would see a hole.
            var probe = CreateProbe(harness);
            var report = probe.Simulate(
                origin + direction * MuzzleOffset,
                direction * power,
                gravityScale: 1f,
                projectileRadius: harness.Weapon.ProjectileRadius,
                timeoutSeconds: harness.Weapon.TimeoutSeconds,
                TrajectoryObstacles.TerrainOnly(harness.OpponentPosition));

            var impactPoint = Vector2.zero;
            var exploded = false;
            harness.Simulation.ProjectileExploded += (id, impact) =>
            {
                impactPoint = impact.Point;
                exploded = true;
            };

            var fireResult = harness.Simulation.TryFire(new AimState(origin, direction, power, true));
            Assert.AreEqual(BattleFireOutcome.Fired, fireResult.Outcome);
            for (var i = 0; i < 2400 && !exploded; i++)
            {
                harness.Simulation.Tick(TickSeconds);
            }

            Assert.IsTrue(exploded, "the real shot resolves");
            Assert.AreEqual(TrajectoryEndKind.Terrain, report.EndKind, "the arc ends on the ground");
            Assert.AreEqual(impactPoint.x, report.EndPoint.x, 0.15f, "predicted impact X matches the simulation");
            Assert.AreEqual(impactPoint.y, report.EndPoint.y, 0.15f, "predicted impact Y matches the simulation");
        }

        [Test]
        public void Probe_UnitShot_HitsTargetWhereTheRealShotLands()
        {
            var harness = CreateHarness(gravityScale: 1f);
            var origin = new Vector2(-3f, 0f);
            var direction = new Vector2(1f, 0.1f).normalized; // flat shot straight at the opponent
            const float power = 14f;

            // Predict before firing so the crater/knockback from the real impact
            // cannot perturb the probed terrain or target position.
            var probe = CreateProbe(harness);
            var bodies = new List<TrajectoryUnitBody>
            {
                new TrajectoryUnitBody(harness.OpponentPosition, UnitCollisionHalfWidth, harness.OpponentHalfHeight, isAlly: false, isTarget: true)
            };
            var report = probe.Simulate(
                origin + direction * MuzzleOffset,
                direction * power,
                gravityScale: 1f,
                projectileRadius: harness.Weapon.ProjectileRadius,
                timeoutSeconds: harness.Weapon.TimeoutSeconds,
                new TrajectoryObstacles(bodies, harness.OpponentPosition));

            var impactPoint = Vector2.zero;
            var exploded = false;
            harness.Simulation.ProjectileExploded += (id, impact) =>
            {
                impactPoint = impact.Point;
                exploded = true;
            };

            var fireResult = harness.Simulation.TryFire(new AimState(origin, direction, power, true));
            Assert.AreEqual(BattleFireOutcome.Fired, fireResult.Outcome);
            for (var i = 0; i < 2400 && !exploded; i++)
            {
                harness.Simulation.Tick(TickSeconds);
            }

            Assert.IsTrue(exploded, "the real shot resolves");
            Assert.AreEqual(TrajectoryEndKind.Unit, report.EndKind, "the flat shot strikes the opponent");
            Assert.IsTrue(report.EndedOnTargetUnit);
            Assert.AreEqual(impactPoint.x, report.EndPoint.x, 0.2f, "predicted unit impact X matches the simulation");
            Assert.AreEqual(impactPoint.y, report.EndPoint.y, 0.2f, "predicted unit impact Y matches the simulation");
        }

        [Test]
        public void SolverAndProbe_Together_ProduceAShotThatHitsTheTarget()
        {
            var harness = CreateHarness(gravityScale: 1f);
            var origin = new Vector2(-3f, 0f);
            var target = harness.OpponentPosition;
            const float speed = 12f;
            const float gravity = 9.81f;

            // Account for the muzzle offset by re-solving from the muzzle point once.
            var count = BallisticsSolver.SolveLaunchVelocities(origin, target, speed, gravity, out var low, out _);
            Assert.GreaterOrEqual(count, 1);
            var muzzle = origin + low.normalized * MuzzleOffset;
            BallisticsSolver.SolveLaunchVelocities(muzzle, target, speed, gravity, out var refined, out _);

            var probe = CreateProbe(harness);
            var bodies = new List<TrajectoryUnitBody>
            {
                new TrajectoryUnitBody(target, UnitCollisionHalfWidth, harness.OpponentHalfHeight, isAlly: false, isTarget: true)
            };
            var report = probe.Simulate(
                muzzle, refined, gravityScale: 1f,
                projectileRadius: harness.Weapon.ProjectileRadius,
                timeoutSeconds: harness.Weapon.TimeoutSeconds,
                new TrajectoryObstacles(bodies, target));

            Assert.IsTrue(report.EndedOnTargetUnit, "a solved-and-refined arc lands on the target unit");
            Assert.Greater(report.ClearPathRatio, 0.8f, "the winning arc travels almost the whole way to the target");
        }

        // --- Harness ----------------------------------------------------------

        private sealed class ProbeHarness
        {
            public BattleSimulation Simulation;
            public BattleTerrain Terrain;
            public WeaponDefinition Weapon;
            public Vector2 OpponentPosition;
            public float OpponentHalfHeight;
        }

        private static TrajectoryProbe CreateProbe(ProbeHarness harness)
        {
            return new TrajectoryProbe(harness.Terrain.Grid, Physics2D.gravity, TickSeconds, SubStepLength, FallDeathY);
        }

        private static ProbeHarness CreateHarness(float gravityScale)
        {
            var bounds = new Rect(-10f, -8f, 20f, 16f);
            var terrain = BattleTerrain.CreateProcedural(bounds);
            terrain.StampSolidRect(new Vector2(0f, -3f), new Vector2(20f, 4f));

            var playerArchetype = new CharacterArchetype("archetype.player", "player", 100, 1f, 3f, 3f, 0.4f, 1.6f, 0.9f, 1.1f, 0.7f);
            var opponentArchetype = new CharacterArchetype("archetype.opponent", "opponent", 100, 1f, 3f, 3f, 0.4f, 1.6f, 0.9f, 1.1f, 0.7f);

            var playerPosition = new Vector2(-3f, 0f);
            var opponentPosition = new Vector2(3f, 0f);
            var playerUnit = new BattleUnitState("player.test.01", BattleTeamId.Player, playerPosition, playerArchetype.MaxHealth, 1);
            var opponentUnit = new BattleUnitState("opponent.test.01", BattleTeamId.Opponent, opponentPosition, opponentArchetype.MaxHealth, -1);
            var playerTeam = new BattleTeamState(BattleTeamId.Player, new[] { playerUnit });
            var opponentTeam = new BattleTeamState(BattleTeamId.Opponent, new[] { opponentUnit });
            var state = new BattleState(new[] { playerTeam, opponentTeam }, new TurnState(BattleTeamId.None, string.Empty, 0, false));

            var profile = new UnitMotionProfile(0.34f, 0.5f, 0.3f, 0.42f, 1.6f);
            var motors = new Dictionary<string, UnitMotor>(System.StringComparer.Ordinal)
            {
                [playerUnit.UnitId] = new UnitMotor(terrain.Grid, profile, playerUnit.Position, FallDeathY),
                [opponentUnit.UnitId] = new UnitMotor(terrain.Grid, profile, opponentUnit.Position, FallDeathY)
            };

            var weapon = WeaponDefinition.CreateRuntime(
                "weapon.test.ballistics", "weapon.test.ballistics.name", WeaponRole.ArcingExplosive,
                60, 2.5f, 0.14f, gravityScale, 8f, 6f, 20f, -1, null, null);
            var playerInventory = new WeaponInventoryRuntime(new[] { weapon });
            var opponentInventory = new WeaponInventoryRuntime(new[] { weapon });
            var executor = new WeaponExecutor();
            var weaponsById = new Dictionary<string, WeaponDefinition>(System.StringComparer.Ordinal)
            {
                [weapon.WeaponId] = weapon
            };

            var turnService = new TurnService();
            turnService.StartFirstTurn(state, BattleTeamId.Player);

            var settings = new BattleSimulationSettings(
                fallDeathY: FallDeathY,
                simulationTickSeconds: TickSeconds,
                playerTurnDurationSeconds: 30f,
                occupiedRadius: 0.72f,
                unitCollisionHalfWidth: UnitCollisionHalfWidth,
                knockbackLandingSearchPadding: 0.46f,
                groundRayStartHeight: 3.5f,
                groundRayDistance: 8f);

            var simulation = new BattleSimulation(
                state, turnService, terrain, motors,
                playerInventory, opponentInventory, executor,
                Physics2D.gravity, settings, weaponsById,
                playerArchetype, opponentArchetype);

            return new ProbeHarness
            {
                Simulation = simulation,
                Terrain = terrain,
                Weapon = weapon,
                OpponentPosition = opponentPosition,
                OpponentHalfHeight = opponentArchetype.BodyHeight * 0.5f
            };
        }
    }
}
