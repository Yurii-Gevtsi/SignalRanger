using System.Collections.Generic;
using Game.Gameplay.Battle.AI;
using Game.Gameplay.Battle.AI.Ballistics;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Terrain;
using Game.Gameplay.Battle.Weapons;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class BotShotPlannerTests
    {
        private const float TickSeconds = 1f / 120f;
        private const float FallDeathY = -6f;
        private const float MuzzleOffset = 0.45f;
        private const float UnitHalfWidth = 0.34f;
        private const float UnitHalfHeight = 0.5f;

        private static BotProfile Profile(float scatterBase = 0f, float perfectChance = 1f, float jitter = 0f)
        {
            return BotProfile.CreateRuntime(
                powerSteps: 12, angleStepDegrees: 2f,
                scatterBase: scatterBase, scatterPerDistance: 0f, perfectShotChance: perfectChance,
                scoreJitter: jitter, evaluateAllWeapons: false,
                movementEnabled: false, jumpEnabled: false, minEnemySeparation: 0f,
                maxWalkBudgetSeconds: 0f, maxPositionCandidates: 1,
                thinkPauseSeconds: 0f, aimPauseSeconds: 0f);
        }

        private static WeaponDefinition ArcWeapon()
        {
            return WeaponDefinition.CreateRuntime(
                "weapon.test.plan", "weapon.test.plan.name", WeaponRole.ArcingExplosive,
                60, 2.5f, 0.14f, 1f, 8f, 6f, 20f, -1, null, null);
        }

        private static TrajectoryProbe Probe(BattleTerrain terrain)
        {
            return new TrajectoryProbe(terrain.Grid, Physics2D.gravity, TickSeconds, BattleSimulationSubStep, FallDeathY);
        }

        // Kept in sync with BattleSimulation.ProjectileSubStepLength via that constant.
        private const float BattleSimulationSubStep = Game.Gameplay.Battle.Simulation.BattleSimulation.ProjectileSubStepLength;

        [Test]
        public void BallisticShotPlanner_ClearsAWallBetweenOriginAndTarget()
        {
            var terrain = BattleTerrain.CreateProcedural(new Rect(-10f, -8f, 20f, 16f));
            terrain.StampSolidRect(new Vector2(0f, -3f), new Vector2(20f, 4f)); // ground, surface y = -1
            terrain.StampSolidRect(new Vector2(0f, 0.75f), new Vector2(0.6f, 3.5f)); // wall from y=-1 up to y=2.5

            var weapon = ArcWeapon();
            var origin = new Vector2(-4f, 0f);
            var target = new Vector2(4f, 0f);
            var bodies = new List<TrajectoryUnitBody>
            {
                new TrajectoryUnitBody(target, UnitHalfWidth, UnitHalfHeight, isAlly: false, isTarget: true)
            };

            var planner = new BallisticShotPlanner(Probe(terrain), Profile(), new SystemBotRandom(1), Physics2D.gravity);
            var context = new BotShotContext(origin, target, weapon, weapon.CreateAimSettings(AimSettings.Default), bodies);

            var plan = planner.PlanShot(context);

            Assert.IsTrue(plan.HasValue, "the planner finds a shot over the wall");
            var report = ReProbe(terrain, weapon, plan.Value, bodies, target);
            Assert.AreEqual(TrajectoryEndKind.Unit, report.EndKind, "the chosen arc reaches the target rather than the wall");
            Assert.IsTrue(report.EndedOnTargetUnit);
            Assert.Greater(plan.Value.Aim.Direction.y, 0.3f, "clearing the wall needs a lofted launch");
        }

        [Test]
        public void BallisticShotPlanner_IsDeterministic_WithSameSeed()
        {
            var terrain = BattleTerrain.CreateProcedural(new Rect(-10f, -8f, 20f, 16f));
            terrain.StampSolidRect(new Vector2(0f, -3f), new Vector2(20f, 4f));

            var weapon = ArcWeapon();
            var origin = new Vector2(-3f, 0f);
            var target = new Vector2(3f, 0f);
            var bodies = new List<TrajectoryUnitBody>
            {
                new TrajectoryUnitBody(target, UnitHalfWidth, UnitHalfHeight, isAlly: false, isTarget: true)
            };
            var context = new BotShotContext(origin, target, weapon, weapon.CreateAimSettings(AimSettings.Default), bodies);

            var planA = new BallisticShotPlanner(Probe(terrain), Profile(jitter: 0.4f), new SystemBotRandom(777), Physics2D.gravity).PlanShot(context);
            var planB = new BallisticShotPlanner(Probe(terrain), Profile(jitter: 0.4f), new SystemBotRandom(777), Physics2D.gravity).PlanShot(context);

            Assert.IsTrue(planA.HasValue && planB.HasValue);
            Assert.AreEqual(planA.Value.Aim.Power, planB.Value.Aim.Power, 1e-4f);
            Assert.AreEqual(planA.Value.Aim.Direction.x, planB.Value.Aim.Direction.x, 1e-4f);
            Assert.AreEqual(planA.Value.Aim.Direction.y, planB.Value.Aim.Direction.y, 1e-4f);
        }

        [Test]
        public void TargetScatter_PerfectChanceOne_ReturnsExactTarget()
        {
            var target = new Vector2(5f, 2f);
            var scattered = TargetScatter.Apply(target, Vector2.zero, Profile(scatterBase: 3f, perfectChance: 1f), new SystemBotRandom(3));
            Assert.AreEqual(target, scattered);
        }

        [Test]
        public void TargetScatter_StaysWithinRadius()
        {
            var target = new Vector2(5f, 0f);
            var origin = Vector2.zero;
            var profile = Profile(scatterBase: 1.5f, perfectChance: 0f);
            var random = new SystemBotRandom(9);

            for (var i = 0; i < 50; i++)
            {
                var scattered = TargetScatter.Apply(target, origin, profile, random);
                Assert.LessOrEqual(Vector2.Distance(scattered, target), 1.5f + 1e-3f, "scatter stays inside the profile radius");
            }
        }

        private static TrajectoryReport ReProbe(
            BattleTerrain terrain, WeaponDefinition weapon, BotShotPlan plan,
            IReadOnlyList<TrajectoryUnitBody> bodies, Vector2 target)
        {
            var direction = plan.Aim.Direction;
            return Probe(terrain).Simulate(
                plan.Aim.Origin + direction * MuzzleOffset,
                direction * plan.Aim.Power,
                weapon.GravityScale,
                weapon.ProjectileRadius,
                weapon.TimeoutSeconds,
                new TrajectoryObstacles(bodies, target));
        }
    }
}
