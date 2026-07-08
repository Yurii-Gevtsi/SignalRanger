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
        public void FallPastOverhang_SlidesOffInsteadOfHanging()
        {
            var grid = CreateFlatGround();
            // Thin shelf hanging in the air; the unit clips it with its head side.
            grid.StampRect(new Vector2(0.45f, 2f), new Vector2(0.8f, 0.12f), true, out _);
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(-0.35f, 3.4f), KillY);

            motor.AddImpulse(new Vector2(1.4f, -0.5f));
            var outcome = Simulate(motor, 10f, out _);

            Assert.AreEqual(MotorStepOutcome.Landed, outcome);
            Assert.AreEqual(0.5f, motor.Position.y, 0.2f, "unit ends on the ground, not stuck at the shelf height");
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
            for (var i = 0; i < 600 && !blocked; i++)
            {
                blocked = motor.Walk(1, Step) == WalkOutcome.Blocked;
            }

            Assert.IsTrue(blocked, "tall wall stops walking");
            Assert.Greater(motor.Position.y, 0.6f, "unit climbed the ledge on the way");
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
        public void Jump_ClearsGap_AndLandsAhead()
        {
            var grid = CreateFlatGround();
            var motor = new UnitMotor(grid, CreateProfile(), new Vector2(0f, 0.5f), KillY);

            motor.StartJump(1.6f, 0.75f);
            var outcome = Simulate(motor, 10f, out _);

            Assert.AreEqual(MotorStepOutcome.Landed, outcome);
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
