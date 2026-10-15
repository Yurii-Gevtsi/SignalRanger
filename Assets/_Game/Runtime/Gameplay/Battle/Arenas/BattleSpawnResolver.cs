using Game.Gameplay.Battle.Motion;
using Game.Gameplay.Battle.Terrain;
using UnityEngine;

namespace Game.Gameplay.Battle.Arenas
{
    /// <summary>
    /// Resolves authored spawn anchors to the nearest position where the whole
    /// unit fits and at least one initial walk step is possible.
    /// </summary>
    public sealed class BattleSpawnResolver
    {
        private readonly TerrainGrid _terrain;
        private readonly UnitMotionProfile _profile;
        private readonly float _killY;
        private readonly float _rayStartHeight;
        private readonly float _rayDistance;
        private readonly int _searchRadiusCells;

        public BattleSpawnResolver(
            TerrainGrid terrain,
            UnitMotionProfile profile,
            float killY,
            float rayStartHeight,
            float rayDistance,
            float searchRadius = 2.5f)
        {
            _terrain = terrain;
            _profile = profile;
            _killY = killY;
            _rayStartHeight = Mathf.Max(0f, rayStartHeight);
            _rayDistance = Mathf.Max(0f, rayDistance);
            _searchRadiusCells = terrain == null
                ? 0
                : Mathf.Max(1, Mathf.CeilToInt(searchRadius / terrain.CellSize.x));
        }

        public bool TryResolve(Vector2 desiredPoint, out Vector2 resolvedPoint)
        {
            resolvedPoint = desiredPoint;
            if (_terrain == null || _profile == null)
            {
                return false;
            }

            for (var distance = 0; distance <= _searchRadiusCells; distance++)
            {
                if (TryCandidate(desiredPoint, distance, out resolvedPoint))
                {
                    return true;
                }

                if (distance == 0)
                {
                    continue;
                }

                if (TryCandidate(desiredPoint, -distance, out resolvedPoint))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryCandidate(Vector2 desiredPoint, int offsetCells, out Vector2 resolvedPoint)
        {
            var candidateX = desiredPoint.x + offsetCells * _terrain.CellSize.x;
            var scanTop = Mathf.Max(
                desiredPoint.y + _rayStartHeight,
                _profile.HalfHeight + _rayStartHeight);
            var maxDrop = _rayDistance + _rayStartHeight;
            if (_terrain.IsSolidWorld(new Vector2(candidateX, scanTop)))
            {
                // An authored anchor can fall inside a tall open-world prop
                // (for example, a ring or canyon wall). Starting the ray in
                // solid skips the upper surface and may land inside a lower
                // pocket. Search from above the whole terrain in that case;
                // empty cave interiors are unaffected because their scan start
                // is not solid and still resolves to the cave floor.
                scanTop = _terrain.WorldMaxY;
                maxDrop = Mathf.Max(maxDrop, scanTop - desiredPoint.y + _terrain.CellSize.y);
            }
            if (!_terrain.TryFindSurfaceBelow(
                    candidateX,
                    scanTop,
                    maxDrop,
                    out var surfaceY))
            {
                resolvedPoint = desiredPoint;
                return false;
            }

            resolvedPoint = new Vector2(candidateX, surfaceY + _profile.HalfHeight);
            var probe = new UnitMotor(_terrain, _profile, resolvedPoint, _killY);
            return probe.IsSafeSpawn();
        }
    }
}
