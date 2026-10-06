using UnityEngine;

namespace Game.Gameplay.Battle.Arenas
{
    public static class PrototypeArenaLibrary
    {
        private const float CitySceneWorldScale = 4.485f;
        private const float CityAssetProductionScale = 2f;
        // A 4096px production canvas at 256 PPU and this two-times asset
        // production scale spans 36 world units. That fills the shared scene
        // camera frame like the approved City/Forest/Desert arenas do.
        private const float StandardSceneWorldScale = 4.5f;
        private const float StandardAssetProductionScale = 2f;

        public static ArenaDefinition[] CreateAll()
        {
            return new[]
            {
                CreateTrainingMeadow(),
                CreateDesertSciFi(),
                CreateForest(),
                CreateSceneArena("arena.city", "City Arena", "City", new Vector2(-8.75f, -3.2f), new Vector2(8.75f, -3.2f), new[] { ("section_01", 29, 68, 833, 689, -9), ("section_02", 1183, 167, 2048, 813, -8), ("section_03", 544, 584, 1315, 1123, -7), ("section_04", 0, 626, 618, 1116, -6), ("section_05", 1241, 742, 2048, 1129, -5) }, CitySceneWorldScale, CityAssetProductionScale),
                CreateGrib(),
                CreateLaguna(),
                CreateMonument(),
                CreateKraken(),
                CreateVines(),
                CreateRobots(),
                CreateBones()
            };
        }

        private static ArenaDefinition CreateSceneArena(
            string id,
            string name,
            string folder,
            Vector2 player,
            Vector2 opponent,
            (string id, int x0, int y0, int x1, int y1, int order)[] pieces,
            float sceneWorldScale = 3.6f,
            float assetProductionScale = 1f)
        {
            var surfaces = new ArenaSpriteSurface[pieces.Length];
            for (var i = 0; i < pieces.Length; i++)
            {
                var piece = pieces[i];
                surfaces[i] = new ArenaSpriteSurface(
                    piece.id,
                    $"Visuals/Arenas/{folder}/{id.Substring(6)}_{piece.id}_visual",
                    $"Visuals/Arenas/{folder}/{id.Substring(6)}_{piece.id}_mask",
                    new Vector2(
                        ((piece.x0 + piece.x1) * 0.5f - 1024f) / 256f * sceneWorldScale,
                        (576f - (piece.y0 + piece.y1) * 0.5f) / 256f * sceneWorldScale),
                    sceneWorldScale / assetProductionScale,
                    piece.order);
            }
            return ArenaDefinition.CreateRuntime(id, name, new Vector3(0f, -1.9f, -10f), 6.2f, player, opponent, System.Array.Empty<ArenaTerrainBlock>(), surfaces, lockCameraAtZoomOutLimit: true);
        }

        private const string ForestResourceRoot = "Visuals/Arenas/Forest/";
        private const float ForestWorldScale = 5.3625f;
        private const float ForestAssetProductionScale = 2f;
        // Forest is authored on a 1080p-equivalent camera frame; 50 screen
        // pixels are approximately 0.83 world units at the current framing.
        private const float ForestOpponentSpawnX = 4.8f;
        private const float ForestOpponentSpawnY = 2.5f;

        private static ArenaDefinition CreateForest()
        {
            return ArenaDefinition.CreateRuntime(
                "arena.forest",
                "Forest Arena",
                new Vector3(0f, -1.4f, -10f),
                6.2f,
                new Vector2(-8f, -1.6f),
                new Vector2(ForestOpponentSpawnX, ForestOpponentSpawnY),
                System.Array.Empty<ArenaTerrainBlock>(),
                new[]
                {
                    new ArenaSpriteSurface(
                        "reconstructed",
                        ForestResourceRoot + "forest_reconstructed_visual",
                        ForestResourceRoot + "forest_reconstructed_mask",
                        Vector2.zero,
                        ForestWorldScale / ForestAssetProductionScale,
                        -9)
                },
                limitZoomOutToSpriteBounds: false,
                spriteArenaVerticalAnchor: 1f,
                spriteArenaVerticalScreenOffset: 0.15f,
                lockCameraAtZoomOutLimit: true);
        }

        private const string GribResourceRoot = "Visuals/Arenas/Grib/";
        private const float GribWorldScale = 5.4f;
        private const float GribAssetProductionScale = 2f;

        private static ArenaDefinition CreateGrib()
        {
            return ArenaDefinition.CreateRuntime(
                "arena.grib",
                "Mushroom Arena",
                new Vector3(0f, -1.9f, -10f),
                6.2f,
                new Vector2(-8.75f, -2.55f),
                new Vector2(8.75f, -2.55f),
                System.Array.Empty<ArenaTerrainBlock>(),
                new[]
                {
                    new ArenaSpriteSurface(
                        "reconstructed",
                        GribResourceRoot + "grib_reconstructed_visual",
                        GribResourceRoot + "grib_reconstructed_mask",
                        Vector2.zero,
                        GribWorldScale / GribAssetProductionScale,
                        -9)
                },
                lockCameraAtZoomOutLimit: true);
        }

        private const string LagunaResourceRoot = "Visuals/Arenas/Laguna/";
        private const float LagunaWorldScale = StandardSceneWorldScale;
        private const float LagunaAssetProductionScale = StandardAssetProductionScale;

        private static ArenaDefinition CreateLaguna()
        {
            return ArenaDefinition.CreateRuntime(
                "arena.laguna",
                "Lagoon Arena",
                new Vector3(0f, -1.9f, -10f),
                6.2f,
                new Vector2(-8.75f, -2.2f),
                new Vector2(8.75f, -2.2f),
                System.Array.Empty<ArenaTerrainBlock>(),
                new[]
                {
                    new ArenaSpriteSurface(
                        "reconstructed",
                        LagunaResourceRoot + "laguna_reconstructed_visual",
                        LagunaResourceRoot + "laguna_reconstructed_mask",
                        new Vector2(0f, -0.45f),
                        LagunaWorldScale / LagunaAssetProductionScale,
                        -9)
                },
                lockCameraAtZoomOutLimit: true);
        }

        private const string KrakenResourceRoot = "Visuals/Arenas/Kraken/";
        private const float KrakenWorldScale = StandardSceneWorldScale;
        private const float KrakenAssetProductionScale = 2f;

        private static ArenaDefinition CreateKraken()
        {
            return ArenaDefinition.CreateRuntime(
                "arena.kraken",
                "Kraken Arena",
                new Vector3(0f, -1.9f, -10f),
                6.2f,
                new Vector2(-8.75f, -3.1f),
                new Vector2(8.75f, -3.1f),
                System.Array.Empty<ArenaTerrainBlock>(),
                new[]
                {
                    new ArenaSpriteSurface(
                        "reconstructed",
                        KrakenResourceRoot + "kraken_reconstructed_visual",
                        KrakenResourceRoot + "kraken_reconstructed_mask",
                        Vector2.zero,
                        KrakenWorldScale / KrakenAssetProductionScale,
                        -9)
                },
                lockCameraAtZoomOutLimit: true);
        }

        private const string MonumentResourceRoot = "Visuals/Arenas/Monument/";
        private const float MonumentWorldScale = StandardSceneWorldScale;
        private const float MonumentAssetProductionScale = 2f;

        private static ArenaDefinition CreateMonument()
        {
            return ArenaDefinition.CreateRuntime(
                "arena.monument",
                "Monument Arena",
                new Vector3(0f, -1.9f, -10f),
                6.2f,
                new Vector2(-8.75f, -3.1f),
                new Vector2(8.75f, -3.1f),
                System.Array.Empty<ArenaTerrainBlock>(),
                new[]
                {
                    new ArenaSpriteSurface(
                        "reconstructed",
                        MonumentResourceRoot + "monument_reconstructed_visual",
                        MonumentResourceRoot + "monument_reconstructed_mask",
                        Vector2.zero,
                        MonumentWorldScale / MonumentAssetProductionScale,
                        -9)
                },
                lockCameraAtZoomOutLimit: true);
        }

        private const string RobotsResourceRoot = "Visuals/Arenas/Robots/";
        private const float RobotsWorldScale = StandardSceneWorldScale;
        private const float RobotsAssetProductionScale = 2f;

        private static ArenaDefinition CreateRobots()
        {
            return ArenaDefinition.CreateRuntime(
                "arena.robots",
                "Robots Arena",
                new Vector3(0f, -1.9f, -10f),
                6.2f,
                new Vector2(-8.75f, -3.1f),
                new Vector2(8.75f, -3.1f),
                System.Array.Empty<ArenaTerrainBlock>(),
                new[]
                {
                    new ArenaSpriteSurface(
                        "reconstructed",
                        RobotsResourceRoot + "robots_reconstructed_visual",
                        RobotsResourceRoot + "robots_reconstructed_mask",
                        Vector2.zero,
                        RobotsWorldScale / RobotsAssetProductionScale,
                        -9)
                },
                lockCameraAtZoomOutLimit: true);
        }

        private const string VinesResourceRoot = "Visuals/Arenas/Vines/";
        private const float VinesWorldScale = StandardSceneWorldScale;
        private const float VinesAssetProductionScale = 2f;

        private static ArenaDefinition CreateVines()
        {
            return ArenaDefinition.CreateRuntime(
                "arena.vines",
                "Vines Arena",
                new Vector3(0f, -1.9f, -10f),
                6.2f,
                // The left ring has an enclosed lower arc. Keep the authored
                // player anchor above its upper open surface so landing
                // resolution cannot place the unit inside the ring.
                new Vector2(-8.75f, 3f),
                new Vector2(8.75f, -2.2f),
                System.Array.Empty<ArenaTerrainBlock>(),
                new[]
                {
                    new ArenaSpriteSurface(
                        "reconstructed",
                        VinesResourceRoot + "vines_reconstructed_visual",
                        VinesResourceRoot + "vines_reconstructed_mask",
                        Vector2.zero,
                        VinesWorldScale / VinesAssetProductionScale,
                        -9)
                },
                lockCameraAtZoomOutLimit: true);
        }

        private const string DesertResourceRoot = "Visuals/Arenas/DesertSciFi/";
        private const float DesertPixelsPerUnit = 256f;
        private const float DesertWorldScale = 2.5f;
        private const float DesertCanvasWidth = 4096f;
        private const float DesertCanvasHeight = 2304f;
        private static readonly Vector2 DesertPlayerSpawn = new(-10f, 2.7f);
        private static readonly Vector2 DesertOpponentSpawn = new(12f, -5.8f);

        private static ArenaDefinition CreateDesertSciFi()
        {
            return ArenaDefinition.CreateRuntime(
                "arena.desert-sci-fi",
                "Desert Sci-Fi Arena",
                new Vector3(0f, -2f, -10f),
                9.25f,
                DesertPlayerSpawn,
                DesertOpponentSpawn,
                System.Array.Empty<ArenaTerrainBlock>(),
                new[]
                {
                    Surface("upper_left", "arena_desert_sci_fi_upper_left_01", 0, 0, 1536, 1458, 0),
                    Surface("floating_large", "arena_desert_sci_fi_floating_large_01", 1792, 666, 676, 426, 5,
                        scaleMultiplier: 0.7f,
                        horizontalMotionRange: 3.5f, horizontalMotionSpeed: 0.45f),
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
            int sortingOrder,
            float scaleMultiplier = 1f,
            float horizontalMotionRange = 0f,
            float horizontalMotionSpeed = 0f)
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
                DesertWorldScale * Mathf.Max(0f, scaleMultiplier),
                sortingOrder,
                horizontalMotionRange,
                horizontalMotionSpeed);
        }

        private static ArenaDefinition CreateTrainingMeadow()
        {
            return ArenaDefinition.CreateRuntime(
                "arena.training-meadow",
                "Training Meadow",
                new Vector3(0f, 0.8f, -10f),
                6.2f,
                new Vector2(-6.3f, -0.65f),
                new Vector2(6.2f, 2.1f),
                System.Array.Empty<ArenaTerrainBlock>(),
                new[]
                {
                    new ArenaSpriteSurface(
                        "reconstructed",
                        TrainingResourceRoot + "training_reconstructed_visual",
                        TrainingResourceRoot + "training_reconstructed_mask",
                        Vector2.zero,
                        TrainingWorldScale / TrainingAssetProductionScale,
                        -9)
                });
        }

        private static ArenaDefinition CreateBones()
        {
            return ArenaDefinition.CreateRuntime(
                "arena.bones",
                "Bones Arena",
                new Vector3(0f, 0.5f, -10f),
                6.2f,
                new Vector2(-10.5f, 2.4f),
                new Vector2(10.3f, 2.4f),
                System.Array.Empty<ArenaTerrainBlock>(),
                new[]
                {
                    new ArenaSpriteSurface(
                        "reconstructed",
                        BonesResourceRoot + "bones_reconstructed_visual",
                        BonesResourceRoot + "bones_reconstructed_mask",
                        Vector2.zero,
                        BonesWorldScale / BonesAssetProductionScale,
                        -9)
                },
                lockCameraAtZoomOutLimit: true);
        }

        private const string TrainingResourceRoot = "Visuals/Arenas/Training/";
        private const float TrainingWorldScale = 6.72f;
        private const float TrainingAssetProductionScale = 2.4f;
        private const string BonesResourceRoot = "Visuals/Arenas/Bones/";
        private const float BonesWorldScale = 6.2f;
        private const float BonesAssetProductionScale = 2f;
    }
}
