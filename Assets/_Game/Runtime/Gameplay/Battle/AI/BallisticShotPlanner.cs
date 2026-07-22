using Game.Gameplay.Battle.AI.Ballistics;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Weapons;
using UnityEngine;

namespace Game.Gameplay.Battle.AI
{
    /// <summary>
    /// Medium/hard shot planning: sample launch powers, solve the flat and
    /// lofted arc for each with <see cref="BallisticsSolver"/>, forward-simulate
    /// every candidate through <see cref="TrajectoryProbe"/> and score it. The
    /// arc is rounded to the aim step the player is limited to, and the winner is
    /// probed with the exact direction that will fire, so the plan can never
    /// surprise the simulation. Obstacles are respected because a blocked arc
    /// simply scores low on clear-path and never wins against one that reaches.
    /// </summary>
    public sealed class BallisticShotPlanner : IBotShotPlanner
    {
        // Must match ProjectileWeaponEffect.MuzzleOffset so the probe launches
        // from exactly where the real projectile spawns.
        private const float MuzzleOffset = 0.45f;

        // Scoring weights. Tunable; kept here (not in BotProfile) until difficulty
        // needs to differentiate them.
        private const float HitWeight = 1f;
        private const float ClearPathWeight = 0.35f;
        private const float AngleWeight = 0.1f;
        private const float SelfRiskWeight = 0.8f;

        private readonly TrajectoryProbe _probe;
        private readonly BotProfile _profile;
        private readonly IBotRandom _random;
        private readonly float _gravityMagnitudePerScale;

        public BallisticShotPlanner(TrajectoryProbe probe, BotProfile profile, IBotRandom random, Vector2 gravityBase)
        {
            _probe = probe;
            _profile = profile;
            _random = random;
            _gravityMagnitudePerScale = Mathf.Abs(gravityBase.y);
        }

        public BotShotPlan? PlanShot(in BotShotContext context)
        {
            var weapon = context.Weapon;
            if (weapon == null)
            {
                return null;
            }

            var gravityMagnitude = _gravityMagnitudePerScale * weapon.GravityScale;
            var minPower = context.AimSettings.MinPower;
            var maxPower = context.AimSettings.MaxPower;
            var steps = _profile.PowerSteps;

            BotShotPlan? best = null;
            var bestScore = float.NegativeInfinity;

            for (var i = 0; i < steps; i++)
            {
                var t = steps == 1 ? 1f : i / (float)(steps - 1);
                var speed = Mathf.Lerp(minPower, maxPower, t);

                var solutions = BallisticsSolver.SolveLaunchVelocities(
                    context.Origin, context.AimTarget, speed, gravityMagnitude, out var low, out var high);
                if (solutions == 0)
                {
                    continue;
                }

                EvaluateArc(context, weapon, speed, gravityMagnitude, low, ref best, ref bestScore);
                if (solutions == 2)
                {
                    EvaluateArc(context, weapon, speed, gravityMagnitude, high, ref best, ref bestScore);
                }
            }

            return best;
        }

        private void EvaluateArc(
            in BotShotContext context,
            WeaponDefinition weapon,
            float speed,
            float gravityMagnitude,
            Vector2 arcVelocity,
            ref BotShotPlan? best,
            ref float bestScore)
        {
            if (arcVelocity.sqrMagnitude < 1e-6f)
            {
                return;
            }

            // Refine once from the muzzle point so the offset does not bias the aim.
            var approxDirection = arcVelocity.normalized;
            var muzzle = context.Origin + approxDirection * MuzzleOffset;
            var direction = approxDirection;
            var refinedCount = BallisticsSolver.SolveLaunchVelocities(
                muzzle, context.AimTarget, speed, gravityMagnitude, out var refinedLow, out var refinedHigh);
            if (refinedCount > 0)
            {
                direction = ClosestDirection(approxDirection, refinedLow, refinedHigh, refinedCount);
            }

            // Round to the aim step the player is limited to; charge the rounding as a penalty.
            var angle = Mathf.Atan2(direction.y, direction.x);
            var step = _profile.AngleStepDegrees * Mathf.Deg2Rad;
            var roundedAngle = Mathf.Round(angle / step) * step;
            var anglePenalty = Mathf.Clamp01(Mathf.Abs(roundedAngle - angle) / step);
            var firedDirection = new Vector2(Mathf.Cos(roundedAngle), Mathf.Sin(roundedAngle));

            var firedMuzzle = context.Origin + firedDirection * MuzzleOffset;
            var report = _probe.Simulate(
                firedMuzzle,
                firedDirection * speed,
                weapon.GravityScale,
                weapon.ProjectileRadius,
                weapon.TimeoutSeconds,
                new TrajectoryObstacles(context.Bodies, context.AimTarget));

            // Never fire on a friendly: a direct ally hit disqualifies the arc
            // outright. Graded ally-splash risk (distance along the trajectory)
            // only matters once team battles exist and can be added to Score then.
            if (report.EndedOnAllyUnit)
            {
                return;
            }

            var score = Score(report, anglePenalty, weapon, context.Origin);
            score *= 1f + _random.NextFloat() * _profile.ScoreJitter;

            if (score <= bestScore)
            {
                return;
            }

            bestScore = score;
            best = new BotShotPlan(new AimState(context.Origin, firedDirection, speed, true), weapon.WeaponId, score);
        }

        private static Vector2 ClosestDirection(Vector2 reference, Vector2 low, Vector2 high, int count)
        {
            var lowDir = low.normalized;
            if (count < 2)
            {
                return lowDir;
            }

            var highDir = high.normalized;
            return Vector2.Dot(lowDir, reference) >= Vector2.Dot(highDir, reference) ? lowDir : highDir;
        }

        private static float Score(TrajectoryReport report, float anglePenalty, WeaponDefinition weapon, Vector2 origin)
        {
            var impactRadius = weapon.ImpactRadius;
            var hit = Mathf.Clamp01(1f - report.DistanceToTarget / (2f * impactRadius));
            var selfDistance = Vector2.Distance(report.EndPoint, origin);
            var selfRisk = Mathf.Clamp01(1f - selfDistance / impactRadius);

            return HitWeight * hit
                + ClearPathWeight * report.ClearPathRatio
                - AngleWeight * anglePenalty
                - SelfRiskWeight * selfRisk;
        }
    }
}
