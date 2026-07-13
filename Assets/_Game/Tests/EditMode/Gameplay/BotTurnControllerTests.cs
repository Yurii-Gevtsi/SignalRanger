using System.Collections.Generic;
using Game.Gameplay.Battle.AI;
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
    public sealed class BotTurnControllerTests
    {
        private const float TickSeconds = 1f / 120f;
        private const float FallDeathY = -6f;
        private const float UnitCollisionHalfWidth = 0.34f;

        private sealed class Harness
        {
            public BattleState State;
            public BattleSimulation Simulation;
            public BattleTerrain Terrain;
            public CharacterArchetype PlayerArchetype;
            public CharacterArchetype OpponentArchetype;
        }

        [Test]
        public void BotTurnController_FiresOnItsTurn_AndHandsTurnBackToPlayer()
        {
            var harness = CreateHarness();
            AdvanceToOpponentTurn(harness);

            var controller = CreateController(harness, AiDifficulty.Normal, seed: 42);

            var opponentSpawned = false;
            harness.Simulation.ProjectileSpawned += projectile =>
            {
                if (projectile.OwnerUnitId.StartsWith("opponent"))
                {
                    opponentSpawned = true;
                }
            };

            for (var i = 0; i < 6000 && harness.State.Turn.ActiveTeam == BattleTeamId.Opponent; i++)
            {
                controller.Tick(TickSeconds);
                harness.Simulation.Tick(TickSeconds);
            }

            Assert.IsTrue(opponentSpawned, "the bot fires a projectile on its turn");
            Assert.AreEqual(BattleTeamId.Player, harness.State.Turn.ActiveTeam, "the resolved shot hands the turn back to the player");
        }

        [Test]
        public void BotTurnController_FiresOnTwoConsecutiveTurns()
        {
            var harness = CreateHarness();
            var controller = CreateController(harness, AiDifficulty.Normal, seed: 7);

            for (var round = 0; round < 2; round++)
            {
                AdvanceToOpponentTurn(harness);
                Assert.IsFalse(harness.Simulation.Result.IsFinished, "battle should still be running");

                var firedThisTurn = false;

                void OnSpawn(Game.Gameplay.Battle.Simulation.SimProjectile projectile)
                {
                    if (projectile.OwnerUnitId.StartsWith("opponent"))
                    {
                        firedThisTurn = true;
                    }
                }

                harness.Simulation.ProjectileSpawned += OnSpawn;
                for (var i = 0; i < 6000 && harness.State.Turn.ActiveTeam == BattleTeamId.Opponent; i++)
                {
                    controller.Tick(TickSeconds);
                    harness.Simulation.Tick(TickSeconds);
                }

                harness.Simulation.ProjectileSpawned -= OnSpawn;
                Assert.IsTrue(firedThisTurn, $"the bot fires on opponent turn {round + 1}");
                Assert.AreEqual(BattleTeamId.Player, harness.State.Turn.ActiveTeam);
            }
        }

        [Test]
        public void BotTurnController_IsIdle_DuringPlayerTurn()
        {
            var harness = CreateHarness();
            var controller = CreateController(harness, AiDifficulty.Normal, seed: 1);

            controller.Tick(TickSeconds);

            Assert.AreEqual(BotActivity.Idle, controller.Activity, "the bot does nothing while it is the player's turn");
            Assert.AreEqual(BattleTeamId.Player, harness.State.Turn.ActiveTeam);
        }

        private static BotTurnController CreateController(Harness harness, AiDifficulty difficulty, int seed)
        {
            var probe = new TrajectoryProbe(
                harness.Terrain.Grid, Physics2D.gravity, TickSeconds, BattleSimulation.ProjectileSubStepLength, FallDeathY);
            var profile = BotProfileLibrary.For(difficulty);
            var random = new SystemBotRandom(seed);
            IBotShotPlanner planner = difficulty == AiDifficulty.Easy
                ? new HeuristicShotPlanner(difficulty)
                : new BallisticShotPlanner(probe, profile, random, Physics2D.gravity);
            return new BotTurnController(
                harness.Simulation, planner, profile, random,
                UnitCollisionHalfWidth, harness.PlayerArchetype, harness.OpponentArchetype, AimSettings.Default);
        }

        private static void AdvanceToOpponentTurn(Harness harness)
        {
            // Settle any knockback from a prior turn before the player fires.
            for (var i = 0; i < 3000 && harness.Simulation.HasActiveMotion; i++)
            {
                harness.Simulation.Tick(TickSeconds);
            }

            // Fire the player's shot up and away so it resolves without touching the opponent.
            var fired = harness.Simulation.TryFire(new AimState(new Vector2(-3f, 0f), new Vector2(-0.3f, 1f), 8f, true));
            Assert.AreEqual(BattleFireOutcome.Fired, fired.Outcome);
            for (var i = 0; i < 3000 && harness.State.Turn.ActiveTeam != BattleTeamId.Opponent; i++)
            {
                harness.Simulation.Tick(TickSeconds);
            }

            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam);
        }

        private static Harness CreateHarness()
        {
            var bounds = new Rect(-10f, -8f, 20f, 16f);
            var terrain = BattleTerrain.CreateProcedural(bounds);
            terrain.StampSolidRect(new Vector2(0f, -3f), new Vector2(20f, 4f));

            var playerArchetype = new CharacterArchetype("archetype.player", "player", 100, 1f, 3f, 3f, 0.4f, 1.6f, 0.9f, 1.1f, 0.7f);
            var opponentArchetype = new CharacterArchetype("archetype.opponent", "opponent", 100, 1f, 3f, 3f, 0.4f, 1.6f, 0.9f, 1.1f, 0.7f);

            var playerUnit = new BattleUnitState("player.test.01", BattleTeamId.Player, new Vector2(-3f, 0f), playerArchetype.MaxHealth, 1);
            var opponentUnit = new BattleUnitState("opponent.test.01", BattleTeamId.Opponent, new Vector2(3f, 0f), opponentArchetype.MaxHealth, -1);
            var playerTeam = new BattleTeamState(BattleTeamId.Player, new[] { playerUnit });
            var opponentTeam = new BattleTeamState(BattleTeamId.Opponent, new[] { opponentUnit });
            var state = new BattleState(new[] { playerTeam, opponentTeam }, new TurnState(BattleTeamId.None, string.Empty, 0, false));

            var profile = new UnitMotionProfile(0.34f, 0.5f, 0.3f, 0.42f, 1.6f);
            var motors = new Dictionary<string, UnitMotor>(System.StringComparer.Ordinal)
            {
                [playerUnit.UnitId] = new UnitMotor(terrain.Grid, profile, playerUnit.Position, FallDeathY),
                [opponentUnit.UnitId] = new UnitMotor(terrain.Grid, profile, opponentUnit.Position, FallDeathY)
            };

            // Low damage so the bot cannot finish the battle across the multi-turn test.
            var weapon = WeaponDefinition.CreateRuntime(
                "weapon.test.botturn", "weapon.test.botturn.name", WeaponRole.ArcingExplosive,
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

            return new Harness
            {
                State = state,
                Simulation = simulation,
                Terrain = terrain,
                PlayerArchetype = playerArchetype,
                OpponentArchetype = opponentArchetype
            };
        }
    }
}
