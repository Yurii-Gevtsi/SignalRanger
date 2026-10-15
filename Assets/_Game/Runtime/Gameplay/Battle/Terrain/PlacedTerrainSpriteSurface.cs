using UnityEngine;

namespace Game.Gameplay.Battle.Terrain
{
    /// <summary>
    /// Stationary terrain added during combat. Its mutable sprite pixels also own
    /// collision, so rotation, carving and later arena refreshes use one shape.
    /// </summary>
    public sealed class PlacedTerrainSpriteSurface
    {
        private const byte SolidAlpha = 96;
        private const int MaximumTextureWidth = 512;
        private readonly Color32[] _pixels;
        private readonly Texture2D _texture;
        private readonly Vector2 _axisX;
        private readonly Vector2 _axisY;

        public PlacedTerrainSpriteSurface(Texture2D source, Vector2 center, Vector2 size, float rotationDegrees)
        {
            SourceTexture = source;
            Center = center;
            Size = size;
            RotationDegrees = rotationDegrees;
            var radians = rotationDegrees * Mathf.Deg2Rad;
            _axisX = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
            _axisY = new Vector2(-_axisX.y, _axisX.x);
            var extent = (new Vector2(Mathf.Abs(_axisX.x), Mathf.Abs(_axisX.y)) * size.x
                + new Vector2(Mathf.Abs(_axisY.x), Mathf.Abs(_axisY.y)) * size.y) * 0.5f;
            WorldBounds = new Rect(center - extent, extent * 2f);
            var width = Mathf.Min(MaximumTextureWidth, source.width);
            var height = Mathf.Max(1, Mathf.RoundToInt(source.height * (float)width / source.width));
            _pixels = new Color32[width * height];
            var sourcePixels = source.GetPixels32();
            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    _pixels[y * width + x] = sourcePixels[Mathf.Min(source.height - 1,
                        Mathf.FloorToInt((y + 0.5f) * source.height / height)) * source.width
                        + Mathf.Min(source.width - 1, Mathf.FloorToInt((x + 0.5f) * source.width / width))];
            _texture = new Texture2D(width, height, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Bilinear, wrapMode = TextureWrapMode.Clamp,
                name = "Placed Terrain Texture"
            };
            _texture.SetPixels32(_pixels);
            _texture.Apply(false, false);
            Sprite = Sprite.Create(_texture, new Rect(0, 0, width, height), Vector2.one * 0.5f,
                100f, 0, SpriteMeshType.FullRect);
            Sprite.name = "Placed Terrain Sprite";
        }

        public Texture2D SourceTexture { get; }
        public Sprite Sprite { get; }
        public Vector2 Center { get; }
        public Vector2 Size { get; }
        public float RotationDegrees { get; }
        public Rect WorldBounds { get; }

        public bool IsSolidWorld(Vector2 point)
        {
            var offset = point - Center;
            var u = Vector2.Dot(offset, _axisX) / Size.x + 0.5f;
            var v = Vector2.Dot(offset, _axisY) / Size.y + 0.5f;
            if (u < 0f || u >= 1f || v < 0f || v >= 1f) return false;
            return _pixels[Mathf.FloorToInt(v * _texture.height) * _texture.width
                + Mathf.FloorToInt(u * _texture.width)].a >= SolidAlpha;
        }

        public void Rasterize(TerrainGrid grid, RectInt region)
        {
            var min = grid.WorldToCell(WorldBounds.min);
            var max = grid.WorldToCell(WorldBounds.max);
            for (var y = Mathf.Max(0, region.yMin, min.y); y <= Mathf.Min(grid.Height - 1, region.yMax - 1, max.y); y++)
                for (var x = Mathf.Max(0, region.xMin, min.x); x <= Mathf.Min(grid.Width - 1, region.xMax - 1, max.x); x++)
                    if (IsSolidWorld(grid.CellToWorld(x, y)))
                    {
                        grid.SetSolidCell(x, y, true);
                        grid.SetMaterialCell(x, y, TerrainMaterial.Titanium);
                    }
        }

        public void ApplyCrater(Vector2 center, float radius)
        {
            if (!WorldBounds.Overlaps(new Rect(center - Vector2.one * radius, Vector2.one * radius * 2f))) return;
            var radiusSquared = radius * radius;
            var changed = false;
            for (var y = 0; y < _texture.height; y++)
                for (var x = 0; x < _texture.width; x++)
                {
                    var index = y * _texture.width + x;
                    if (_pixels[index].a == 0) continue;
                    var point = Center + _axisX * (((x + 0.5f) / _texture.width - 0.5f) * Size.x)
                        + _axisY * (((y + 0.5f) / _texture.height - 0.5f) * Size.y);
                    if ((point - center).sqrMagnitude <= radiusSquared)
                    {
                        _pixels[index] = new Color32(0, 0, 0, 0);
                        changed = true;
                    }
                }
            if (!changed) return;
            _texture.SetPixels32(_pixels);
            _pendingUpload = true;
        }

        private bool _pendingUpload;

        /// <summary>Uploads the craters cut since the last frame in one go.</summary>
        public void FlushUpload()
        {
            if (!_pendingUpload || _texture == null)
            {
                return;
            }

            _pendingUpload = false;
            _texture.Apply(false, false);
        }

        public void Dispose()
        {
            if (Application.isPlaying)
            {
                Object.Destroy(Sprite);
                Object.Destroy(_texture);
            }
            else
            {
                Object.DestroyImmediate(Sprite);
                Object.DestroyImmediate(_texture);
            }
        }
    }
}
