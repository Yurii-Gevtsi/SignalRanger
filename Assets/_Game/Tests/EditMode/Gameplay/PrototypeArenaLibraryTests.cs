using System.Linq;
using Game.Gameplay.Battle.Arenas;
using NUnit.Framework;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class PrototypeArenaLibraryTests
    {
        [Test]
        public void CreateAll_ReturnsPlayablePrototypeArenas()
        {
            var arenas = PrototypeArenaLibrary.CreateAll();

            Assert.That(arenas, Has.Length.GreaterThanOrEqualTo(1));
            foreach (var arena in arenas)
            {
                Assert.That(arena.ArenaId, Is.Not.Empty);
                Assert.That(arena.DisplayName, Is.Not.Empty);
                Assert.That(arena.CameraSize, Is.GreaterThan(0f));
                Assert.That(arena.PlayerSpawn, Is.Not.EqualTo(arena.OpponentSpawn));
                Assert.That(arena.TerrainBlocks.Count + arena.SpriteSurfaces.Count, Is.GreaterThan(0));
            }
        }

        [Test]
        public void DesertSciFi_UsesTheSixFragmentsFromTheMarkedLayout()
        {
            var arenas = PrototypeArenaLibrary.CreateAll();
            var arena = System.Array.Find(arenas, item => item.ArenaId == "arena.desert-sci-fi");

            Assert.IsNotNull(arena);
            Assert.That(arena.SpriteSurfaces.Count, Is.EqualTo(6));
            Assert.That(arena.TerrainBlocks, Is.Empty);
            Assert.That(arena.SpriteSurfaces.Select(surface => surface.VisualResourcePath), Is.Unique);
            Assert.That(arena.SpriteSurfaces.Select(surface => surface.MaskResourcePath), Is.Unique);
            Assert.That(arena.CameraSize, Is.EqualTo(9.25f));
            Assert.That(arena.CameraPosition.y, Is.EqualTo(-2f));
            Assert.That(arena.SpriteSurfaces[0].Scale, Is.EqualTo(2.5f));
        }
    }
}
