using System.Collections.Generic;
using Game.Gameplay.Battle.AI.Ballistics;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Weapons;
using UnityEngine;

namespace Game.Gameplay.Battle.AI.Movement
{
    /// <summary>
    /// Chooses where the bot should stand this turn and how to get there. It
    /// scores every reachable spot — walked to, and (on hard) jumped to — by the
    /// best shot available from there minus travel cost and the danger of
    /// standing too close to the enemy, and returns the winning move. The shot is
    /// re-planned once the bot arrives, so this only decides the position.
    /// </summary>
    public sealed class BotPositionPlanner
    {
        private const float SampleSpacing = 0.5f;
        private const float TravelWeight = 0.15f;     // per second of walking
        private const float ExposureWeight = 0.4f;    // per world unit inside the danger gap
        private const float JumpTravelSeconds = 0.8f; // flat cost charged to a jump
        private const float MoveEpsilon = 0.15f;

        private readonly WalkReachabilityScanner _scanner;
        private readonly JumpCandidateGenerator _jumpGenerator;
        private readonly IBotShotPlanner _shotPlanner;
        private readonly BotProfile _profile;
        private readonly float _walkSpeed;
        private readonly List<Vector2> _positions = new();
        private readonly List<int> _candidateOrder = new();

        public BotPositionPlanner(
            WalkReachabilityScanner scanner,
            IBotShotPlanner shotPlanner,
            BotProfile profile,
            float walkSpeed,
            JumpCandidateGenerator jumpGenerator = null)
        {
            _scanner = scanner;
            _jumpGenerator = jumpGenerator;
            _shotPlanner = shotPlanner;
            _profile = profile;
            _walkSpeed = Mathf.Max(0.1f, walkSpeed);
        }

        /// <summary>Number of positions scored in the last <see cref="SelectPosition"/> call (telemetry).</summary>
        public int LastCandidateCount { get; private set; }

        public BotMoveDecision SelectPosition(
            Vector2 start,
            Vector2 scatteredTarget,
            Vector2 enemyPosition,
            WeaponDefinition weapon,
            AimSettings aimSettings,
            IReadOnlyList<TrajectoryUnitBody> bodies)
        {
            var maxDistance = _profile.MaxWalkBudgetSeconds * _walkSpeed;
            _scanner.ScanReachable(start, maxDistance, SampleSpacing, _positions);
            BuildBalancedOrder(start);

            var best = BotMoveDecision.Stay(start);
            var bestScore = float.NegativeInfinity;
            var candidateCount = Mathf.Min(_candidateOrder.Count, _profile.MaxPositionCandidates);
            LastCandidateCount = candidateCount;

            for (var i = 0; i < candidateCount; i++)
            {
                var position = _positions[_candidateOrder[i]];
                var travelSeconds = Mathf.Abs(position.x - start.x) / _walkSpeed;
                if (!TryScore(position, start, scatteredTarget, enemyPosition, weapon, aimSettings, bodies, travelSeconds, out var score))
                {
                    continue;
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    best = Mathf.Abs(position.x - start.x) <= MoveEpsilon
                        ? BotMoveDecision.Stay(position)
                        : BotMoveDecision.Walk(position);
                }
            }

            if (_jumpGenerator != null && _profile.JumpEnabled)
            {
                bestScore = ConsiderJumps(start, scatteredTarget, enemyPosition, weapon, aimSettings, bodies, bestScore, ref best);
            }

            return best;
        }

        private float ConsiderJumps(
            Vector2 start, Vector2 scatteredTarget, Vector2 enemyPosition,
            WeaponDefinition weapon, AimSettings aimSettings, IReadOnlyList<TrajectoryUnitBody> bodies,
            float bestScore, ref BotMoveDecision best)
        {
            for (var dir = -1; dir <= 1; dir += 2)
            {
                if (!_jumpGenerator.TryJumpLanding(start, dir, out var landing))
                {
                    continue;
                }

                if (!TryScore(landing, start, scatteredTarget, enemyPosition, weapon, aimSettings, bodies, JumpTravelSeconds, out var score))
                {
                    continue;
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    var negligible = Mathf.Abs(landing.x - start.x) <= MoveEpsilon && Mathf.Abs(landing.y - start.y) <= MoveEpsilon;
                    best = negligible ? BotMoveDecision.Stay(landing) : BotMoveDecision.Jump(landing, dir);
                }
            }

            return bestScore;
        }

        private bool TryScore(
            Vector2 position, Vector2 start, Vector2 scatteredTarget, Vector2 enemyPosition,
            WeaponDefinition weapon, AimSettings aimSettings, IReadOnlyList<TrajectoryUnitBody> bodies,
            float travelSeconds, out float score)
        {
            score = 0f;
            if (OverlapsUnit(position, bodies))
            {
                return false;
            }

            var plan = _shotPlanner.PlanShot(new BotShotContext(position, scatteredTarget, weapon, aimSettings, bodies));
            if (!plan.HasValue)
            {
                return false;
            }

            var exposure = Mathf.Max(0f, _profile.MinEnemySeparation - Vector2.Distance(position, enemyPosition));
            score = plan.Value.Score - TravelWeight * travelSeconds - ExposureWeight * exposure;
            return true;
        }

        /// <summary>
        /// The scanner fills positions as [start, left near-to-far, right
        /// near-to-far]; taking the first MaxPositionCandidates of that would
        /// spend the whole budget on the left side. Alternate the sides so both
        /// directions compete regardless of the cap.
        /// </summary>
        private void BuildBalancedOrder(Vector2 start)
        {
            _candidateOrder.Clear();
            if (_positions.Count == 0)
            {
                return;
            }

            _candidateOrder.Add(0);
            var rightStart = _positions.Count;
            for (var i = 1; i < _positions.Count; i++)
            {
                if (_positions[i].x > start.x)
                {
                    rightStart = i;
                    break;
                }
            }

            var left = 1;
            var right = rightStart;
            while (left < rightStart || right < _positions.Count)
            {
                if (left < rightStart)
                {
                    _candidateOrder.Add(left++);
                }

                if (right < _positions.Count)
                {
                    _candidateOrder.Add(right++);
                }
            }
        }

        /// <summary>
        /// The motor does not collide with units, so occupied spots are walkable —
        /// but standing inside another unit reads as a glitch, so such candidates
        /// are skipped rather than scored.
        /// </summary>
        private static bool OverlapsUnit(Vector2 position, IReadOnlyList<TrajectoryUnitBody> bodies)
        {
            for (var i = 0; i < bodies.Count; i++)
            {
                var body = bodies[i];
                if (Mathf.Abs(position.x - body.Position.x) <= body.HalfWidth * 2f
                    && Mathf.Abs(position.y - body.Position.y) <= body.HalfHeight * 2f)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
