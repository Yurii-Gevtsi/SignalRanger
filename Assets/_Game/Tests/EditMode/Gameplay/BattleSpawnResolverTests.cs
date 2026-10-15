using Game.Gameplay.Battle.Arenas;
using Game.Gameplay.Battle.Motion;
using Game.Gameplay.Battle.Terrain;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class BattleSpawnResolverTests
    {
        [Test]
        public void Resolve_WhenRayStartsInsideSolid_UsesTheUpperSurface()
        {
            var grid = new TerrainGrid(new Vector2(-5f, -5f), Vector2.one / 32f, 320, 320);
            grid.StampRect(new Vector2(0f, 3.5f), new Vector2(4f, 1f), true, out _);
            var profile = new UnitMotionProfile(
                halfWidth: 0.34f,
                halfHeight: 0.475f,
                crawlHeight: 0.3f,
                walkStepHeight: 0.42f,
                walkSpeed: 2.4f);
            var resolver = new BattleSpawnResolver(grid, profile, -6f, 3.5f, 8f, 0.5f);

            Assert.IsTrue(resolver.TryResolve(Vector2.zero, out var resolved));
            Assert.That(resolved.y, Is.GreaterThan(4.4f));
        }
    }
}
