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

        private BattleTerrain(TerrainGrid grid, List<DestructibleVisual> visuals = null)
        {
            Grid = grid;
            _visuals = visuals ?? new List<DestructibleVisual>();
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

            var grid = CreateGrid(worldBounds, Vector2.one / SpriteCollisionCellsPerUnit);
            var visuals = new List<DestructibleVisual>(renderers.Count);
            for (var i = 0; i < renderers.Count; i++)
            {
                var visual = CreateVisual(renderers[i]);
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
            if (!Grid.CarveCircle(worldCenter, worldRadius, out _))
            {
                return false;
            }

            for (var i = 0; i < _visuals.Count; i++)
            {
                _visuals[i].ApplyCrater(worldCenter, worldRadius);
            }

            return true;
        }

        public void StampSolidRect(Vector2 worldCenter, Vector2 worldSize)
        {
            Grid.StampRect(worldCenter, worldSize, true, out _);
        }

        public void Dispose()
        {
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

        private static DestructibleVisual CreateVisual(SpriteRenderer renderer)
        {
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
                runtimeTexture.GetPixels32(),
                Mathf.RoundToInt(sourceSprite.rect.x),
                Mathf.RoundToInt(sourceSprite.rect.y),
                pixelWidth,
                pixelHeight,
                worldMin,
                worldSize);
        }

        private static void RasterizeIntoGrid(DestructibleVisual visual, TerrainGrid grid)
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
                    if (HasSolidSurfaceAbove(visual, minPixelX, maxPixelX, minPixelY))
                    {
                        grid.SetSolidCell(cellX, cellY, true);
                    }
                }
            }
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
                Color32[] pixels,
                int textureRectX,
                int textureRectY,
                int pixelWidth,
                int pixelHeight,
                Vector2 worldMin,
                Vector2 worldSize)
            {
                Texture = texture;
                Sprite = sprite;
                Pixels = pixels;
                TextureRectX = textureRectX;
                TextureRectY = textureRectY;
                PixelWidth = pixelWidth;
                PixelHeight = pixelHeight;
                WorldMin = worldMin;
                WorldSize = worldSize;
            }

            public Texture2D Texture { get; }
            public Sprite Sprite { get; }
            public Color32[] Pixels { get; }
            public int TextureRectX { get; }
            public int TextureRectY { get; }
            public int PixelWidth { get; }
            public int PixelHeight { get; }
            public Vector2 WorldMin { get; }
            public Vector2 WorldSize { get; }

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
                    Object.Destroy(Sprite);
                }

                if (Texture != null)
                {
                    Object.Destroy(Texture);
                }
            }
        }
    }
}
