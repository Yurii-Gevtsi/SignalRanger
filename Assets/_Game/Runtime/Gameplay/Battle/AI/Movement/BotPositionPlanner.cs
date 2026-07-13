using System.Collections.Generic;
using Game.Gameplay.Battle.AI.Ballistics;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Weapons;
using UnityEngine;

namespace Game.Gameplay.Battle.AI.Movement
{
    /// <summary>
    /// Chooses where the bot should stand this turn. It scans walkable ground,
    /// scores each candidate by the best shot available from there minus the cost
    /// of getting there and the danger of standing too close to the enemy, and
    /// returns the winning position. The shot itself is re-planned once the bot
    /// arrives, so this only decides *where* to move, never the final aim.
    /// </summary>
    public sealed class BotPositionPlanner
    {
        private const float SampleSpacing = 0.5f;
        private const float TravelWeight = 0.15f;   // per second of walking
        private const float ExposureWeight = 0.4f;  // per world unit inside the danger gap

        private readonly WalkReachabilityScanner _scanner;
        private readonly IBotShotPlanner _shotPlanner;
        private readonly BotProfile _profile;
        private readonly float _walkSpeed;
        private readonly List<Vector2> _positions = new();

        public BotPositionPlanner(
            WalkReachabilityScanner scanner,
            IBotShotPlanner shotPlanner,
            BotProfile profile,
            float walkSpeed)
        {
            _scanner = scanner;
            _shotPlanner = shotPlanner;
            _profile = profile;
            _walkSpeed = Mathf.Max(0.1f, walkSpeed);
        }

        public Vector2 SelectPosition(
            Vector2 start,
            Vector2 scatteredTarget,
            Vector2 enemyPosition,
            WeaponDefinition weapon,
            AimSettings aimSettings,
            IReadOnlyList<TrajectoryUnitBody> bodies)
        {
            var maxDistance = _profile.MaxWalkBudgetSeconds * _walkSpeed;
            _scanner.ScanReachable(start, maxDistance, SampleSpacing, _positions);

            var best = start;
            var bestScore = float.NegativeInfinity;
            var candidateCount = Mathf.Min(_positions.Count, _profile.MaxPositionCandidates);

            for (var i = 0; i < candidateCount; i++)
            {
                var position = _positions[i];
                var plan = _shotPlanner.PlanShot(
                    new BotShotContext(position, scatteredTarget, weapon, aimSettings, bodies));
                if (!plan.HasValue)
                {
                    continue;
                }

                var travelSeconds = Mathf.Abs(position.x - start.x) / _walkSpeed;
                var exposure = Mathf.Max(0f, _profile.MinEnemySeparation - Vector2.Distance(position, enemyPosition));
                var score = plan.Value.Score - TravelWeight * travelSeconds - ExposureWeight * exposure;

                if (score > bestScore)
                {
                    bestScore = score;
                    best = position;
                }
            }

            return best;
        }
    }
}
