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
                CreateSplitRidge(),
                CreateCentralCover()
            };
        }

        private static ArenaDefinition CreateTrainingMeadow()
        {
            return ArenaDefinition.CreateRuntime(
                "arena.training-meadow",
                "Training Meadow",
                new Vector3(0f, 1.2f, -10f),
                5.6f,
                new Vector2(-4.4f, -0.85f),
                new Vector2(4.2f, -0.95f),
                new[]
                {
                    Sky(),
                    new ArenaTerrainBlock("ground.main", new Vector2(0f, -2.45f), new Vector2(15f, 1.1f), GrassDark(), true, 0),
                    new ArenaTerrainBlock("hill.left", new Vector2(-4.4f, -1.85f), new Vector2(3.2f, 1.2f), GrassLight(), true, 0),
                    new ArenaTerrainBlock("hill.right", new Vector2(4.2f, -1.95f), new Vector2(3.4f, 1f), GrassLight(), true, 0),
                    new ArenaTerrainBlock("cover.center", new Vector2(0f, -1.55f), new Vector2(1.2f, 1.25f), Soil(), true, 1)
                });
        }

        private static ArenaDefinition CreateSplitRidge()
        {
            return ArenaDefinition.CreateRuntime(
                "arena.split-ridge",
                "Split Ridge",
                new Vector3(0f, 1.35f, -10f),
                5.8f,
                new Vector2(-5.1f, -0.95f),
                new Vector2(5.1f, -0.95f),
                new[]
                {
                    Sky(),
                    new ArenaTerrainBlock("ground.left", new Vector2(-4.7f, -2.35f), new Vector2(5.8f, 1.2f), GrassDark(), true, 0),
                    new ArenaTerrainBlock("ground.right", new Vector2(4.7f, -2.35f), new Vector2(5.8f, 1.2f), GrassDark(), true, 0),
                    new ArenaTerrainBlock("platform.left", new Vector2(-5.1f, -1.85f), new Vector2(2.4f, 0.9f), GrassLight(), true, 1),
                    new ArenaTerrainBlock("platform.right", new Vector2(5.1f, -1.85f), new Vector2(2.4f, 0.9f), GrassLight(), true, 1),
                    new ArenaTerrainBlock("cover.low-center", new Vector2(0f, -2.05f), new Vector2(1.5f, 0.85f), Rock(), true, 1)
                });
        }

        private static ArenaDefinition CreateCentralCover()
        {
            return ArenaDefinition.CreateRuntime(
                "arena.central-cover",
                "Central Cover",
                new Vector3(0f, 1.05f, -10f),
                5.5f,
                new Vector2(-3.8f, -0.85f),
                new Vector2(3.8f, -0.85f),
                new[]
                {
                    Sky(),
                    new ArenaTerrainBlock("ground.main", new Vector2(0f, -2.45f), new Vector2(14.4f, 1.1f), GrassDark(), true, 0),
                    new ArenaTerrainBlock("cover.left-low", new Vector2(-1.05f, -1.65f), new Vector2(0.9f, 1f), Soil(), true, 1),
                    new ArenaTerrainBlock("cover.right-high", new Vector2(0.55f, -1.35f), new Vector2(1f, 1.6f), Rock(), true, 1),
                    new ArenaTerrainBlock("ledge.player", new Vector2(-3.8f, -1.82f), new Vector2(2.3f, 0.82f), GrassLight(), true, 1),
                    new ArenaTerrainBlock("ledge.opponent", new Vector2(3.8f, -1.82f), new Vector2(2.3f, 0.82f), GrassLight(), true, 1)
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

        private static Color Rock()
        {
            return new Color(0.46f, 0.49f, 0.52f);
        }
    }
}
