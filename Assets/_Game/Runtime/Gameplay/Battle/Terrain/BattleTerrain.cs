using UnityEngine;

namespace Game.Gameplay.Battle.Terrain
{
    /// <summary>
    /// Owns the arena solidity grid and keeps the destructible visual layer
    /// (a runtime copy of the terrain texture) in sync with it. Craters and
    /// cover stamps go through here so gameplay collisions and visuals can
    /// never disagree.
    /// </summary>
    public sealed class BattleTerrain
    {
        private const byte SolidAlphaThreshold = 96;
        private const float ProceduralCellsPerUnit = 32f;

        private readonly Texture2D _texture;
        private readonly Sprite _sprite;
        private readonly Color32[] _pixels;
        private readonly int _textureRectX;
        private readonly int _textureRectY;
        private readonly int _spritePixelWidth;
        private readonly int _spritePixelHeight;
        private readonly int _spriteCellOffsetX;
        private readonly int _spriteCellOffsetY;

        private BattleTerrain(TerrainGrid grid)
        {
            Grid = grid;
        }

        private BattleTerrain(
            TerrainGrid grid,
            Texture2D texture,
            Sprite sprite,
            Color32[] pixels,
            int textureRectX,
            int textureRectY,
            int spritePixelWidth,
            int spritePixelHeight,
            int spriteCellOffsetX,
            int spriteCellOffsetY)
        {
            Grid = grid;
            _texture = texture;
            _sprite = sprite;
            _pixels = pixels;
            _textureRectX = textureRectX;
            _textureRectY = textureRectY;
            _spritePixelWidth = spritePixelWidth;
            _spritePixelHeight = spritePixelHeight;
            _spriteCellOffsetX = spriteCellOffsetX;
            _spriteCellOffsetY = spriteCellOffsetY;
        }

        public TerrainGrid Grid { get; }

        public bool HasDestructibleVisual => _texture != null;

        /// <summary>Builds a grid-only terrain for arenas made of procedural blocks.</summary>
        public static BattleTerrain CreateProcedural(Rect worldBounds)
        {
            var cellSize = new Vector2(1f / ProceduralCellsPerUnit, 1f / ProceduralCellsPerUnit);
            var width = Mathf.Max(1, Mathf.CeilToInt(worldBounds.width / cellSize.x));
            var height = Mathf.Max(1, Mathf.CeilToInt(worldBounds.height / cellSize.y));
            return new BattleTerrain(new TerrainGrid(worldBounds.min, cellSize, width, height));
        }

        /// <summary>
        /// Builds terrain around a destructible sprite layer: grid cells align
        /// 1:1 with sprite pixels and extend to cover the whole arena bounds.
        /// The renderer is switched to a runtime copy of the texture so craters
        /// can erase pixels without touching the imported asset.
        /// </summary>
        public static BattleTerrain CreateFromSprite(SpriteRenderer renderer, Sprite sourceSprite, Rect worldBounds)
        {
            if (renderer == null || sourceSprite == null || sourceSprite.texture == null)
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
            var runtimeTexture = new Texture2D(sourceTexture.width, sourceTexture.height, TextureFormat.RGBA32, false);
            runtimeTexture.filterMode = sourceTexture.filterMode;
            runtimeTexture.wrapMode = TextureWrapMode.Clamp;
            runtimeTexture.SetPixels32(sourcePixels);
            runtimeTexture.Apply();

            var runtimeSprite = Sprite.Create(
                runtimeTexture,
                sourceSprite.rect,
                new Vector2(sourceSprite.pivot.x / sourceSprite.rect.width, sourceSprite.pivot.y / sourceSprite.rect.height),
                sourceSprite.pixelsPerUnit,
                0,
                SpriteMeshType.FullRect);
            renderer.sprite = runtimeSprite;

            var transform = renderer.transform;
            var scale = transform.lossyScale;
            var cellSize = new Vector2(
                Mathf.Abs(scale.x) / sourceSprite.pixelsPerUnit,
                Mathf.Abs(scale.y) / sourceSprite.pixelsPerUnit);
            var spritePixelWidth = Mathf.RoundToInt(sourceSprite.rect.width);
            var spritePixelHeight = Mathf.RoundToInt(sourceSprite.rect.height);
            var spriteWorldMin = (Vector2)transform.position - Vector2.Scale(sourceSprite.pivot, cellSize);

            // Grid origin extends past the sprite so covers/blocks outside the
            // texture still land on the same mask; keep it pixel-aligned so the
            // sprite maps to whole cells.
            var extraLeft = Mathf.Max(0, Mathf.CeilToInt((spriteWorldMin.x - worldBounds.xMin) / cellSize.x));
            var extraBottom = Mathf.Max(0, Mathf.CeilToInt((spriteWorldMin.y - worldBounds.yMin) / cellSize.y));
            var extraRight = Mathf.Max(0, Mathf.CeilToInt((worldBounds.xMax - (spriteWorldMin.x + spritePixelWidth * cellSize.x)) / cellSize.x));
            var extraTop = Mathf.Max(0, Mathf.CeilToInt((worldBounds.yMax - (spriteWorldMin.y + spritePixelHeight * cellSize.y)) / cellSize.y));
            var origin = spriteWorldMin - new Vector2(extraLeft * cellSize.x, extraBottom * cellSize.y);
            var grid = new TerrainGrid(
                origin,
                cellSize,
                extraLeft + spritePixelWidth + extraRight,
                extraBottom + spritePixelHeight + extraTop);

            var rectX = Mathf.RoundToInt(sourceSprite.rect.x);
            var rectY = Mathf.RoundToInt(sourceSprite.rect.y);
            var runtimePixels = runtimeTexture.GetPixels32();
            for (var y = 0; y < spritePixelHeight; y++)
            {
                var pixelRow = (rectY + y) * sourceTexture.width + rectX;
                for (var x = 0; x < spritePixelWidth; x++)
                {
                    if (runtimePixels[pixelRow + x].a >= SolidAlphaThreshold)
                    {
                        grid.SetSolidCell(extraLeft + x, extraBottom + y, true);
                    }
                }
            }

            return new BattleTerrain(
                grid,
                runtimeTexture,
                runtimeSprite,
                runtimePixels,
                rectX,
                rectY,
                spritePixelWidth,
                spritePixelHeight,
                extraLeft,
                extraBottom);
        }

        /// <summary>
        /// Carves a crater in the mask and erases the matching pixels of the
        /// destructible layer. Returns true when any ground was removed.
        /// </summary>
        public bool ApplyCrater(Vector2 worldCenter, float worldRadius)
        {
            if (!Grid.CarveCircle(worldCenter, worldRadius, out var dirtyCells))
            {
                return false;
            }

            SyncTextureRegion(dirtyCells);
            return true;
        }

        /// <summary>Adds a solid rectangle (deployed cover) to the mask.</summary>
        public void StampSolidRect(Vector2 worldCenter, Vector2 worldSize)
        {
            Grid.StampRect(worldCenter, worldSize, true, out _);
        }

        public void Dispose()
        {
            if (_sprite != null)
            {
                Object.Destroy(_sprite);
            }

            if (_texture != null)
            {
                Object.Destroy(_texture);
            }
        }

        private void SyncTextureRegion(RectInt dirtyCells)
        {
            if (_texture == null)
            {
                return;
            }

            // Intersect the dirty cell region with the sprite's pixel area.
            var minPixelX = Mathf.Max(dirtyCells.xMin - _spriteCellOffsetX, 0);
            var maxPixelX = Mathf.Min(dirtyCells.xMax - 1 - _spriteCellOffsetX, _spritePixelWidth - 1);
            var minPixelY = Mathf.Max(dirtyCells.yMin - _spriteCellOffsetY, 0);
            var maxPixelY = Mathf.Min(dirtyCells.yMax - 1 - _spriteCellOffsetY, _spritePixelHeight - 1);
            if (minPixelX > maxPixelX || minPixelY > maxPixelY)
            {
                return;
            }

            var regionWidth = maxPixelX - minPixelX + 1;
            var regionHeight = maxPixelY - minPixelY + 1;
            var region = new Color32[regionWidth * regionHeight];
            for (var y = 0; y < regionHeight; y++)
            {
                var pixelRow = (_textureRectY + minPixelY + y) * _texture.width + _textureRectX + minPixelX;
                var cellRow = minPixelY + y + _spriteCellOffsetY;
                for (var x = 0; x < regionWidth; x++)
                {
                    var pixel = _pixels[pixelRow + x];
                    if (pixel.a >= SolidAlphaThreshold && !Grid.IsSolidCell(minPixelX + x + _spriteCellOffsetX, cellRow))
                    {
                        pixel.a = 0;
                        _pixels[pixelRow + x] = pixel;
                    }

                    region[y * regionWidth + x] = pixel;
                }
            }

            _texture.SetPixels32(_textureRectX + minPixelX, _textureRectY + minPixelY, regionWidth, regionHeight, region);
            _texture.Apply();
        }
    }
}
