using UnityEngine;

namespace Game.Gameplay.Battle.Arenas
{
    public static class PrototypeArenaLibrary
    {
        public static ArenaDefinition[] CreateAll()
        {
            return new[]
            {
                CreateTrainingMeadow(),
                CreateDesertSciFi(),
                CreateForest(),
                CreateSceneArena("arena.city", "City Arena", "City", new Vector2(-8.75f, -3.2f), new Vector2(8.75f, -3.2f), new[] { ("left", 0, 43, 874, 1151, -8), ("center", 596, 414, 1444, 1152, -7), ("right", 1166, 126, 2048, 1152, -6) }),
                CreateSceneArena("arena.grib", "Mushroom Arena", "Grib", new Vector2(-8.75f, -2.55f), new Vector2(8.75f, -2.55f), new[] { ("left", 1, 70, 744, 1134, -8), ("center", 506, 286, 1344, 1135, -7), ("right", 1056, 203, 1824, 1136, -6), ("far_right", 1576, 356, 2043, 1136, -5) }),
                CreateSceneArena("arena.laguna", "Lagoon Arena", "Laguna", new Vector2(-8.75f, -2.2f), new Vector2(8.75f, -2.2f), new[] { ("left", 0, 248, 874, 1004, -8), ("center", 596, 406, 1444, 1011, -7), ("right", 1156, 238, 2048, 1017, -6) })
            };
        }

        private static ArenaDefinition CreateSceneArena(string id, string name, string folder, Vector2 player, Vector2 opponent, (string id, int x0, int y0, int x1, int y1, int order)[] pieces)
        {
            var surfaces = new ArenaSpriteSurface[pieces.Length];
            for (var i = 0; i < pieces.Length; i++)
            {
                var piece = pieces[i];
                // A full 2048px scene must remain wider than the widest phone
                // zoom-out frame, including devices with a very wide viewport.
                const float sceneWorldScale = 3.6f;
                surfaces[i] = new ArenaSpriteSurface(piece.id, $"Visuals/Arenas/{folder}/{id.Substring(6)}_{piece.id}_visual", $"Visuals/Arenas/{folder}/{id.Substring(6)}_{piece.id}_mask", new Vector2(((piece.x0 + piece.x1) * 0.5f - 1024f) / 256f * sceneWorldScale, (576f - (piece.y0 + piece.y1) * 0.5f) / 256f * sceneWorldScale), sceneWorldScale, piece.order);
            }
            return ArenaDefinition.CreateRuntime(id, name, new Vector3(0f, 0f, -10f), 6.2f, player, opponent, System.Array.Empty<ArenaTerrainBlock>(), surfaces, lockCameraAtZoomOutLimit: true);
        }

        private const string ForestResourceRoot = "Visuals/Arenas/Forest/";
        private const float ForestWorldScale = 2.5f;

        private static ArenaDefinition CreateForest()
        {
            return ArenaDefinition.CreateRuntime(
                "arena.forest",
                "Forest Arena",
                new Vector3(0f, 1.0f, -10f),
                6.2f,
                new Vector2(-8f, -1.6f),
                new Vector2(8f, -1.6f),
                System.Array.Empty<ArenaTerrainBlock>(),
                new[]
                {
                    ForestSurface("left", "forest_left", -7, -4.2f),
                    ForestSurface("extension", "forest_extension", -9),
                    ForestSurface("center", "forest_center", -8),
                    ForestSurface("right", "forest_right", -7, 4.5f)
                },
                limitZoomOutToSpriteBounds: false,
                spriteArenaVerticalAnchor: 1f,
                spriteArenaVerticalScreenOffset: 0.15f,
                lockCameraAtZoomOutLimit: true);
        }

        private static ArenaSpriteSurface ForestSurface(string instanceId, string assetId, int sortingOrder, float horizontalOffset = 0f)
        {
            var piece = assetId switch
            {
                "forest_left" => (814f, 767f, -2.41016f, -0.59570f),
                "forest_center" => (833f, 309f, 0.07227f, -1.49023f),
                "forest_right" => (882f, 1072f, 2.27734f, 0f),
                "forest_extension" => (1672f, 175f, 0f, -1.64f),
                _ => (0f, 0f, 0f, 0f)
            };
            return new ArenaSpriteSurface(
                instanceId,
                ForestResourceRoot + assetId + "_visual",
                ForestResourceRoot + assetId + "_mask",
                new Vector2(piece.Item3 * ForestWorldScale + horizontalOffset, piece.Item4 * ForestWorldScale),
                ForestWorldScale,
                sortingOrder);
        }

        private const string DesertResourceRoot = "Visuals/Arenas/DesertSciFi/";
        private const float DesertPixelsPerUnit = 256f;
        private const float DesertWorldScale = 2.5f;
        private const float DesertCanvasWidth = 4096f;
        private const float DesertCanvasHeight = 2304f;

        private static ArenaDefinition CreateDesertSciFi()
        {
            return ArenaDefinition.CreateRuntime(
                "arena.desert-sci-fi",
                "Desert Sci-Fi Arena",
                new Vector3(0f, -1.75f, -10f),
                9.25f,
                new Vector2(-14.25f, -6.625f),
                new Vector2(14.25f, -6.625f),
                System.Array.Empty<ArenaTerrainBlock>(),
                new[]
                {
                    Surface("cluster_01", "arena_desert_sci_fi_upper_left_01", 91, 220, 1252, 1364, 0),
                    Surface("cluster_02", "arena_desert_sci_fi_inner_left_01", 778, 688, 726, 1266, 1),
                    Surface("cluster_03", "arena_desert_sci_fi_lower_left_01", 0, 1408, 1554, 844, 2),
                    Surface("cluster_04", "arena_desert_sci_fi_center_01", 1098, 864, 1726, 1396, 3),
                    Surface("cluster_05", "arena_desert_sci_fi_floating_large_01", 1188, 310, 888, 518, 5),
                    Surface("cluster_06_a", "arena_desert_sci_fi_floating_small_01", 2142, 740, 612, 384, 5),
                    Surface("cluster_06_b", "arena_desert_sci_fi_floating_small_01", 2868, 452, 612, 384, 5),
                    Surface("cluster_07", "arena_desert_sci_fi_upper_right_01", 3328, 288, 768, 1326, 0),
                    Surface("cluster_08", "arena_desert_sci_fi_lower_right_01", 2668, 1018, 1428, 1232, 2)
                });
        }

        private static ArenaSpriteSurface Surface(
            string instanceId,
            string assetId,
            float x,
            float y,
            float width,
            float height,
            int sortingOrder)
        {
            var centerX = x + width * 0.5f;
            var centerY = y + height * 0.5f;
            var position = new Vector2(
                (centerX - DesertCanvasWidth * 0.5f) / DesertPixelsPerUnit * DesertWorldScale,
                (DesertCanvasHeight * 0.5f - centerY) / DesertPixelsPerUnit * DesertWorldScale);
            return new ArenaSpriteSurface(
                instanceId,
                DesertResourceRoot + assetId + "_visual",
                DesertResourceRoot + assetId + "_mask",
                position,
                DesertWorldScale,
                sortingOrder);
        }

        private static ArenaDefinition CreateTrainingMeadow()
        {
            return ArenaDefinition.CreateRuntime(
                "arena.training-meadow",
                "Training Meadow",
                new Vector3(0f, 1.2f, -10f),
                5.6f,
                new Vector2(-2.5f, -0.85f),
                new Vector2(4.5f, -0.95f),
                new[]
                {
                    Sky(),
                    // The opening lesson needs a deterministic, clear lane for its
                    // left/right movement steps. Obstacles would make a required
                    // input impossible at some spawn positions.
                    new ArenaTerrainBlock("ground.main", new Vector2(0f, -2.45f), new Vector2(15f, 1.1f), GrassDark(), true, 0)
                });
        }

        private static ArenaTerrainBlock Sky()
        {
            return new ArenaTerrainBlock("background.sky", new Vector2(0f, 1.4f), new Vector2(18f, 9f), new Color(0.64f, 0.83f, 0.96f), false, -20);
        }

        private static Color GrassDark()
        {
            return new Color(0.31f, 0.62f, 0.34f);
        }

        private static Color GrassLight()
        {
            return new Color(0.37f, 0.67f, 0.37f);
        }

        private static Color Soil()
        {
            return new Color(0.45f, 0.36f, 0.26f);
        }
    }
}
