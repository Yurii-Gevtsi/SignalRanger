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
            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam, "instant utility actions still spend the turn");
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
            Assert.AreEqual(84, player.Health);
            Assert.AreEqual(BattleTeamId.Player, harness.State.Turn.ActiveTeam);
            Assert.IsTrue(harness.Simulation.CanAcceptMovementInput());
            Assert.IsFalse(harness.Simulation.ActiveInventory.CanFireSelected);
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
            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam);
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
                TickMany(harness.Simulation, 20, 1f / 120f);
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
            TickMany(harness.Simulation, 30, 1f / 120f);
            Assert.AreEqual(
                BattleTeamId.Opponent,
                harness.State.Turn.ActiveTeam,
                "the turn does not hang when the magazine empties mid-burst");
        }

        [Test]
        public void Mine_BlocksSecondAttackInSameTurn_AndCapsRetreatTime()
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

            Assert.AreEqual(
                BattleFireOutcome.Fired,
                harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome);
            for (var i = 0; i < 600 && !deployed; i++)
            {
                harness.Simulation.Tick(1f / 120f);
            }

            Assert.IsTrue(deployed);
            Assert.AreEqual(
                BattleFireOutcome.Blocked,
                harness.Simulation.TryFire(new AimState(Vector2.zero, Vector2.zero, 0f, false)).Outcome,
                "a planted mine is the turn's attack - no second weapon use");
            Assert.LessOrEqual(
                harness.Simulation.MovementTimeRemaining,
                5f,
                "planting a mine leaves only a short retreat window");
        }

        [Test]
        public void Mortar_MainImpact_SpawnsFiveHalfStrengthFragments()
        {
            var mortar = CreateWeapon(
                "weapon.mortira",
                damage: 48,
                impactRadius: 1.8f,
                gravityScale: 0f,
                timeoutSeconds: 0.2f);
            var harness = CreateHarness(mortar);
            var fragments = new List<SimProjectile>();
            var fragmentImpacts = new List<WeaponDefinition>();
            harness.Simulation.ProjectileSpawned += projectile =>
            {
                if (projectile.Definition.ProjectileId.EndsWith(".fragment", System.StringComparison.Ordinal))
                {
                    fragments.Add(projectile);
                }
            };
            harness.Simulation.ImpactResolved += impact =>
            {
                if (impact.Weapon?.WeaponId == "weapon.mortira.fragment")
                {
                    fragmentImpacts.Add(impact.Weapon);
                }
            };

            Assert.AreEqual(
                BattleFireOutcome.Fired,
                harness.Simulation.TryFire(new AimState(new Vector2(-3f, 4f), Vector2.up, 6f, true)).Outcome);
            TickMany(harness.Simulation, 30, 1f / 120f);

            Assert.AreEqual(5, fragments.Count);
            Assert.That(fragments[0].Definition.Radius, Is.EqualTo(mortar.ProjectileRadius * 0.5f).Within(0.001f));
            Assert.AreEqual(24, fragments[0].Definition.Damage);
            Assert.That(fragments[0].Definition.ImpactRadius, Is.EqualTo(0.9f).Within(0.001f));
            foreach (var fragment in fragments)
            {
                Assert.Less(fragment.Velocity.y, 0f, "fragments launch back against the shell's travel direction");
            }

            TickMany(harness.Simulation, 420, 1f / 120f);
            Assert.AreEqual(5, fragmentImpacts.Count);
            Assert.That(fragmentImpacts[0].ImpactRadius, Is.EqualTo(mortar.ImpactRadius * 0.5f).Within(0.001f));
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
            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam);
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
            Assert.AreEqual(BattleTeamId.Opponent, harness.State.Turn.ActiveTeam);
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

            // Slow shot (3 m/s) so the 3 s drill budget genuinely runs out inside
            // the long second section instead of the rocket tunnelling through.
            var result = harness.Simulation.TryFire(new AimState(new Vector2(-3f, 4f), Vector2.right, 3f, true));
            Assert.AreEqual(BattleFireOutcome.Fired, result.Outcome);

            TickMany(harness.Simulation, 120, 1f / 120f);
            Assert.AreEqual(1, harness.Simulation.ProjectilesInFlight, "the rocket exits the first terrain section without exploding");
            Assert.That(harness.Simulation.Projectiles[0].Velocity.magnitude, Is.EqualTo(3f).Within(0.01f));
            Assert.Less(harness.Simulation.Projectiles[0].TerrainDrillTimeRemaining, 3f);
            Assert.Greater(harness.Simulation.Projectiles[0].TerrainDrillTimeRemaining, 2f);

            TickMany(harness.Simulation, 600, 1f / 120f);
            Assert.AreEqual(0, harness.Simulation.ProjectilesInFlight, "the rocket explodes after consuming three cumulative seconds inside terrain");
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
