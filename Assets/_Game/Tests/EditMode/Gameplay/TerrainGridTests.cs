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
    }
}
