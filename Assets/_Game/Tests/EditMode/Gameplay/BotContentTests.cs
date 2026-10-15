using System.Collections.Generic;
using Game.Gameplay.Battle.AI;
using Game.Gameplay.Battle.AI.Ballistics;
using Game.Gameplay.Battle.AI.Movement;
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
    public sealed class BotContentTests
    {
        private const float TickSeconds = 1f / 120f;
        private const float FallDeathY = -6f;
        private const float UnitCollisionHalfWidth = 0.34f;

        [Test]
        public void BotProfileProvider_FallsBackToLibrary_WhenNoAsset()
        {
            // No authored asset under Resources/Bot in the test project, so the
            // provider must hand back the runtime defaults for each tier.
            var easy = BotProfileProvider.Resolve(AiDifficulty.Easy);
            var normal = BotProfileProvider.Resolve(AiDifficulty.Normal);
            var hard = BotProfileProvider.Resolve(AiDifficulty.Hard);

            Assert.IsNotNull(easy);
            Assert.IsNotNull(normal);
            Assert.IsNotNull(hard);
            Assert.IsFalse(easy.MovementEnabled, "easy stands still");
            Assert.IsTrue(normal.MovementEnabled, "normal walks");
            Assert.IsFalse(normal.JumpEnabled, "normal does not jump");
            Assert.IsTrue(hard.JumpEnabled, "hard jumps");
        }

        [Test]
        public void BotTurnController_RecordsTelemetry_AfterATurn()
        {
            var harness = CreateHarness();
            AdvanceToOpponentTurn(harness);
            var controller = CreateController(harness, AiDifficulty.Normal, seed: 11);

            for (var i = 0; i < 6000 && harness.State.Turn.ActiveTeam == BattleTeamId.Opponent; i++)
            {
                controller.Tick(TickSeconds);
                harness.Simulation.Tick(TickSeconds);
            }

            Assert.AreEqual(BattleTeamId.Player, harness.State.Turn.ActiveTeam, "the turn completed");
            Assert.Greater(controller.LastPositionsConsidered, 0, "the bot scored at least one candidate position");
            Assert.GreaterOrEqual(controller.LastPlanningMilliseconds, 0.0);
            Assert.That(controller.LastMoveKind, Is.EqualTo(BotMoveKind.Stay)
                .Or.EqualTo(BotMoveKind.Walk).Or.EqualTo(BotMoveKind.Jump));
        }

        private static BotTurnController CreateController(Harness harness, AiDifficulty difficulty, int seed)
        {
            var probe = new TrajectoryProbe(
                harness.Terrain.Grid, Physics2D.gravity, TickSeconds, BattleSimulation.ProjectileSubStepLength, FallDeathY);
            var profile = BotProfileProvider.Resolve(difficulty);
            var random = new SystemBotRandom(seed);
            var planner = new BallisticShotPlanner(probe, profile, random, Physics2D.gravity);
            var scanner = new WalkReachabilityScanner(harness.Terrain.Grid, harness.MotionProfile, FallDeathY);
            var generator = new JumpCandidateGenerator(harness.Terrain.Grid, harness.MotionProfile, harness.OpponentArchetype, FallDeathY);
            var positionPlanner = new BotPositionPlanner(scanner, planner, profile, harness.MotionProfile.WalkSpeed, generator);
            return new BotTurnController(
                harness.Simulation, planner, positionPlanner, profile, random,
                UnitCollisionHalfWidth, harness.PlayerArchetype, harness.OpponentArchetype, AimSettings.Default);
        }

        private sealed class Harness
        {
            public BattleState State;
            public BattleSimulation Simulation;
            public BattleTerrain Terrain;
            public CharacterArchetype PlayerArchetype;
            public CharacterArchetype OpponentArchetype;
            public UnitMotionProfile MotionProfile;
        }

        private static void AdvanceToOpponentTurn(Harness harness)
        {
            for (var i = 0; i < 3000 && harness.Simulation.HasActiveMotion; i++)
            {
                harness.Simulation.Tick(TickSeconds);
            }

            var fired = harness.Simulation.TryFire(new AimState(new Vector2(-3f, 0.5f), new Vector2(-0.3f, 1f), 8f, true));
            Assert.AreEqual(BattleFireOutcome.Fired, fired.Outcome);
            for (var i = 0; i < 3000 && harness.State.Turn.ActiveTeam != BattleTeamId.Opponent; i++)
            {
                harness.Simulation.Tick(TickSeconds);
            }

            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam);
        }

        private static Harness CreateHarness()
        {
            var terrain = BattleTerrain.CreateProcedural(new Rect(-10f, -8f, 20f, 16f));
            terrain.StampSolidRect(new Vector2(0f, -2f), new Vector2(20f, 4f)); // surface y = 0

            var playerArchetype = new CharacterArchetype("archetype.player", "player", 100, 1f, 3f, 3f, 0.4f, 1.6f, 0.9f, 1.1f, 0.7f);
            var opponentArchetype = new CharacterArchetype("archetype.opponent", "opponent", 100, 1f, 3f, 3f, 0.4f, 1.6f, 0.9f, 1.1f, 0.7f);

            var playerUnit = new BattleUnitState("player.test.01", BattleTeamId.Player, new Vector2(-3f, 0.5f), 100, 1);
            var opponentUnit = new BattleUnitState("opponent.test.01", BattleTeamId.Opponent, new Vector2(3f, 0.5f), 100, -1);
            var playerTeam = new BattleTeamState(BattleTeamId.Player, new[] { playerUnit });
            var opponentTeam = new BattleTeamState(BattleTeamId.Opponent, new[] { opponentUnit });
            var state = new BattleState(new[] { playerTeam, opponentTeam }, new TurnState(BattleTeamId.None, string.Empty, 0, false));

            var motionProfile = new UnitMotionProfile(0.34f, 0.5f, 0.3f, 0.42f, 1.6f);
            var motors = new Dictionary<string, UnitMotor>(System.StringComparer.Ordinal)
            {
                [playerUnit.UnitId] = new UnitMotor(terrain.Grid, motionProfile, playerUnit.Position, FallDeathY),
                [opponentUnit.UnitId] = new UnitMotor(terrain.Grid, motionProfile, opponentUnit.Position, FallDeathY)
            };

            var weapon = WeaponDefinition.CreateRuntime(
                "weapon.test.content", "weapon.test.content.name", WeaponRole.ArcingExplosive,
                10, 2.5f, 0.14f, 1f, 8f, 6f, 20f, -1, null, null);
            var playerInventory = new WeaponInventoryRuntime(new[] { weapon });
            var opponentInventory = new WeaponInventoryRuntime(new[] { weapon });
            var executor = new WeaponExecutor();
            var weaponsById = new Dictionary<string, WeaponDefinition>(System.StringComparer.Ordinal) { [weapon.WeaponId] = weapon };

            var turnService = new TurnService();
            turnService.StartFirstTurn(state, BattleTeamId.Player);

            var settings = new BattleSimulationSettings(
                fallDeathY: FallDeathY, simulationTickSeconds: TickSeconds, playerTurnDurationSeconds: 30f,
                occupiedRadius: 0.72f, unitCollisionHalfWidth: UnitCollisionHalfWidth,
                knockbackLandingSearchPadding: 0.46f, groundRayStartHeight: 3.5f, groundRayDistance: 8f);

            var simulation = new BattleSimulation(
                state, turnService, terrain, motors, playerInventory, opponentInventory, executor,
                Physics2D.gravity, settings, weaponsById, playerArchetype, opponentArchetype);

            foreach (var motor in motors.Values)
            {
                motor.WakeIfUnsupported();
            }

            for (var i = 0; i < 2000 && simulation.HasActiveMotion; i++)
            {
                simulation.Tick(TickSeconds);
            }

            return new Harness
            {
                State = state,
                Simulation = simulation,
                Terrain = terrain,
                PlayerArchetype = playerArchetype,
                OpponentArchetype = opponentArchetype,
                MotionProfile = motionProfile
            };
        }
    }
}
