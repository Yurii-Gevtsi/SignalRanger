using System.Collections.Generic;
using Game.Gameplay.Battle.AI;
using Game.Gameplay.Battle.AI.Ballistics;
using Game.Gameplay.Battle.AI.Movement;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Motion;
using Game.Gameplay.Battle.Simulation;
using Game.Gameplay.Battle.Terrain;
using Game.Gameplay.Battle.Weapons;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class BotMovementTests
    {
        private const float TickSeconds = 1f / 120f;
        private const float FallDeathY = -6f;
        private const float UnitHalfWidth = 0.34f;
        private const float UnitHalfHeight = 0.5f;

        private static UnitMotionProfile MotionProfile()
        {
            return new UnitMotionProfile(0.34f, 0.5f, 0.3f, 0.42f, 1.6f);
        }

        // Bot body centre resting on a surface at world y = 0.
        private static readonly float GroundedY = 0.5f;

        // --- Reachability scanner ---------------------------------------------

        [Test]
        public void Scanner_FindsPositions_OnBothSides()
        {
            var terrain = FlatTerrain();
            var scanner = new WalkReachabilityScanner(terrain.Grid, MotionProfile(), FallDeathY);
            var results = new List<Vector2>();

            scanner.ScanReachable(new Vector2(0f, GroundedY), maxDistance: 4f, sampleSpacing: 0.5f, results);

            Assert.IsTrue(results.Exists(p => p.x < -0.4f), "reachable ground to the left");
            Assert.IsTrue(results.Exists(p => p.x > 0.4f), "reachable ground to the right");
        }

        [Test]
        public void Scanner_StopsAtWall()
        {
            var terrain = FlatTerrain();
            terrain.StampSolidRect(new Vector2(2f, 2f), new Vector2(0.6f, 4f)); // tall wall at x = 2
            var scanner = new WalkReachabilityScanner(terrain.Grid, MotionProfile(), FallDeathY);
            var results = new List<Vector2>();

            scanner.ScanReachable(new Vector2(0f, GroundedY), maxDistance: 6f, sampleSpacing: 0.5f, results);

            Assert.IsFalse(results.Exists(p => p.x > 1.9f), "the wall blocks travel past it");
            Assert.IsTrue(results.Exists(p => p.x < -0.4f), "the open side is still reachable");
        }

        [Test]
        public void Scanner_DoesNotWalkIntoTheKillZone()
        {
            // Ground only over x in [-10, 2]; a void (kill zone) lies to the right.
            var terrain = BattleTerrain.CreateProcedural(new Rect(-10f, -8f, 20f, 16f));
            terrain.StampSolidRect(new Vector2(-4f, -2f), new Vector2(12f, 4f)); // surface y = 0, cliff at x = 2
            var scanner = new WalkReachabilityScanner(terrain.Grid, MotionProfile(), FallDeathY);
            var results = new List<Vector2>();

            scanner.ScanReachable(new Vector2(0f, GroundedY), maxDistance: 8f, sampleSpacing: 0.5f, results);

            Assert.IsFalse(results.Exists(p => p.x > 2.5f), "no safe ground past the cliff into the kill zone");
            Assert.IsTrue(results.Exists(p => p.x < -0.4f), "the solid side is reachable");
        }

        // --- Position planner --------------------------------------------------

        [Test]
        public void PositionPlanner_RetreatsFromAnEnemyThatIsTooClose()
        {
            var terrain = FlatTerrain();
            var weapon = ArcWeapon();
            var probe = new TrajectoryProbe(terrain.Grid, Physics2D.gravity, TickSeconds, BattleSimulation.ProjectileSubStepLength, FallDeathY);
            var shotPlanner = new BallisticShotPlanner(probe, RetreatProfile(), new SystemBotRandom(5), Physics2D.gravity);
            var scanner = new WalkReachabilityScanner(terrain.Grid, MotionProfile(), FallDeathY);
            var positionPlanner = new BotPositionPlanner(scanner, shotPlanner, RetreatProfile(), MotionProfile().WalkSpeed);

            var start = new Vector2(0f, GroundedY);
            var enemy = new Vector2(3f, GroundedY);
            var bodies = new List<TrajectoryUnitBody>
            {
                new TrajectoryUnitBody(enemy, UnitHalfWidth, UnitHalfHeight, isAlly: false, isTarget: true)
            };

            var chosen = positionPlanner.SelectPosition(
                start, enemy, enemy, weapon, weapon.CreateAimSettings(AimSettings.Default), bodies);

            Assert.Less(chosen.x, start.x - 0.5f, "the bot steps away from an enemy that is inside its safety gap");
        }

        [Test]
        public void PositionPlanner_StaysPut_WhenAlreadyWellPlaced()
        {
            var terrain = FlatTerrain();
            var weapon = ArcWeapon();
            var probe = new TrajectoryProbe(terrain.Grid, Physics2D.gravity, TickSeconds, BattleSimulation.ProjectileSubStepLength, FallDeathY);
            var profile = RetreatProfile(); // zero jitter, safety gap 5
            var shotPlanner = new BallisticShotPlanner(probe, profile, new SystemBotRandom(5), Physics2D.gravity);
            var scanner = new WalkReachabilityScanner(terrain.Grid, MotionProfile(), FallDeathY);
            var positionPlanner = new BotPositionPlanner(scanner, shotPlanner, profile, MotionProfile().WalkSpeed);

            var start = new Vector2(0f, GroundedY);
            var enemy = new Vector2(7f, GroundedY); // well beyond the safety gap
            var bodies = new List<TrajectoryUnitBody>
            {
                new TrajectoryUnitBody(enemy, UnitHalfWidth, UnitHalfHeight, isAlly: false, isTarget: true)
            };

            var chosen = positionPlanner.SelectPosition(
                start, enemy, enemy, weapon, weapon.CreateAimSettings(AimSettings.Default), bodies);

            Assert.LessOrEqual(Mathf.Abs(chosen.x - start.x), 1f, "with a clear shot and a safe distance, travel cost keeps the bot roughly in place");
        }

        private static BattleTerrain FlatTerrain()
        {
            var terrain = BattleTerrain.CreateProcedural(new Rect(-10f, -8f, 20f, 16f));
            terrain.StampSolidRect(new Vector2(0f, -2f), new Vector2(20f, 4f)); // surface y = 0
            return terrain;
        }

        private static WeaponDefinition ArcWeapon()
        {
            return WeaponDefinition.CreateRuntime(
                "weapon.test.move", "weapon.test.move.name", WeaponRole.ArcingExplosive,
                60, 2.5f, 0.14f, 1f, 8f, 6f, 20f, -1, null, null);
        }

        private static BotProfile RetreatProfile()
        {
            return BotProfile.CreateRuntime(
                powerSteps: 10, angleStepDegrees: 2f, minClearPathRatio: 0.9f,
                scatterBase: 0f, scatterPerDistance: 0f, perfectShotChance: 1f,
                scoreJitter: 0f, allySafetyWeight: 1f, evaluateAllWeapons: false,
                movementEnabled: true, jumpEnabled: false, minEnemySeparation: 5f,
                maxWalkBudgetSeconds: 3f, maxPositionCandidates: 12, retreatAfterShot: false,
                thinkPauseSeconds: 0f, aimPauseSeconds: 0f);
        }
    }
}
