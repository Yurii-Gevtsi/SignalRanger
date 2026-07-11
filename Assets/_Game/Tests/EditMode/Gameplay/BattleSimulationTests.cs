using System.Collections.Generic;
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
        private sealed class RecordingSpawner : IProjectileSpawner
        {
            public List<ProjectileSpawnRequest> Requests { get; } = new();

            public void Spawn(in ProjectileSpawnRequest request)
            {
                Requests.Add(request);
            }
        }

        private sealed class Harness
        {
            public BattleState State;
            public BattleSimulation Simulation;
            public BattleTerrain Terrain;
            public Dictionary<string, UnitMotor> Motors;
            public RecordingSpawner Spawner;
        }

        private static CharacterArchetype CreateArchetype(string id)
        {
            return new CharacterArchetype(id, id, 100, 1f, 3f, 3f, 0.4f, 1.6f, 0.9f, 1.1f, 0.7f);
        }

        private static WeaponDefinition CreateWeapon(
            string id,
            WeaponRole role = WeaponRole.ArcingExplosive,
            int damage = 60,
            float impactRadius = 2.5f,
            int ammo = -1)
        {
            return WeaponDefinition.CreateRuntime(
                id, id + ".name", role,
                damage, impactRadius, 0.14f, 1f, 6f, 8f, 20f, ammo, null, null);
        }

        private static Harness CreateHarness(WeaponDefinition playerWeapon, WeaponDefinition opponentWeapon = null)
        {
            var bounds = new Rect(-10f, -8f, 20f, 16f);
            var terrain = BattleTerrain.CreateProcedural(bounds);
            terrain.StampSolidRect(new Vector2(0f, -3f), new Vector2(20f, 4f));

            var playerArchetype = CreateArchetype("archetype.player");
            var opponentArchetype = CreateArchetype("archetype.opponent");

            var playerUnit = new BattleUnitState("player.test.01", BattleTeamId.Player, new Vector2(-3f, 0f), playerArchetype.MaxHealth, 1);
            var opponentUnit = new BattleUnitState("opponent.test.01", BattleTeamId.Opponent, new Vector2(3f, 0f), opponentArchetype.MaxHealth, -1);
            var playerTeam = new BattleTeamState(BattleTeamId.Player, new[] { playerUnit });
            var opponentTeam = new BattleTeamState(BattleTeamId.Opponent, new[] { opponentUnit });
            var state = new BattleState(new[] { playerTeam, opponentTeam }, new TurnState(BattleTeamId.None, string.Empty, 0, false));

            var profile = new UnitMotionProfile(0.34f, 0.5f, 0.3f, 0.42f, 1.6f);
            var motors = new Dictionary<string, UnitMotor>(System.StringComparer.Ordinal)
            {
                [playerUnit.UnitId] = new UnitMotor(terrain.Grid, profile, playerUnit.Position, -6f),
                [opponentUnit.UnitId] = new UnitMotor(terrain.Grid, profile, opponentUnit.Position, -6f)
            };

            var resolvedOpponentWeapon = opponentWeapon ?? playerWeapon;
            var playerInventory = new WeaponInventoryRuntime(new[] { playerWeapon });
            var opponentInventory = new WeaponInventoryRuntime(new[] { resolvedOpponentWeapon });
            var executor = new WeaponExecutor();
            var spawner = new RecordingSpawner();
            var weaponsById = new Dictionary<string, WeaponDefinition>(System.StringComparer.Ordinal)
            {
                [playerWeapon.WeaponId] = playerWeapon,
                [resolvedOpponentWeapon.WeaponId] = resolvedOpponentWeapon
            };

            var turnService = new TurnService();
            turnService.StartFirstTurn(state, BattleTeamId.Player);

            var settings = new BattleSimulationSettings(
                fallDeathY: -6f,
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
                spawner,
                weaponsById,
                playerArchetype,
                opponentArchetype);

            return new Harness
            {
                State = state,
                Simulation = simulation,
                Terrain = terrain,
                Motors = motors,
                Spawner = spawner
            };
        }

        [Test]
        public void Fire_ThenImpact_DamagesAndKnocksBackTarget_AndAdvancesTurn()
        {
            var weapon = CreateWeapon("weapon.test.arc");
            var harness = CreateHarness(weapon);
            var opponent = harness.State.GetUnit("opponent.test.01");
            var opponentMotor = harness.Motors["opponent.test.01"];
            var startHealth = opponent.Health;

            var aim = new AimState(new Vector2(-3f, 1f), new Vector2(1f, 0.15f), 14f, true);
            var fireResult = harness.Simulation.TryFire(aim);

            Assert.AreEqual(BattleFireOutcome.Fired, fireResult.Outcome);
            Assert.AreEqual(1, harness.Spawner.Requests.Count, "default weapon fire spawns exactly one projectile");

            var turnAdvanced = false;
            var turnAdvanceReason = TurnAdvanceReason.PlayerTimerExpired;
            harness.Simulation.TurnAdvanced += (team, reason) =>
            {
                turnAdvanced = true;
                turnAdvanceReason = reason;
            };

            var impactPoint = opponent.Position + new Vector2(0.3f, 0f);
            var projectileId = weapon.CreateProjectileDefinition().ProjectileId;
            var impact = new ProjectileImpact(projectileId, "player.test.01", impactPoint, new Vector2(6f, 1f), weapon.Damage, weapon.ImpactRadius);
            var outcome = harness.Simulation.NotifyProjectileImpact(impact);

            Assert.IsFalse(outcome.BattleFinished);
            Assert.Less(opponent.Health, startHealth, "radial damage reduced the target's health");
            Assert.IsFalse(opponentMotor.IsStable, "explosion knockback pushed the target off its feet");
            Assert.IsTrue(turnAdvanced, "turn advances once the last in-flight shot resolves");
            Assert.AreEqual(TurnAdvanceReason.ProjectileImpactResolved, turnAdvanceReason);
            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam);
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

            var expired = harness.Simulation.TickPlayerTurnTimer(31f);

            Assert.IsTrue(expired);
            Assert.IsTrue(advanced);
            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam);
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
            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam, "instant utility actions still spend the turn");
        }
    }
}
