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
                CreateSceneArena("arena.city", "City Arena", "City", new Vector2(-8.75f, -3.2f), new Vector2(8.75f, -3.2f), new[] { ("section_01", 29, 68, 833, 688, -9), ("section_02", 1183, 167, 2047, 812, -8), ("section_03", 544, 584, 1314, 1123, -7), ("section_04", 0, 627, 617, 1116, -6), ("section_05", 1242, 743, 2048, 1129, -5) }, 1.8f),
                CreateSceneArena("arena.grib", "Mushroom Arena", "Grib", new Vector2(-8.75f, -2.55f), new Vector2(8.75f, -2.55f), new[] { ("section_01", 45, 7, 827, 528, -9), ("section_02", 1065, 132, 1752, 717, -8), ("section_03", 533, 381, 1222, 1144, -7), ("section_04", 1172, 460, 2048, 1133, -6), ("section_05", 0, 479, 710, 1097, -5) }, 1.8f),
                CreateSceneArena("arena.laguna", "Lagoon Arena", "Laguna", new Vector2(-8.75f, -2.2f), new Vector2(8.75f, -2.2f), new[] { ("section_01", 1255, 152, 2048, 710, -9), ("section_02", 0, 187, 686, 696, -8), ("section_03", 466, 343, 1106, 1039, -7), ("section_04", 0, 624, 589, 1035, -6), ("section_05", 1071, 678, 2048, 1047, -5) }, 1.8f),
                CreateSceneArena("arena.monument", "Monument Arena", "Monument", new Vector2(-8.75f, -3.1f), new Vector2(8.75f, -3.1f), new[] { ("section_01", 1147, 85, 1571, 676, -9), ("section_02", 584, 91, 1508, 1047, -8), ("section_03", 1379, 383, 2046, 1055, -7), ("section_04", 3, 447, 842, 1053, -6) }, 1.8f),
                CreateSceneArena("arena.kraken", "Kraken Arena", "Kraken", new Vector2(-8.75f, -3.1f), new Vector2(8.75f, -3.1f), new[] { ("left", 0, 128, 794, 1143, -8), ("center", 566, 64, 1394, 1152, -7), ("right", 1096, 207, 2048, 1152, -6) }, 1.8f),
                CreateSceneArena("arena.vines", "Vines Arena", "Vines", new Vector2(-8.75f, -2.2f), new Vector2(8.75f, -2.2f), new[] { ("left", 0, 109, 684, 985, -8), ("center", 446, 165, 1254, 990, -7), ("right", 976, 165, 1824, 990, -6), ("far_right", 1436, 194, 2048, 956, -5) }),
                CreateSceneArena("arena.robots", "Robots Arena", "Robots", new Vector2(-8.75f, -3.1f), new Vector2(8.75f, -3.1f), new[] { ("left", 72, 159, 874, 1049, -8), ("center", 566, 159, 1454, 1049, -7), ("right", 1136, 221, 1999, 1050, -6) })
            };
        }

        private static ArenaDefinition CreateSceneArena(string id, string name, string folder, Vector2 player, Vector2 opponent, (string id, int x0, int y0, int x1, int y1, int order)[] pieces, float sceneWorldScale = 3.6f)
        {
            var surfaces = new ArenaSpriteSurface[pieces.Length];
            for (var i = 0; i < pieces.Length; i++)
            {
                var piece = pieces[i];
                surfaces[i] = new ArenaSpriteSurface(piece.id, $"Visuals/Arenas/{folder}/{id.Substring(6)}_{piece.id}_visual", $"Visuals/Arenas/{folder}/{id.Substring(6)}_{piece.id}_mask", new Vector2(((piece.x0 + piece.x1) * 0.5f - 1024f) / 256f * sceneWorldScale, (576f - (piece.y0 + piece.y1) * 0.5f) / 256f * sceneWorldScale), sceneWorldScale, piece.order);
            }
            return ArenaDefinition.CreateRuntime(id, name, new Vector3(0f, -1.9f, -10f), 6.2f, player, opponent, System.Array.Empty<ArenaTerrainBlock>(), surfaces, lockCameraAtZoomOutLimit: true);
        }

        private const string ForestResourceRoot = "Visuals/Arenas/Forest/";
        private const float ForestWorldScale = 1.7875f;

        private static ArenaDefinition CreateForest()
        {
            return ArenaDefinition.CreateRuntime(
                "arena.forest",
                "Forest Arena",
                new Vector3(0f, -1.4f, -10f),
                6.2f,
                new Vector2(-8f, -1.6f),
                new Vector2(8f, -1.6f),
                System.Array.Empty<ArenaTerrainBlock>(),
                new[]
                {
                    ForestSurface("section_01", 1272, 4, 733, 628, -9),
                    ForestSurface("section_02", 116, 343, 579, 789, -8),
                    ForestSurface("section_03", 1250, 563, 740, 586, -7),
                    ForestSurface("section_04", 617, 728, 685, 353, -6)
                },
                limitZoomOutToSpriteBounds: false,
                spriteArenaVerticalAnchor: 1f,
                spriteArenaVerticalScreenOffset: 0.15f,
                lockCameraAtZoomOutLimit: true);
        }

        private static ArenaSpriteSurface ForestSurface(string assetId, float x, float y, float width, float height, int sortingOrder)
        {
            return new ArenaSpriteSurface(
                assetId,
                ForestResourceRoot + "forest_" + assetId + "_visual",
                ForestResourceRoot + "forest_" + assetId + "_mask",
                new Vector2(((x + width * 0.5f) - 1024f) / 256f * ForestWorldScale, (576f - (y + height * 0.5f)) / 256f * ForestWorldScale),
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
                new Vector3(0f, -2f, -10f),
                9.25f,
                new Vector2(-14.25f, -6.625f),
                new Vector2(14.25f, -6.625f),
                System.Array.Empty<ArenaTerrainBlock>(),
                new[]
                {
                    Surface("upper_left", "arena_desert_sci_fi_upper_left_01", 0, 0, 1536, 1458, 0),
                    Surface("floating_large", "arena_desert_sci_fi_floating_large_01", 1792, 256, 676, 426, 5),
                    Surface("upper_right", "arena_desert_sci_fi_upper_right_01", 3132, 66, 964, 1210, 0),
                    Surface("lower_left", "arena_desert_sci_fi_lower_left_01", 0, 1446, 1536, 812, 2),
                    Surface("center", "arena_desert_sci_fi_center_01", 1526, 976, 1194, 1290, 3),
                    Surface("lower_right", "arena_desert_sci_fi_lower_right_01", 2708, 1264, 1388, 988, 2)
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
