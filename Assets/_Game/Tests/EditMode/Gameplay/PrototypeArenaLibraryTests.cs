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
                Assert.That(arena.TerrainBlocks, Is.Not.Empty);
            }
        }
    }
}
