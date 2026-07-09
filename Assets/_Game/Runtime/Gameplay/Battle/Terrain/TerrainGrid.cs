using UnityEngine;

namespace Game.Gameplay.Battle.Terrain
{
    /// <summary>
    /// Solidity mask for the whole arena: the single source of truth for
    /// what counts as ground. Pure data + math (no scene objects) so unit
    /// motion and destruction can be edit-mode tested.
    /// </summary>
    public sealed class TerrainGrid
    {
        private readonly bool[] _solid;

        public TerrainGrid(Vector2 origin, Vector2 cellSize, int width, int height)
        {
            Origin = origin;
            CellSize = new Vector2(Mathf.Max(0.0001f, cellSize.x), Mathf.Max(0.0001f, cellSize.y));
            Width = Mathf.Max(1, width);
            Height = Mathf.Max(1, height);
            _solid = new bool[Width * Height];
        }

        /// <summary>World position of the bottom-left corner of cell (0, 0).</summary>
        public Vector2 Origin { get; }
        public Vector2 CellSize { get; }
        public int Width { get; }
        public int Height { get; }

        public float WorldMinX => Origin.x;
        public float WorldMinY => Origin.y;
        public float WorldMaxX => Origin.x + Width * CellSize.x;
        public float WorldMaxY => Origin.y + Height * CellSize.y;

        public bool IsSolidCell(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return false;
            }

            return _solid[y * Width + x];
        }

        public void SetSolidCell(int x, int y, bool solid)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return;
            }

            _solid[y * Width + x] = solid;
        }

        public bool IsSolidWorld(Vector2 world)
        {
            var cell = WorldToCell(world);
            return IsSolidCell(cell.x, cell.y);
        }

        public Vector2Int WorldToCell(Vector2 world)
        {
            return new Vector2Int(
                Mathf.FloorToInt((world.x - Origin.x) / CellSize.x),
                Mathf.FloorToInt((world.y - Origin.y) / CellSize.y));
        }

        /// <summary>World position of the center of the given cell.</summary>
        public Vector2 CellToWorld(int x, int y)
        {
            return new Vector2(
                Origin.x + (x + 0.5f) * CellSize.x,
                Origin.y + (y + 0.5f) * CellSize.y);
        }

        /// <summary>
        /// Clears all solid cells inside the world-space circle. Returns true
        /// when at least one cell changed; <paramref name="dirtyCells"/> is the
        /// changed cell region (valid only when the method returns true).
        /// </summary>
        public bool CarveCircle(Vector2 worldCenter, float worldRadius, out RectInt dirtyCells)
        {
            dirtyCells = default;
            if (worldRadius <= 0f)
            {
                return false;
            }

            var radiusX = worldRadius / CellSize.x;
            var radiusY = worldRadius / CellSize.y;
            var center = new Vector2(
                (worldCenter.x - Origin.x) / CellSize.x,
                (worldCenter.y - Origin.y) / CellSize.y);
            var minX = Mathf.Clamp(Mathf.FloorToInt(center.x - radiusX) - 1, 0, Width - 1);
            var maxX = Mathf.Clamp(Mathf.CeilToInt(center.x + radiusX) + 1, 0, Width - 1);
            var minY = Mathf.Clamp(Mathf.FloorToInt(center.y - radiusY) - 1, 0, Height - 1);
            var maxY = Mathf.Clamp(Mathf.CeilToInt(center.y + radiusY) + 1, 0, Height - 1);
            var changed = false;

            for (var y = minY; y <= maxY; y++)
            {
                var normalizedY = (y + 0.5f - center.y) / radiusY;
                for (var x = minX; x <= maxX; x++)
                {
                    var normalizedX = (x + 0.5f - center.x) / radiusX;
                    if (normalizedX * normalizedX + normalizedY * normalizedY > 1f)
                    {
                        continue;
                    }

                    var index = y * Width + x;
                    if (!_solid[index])
                    {
                        continue;
                    }

                    _solid[index] = false;
                    changed = true;
                }
            }

            if (changed)
            {
                dirtyCells = new RectInt(minX, minY, maxX - minX + 1, maxY - minY + 1);
            }

            return changed;
        }

        /// <summary>Marks every cell inside the world-space rectangle as solid (or clears it).</summary>
        public bool StampRect(Vector2 worldCenter, Vector2 worldSize, bool solid, out RectInt dirtyCells)
        {
            dirtyCells = default;
            var halfSize = worldSize * 0.5f;
            var min = WorldToCell(worldCenter - halfSize);
            // The max edge is exclusive: a rect ending exactly on a cell
            // boundary must not claim the next cell.
            var maxX = Mathf.CeilToInt((worldCenter.x + halfSize.x - Origin.x) / CellSize.x) - 1;
            var maxY = Mathf.CeilToInt((worldCenter.y + halfSize.y - Origin.y) / CellSize.y) - 1;
            if (maxX < min.x || maxY < min.y)
            {
                return false;
            }

            var minX = Mathf.Clamp(min.x, 0, Width - 1);
            maxX = Mathf.Clamp(maxX, 0, Width - 1);
            var minY = Mathf.Clamp(min.y, 0, Height - 1);
            maxY = Mathf.Clamp(maxY, 0, Height - 1);
            var changed = false;

            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    var index = y * Width + x;
                    if (_solid[index] == solid)
                    {
                        continue;
                    }

                    _solid[index] = solid;
                    changed = true;
                }
            }

            if (changed)
            {
                dirtyCells = new RectInt(minX, minY, maxX - minX + 1, maxY - minY + 1);
            }

            return changed;
        }

        /// <summary>
        /// Scans straight down from the start height and returns the world Y of
        /// the top edge of the first solid cell in the column.
        /// </summary>
        public bool TryFindSurfaceBelow(float worldX, float startWorldY, float maxDrop, out float surfaceWorldY)
        {
            surfaceWorldY = 0f;
            var x = Mathf.FloorToInt((worldX - Origin.x) / CellSize.x);
            if (x < 0 || x >= Width)
            {
                return false;
            }

            var startY = Mathf.Clamp(Mathf.FloorToInt((startWorldY - Origin.y) / CellSize.y), 0, Height - 1);
            var minY = Mathf.Max(0, Mathf.FloorToInt((startWorldY - maxDrop - Origin.y) / CellSize.y));
            var wasSolid = IsSolidCell(x, startY);
            for (var y = startY; y >= minY; y--)
            {
                var solid = _solid[y * Width + x];
                if (solid && !wasSolid)
                {
                    surfaceWorldY = Origin.y + (y + 1) * CellSize.y;
                    return true;
                }

                wasSolid = solid;
            }

            return false;
        }

        /// <summary>
        /// Steps one cell at a time from <paramref name="from"/> towards
        /// <paramref name="to"/> and reports the first solid point hit.
        /// The same anti-tunneling march the unit and projectile motion use.
        /// </summary>
        public bool Raycast(Vector2 from, Vector2 to, out Vector2 hitPoint)
        {
            hitPoint = to;
            var delta = to - from;
            var stepCount = Mathf.Max(1, Mathf.CeilToInt(Mathf.Max(
                Mathf.Abs(delta.x) / CellSize.x,
                Mathf.Abs(delta.y) / CellSize.y)));
            var step = delta / stepCount;
            var position = from;
            for (var i = 0; i < stepCount; i++)
            {
                position += step;
                if (IsSolidWorld(position))
                {
                    hitPoint = position;
                    return true;
                }
            }

            return false;
        }
    }
}
