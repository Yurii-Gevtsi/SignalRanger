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
    public sealed class BotHardTests
    {
        private const float TickSeconds = 1f / 120f;
        private const float FallDeathY = -6f;
        private const float UnitCollisionHalfWidth = 0.34f;
        private const float GroundedY = 0.5f; // body centre resting on a surface at y = 0

        private static UnitMotionProfile MotionProfile() => new(0.34f, 0.5f, 0.3f, 0.42f, 1.6f);

        private static CharacterArchetype Archetype(string id) =>
            new(id, id, 100, 1f, 3f, 3f, 0.4f, 1.6f, 0.9f, 1.1f, 0.7f);

        // Prefers standing closer to the target — lets a test force a jump across
        // a pit that walking cannot cross.
        private sealed class TargetSeekingPlanner : IBotShotPlanner
        {
            public BotShotPlan? PlanShot(in BotShotContext context)
            {
                var toTarget = context.AimTarget - context.Origin;
                var direction = toTarget.sqrMagnitude > 0.01f ? toTarget.normalized : Vector2.up;
                var aim = new AimState(context.Origin, direction, context.AimSettings.MaxPower * 0.6f, true);
                var weaponId = context.Weapon != null ? context.Weapon.WeaponId : string.Empty;
                return new BotShotPlan(aim, weaponId, -toTarget.magnitude);
            }
        }

        // --- Jump generator ----------------------------------------------------

        [Test]
        public void JumpGenerator_LandsHigher_OnAReachableStep()
        {
            var terrain = FlatTerrain();
            terrain.StampSolidRect(new Vector2(1.6f, 0.25f), new Vector2(2.2f, 0.5f)); // 0.5-high step to the right
            var generator = new JumpCandidateGenerator(terrain.Grid, MotionProfile(), Archetype("a"), FallDeathY);

            var landed = generator.TryJumpLanding(new Vector2(0f, GroundedY), direction: 1, out var landing);

            Assert.IsTrue(landed, "a short step is reachable by jumping");
            Assert.Greater(landing.y, GroundedY + 0.2f, "the bot ends up standing on the higher step");
        }

        [Test]
        public void JumpGenerator_RefusesToJumpIntoTheVoid()
        {
            // Ground only over x in [-10, 0]; a void lies to the right.
            var terrain = BattleTerrain.CreateProcedural(new Rect(-10f, -8f, 20f, 16f));
            terrain.StampSolidRect(new Vector2(-5f, -2f), new Vector2(10f, 4f)); // surface y = 0, cliff at x = 0
            var generator = new JumpCandidateGenerator(terrain.Grid, MotionProfile(), Archetype("a"), FallDeathY);

            var landed = generator.TryJumpLanding(new Vector2(-0.5f, GroundedY), direction: 1, out _);

            Assert.IsFalse(landed, "a jump that would drop into the kill zone is not a landing");
        }

        // --- Position planner --------------------------------------------------

        [Test]
        public void PositionPlanner_ChoosesToJump_AcrossAPitTowardTheTarget()
        {
            var terrain = PitPlatformTerrain();
            var scanner = new WalkReachabilityScanner(terrain.Grid, MotionProfile(), FallDeathY);
            var generator = new JumpCandidateGenerator(terrain.Grid, MotionProfile(), Archetype("a"), FallDeathY);
            var profile = HardProfile();
            var planner = new BotPositionPlanner(scanner, new TargetSeekingPlanner(), profile, MotionProfile().WalkSpeed, generator);

            var start = new Vector2(-0.3f, GroundedY);
            var target = new Vector2(5f, GroundedY);
            var bodies = new List<TrajectoryUnitBody>
            {
                new TrajectoryUnitBody(target, UnitCollisionHalfWidth, 0.5f, isAlly: false, isTarget: true)
            };

            var decision = planner.SelectPosition(start, target, target, ArcWeapon(), AimSettings.Default, bodies);

            Assert.AreEqual(BotMoveKind.Jump, decision.Kind, "walking cannot cross the pit, so the bot jumps to close on the target");
            Assert.Greater(decision.Position.x, 0.5f, "the jump lands on the far side of the pit (walking stalls at the near edge)");
        }

        // --- Controller end-to-end --------------------------------------------

        [Test]
        public void BotTurnController_JumpsAcrossThePit_ThenFires()
        {
            var harness = CreateHarness();
            AdvanceToOpponentTurn(harness);
            var opponent = harness.State.GetUnit("opponent.test.01");

            var controller = CreateHardController(harness, new TargetSeekingPlanner());

            var fired = false;
            harness.Simulation.ProjectileSpawned += p =>
            {
                if (p.OwnerUnitId.StartsWith("opponent")) fired = true;
            };

            var crossedPit = false;
            for (var i = 0; i < 8000 && harness.State.Turn.ActiveTeam == BattleTeamId.Opponent; i++)
            {
                controller.Tick(TickSeconds);
                harness.Simulation.Tick(TickSeconds);
                if (opponent.Position.x > 0.5f)
                {
                    // Only a jump can put the unit past the pit walking cannot cross.
                    crossedPit = true;
                }
            }

            Assert.IsTrue(crossedPit, "the hard bot jumps across the pit walking cannot cross");
            Assert.IsTrue(fired, "the bot fires after repositioning");
            Assert.AreEqual(BattleTeamId.Player, harness.State.Turn.ActiveTeam);
        }

        private static BotTurnController CreateHardController(Harness harness, IBotShotPlanner planner)
        {
            var profile = HardProfile();
            var random = new SystemBotRandom(3);
            var scanner = new WalkReachabilityScanner(harness.Terrain.Grid, harness.MotionProfile, FallDeathY);
            var generator = new JumpCandidateGenerator(harness.Terrain.Grid, harness.MotionProfile, harness.OpponentArchetype, FallDeathY);
            var positionPlanner = new BotPositionPlanner(scanner, planner, profile, harness.MotionProfile.WalkSpeed, generator);
            return new BotTurnController(
                harness.Simulation, planner, positionPlanner, profile, random,
                UnitCollisionHalfWidth, harness.PlayerArchetype, harness.OpponentArchetype, AimSettings.Default);
        }

        private static BotProfile HardProfile()
        {
            return BotProfile.CreateRuntime(
                powerSteps: 10, angleStepDegrees: 2f, minClearPathRatio: 0.9f,
                scatterBase: 0f, scatterPerDistance: 0f, perfectShotChance: 1f,
                scoreJitter: 0f, allySafetyWeight: 1f, evaluateAllWeapons: true,
                movementEnabled: true, jumpEnabled: true, minEnemySeparation: 0f,
                maxWalkBudgetSeconds: 2f, maxPositionCandidates: 16, retreatAfterShot: false,
                thinkPauseSeconds: 0f, aimPauseSeconds: 0f);
        }

        private static BattleTerrain FlatTerrain()
        {
            var terrain = BattleTerrain.CreateProcedural(new Rect(-10f, -8f, 20f, 16f));
            terrain.StampSolidRect(new Vector2(0f, -2f), new Vector2(20f, 4f)); // surface y = 0
            return terrain;
        }

        /// <summary>
        /// Left ground (x in [-10, 0]) and a far platform (x in [1.2, 8]) at the
        /// same height, split by a ~1.2-wide void pit. The pit is too wide to walk
        /// across (the unit falls in) but within a single jump's reach — the
        /// measured window where jumping strictly beats walking here.
        /// </summary>
        private static BattleTerrain PitPlatformTerrain()
        {
            var terrain = BattleTerrain.CreateProcedural(new Rect(-10f, -8f, 20f, 16f));
            terrain.StampSolidRect(new Vector2(-5f, -2f), new Vector2(10f, 4f));  // left ground x[-10,0], surface 0
            terrain.StampSolidRect(new Vector2(4.6f, -2f), new Vector2(6.8f, 4f)); // far platform x[1.2,8], surface 0
            return terrain;
        }

        private static WeaponDefinition ArcWeapon()
        {
            return WeaponDefinition.CreateRuntime(
                "weapon.test.hard", "weapon.test.hard.name", WeaponRole.ArcingExplosive,
                40, 2.5f, 0.14f, 1f, 8f, 6f, 20f, -1, null, null);
        }

        // --- Harness -----------------------------------------------------------

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

            // Player fires straight up from its platform so the shot resolves on
            // itself without disturbing the opponent, handing over the turn.
            var fired = harness.Simulation.TryFire(new AimState(new Vector2(5f, GroundedY), new Vector2(0f, 1f), 6f, true));
            Assert.AreEqual(BattleFireOutcome.Fired, fired.Outcome);
            for (var i = 0; i < 3000 && harness.State.Turn.ActiveTeam != BattleTeamId.Opponent; i++)
            {
                harness.Simulation.Tick(TickSeconds);
            }

            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam);
        }

        private static Harness CreateHarness()
        {
            // Opponent on the low ground must jump the pit to reach the player's
            // higher platform; walking that way falls into the void.
            var terrain = PitPlatformTerrain();

            var playerArchetype = Archetype("archetype.player");
            var opponentArchetype = Archetype("archetype.opponent");

            var playerUnit = new BattleUnitState("player.test.01", BattleTeamId.Player, new Vector2(5f, GroundedY), 100, 1);
            var opponentUnit = new BattleUnitState("opponent.test.01", BattleTeamId.Opponent, new Vector2(-0.3f, GroundedY), 100, -1);
            var playerTeam = new BattleTeamState(BattleTeamId.Player, new[] { playerUnit });
            var opponentTeam = new BattleTeamState(BattleTeamId.Opponent, new[] { opponentUnit });
            var state = new BattleState(new[] { playerTeam, opponentTeam }, new TurnState(BattleTeamId.None, string.Empty, 0, false));

            var motionProfile = MotionProfile();
            var motors = new Dictionary<string, UnitMotor>(System.StringComparer.Ordinal)
            {
                [playerUnit.UnitId] = new UnitMotor(terrain.Grid, motionProfile, playerUnit.Position, FallDeathY),
                [opponentUnit.UnitId] = new UnitMotor(terrain.Grid, motionProfile, opponentUnit.Position, FallDeathY)
            };

            var weapon = WeaponDefinition.CreateRuntime(
                "weapon.test.hardturn", "weapon.test.hardturn.name", WeaponRole.ArcingExplosive,
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

            // Settle the units onto the terrain before the scenario starts.
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
