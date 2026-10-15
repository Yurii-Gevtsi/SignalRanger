using Game.Gameplay.Battle.Motion;
using Game.Gameplay.Battle.Terrain;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class UnitMotorTests
    {
        private const float KillY = -8f;
        private const float Step = 1f / 120f;

        private static TerrainGrid CreateFlatGround()
        {
            var grid = new TerrainGrid(new Vector2(-8f, -6f), new Vector2(1f / 32f, 1f / 32f), 512, 384);
            grid.StampRect(new Vector2(0f, -3f), new Vector2(16f, 6f), true, out _);
            return grid;
        }

        private static UnitMotionProfile CreateProfile()
        {
            return new UnitMotionProfile(
                halfWidth: 0.34f,
                halfHeight: 0.5f,
                crawlHeight: 0.3f,
                walkStepHeight: 0.42f,
                walkSpeed: 1.6f);
        }

        /// <summary>
        /// Stamps a staircase slope: each column is one cell wide and rises
        /// <paramref name="riseCellsPerColumn"/> cells above the previous one.
        /// </summary>
        private static void StampSlope(TerrainGrid grid, float startX, float baseY, int columns, int riseCellsPerColumn)
        {
            var cell = grid.CellSize;
            for (var i = 0; i < columns; i++)
            {
                var height = (i + 1) * riseCellsPerColumn * cell.y;
                grid.StampRect(
                    new Vector2(startX + (i + 0.5f) * cell.x, baseY + height * 0.5f),
                    new Vector2(cell.x, height),
                    true,
                    out _);
            }
        }

        [Test]
        public void SidewaysJumpIntoALedgeCornerAlwaysComesToRest()
        {
            // A ledge one body high on the right; the droid jumps sideways into its edge from many spots.
            var failures = new System.Collections.Generic.List<string>();
            for (var startX = -1.6f; startX <= -0.1f; startX += 0.05f)
            {
                foreach (var speedX in new[] { 2.5f, 4f, 6f })
                {
                    var grid = CreateFlatGround();
                    grid.StampRect(new Vector2(2f, 0.45f), new Vector2(4f, 0.9f), true, out _);
                    var motor = new UnitMotor(grid, CreateProfile(), new Vector2(startX, 0.5f), KillY);
                    motor.AddImpulse(new Vector2(speedX, 3.2f));
                    Simulate(motor, 4f, out var elapsed);
                    if (!motor.IsStable)
                    {
                        failures.Add($"x {startX:F2} vx {speedX}: still moving at {motor.Position}");
                    }
                }
            }

            Assert.That(failures, Is.Empty, string.Join(System.Environment.NewLine, failures));
        }

        private static MotorStepOutcome Simulate(UnitMotor motor, float maxSeconds, out float elapsed)
        {
            var outcome = MotorStepOutcome.Airborne;
            elapsed = 0f;
            while (elapsed < maxSeconds && !motor.IsStable)
            {
                outcome = motor.Step(Step);
                elapsed += Step;
                if (outcome == MotorStepOutcome.FellOutOfBounds)
                {
                    return outcome;
                }
            }

            return outcome;
        }

        [Test]
        public void Impulse_LandsBackOnGround_WithoutTunneling()
        {
            var grid = CreateFlatGround();
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(0f, 0.5f), KillY);

            motor.AddImpulse(new Vector2(3.2f, 2.4f));
            var outcome = Simulate(motor, 10f, out _);

            Assert.AreEqual(MotorStepOutcome.Landed, outcome);
            Assert.AreEqual(0.5f, motor.Position.y, 0.15f, "unit rests with feet on the surface");
            Assert.Greater(motor.Position.x, 0.5f, "knockback carried the unit sideways");
        }

        [Test]
        public void SafeSpawn_RecognizesFlatGround()
        {
            var grid = CreateFlatGround();
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(0f, 0.5f), KillY);

            Assert.IsTrue(motor.IsSafeSpawn());
        }

        [Test]
        public void SafeSpawn_RejectsUnsupportedBody()
        {
            var grid = CreateFlatGround();
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(0f, 1.35f), KillY);

            Assert.IsFalse(motor.IsSafeSpawn());
        }

        [Test]
        public void SafeSpawn_RejectsOneCellEdgeWithNoWalkableExit()
        {
            var grid = new TerrainGrid(new Vector2(-4f, -6f), new Vector2(1f / 32f, 1f / 32f), 256, 384);
            grid.StampRect(new Vector2(-0.2f, -3f), new Vector2(1f / 32f, 6f), true, out _);
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(-0.2f, 0.5f), KillY);

            Assert.IsFalse(motor.IsSafeSpawn());
        }

        [Test]
        public void HighSpeedFall_DoesNotTunnelThroughGround()
        {
            var grid = CreateFlatGround();
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(0f, 4f), KillY);

            // Far faster than any explosion in game tuning.
            motor.AddImpulse(new Vector2(0f, -25f));
            var outcome = Simulate(motor, 5f, out _);

            Assert.AreEqual(MotorStepOutcome.Landed, outcome);
            Assert.AreEqual(0.5f, motor.Position.y, 0.2f);
        }

        [Test]
        public void FallIntoCrater_SettlesOnCraterFloor()
        {
            var grid = CreateFlatGround();
            grid.CarveCircle(new Vector2(0f, 0f), 1.1f, out _);
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(0f, 2f), KillY);

            motor.AddImpulse(new Vector2(0f, -0.5f));
            var outcome = Simulate(motor, 10f, out _);

            Assert.AreEqual(MotorStepOutcome.Landed, outcome);
            Assert.Less(motor.Position.y, 0.45f, "unit rests inside the crater, not on the old surface");
            Assert.IsTrue(motor.IsGrounded(motor.Position));
        }

        [Test]
        public void FlightUnderOverhang_GrazesItWithHead_AndStillLandsOnGround()
        {
            var grid = CreateFlatGround();
            // Thin shelf above the flight path: only the unit's head can touch it.
            grid.StampRect(new Vector2(0.45f, 2f), new Vector2(0.8f, 0.12f), true, out _);
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(-0.6f, 1.3f), KillY);

            // Rising arc whose apex pushes the head into the shelf underside.
            motor.AddImpulse(new Vector2(2f, 2.2f));
            var outcome = Simulate(motor, 10f, out _);

            Assert.AreEqual(MotorStepOutcome.Landed, outcome);
            Assert.AreEqual(0.5f, motor.Position.y, 0.25f, "unit ends on the ground, not hanging at the shelf");
        }

        [Test]
        public void JumpIntoSideWall_StopsJumpTrailAndFallsInsteadOfHanging()
        {
            var grid = CreateFlatGround();
            grid.StampRect(new Vector2(0.3f, 1.2f), new Vector2(0.18f, 2.4f), true, out _);
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(-0.65f, 0.5f), KillY);

            motor.StartJump(2f, 0.75f);
            for (var i = 0; i < 240 && motor.IsJumping; i++)
            {
                motor.Step(Step);
            }

            Assert.IsFalse(motor.IsJumping, "a side collision cancels the jump visual state");
            Assert.IsFalse(motor.IsStable, "only the lower track can make the unit stable");
            Assert.Less(motor.Velocity.y, 0f, "a non-support contact transitions into falling");
        }

        [Test]
        public void SideSnagOnThinOverhang_RecoversThenLandsInsteadOfLockingTheBattle()
        {
            var grid = CreateFlatGround();
            grid.StampRect(new Vector2(0.3f, 1.65f), new Vector2(0.18f, 1.8f), true, out _);
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(-0.7f, 1.35f), KillY);

            motor.AddImpulse(new Vector2(3.8f, 0.45f));
            var outcome = Simulate(motor, 10f, out var elapsed);

            Assert.That(outcome, Is.EqualTo(MotorStepOutcome.Landed));
            Assert.That(motor.IsStable, Is.True);
            Assert.That(elapsed, Is.LessThan(10f), "a side snag must resolve rather than leaving the battle in an airborne loop");
            Assert.That(motor.IsGrounded(motor.Position), Is.True);
        }

        [Test]
        public void FallOffMap_ReportsOutOfBounds()
        {
            var grid = CreateFlatGround();
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(-7.5f, 1f), KillY);

            motor.AddImpulse(new Vector2(-6f, 1.5f));
            var outcome = Simulate(motor, 10f, out _);

            Assert.AreEqual(MotorStepOutcome.FellOutOfBounds, outcome);
        }

        [Test]
        public void Walk_MovesAcrossFlatGround()
        {
            var grid = CreateFlatGround();
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(0f, 0.5f), KillY);

            for (var i = 0; i < 120; i++)
            {
                motor.Walk(1, Step);
            }

            Assert.Greater(motor.Position.x, 1.2f);
            Assert.IsTrue(motor.IsStable);
            Assert.AreEqual(0.5f, motor.Position.y, 0.15f);
        }

        [Test]
        public void Walk_ClimbsSmallLedge_AndIsBlockedByWall()
        {
            var grid = CreateFlatGround();
            // Ledge within step height.
            grid.StampRect(new Vector2(1.5f, 0.15f), new Vector2(1f, 0.3f), true, out _);
            // Tall wall further ahead.
            grid.StampRect(new Vector2(3f, 1.5f), new Vector2(0.4f, 3f), true, out _);
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(0.4f, 0.5f), KillY);

            var blocked = false;
            var highestY = motor.Position.y;
            for (var i = 0; i < 600 && !blocked; i++)
            {
                blocked = motor.Walk(1, Step) == WalkOutcome.Blocked;
                highestY = Mathf.Max(highestY, motor.Position.y);
            }

            Assert.IsTrue(blocked, "tall wall stops walking");
            Assert.Greater(highestY, 0.6f, "unit climbed the ledge on the way");
            Assert.Less(motor.Position.x, 3f, "unit stopped before the wall");
        }

        [Test]
        public void Walk_OffLedge_StartsFalling_ThenLandsBelow()
        {
            var grid = CreateFlatGround();
            // Remove the ground to the right so there is a cliff at x = 2.
            grid.StampRect(new Vector2(5f, -3f), new Vector2(6f, 6.2f), false, out _);
            // Lower floor far below.
            grid.StampRect(new Vector2(5f, -5f), new Vector2(6f, 1f), true, out _);
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(1.5f, 0.5f), KillY);

            var falling = WalkOutcome.None;
            for (var i = 0; i < 900 && falling != WalkOutcome.StartedFalling; i++)
            {
                falling = motor.Walk(1, Step);
            }

            Assert.AreEqual(WalkOutcome.StartedFalling, falling);
            var outcome = Simulate(motor, 10f, out _);
            Assert.AreEqual(MotorStepOutcome.Landed, outcome);
            Assert.Less(motor.Position.y, -3f, "unit landed on the lower floor");
        }

        [Test]
        public void Walk_OnFlatGround_WithMisalignedStart_NeverStumbles()
        {
            var grid = CreateFlatGround();
            // Feet in the upper half of a cell: the alignment every landing
            // used to leave behind, which made flat ground read as a ledge.
            var badAlignment = grid.CellSize.y * 0.75f;
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(0f, 0.5f + badAlignment), KillY);

            var stumbles = 0;
            for (var i = 0; i < 240; i++)
            {
                if (motor.Walk(1, Step) == WalkOutcome.StartedFalling)
                {
                    stumbles++;
                }
            }

            Assert.AreEqual(0, stumbles, "flat ground must never read as a ledge");
            Assert.IsTrue(motor.IsStable);
            Assert.Greater(motor.Position.x, 2.5f, "walked at full speed without stutter");
            Assert.AreEqual(0.5f, motor.Position.y, 0.05f, "feet snapped onto the surface");
        }

        [Test]
        public void Walk_AfterLanding_RunsSmoothlyAtFullSpeed()
        {
            var grid = CreateFlatGround();
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(0f, 0.5f), KillY);
            motor.AddImpulse(new Vector2(1.4f, 2.1f));
            Assert.AreEqual(MotorStepOutcome.Landed, Simulate(motor, 10f, out _));

            var startX = motor.Position.x;
            var stumbles = 0;
            for (var i = 0; i < 240; i++)
            {
                if (motor.Walk(1, Step) == WalkOutcome.StartedFalling)
                {
                    stumbles++;
                }
            }

            Assert.AreEqual(0, stumbles, "landing alignment must not cause walk stutter");
            Assert.Greater(motor.Position.x - startX, 2.5f, "walked at full speed after landing");
        }

        [Test]
        public void Walk_ClimbsWalkableSlope()
        {
            var grid = CreateFlatGround();
            // One cell of rise per column: a 45-degree hill, within the 60-degree limit.
            StampSlope(grid, 1f, 0f, 64, 1);
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(0.3f, 0.5f), KillY);

            var blocked = false;
            for (var i = 0; i < 1200 && motor.Position.x < 2.6f; i++)
            {
                if (motor.Walk(1, Step) == WalkOutcome.Blocked)
                {
                    blocked = true;
                    break;
                }
            }

            Assert.IsFalse(blocked, "a 45-degree hill is walkable");
            Assert.Greater(motor.Position.y, 1.8f, "unit climbed the hill");
        }

        [Test]
        public void Walk_IsBlockedOnNearVerticalSlope()
        {
            var grid = CreateFlatGround();
            // Four cells of rise per column (~76 degrees): a ship-hull incline
            // where every single column stays under the step height, so only
            // the slope rule can stop it.
            StampSlope(grid, 1f, 0f, 40, 4);
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(0.3f, 0.5f), KillY);

            var blocked = false;
            var highestY = motor.Position.y;
            for (var i = 0; i < 2000 && !blocked; i++)
            {
                blocked = motor.Walk(1, Step) == WalkOutcome.Blocked;
                highestY = Mathf.Max(highestY, motor.Position.y);
            }

            Assert.IsTrue(blocked, "a near-vertical incline is not walkable");
            Assert.Less(highestY - 0.5f, 1.2f, "unit gained at most a step-ish of height before stopping");
        }

        [Test]
        public void Walk_IsBlockedOnSustainedSlopeAboveLimit()
        {
            var grid = CreateFlatGround();
            // Two cells of rise per column (~63 degrees): headroom always
            // exists, so the leaky-bucket slope debt is the only gate.
            StampSlope(grid, 1f, 0f, 90, 2);
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(0.3f, 0.5f), KillY);

            var blocked = false;
            for (var i = 0; i < 4000 && !blocked; i++)
            {
                blocked = motor.Walk(1, Step) == WalkOutcome.Blocked;
            }

            Assert.IsTrue(blocked, "a sustained slope above the limit eventually blocks");
            Assert.Less(motor.Position.x, 3.5f, "unit never crested the slope");
        }

        [Test]
        public void Walk_CannotEnterGapLowerThanBody()
        {
            var grid = CreateFlatGround();
            // Shelf leaving a 0.6-unit gap above the floor: shorter than the
            // one-unit body, so the unit must not squeeze in.
            grid.StampRect(new Vector2(2.5f, 0.75f), new Vector2(1f, 0.3f), true, out _);
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(1f, 0.5f), KillY);

            var blocked = false;
            for (var i = 0; i < 600 && !blocked; i++)
            {
                blocked = motor.Walk(1, Step) == WalkOutcome.Blocked;
            }

            Assert.IsTrue(blocked, "gap lower than the body is impassable");
            Assert.Less(motor.Position.x, 2f, "unit stopped at the shelf edge");
        }

        [Test]
        public void Jump_ClearsGap_AndLandsAhead()
        {
            var grid = CreateFlatGround();
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(0f, 0.5f), KillY);

            motor.StartJump(1.6f, 0.75f);
            Assert.IsTrue(motor.IsJumping, "jump state enables the jet trail");
            var outcome = Simulate(motor, 10f, out _);

            Assert.AreEqual(MotorStepOutcome.Landed, outcome);
            Assert.IsFalse(motor.IsJumping, "jet trail stops as soon as the unit lands");
            Assert.AreEqual(1.6f, motor.Position.x, 0.5f);
            Assert.AreEqual(0.5f, motor.Position.y, 0.2f);
        }

        [Test]
        public void Jump_AgainstCeiling_DoesNotStick()
        {
            var grid = CreateFlatGround();
            grid.StampRect(new Vector2(0.6f, 1.6f), new Vector2(2.4f, 0.3f), true, out _);
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(0f, 0.5f), KillY);

            motor.StartJump(1.6f, 1.4f);
            var outcome = Simulate(motor, 10f, out _);

            Assert.AreEqual(MotorStepOutcome.Landed, outcome);
            Assert.AreEqual(0.5f, motor.Position.y, 0.25f, "bounced off the ceiling and landed back down");
        }

        [Test]
        public void JumpTrailScale_GrowsToApex_ThenFadesOnLanding()
        {
            var grid = CreateFlatGround();
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(0f, 0.5f), KillY);

            motor.StartJump(1.6f, 0.75f);
            var initialScale = motor.JumpTrailScale;
            var peakScale = initialScale;
            var outcome = MotorStepOutcome.Airborne;
            for (var elapsed = 0f; elapsed < 10f && !motor.IsStable; elapsed += Step)
            {
                outcome = motor.Step(Step);
                peakScale = Mathf.Max(peakScale, motor.JumpTrailScale);
            }

            Assert.AreEqual(0.5f, initialScale, 0.001f, "trail starts at half size");
            Assert.Less(initialScale, 1f, "trail grows during ascent");
            Assert.Greater(peakScale, 0.95f, "trail reaches full size around the apex");
            Assert.AreEqual(MotorStepOutcome.Landed, outcome);
            Assert.AreEqual(0f, motor.JumpTrailScale, 0.001f, "trail fully fades on landing");
        }

        [Test]
        public void WakeIfUnsupported_AfterGroundDestroyed_UnitFalls()
        {
            var grid = CreateFlatGround();
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(0f, 0.5f), KillY);
            Assert.IsTrue(motor.IsStable);

            grid.CarveCircle(new Vector2(0f, 0f), 1.3f, out _);
            motor.WakeIfUnsupported();

            Assert.IsFalse(motor.IsStable);
            var outcome = Simulate(motor, 10f, out _);
            Assert.AreEqual(MotorStepOutcome.Landed, outcome);
            Assert.Less(motor.Position.y, 0.45f, "unit fell into the crater");
        }
    }
}
