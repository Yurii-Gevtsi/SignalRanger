using Game.Gameplay.Battle.Terrain;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class TerrainGridTests
    {
        private static TerrainGrid CreateFlatGround(out float surfaceY)
        {
            // 10 x 10 world units, 32 cells per unit, ground fills the lower half.
            var grid = new TerrainGrid(new Vector2(-5f, -5f), new Vector2(1f / 32f, 1f / 32f), 320, 320);
            grid.StampRect(new Vector2(0f, -2.5f), new Vector2(10f, 5f), true, out _);
            surfaceY = 0f;
            return grid;
        }

        [Test]
        public void StampRect_MarksCellsSolid()
        {
            var grid = CreateFlatGround(out _);

            Assert.IsTrue(grid.IsSolidWorld(new Vector2(0f, -1f)));
            Assert.IsFalse(grid.IsSolidWorld(new Vector2(0f, 1f)));
        }

        [Test]
        public void IsSolidWorld_OutsideBounds_IsNotSolid()
        {
            var grid = CreateFlatGround(out _);

            Assert.IsFalse(grid.IsSolidWorld(new Vector2(100f, 0f)));
            Assert.IsFalse(grid.IsSolidWorld(new Vector2(0f, -100f)));
        }

        [Test]
        public void CarveCircle_RemovesGroundAndReportsDirtyRegion()
        {
            var grid = CreateFlatGround(out _);

            var changed = grid.CarveCircle(new Vector2(0f, 0f), 1f, out var dirty);

            Assert.IsTrue(changed);
            Assert.Greater(dirty.width, 0);
            Assert.IsFalse(grid.IsSolidWorld(new Vector2(0f, -0.5f)));
            Assert.IsTrue(grid.IsSolidWorld(new Vector2(0f, -1.5f)), "below the crater stays solid");
            Assert.IsTrue(grid.IsSolidWorld(new Vector2(2f, -0.5f)), "outside the crater stays solid");
        }

        [Test]
        public void CarveCircle_OnEmptyArea_ReportsNoChange()
        {
            var grid = CreateFlatGround(out _);

            Assert.IsFalse(grid.CarveCircle(new Vector2(0f, 3f), 0.5f, out _));
        }

        [Test]
        public void TryFindSurfaceBelow_FindsGroundTop()
        {
            var grid = CreateFlatGround(out var surfaceY);

            Assert.IsTrue(grid.TryFindSurfaceBelow(0f, 3f, 10f, out var foundY));
            Assert.AreEqual(surfaceY, foundY, 1f / 16f);
        }

        [Test]
        public void TryFindSurfaceBelow_FindsCraterFloor_UnderOverhang()
        {
            var grid = CreateFlatGround(out _);
            grid.CarveCircle(new Vector2(0f, -1f), 1.2f, out _);

            // Scan starting inside the crater void: must find the crater floor,
            // not the original top surface.
            Assert.IsTrue(grid.TryFindSurfaceBelow(0f, -0.5f, 10f, out var floorY));
            Assert.Less(floorY, -0.1f);
        }

        [Test]
        public void Raycast_HitsGroundBetweenPoints()
        {
            var grid = CreateFlatGround(out _);

            Assert.IsTrue(grid.Raycast(new Vector2(-2f, 2f), new Vector2(-2f, -2f), out var hit));
            Assert.AreEqual(0f, hit.y, 0.1f);
            Assert.IsFalse(grid.Raycast(new Vector2(-2f, 2f), new Vector2(2f, 2f), out _), "clear path above ground");
        }

        [Test]
        public void Raycast_DoesNotTunnelThroughThinWall()
        {
            var grid = CreateFlatGround(out _);
            // One-cell-wide wall above the ground.
            grid.StampRect(new Vector2(0f, 1f), new Vector2(1f / 32f, 2f), true, out _);

            Assert.IsTrue(grid.Raycast(new Vector2(-3f, 1f), new Vector2(3f, 1.4f), out _));
        }

        [Test]
        public void SpriteTerrain_UsesStableCollisionDensityAndClearsCraterFringe()
        {
            const int textureSize = 128;
            var texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
            var sourceSprite = default(Sprite);
            var runtimeSprite = default(Sprite);
            var runtimeTexture = default(Texture2D);
            var terrainObject = new GameObject("Sprite Terrain Test");
            try
            {
                var pixels = new Color32[textureSize * textureSize];
                for (var index = 0; index < pixels.Length; index++)
                {
                    pixels[index] = new Color32(210, 125, 48, 255);
                }

                // A low-alpha edge pixel represents the antialiased color
                // fringe that used to remain visible after a crater was cut.
                pixels[64 * textureSize + 81] = new Color32(210, 125, 48, 50);
                texture.SetPixels32(pixels);
                texture.Apply(false, false);

                sourceSprite = Sprite.Create(
                    texture,
                    new Rect(0f, 0f, textureSize, textureSize),
                    new Vector2(0.5f, 0.5f),
                    64f,
                    0,
                    SpriteMeshType.FullRect);
                var renderer = terrainObject.AddComponent<SpriteRenderer>();
                renderer.sprite = sourceSprite;
                renderer.transform.localScale = Vector3.one * 2f;

                var terrain = BattleTerrain.CreateFromSprite(renderer, sourceSprite, new Rect(-2f, -2f, 4f, 4f));
                Assert.IsNotNull(terrain);
                Assert.That(terrain.Grid.CellSize.x, Is.EqualTo(1f / 56f).Within(0.00001f));
                Assert.That(terrain.Grid.CellSize.y, Is.EqualTo(1f / 56f).Within(0.00001f));

                runtimeSprite = renderer.sprite;
                runtimeTexture = runtimeSprite.texture;
                Assert.IsTrue(terrain.ApplyCrater(Vector2.zero, 0.5f));

                var clearedFringe = runtimeTexture.GetPixel(81, 64);
                Assert.That(clearedFringe.a, Is.EqualTo(0f).Within(0.001f));
                Assert.That(clearedFringe.r, Is.EqualTo(0f).Within(0.001f));
            }
            finally
            {
                Object.DestroyImmediate(terrainObject);
                if (runtimeSprite != null)
                {
                    Object.DestroyImmediate(runtimeSprite);
                }

                if (runtimeTexture != null)
                {
                    Object.DestroyImmediate(runtimeTexture);
                }

                if (sourceSprite != null)
                {
                    Object.DestroyImmediate(sourceSprite);
                }

                Object.DestroyImmediate(texture);
            }
        }

        [Test]
        public void MultiSpriteTerrain_CarvesEverySurfaceCrossedByOneCrater()
        {
            const int textureSize = 32;
            var leftTexture = CreateSolidTexture(textureSize);
            var rightTexture = CreateSolidTexture(textureSize);
            var leftSource = Sprite.Create(leftTexture, new Rect(0f, 0f, textureSize, textureSize), new Vector2(0.5f, 0.5f), textureSize);
            var rightSource = Sprite.Create(rightTexture, new Rect(0f, 0f, textureSize, textureSize), new Vector2(0.5f, 0.5f), textureSize);
            var leftObject = new GameObject("Left Destructible Surface");
            var rightObject = new GameObject("Right Destructible Surface");
            Sprite leftRuntime = null;
            Sprite rightRuntime = null;
            Texture2D leftRuntimeTexture = null;
            Texture2D rightRuntimeTexture = null;
            try
            {
                var leftRenderer = leftObject.AddComponent<SpriteRenderer>();
                var rightRenderer = rightObject.AddComponent<SpriteRenderer>();
                leftRenderer.sprite = leftSource;
                rightRenderer.sprite = rightSource;
                leftObject.transform.position = new Vector3(-0.5f, 0f, 0f);
                rightObject.transform.position = new Vector3(0.5f, 0f, 0f);

                var terrain = BattleTerrain.CreateFromSprites(
                    new[] { leftRenderer, rightRenderer },
                    new Rect(-2f, -1f, 4f, 2f));
                Assert.IsNotNull(terrain);

                leftRuntime = leftRenderer.sprite;
                rightRuntime = rightRenderer.sprite;
                leftRuntimeTexture = leftRuntime.texture;
                rightRuntimeTexture = rightRuntime.texture;

                Assert.IsTrue(terrain.ApplyCrater(Vector2.zero, 0.25f));
                Assert.AreEqual(0, leftRuntimeTexture.GetPixel(textureSize - 1, textureSize / 2).a);
                Assert.AreEqual(0, rightRuntimeTexture.GetPixel(0, textureSize / 2).a);
                Assert.IsFalse(terrain.Grid.IsSolidWorld(Vector2.zero));
            }
            finally
            {
                Object.DestroyImmediate(leftObject);
                Object.DestroyImmediate(rightObject);
                if (leftRuntime != null) Object.DestroyImmediate(leftRuntime);
                if (rightRuntime != null) Object.DestroyImmediate(rightRuntime);
                if (leftRuntimeTexture != null) Object.DestroyImmediate(leftRuntimeTexture);
                if (rightRuntimeTexture != null) Object.DestroyImmediate(rightRuntimeTexture);
                Object.DestroyImmediate(leftSource);
                Object.DestroyImmediate(rightSource);
                Object.DestroyImmediate(leftTexture);
                Object.DestroyImmediate(rightTexture);
            }
        }

        [Test]
        public void SpriteTerrain_FillsSolidBelowVisibleSurface()
        {
            const int textureSize = 32;
            var texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
            var pixels = new Color32[textureSize * textureSize];
            for (var x = 8; x < 24; x++)
            {
                pixels[24 * textureSize + x] = new Color32(255, 255, 255, 255);
            }

            texture.SetPixels32(pixels);
            texture.Apply(false, false);

            var source = Sprite.Create(texture, new Rect(0f, 0f, textureSize, textureSize), new Vector2(0.5f, 0.5f), textureSize);
            var terrainObject = new GameObject("Surface Fill Terrain");
            Sprite runtimeSprite = null;
            Texture2D runtimeTexture = null;
            try
            {
                var renderer = terrainObject.AddComponent<SpriteRenderer>();
                renderer.sprite = source;

                var terrain = BattleTerrain.CreateFromSprite(renderer, source, new Rect(-1f, -1f, 2f, 2f));
                Assert.IsNotNull(terrain);
                Assert.IsFalse(terrain.Grid.IsSolidWorld(new Vector2(0f, 0.35f)), "space above the visible surface stays empty");
                Assert.IsTrue(terrain.Grid.IsSolidWorld(new Vector2(0f, 0f)), "transparent pixels below a visible surface are filled as solid");
                Assert.IsFalse(terrain.Grid.IsSolidWorld(new Vector2(-0.4f, 0f)), "columns without a visible surface stay empty");

                runtimeSprite = renderer.sprite;
                runtimeTexture = runtimeSprite.texture;
            }
            finally
            {
                Object.DestroyImmediate(terrainObject);
                if (runtimeSprite != null) Object.DestroyImmediate(runtimeSprite);
                if (runtimeTexture != null) Object.DestroyImmediate(runtimeTexture);
                Object.DestroyImmediate(source);
                Object.DestroyImmediate(texture);
            }
        }

        private static Texture2D CreateSolidTexture(int size)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var pixels = new Color32[size * size];
            for (var i = 0; i < pixels.Length; i++)
            {
                pixels[i] = new Color32(255, 255, 255, 255);
            }

            texture.SetPixels32(pixels);
            texture.Apply(false, false);
            return texture;
        }
    }
}
