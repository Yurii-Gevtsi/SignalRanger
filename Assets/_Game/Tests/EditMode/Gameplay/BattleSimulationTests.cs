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
        private sealed class Harness
        {
            public BattleState State;
            public BattleSimulation Simulation;
            public BattleTerrain Terrain;
            public Dictionary<string, UnitMotor> Motors;
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
            int ammo = -1,
            float gravityScale = 1f,
            float timeoutSeconds = 8f)
        {
            return WeaponDefinition.CreateRuntime(
                id, id + ".name", role,
                damage, impactRadius, 0.14f, gravityScale, timeoutSeconds, 6f, 20f, ammo, null, null);
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
                weaponsById,
                playerArchetype,
                opponentArchetype);

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

            for (var i = 0; i < 240 && !turnAdvanced; i++)
            {
                harness.Simulation.Tick(1f / 60f);
            }

            Assert.IsTrue(exploded, "projectile resolves inside the simulation");
            Assert.AreEqual(0, harness.Simulation.ProjectilesInFlight);
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

            for (var i = 0; i < 30 * 120 + 4 && !advanced; i++)
            {
                harness.Simulation.Tick(1f / 120f);
            }

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

            for (var i = 0; i < 60 && !advanced; i++)
            {
                harness.Simulation.Tick(1f / 60f);
            }

            Assert.IsTrue(exploded, "projectile timeout resolves without a scene object");
            Assert.IsTrue(advanced, "timeout of the last projectile advances the turn");
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
            fixedHarness.Simulation.TryStartJump(true);
            TickMany(fixedHarness.Simulation, 180, 1f / 120f);
            fixedHarness.Simulation.TryFire(new AimState(new Vector2(-3f, 0f), Vector2.right, 20f, true));
            TickMany(fixedHarness.Simulation, 120, 1f / 120f);

            chunkedHarness.Simulation.Walk(1, 0f);
            TickMany(chunkedHarness.Simulation, 30, 1f / 60f);
            chunkedHarness.Simulation.Walk(0, 0f);
            chunkedHarness.Simulation.TryStartJump(true);
            TickMany(chunkedHarness.Simulation, 90, 1f / 60f);
            chunkedHarness.Simulation.TryFire(new AimState(new Vector2(-3f, 0f), Vector2.right, 20f, true));
            TickMany(chunkedHarness.Simulation, 60, 1f / 60f);

            Assert.AreEqual(HashSimulation(fixedHarness.Simulation), HashSimulation(chunkedHarness.Simulation));
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
                    harness.Simulation.TryStartJump(true);
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
                }

                return hash;
            }
        }
    }
}
