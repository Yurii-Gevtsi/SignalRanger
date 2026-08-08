using UnityEngine;

namespace Game.Gameplay.Battle.Arenas
{
    public static class PrototypeArenaLibrary
    {
        public static ArenaDefinition[] CreateAll()
        {
            return new[]
            {
                CreateTrainingMeadow()
            };
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
