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
                CreateDesertSciFi()
            };
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
                new Vector2(-3.4f, -0.85f),
                new Vector2(5f, -0.95f),
                new[]
                {
                    Sky(),
                    new ArenaTerrainBlock("ground.main", new Vector2(0f, -2.45f), new Vector2(15f, 1.1f), GrassDark(), true, 0),
                    new ArenaTerrainBlock("hill.left", new Vector2(-4.4f, -1.85f), new Vector2(3.2f, 1.2f), GrassLight(), true, 0),
                    new ArenaTerrainBlock("hill.right", new Vector2(4.2f, -1.95f), new Vector2(3.4f, 1f), GrassLight(), true, 0),
                    new ArenaTerrainBlock("cover.center", new Vector2(0f, -1.55f), new Vector2(1.2f, 1.25f), Soil(), true, 1)
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
