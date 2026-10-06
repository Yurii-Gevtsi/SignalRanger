using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Battle.Terrain
{
    /// <summary>
    /// Owns the arena-wide solidity grid and all mutable destructible sprite
    /// surfaces. Every crater is applied to the shared grid and to each visual
    /// surface it intersects, so separate high-detail sprites behave as one
    /// continuous battlefield.
    /// </summary>
    public sealed class BattleTerrain
    {
        private const byte SolidAlphaThreshold = 96;
        private const float ProceduralCellsPerUnit = 32f;
        private const float SpriteCollisionCellsPerUnit = 56f;
        private const float VisualCraterEdgePaddingPixels = 2f;

        private readonly List<DestructibleVisual> _visuals;
        private readonly List<DestructibleVisual> _movingVisuals;
        private readonly List<PlacedTerrainSpriteSurface> _placedSurfaces = new();

        private BattleTerrain(TerrainGrid grid, List<DestructibleVisual> visuals = null)
        {
            Grid = grid;
            _visuals = visuals ?? new List<DestructibleVisual>();
            _movingVisuals = new List<DestructibleVisual>();
            for (var i = 0; i < _visuals.Count; i++)
            {
                if (_visuals[i].IsMoving)
                {
                    _movingVisuals.Add(_visuals[i]);
                }
            }
        }

        public TerrainGrid Grid { get; }
        public bool HasDestructibleVisual => _visuals.Count > 0;

        public static BattleTerrain CreateProcedural(Rect worldBounds)
        {
            var cellSize = Vector2.one / ProceduralCellsPerUnit;
            return new BattleTerrain(CreateGrid(worldBounds, cellSize));
        }

        public static BattleTerrain CreateFromSprite(SpriteRenderer renderer, Sprite sourceSprite, Rect worldBounds)
        {
            if (renderer == null || sourceSprite == null)
            {
                return null;
            }

            renderer.sprite = sourceSprite;
            return CreateFromSprites(new[] { renderer }, worldBounds);
        }

        public static BattleTerrain CreateFromSprites(IReadOnlyList<SpriteRenderer> renderers, Rect worldBounds)
        {
            if (renderers == null || renderers.Count == 0)
            {
                return null;
            }

            var surfaces = new BattleTerrainSpriteSurface[renderers.Count];
            for (var i = 0; i < renderers.Count; i++)
            {
                surfaces[i] = new BattleTerrainSpriteSurface(renderers[i], null);
            }

            return CreateFromSpriteSurfaces(surfaces, worldBounds);
        }

        public static BattleTerrain CreateFromSpriteSurfaces(
            IReadOnlyList<BattleTerrainSpriteSurface> surfaces,
            Rect worldBounds)
        {
            if (surfaces == null || surfaces.Count == 0)
            {
                return null;
            }

            var grid = CreateGrid(worldBounds, Vector2.one / SpriteCollisionCellsPerUnit);
            var visuals = new List<DestructibleVisual>(surfaces.Count);
            for (var i = 0; i < surfaces.Count; i++)
            {
                var visual = CreateVisual(surfaces[i]);
                if (visual == null)
                {
                    continue;
                }

                RasterizeIntoGrid(visual, grid);
                visuals.Add(visual);
            }

            if (visuals.Count == 0)
            {
                return null;
            }

            return new BattleTerrain(grid, visuals);
        }

        public bool ApplyCrater(Vector2 worldCenter, float worldRadius)
        {
            return ApplyCraterAndGetDestroyedCellCount(worldCenter, worldRadius) > 0;
        }

        public int ApplyCraterAndGetDestroyedCellCount(
            Vector2 worldCenter,
            float worldRadius,
            TerrainMaterial maximumMaterial = TerrainMaterial.Titanium)
        {
            if (!Grid.CarveCircle(worldCenter, worldRadius, maximumMaterial, out var dirtyCells, out var destroyedCellCount))
            {
                return 0;
            }

            for (var i = 0; i < _visuals.Count; i++)
            {
                _visuals[i].ApplyCrater(worldCenter, worldRadius);
            }
            if (maximumMaterial >= TerrainMaterial.Titanium)
                foreach (var surface in _placedSurfaces) surface.ApplyCrater(worldCenter, worldRadius);

            // A crater may cross overlapping cluster sprites. Re-rasterizing
            // the surviving pixels preserves union semantics: a cell remains
            // solid while at least one surface still owns solid mask data.
            // Only cells inside the crater's own dirty rect could have
            // changed, so re-scanning is scoped to that rect instead of each
            // visual's full sprite (which is what made drill/lava freeze the
            // game: one small crater used to re-rasterize every destructible
            // surface in the arena, pixel by pixel).
            for (var i = 0; i < _visuals.Count; i++)
            {
                if (_visuals[i].HasExactMask)
                {
                    RasterizeIntoGrid(_visuals[i], Grid, dirtyCells);
                }
            }

            return destroyedCellCount;
        }

        /// <summary>
        /// Rebuilds the shared collision grid after a sprite-backed surface has
        /// moved. Runtime textures already contain any craters, so rebuilding
        /// from the surviving pixels preserves destruction while removing the
        /// old position from collision queries.
        /// </summary>
        public void RefreshMovingSpriteSurfaces()
        {
            if (_movingVisuals.Count > 0)
            {
                var hasDirtyBounds = false;
                var dirtyWorldBounds = default(Rect);
                for (var i = 0; i < _movingVisuals.Count; i++)
                {
                    var visual = _movingVisuals[i];
                    var previousBounds = visual.WorldBounds;
                    visual.RefreshWorldTransform();
                    var currentBounds = visual.WorldBounds;
                    var bounds = Encapsulate(previousBounds, currentBounds);
                    dirtyWorldBounds = hasDirtyBounds
                        ? Encapsulate(dirtyWorldBounds, bounds)
                        : bounds;
                    hasDirtyBounds = true;
                }

                if (!hasDirtyBounds)
                {
                    return;
                }

                var dirtyCells = WorldBoundsToCellRegion(dirtyWorldBounds);
                Grid.ClearSolidCells(dirtyCells);
                for (var i = 0; i < _visuals.Count; i++)
                {
                    if (Intersects(_visuals[i].WorldBounds, dirtyWorldBounds))
                    {
                        RasterizeIntoGrid(_visuals[i], Grid, dirtyCells);
                    }
                }
                foreach (var surface in _placedSurfaces) surface.Rasterize(Grid, dirtyCells);

                return;
            }

            Grid.ClearSolidCells();
            for (var i = 0; i < _visuals.Count; i++)
            {
                _visuals[i].RefreshWorldTransform();
                RasterizeIntoGrid(_visuals[i], Grid);
            }
            foreach (var surface in _placedSurfaces)
                surface.Rasterize(Grid, new RectInt(0, 0, Grid.Width, Grid.Height));
        }

        public void AddPlacedSurface(PlacedTerrainSpriteSurface surface)
        {
            _placedSurfaces.Add(surface);
            surface.Rasterize(Grid, new RectInt(0, 0, Grid.Width, Grid.Height));
        }

        public void StampSolidRect(Vector2 worldCenter, Vector2 worldSize)
        {
            StampSolidRect(worldCenter, worldSize, TerrainMaterial.Soil);
        }

        public void StampSolidRect(Vector2 worldCenter, Vector2 worldSize, TerrainMaterial material)
        {
            Grid.StampRect(worldCenter, worldSize, true, material, out _);
        }

        public void Dispose()
        {
            foreach (var surface in _placedSurfaces) surface.Dispose();
            _placedSurfaces.Clear();
            for (var i = 0; i < _visuals.Count; i++)
            {
                _visuals[i].Dispose();
            }

            _visuals.Clear();
        }

        private static TerrainGrid CreateGrid(Rect worldBounds, Vector2 cellSize)
        {
            var width = Mathf.Max(1, Mathf.CeilToInt(worldBounds.width / cellSize.x));
            var height = Mathf.Max(1, Mathf.CeilToInt(worldBounds.height / cellSize.y));
            return new TerrainGrid(worldBounds.min, cellSize, width, height);
        }

        private static DestructibleVisual CreateVisual(BattleTerrainSpriteSurface surface)
        {
            var renderer = surface.Renderer;
            var sourceSprite = renderer != null ? renderer.sprite : null;
            if (sourceSprite == null || sourceSprite.texture == null)
            {
                return null;
            }

            Color32[] sourcePixels;
            try
            {
                sourcePixels = sourceSprite.texture.GetPixels32();
            }
            catch (UnityException)
            {
                return null;
            }

            var sourceTexture = sourceSprite.texture;
            byte[] maskPixels = null;
            if (surface.HasCollisionMask)
            {
                if (surface.CollisionMask.width != sourceTexture.width
                    || surface.CollisionMask.height != sourceTexture.height)
                {
                    return null;
                }

                try
                {
                    var sourceMaskPixels = surface.CollisionMask.GetPixels32();
                    maskPixels = new byte[sourceMaskPixels.Length];
                    for (var i = 0; i < sourceMaskPixels.Length; i++)
                    {
                        maskPixels[i] = sourceMaskPixels[i].r;
                    }
                }
                catch (UnityException)
                {
                    return null;
                }

            }

            var runtimeTexture = new Texture2D(sourceTexture.width, sourceTexture.height, TextureFormat.RGBA32, false)
            {
                filterMode = sourceTexture.filterMode,
                wrapMode = TextureWrapMode.Clamp
            };
            runtimeTexture.SetPixels32(sourcePixels);
            runtimeTexture.Apply(false, false);

            var runtimeSprite = Sprite.Create(
                runtimeTexture,
                sourceSprite.rect,
                new Vector2(sourceSprite.pivot.x / sourceSprite.rect.width, sourceSprite.pivot.y / sourceSprite.rect.height),
                sourceSprite.pixelsPerUnit,
                0,
                SpriteMeshType.FullRect);
            renderer.sprite = runtimeSprite;

            var scale = renderer.transform.lossyScale;
            var pixelWorldSize = new Vector2(
                Mathf.Abs(scale.x) / sourceSprite.pixelsPerUnit,
                Mathf.Abs(scale.y) / sourceSprite.pixelsPerUnit);
            var pixelWidth = Mathf.RoundToInt(sourceSprite.rect.width);
            var pixelHeight = Mathf.RoundToInt(sourceSprite.rect.height);
            var worldMin = (Vector2)renderer.transform.position - Vector2.Scale(sourceSprite.pivot, pixelWorldSize);
            var worldSize = new Vector2(pixelWidth * pixelWorldSize.x, pixelHeight * pixelWorldSize.y);

            return new DestructibleVisual(
                runtimeTexture,
                runtimeSprite,
                renderer,
                runtimeTexture.GetPixels32(),
                maskPixels,
                Mathf.RoundToInt(sourceSprite.rect.x),
                Mathf.RoundToInt(sourceSprite.rect.y),
                pixelWidth,
                pixelHeight,
                worldMin,
                worldSize,
                surface.IsMoving);
        }

        private RectInt WorldBoundsToCellRegion(Rect worldBounds)
        {
            var min = Grid.WorldToCell(worldBounds.min);
            var max = Grid.WorldToCell(worldBounds.max - Grid.CellSize * 0.001f);
            var minX = Mathf.Clamp(min.x, 0, Grid.Width - 1);
            var maxX = Mathf.Clamp(max.x, 0, Grid.Width - 1);
            var minY = Mathf.Clamp(min.y, 0, Grid.Height - 1);
            var maxY = Mathf.Clamp(max.y, 0, Grid.Height - 1);
            return new RectInt(minX, minY, maxX - minX + 1, maxY - minY + 1);
        }

        private static Rect Encapsulate(Rect first, Rect second)
        {
            var min = Vector2.Min(first.min, second.min);
            var max = Vector2.Max(first.max, second.max);
            return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }

        private static bool Intersects(Rect first, Rect second)
        {
            return first.xMin < second.xMax
                && first.xMax > second.xMin
                && first.yMin < second.yMax
                && first.yMax > second.yMin;
        }

        private static void RasterizeIntoGrid(DestructibleVisual visual, TerrainGrid grid)
        {
            RasterizeIntoGrid(visual, grid, new RectInt(0, 0, grid.Width, grid.Height));
        }

        private static void RasterizeIntoGrid(DestructibleVisual visual, TerrainGrid grid, RectInt cellRegion)
        {
            if (visual.WorldSize.x <= 0f || visual.WorldSize.y <= 0f)
            {
                return;
            }

            var worldMax = visual.WorldMin + visual.WorldSize;
            var minCell = grid.WorldToCell(visual.WorldMin);
            var maxCell = grid.WorldToCell(worldMax - grid.CellSize * 0.001f);
            var minX = Mathf.Clamp(minCell.x, 0, grid.Width - 1);
            var maxX = Mathf.Clamp(maxCell.x, 0, grid.Width - 1);
            var minY = Mathf.Clamp(minCell.y, 0, grid.Height - 1);
            var maxY = Mathf.Clamp(maxCell.y, 0, grid.Height - 1);

            // Only cells the crater could actually have touched need
            // re-scanning; intersect with the visual's own cell bounds.
            minX = Mathf.Max(minX, cellRegion.xMin);
            maxX = Mathf.Min(maxX, cellRegion.xMax - 1);
            minY = Mathf.Max(minY, cellRegion.yMin);
            maxY = Mathf.Min(maxY, cellRegion.yMax - 1);
            if (minX > maxX || minY > maxY)
            {
                return;
            }

            for (var cellY = minY; cellY <= maxY; cellY++)
            {
                var cellWorldMinY = grid.Origin.y + cellY * grid.CellSize.y;
                var cellWorldMaxY = cellWorldMinY + grid.CellSize.y;
                var minPixelY = Mathf.Clamp(Mathf.FloorToInt((cellWorldMinY - visual.WorldMin.y) / visual.WorldSize.y * visual.PixelHeight), 0, visual.PixelHeight - 1);
                var maxPixelY = Mathf.Clamp(Mathf.CeilToInt((cellWorldMaxY - visual.WorldMin.y) / visual.WorldSize.y * visual.PixelHeight) - 1, minPixelY, visual.PixelHeight - 1);
                for (var cellX = minX; cellX <= maxX; cellX++)
                {
                    var cellWorldMinX = grid.Origin.x + cellX * grid.CellSize.x;
                    var cellWorldMaxX = cellWorldMinX + grid.CellSize.x;
                    var minPixelX = Mathf.Clamp(Mathf.FloorToInt((cellWorldMinX - visual.WorldMin.x) / visual.WorldSize.x * visual.PixelWidth), 0, visual.PixelWidth - 1);
                    var maxPixelX = Mathf.Clamp(Mathf.CeilToInt((cellWorldMaxX - visual.WorldMin.x) / visual.WorldSize.x * visual.PixelWidth) - 1, minPixelX, visual.PixelWidth - 1);
                    var isSolid = visual.HasExactMask
                        ? HasSolidMaskPixel(visual, minPixelX, maxPixelX, minPixelY, maxPixelY)
                        : HasSolidSurfaceAbove(visual, minPixelX, maxPixelX, minPixelY);
                    if (isSolid)
                    {
                        grid.SetSolidCell(cellX, cellY, true);
                    }
                }
            }
        }

        private static bool HasSolidMaskPixel(
            DestructibleVisual visual,
            int minX,
            int maxX,
            int minY,
            int maxY)
        {
            for (var y = minY; y <= maxY; y++)
            {
                var row = (visual.TextureRectY + y) * visual.Texture.width + visual.TextureRectX;
                for (var x = minX; x <= maxX; x++)
                {
                    if (visual.MaskPixels[row + x] >= SolidAlphaThreshold)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool HasSolidSurfaceAbove(DestructibleVisual visual, int minX, int maxX, int minY)
        {
            for (var y = minY; y < visual.PixelHeight; y++)
            {
                var row = (visual.TextureRectY + y) * visual.Texture.width + visual.TextureRectX;
                for (var x = minX; x <= maxX; x++)
                {
                    if (visual.Pixels[row + x].a >= SolidAlphaThreshold)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private sealed class DestructibleVisual
        {
            public DestructibleVisual(
                Texture2D texture,
                Sprite sprite,
                SpriteRenderer renderer,
                Color32[] pixels,
                byte[] maskPixels,
                int textureRectX,
                int textureRectY,
                int pixelWidth,
                int pixelHeight,
                Vector2 worldMin,
                Vector2 worldSize,
                bool isMoving)
            {
                Texture = texture;
                Sprite = sprite;
                Pixels = pixels;
                MaskPixels = maskPixels;
                TextureRectX = textureRectX;
                TextureRectY = textureRectY;
                PixelWidth = pixelWidth;
                PixelHeight = pixelHeight;
                WorldMin = worldMin;
                WorldSize = worldSize;
                Renderer = renderer;
                IsMoving = isMoving;
            }

            public Texture2D Texture { get; }
            public Sprite Sprite { get; }
            public Color32[] Pixels { get; }
            public byte[] MaskPixels { get; }
            public bool HasExactMask => MaskPixels != null;
            public int TextureRectX { get; }
            public int TextureRectY { get; }
            public int PixelWidth { get; }
            public int PixelHeight { get; }
            public Vector2 WorldMin { get; private set; }
            public Vector2 WorldSize { get; private set; }
            public Rect WorldBounds => new Rect(WorldMin, WorldSize);
            public bool IsMoving { get; }
            public SpriteRenderer Renderer { get; }

            public void RefreshWorldTransform()
            {
                if (Renderer == null || Sprite == null)
                {
                    return;
                }

                var scale = Renderer.transform.lossyScale;
                var pixelWorldSize = new Vector2(
                    Mathf.Abs(scale.x) / Sprite.pixelsPerUnit,
                    Mathf.Abs(scale.y) / Sprite.pixelsPerUnit);
                WorldMin = (Vector2)Renderer.transform.position - Vector2.Scale(Sprite.pivot, pixelWorldSize);
                WorldSize = new Vector2(PixelWidth * pixelWorldSize.x, PixelHeight * pixelWorldSize.y);
            }

            public void ApplyCrater(Vector2 worldCenter, float worldRadius)
            {
                if (Texture == null || WorldSize.x <= 0f || WorldSize.y <= 0f)
                {
                    return;
                }

                var centerX = (worldCenter.x - WorldMin.x) / WorldSize.x * PixelWidth;
                var centerY = (worldCenter.y - WorldMin.y) / WorldSize.y * PixelHeight;
                var radiusX = worldRadius / WorldSize.x * PixelWidth + VisualCraterEdgePaddingPixels;
                var radiusY = worldRadius / WorldSize.y * PixelHeight + VisualCraterEdgePaddingPixels;
                var minX = Mathf.Max(Mathf.FloorToInt(centerX - radiusX), 0);
                var maxX = Mathf.Min(Mathf.CeilToInt(centerX + radiusX), PixelWidth - 1);
                var minY = Mathf.Max(Mathf.FloorToInt(centerY - radiusY), 0);
                var maxY = Mathf.Min(Mathf.CeilToInt(centerY + radiusY), PixelHeight - 1);
                if (minX > maxX || minY > maxY)
                {
                    return;
                }

                var width = maxX - minX + 1;
                var height = maxY - minY + 1;
                var region = new Color32[width * height];
                for (var y = 0; y < height; y++)
                {
                    var pixelRow = (TextureRectY + minY + y) * Texture.width + TextureRectX + minX;
                    for (var x = 0; x < width; x++)
                    {
                        var normalizedX = (minX + x + 0.5f - centerX) / radiusX;
                        var normalizedY = (minY + y + 0.5f - centerY) / radiusY;
                        var pixel = Pixels[pixelRow + x];
                        if (normalizedX * normalizedX + normalizedY * normalizedY <= 1f)
                        {
                            pixel = new Color32(0, 0, 0, 0);
                            Pixels[pixelRow + x] = pixel;
                            if (HasExactMask)
                            {
                                MaskPixels[pixelRow + x] = 0;
                            }
                        }

                        region[y * width + x] = pixel;
                    }
                }

                Texture.SetPixels32(TextureRectX + minX, TextureRectY + minY, width, height, region);
                Texture.Apply(false, false);
            }

            public void Dispose()
            {
                if (Sprite != null)
                {
                    DestroyRuntimeObject(Sprite);
                }

                if (Texture != null)
                {
                    DestroyRuntimeObject(Texture);
                }
            }

            private static void DestroyRuntimeObject(Object value)
            {
                if (Application.isPlaying)
                {
                    Object.Destroy(value);
                }
                else
                {
                    Object.DestroyImmediate(value);
                }
            }
        }
    }
}
