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
        public void TrainingMeadow_UsesOneImportedReconstructedDestructibleSurface()
        {
            var arena = System.Array.Find(PrototypeArenaLibrary.CreateAll(), item => item.ArenaId == "arena.training-meadow");

            Assert.IsNotNull(arena);
            Assert.That(arena.TerrainBlocks, Is.Empty);
            Assert.That(arena.SpriteSurfaces.Count, Is.EqualTo(1));
            Assert.That(arena.SpriteSurfaces.Single().VisualResourcePath,
                Is.EqualTo("Visuals/Arenas/Training/training_reconstructed_visual"));
            Assert.That(arena.SpriteSurfaces.Single().MaskResourcePath,
                Is.EqualTo("Visuals/Arenas/Training/training_reconstructed_mask"));
            Assert.That(arena.SpriteSurfaces.Select(surface => surface.Scale), Is.All.EqualTo(2.8f).Within(0.001f));
            Assert.That(arena.PlayerSpawn, Is.EqualTo(new UnityEngine.Vector2(-6.3f, -0.65f)));
            Assert.That(arena.OpponentSpawn, Is.EqualTo(new UnityEngine.Vector2(6.2f, 2.1f)));
        }

        [Test]
        public void BonesArena_UsesTheImportedReconstructedSurface()
        {
            var arena = System.Array.Find(PrototypeArenaLibrary.CreateAll(), item => item.ArenaId == "arena.bones");

            Assert.IsNotNull(arena);
            Assert.That(arena.SpriteSurfaces.Count, Is.EqualTo(1));
            Assert.That(arena.SpriteSurfaces[0].VisualResourcePath, Is.EqualTo("Visuals/Arenas/Bones/bones_reconstructed_visual"));
            Assert.That(arena.SpriteSurfaces[0].MaskResourcePath, Is.EqualTo("Visuals/Arenas/Bones/bones_reconstructed_mask"));
            Assert.That(arena.SpriteSurfaces[0].Scale, Is.EqualTo(3.1f));
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
            Assert.That(arena.PlayerSpawn, Is.EqualTo(new UnityEngine.Vector2(-10f, 2.7f)));
            Assert.That(arena.OpponentSpawn, Is.EqualTo(new UnityEngine.Vector2(12f, -5.8f)));
            Assert.That(arena.SpriteSurfaces[0].Scale, Is.EqualTo(2.5f));

            var floating = arena.SpriteSurfaces.Single(surface => surface.InstanceId == "floating_large");
            Assert.IsTrue(floating.HasHorizontalMotion);
            Assert.That(floating.Scale, Is.EqualTo(1.75f));
            Assert.That(floating.Position.y, Is.EqualTo(2.666015625f));
            Assert.That(floating.HorizontalMotionRange, Is.EqualTo(3.5f));
            Assert.That(floating.HorizontalMotionSpeed, Is.EqualTo(0.45f));
        }

        [Test]
        public void CityArena_CompensatesForItsTwoTimesProductionFragments()
        {
            var arena = System.Array.Find(PrototypeArenaLibrary.CreateAll(), item => item.ArenaId == "arena.city");

            Assert.IsNotNull(arena);
            Assert.That(arena.SpriteSurfaces.Count, Is.EqualTo(5));
            Assert.That(arena.SpriteSurfaces.Select(surface => surface.Scale), Is.All.EqualTo(2.2425f));
        }

        [Test]
        public void LagunaArena_UsesTheReconstructedFullCanvasAtProductionScale()
        {
            var arena = System.Array.Find(PrototypeArenaLibrary.CreateAll(), item => item.ArenaId == "arena.laguna");

            Assert.IsNotNull(arena);
            Assert.That(arena.SpriteSurfaces.Count, Is.EqualTo(1));
            var surface = arena.SpriteSurfaces[0];
            Assert.That(surface.InstanceId, Is.EqualTo("reconstructed"));
            Assert.That(surface.VisualResourcePath, Is.EqualTo("Visuals/Arenas/Laguna/laguna_reconstructed_visual"));
            Assert.That(surface.MaskResourcePath, Is.EqualTo("Visuals/Arenas/Laguna/laguna_reconstructed_mask"));
            Assert.That(surface.Position, Is.EqualTo(new UnityEngine.Vector2(0f, -0.45f)));
            Assert.That(surface.Scale, Is.EqualTo(2.25f));
        }

        [Test]
        public void ForestArena_UsesTheReconstructedFullCanvasAtProductionScale()
        {
            var arena = System.Array.Find(PrototypeArenaLibrary.CreateAll(), item => item.ArenaId == "arena.forest");

            Assert.IsNotNull(arena);
            Assert.That(arena.SpriteSurfaces.Count, Is.EqualTo(1));
            var surface = arena.SpriteSurfaces[0];
            Assert.That(surface.InstanceId, Is.EqualTo("reconstructed"));
            Assert.That(surface.VisualResourcePath, Is.EqualTo("Visuals/Arenas/Forest/forest_reconstructed_visual"));
            Assert.That(surface.MaskResourcePath, Is.EqualTo("Visuals/Arenas/Forest/forest_reconstructed_mask"));
            Assert.That(surface.Position, Is.EqualTo(UnityEngine.Vector2.zero));
            Assert.That(surface.Scale, Is.EqualTo(2.68125f));
        }

        [Test]
        public void GribArena_UsesTheReconstructedFullCanvasAtProductionScale()
        {
            var arena = System.Array.Find(PrototypeArenaLibrary.CreateAll(), item => item.ArenaId == "arena.grib");

            Assert.IsNotNull(arena);
            Assert.That(arena.SpriteSurfaces.Count, Is.EqualTo(1));
            var surface = arena.SpriteSurfaces[0];
            Assert.That(surface.InstanceId, Is.EqualTo("reconstructed"));
            Assert.That(surface.VisualResourcePath, Is.EqualTo("Visuals/Arenas/Grib/grib_reconstructed_visual"));
            Assert.That(surface.MaskResourcePath, Is.EqualTo("Visuals/Arenas/Grib/grib_reconstructed_mask"));
            Assert.That(surface.Position, Is.EqualTo(UnityEngine.Vector2.zero));
            Assert.That(surface.Scale, Is.EqualTo(2.7f));
        }

        [Test]
        public void KrakenArena_UsesTheReconstructedFullCanvasAtProductionScale()
        {
            var arena = System.Array.Find(PrototypeArenaLibrary.CreateAll(), item => item.ArenaId == "arena.kraken");

            Assert.IsNotNull(arena);
            Assert.That(arena.SpriteSurfaces.Count, Is.EqualTo(1));
            var surface = arena.SpriteSurfaces[0];
            Assert.That(surface.InstanceId, Is.EqualTo("reconstructed"));
            Assert.That(surface.VisualResourcePath, Is.EqualTo("Visuals/Arenas/Kraken/kraken_reconstructed_visual"));
            Assert.That(surface.MaskResourcePath, Is.EqualTo("Visuals/Arenas/Kraken/kraken_reconstructed_mask"));
            Assert.That(surface.Position, Is.EqualTo(UnityEngine.Vector2.zero));
            Assert.That(surface.Scale, Is.EqualTo(2.25f));
        }

        [Test]
        public void MonumentArena_UsesTheReconstructedFullCanvasAtProductionScale()
        {
            var arena = System.Array.Find(PrototypeArenaLibrary.CreateAll(), item => item.ArenaId == "arena.monument");

            Assert.IsNotNull(arena);
            Assert.That(arena.SpriteSurfaces.Count, Is.EqualTo(1));
            var surface = arena.SpriteSurfaces[0];
            Assert.That(surface.InstanceId, Is.EqualTo("reconstructed"));
            Assert.That(surface.VisualResourcePath, Is.EqualTo("Visuals/Arenas/Monument/monument_reconstructed_visual"));
            Assert.That(surface.MaskResourcePath, Is.EqualTo("Visuals/Arenas/Monument/monument_reconstructed_mask"));
            Assert.That(surface.Position, Is.EqualTo(UnityEngine.Vector2.zero));
            Assert.That(surface.Scale, Is.EqualTo(2.25f));
        }

        [Test]
        public void RobotsArena_UsesTheReconstructedFullCanvasAtProductionScale()
        {
            var arena = System.Array.Find(PrototypeArenaLibrary.CreateAll(), item => item.ArenaId == "arena.robots");

            Assert.IsNotNull(arena);
            Assert.That(arena.SpriteSurfaces.Count, Is.EqualTo(1));
            var surface = arena.SpriteSurfaces[0];
            Assert.That(surface.InstanceId, Is.EqualTo("reconstructed"));
            Assert.That(surface.VisualResourcePath, Is.EqualTo("Visuals/Arenas/Robots/robots_reconstructed_visual"));
            Assert.That(surface.MaskResourcePath, Is.EqualTo("Visuals/Arenas/Robots/robots_reconstructed_mask"));
            Assert.That(surface.Position, Is.EqualTo(UnityEngine.Vector2.zero));
            Assert.That(surface.Scale, Is.EqualTo(2.25f));
        }

        [Test]
        public void VinesArena_UsesTheReconstructedFullCanvasAtProductionScale()
        {
            var arena = System.Array.Find(PrototypeArenaLibrary.CreateAll(), item => item.ArenaId == "arena.vines");

            Assert.IsNotNull(arena);
            Assert.That(arena.SpriteSurfaces.Count, Is.EqualTo(1));
            var surface = arena.SpriteSurfaces[0];
            Assert.That(surface.InstanceId, Is.EqualTo("reconstructed"));
            Assert.That(surface.VisualResourcePath, Is.EqualTo("Visuals/Arenas/Vines/vines_reconstructed_visual"));
            Assert.That(surface.MaskResourcePath, Is.EqualTo("Visuals/Arenas/Vines/vines_reconstructed_mask"));
            Assert.That(surface.Position, Is.EqualTo(UnityEngine.Vector2.zero));
            Assert.That(surface.Scale, Is.EqualTo(2.25f));
        }

        [Test]
        public void VinesArena_PlayerSpawnIsAboveTheOpenLeftRing()
        {
            var arena = System.Array.Find(PrototypeArenaLibrary.CreateAll(), item => item.ArenaId == "arena.vines");

            Assert.IsNotNull(arena);
            Assert.That(arena.PlayerSpawn, Is.EqualTo(new UnityEngine.Vector2(-8.75f, 3f)));
        }
    }
}
