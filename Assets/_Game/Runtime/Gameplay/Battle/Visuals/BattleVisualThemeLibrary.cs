using Game.Gameplay.Battle.Arenas;
using UnityEngine;

namespace Game.Gameplay.Battle.Visuals
{
    public static class BattleVisualThemeLibrary
    {
        private const string FirstArenaId = "arena.training-meadow";
        private const string DesertArenaId = "arena.desert-sci-fi";
        private const string ForestArenaId = "arena.forest";
        private const string AnimatedSkyBackgroundPrefab = "Visuals/Arenas/Backgrounds/Sky1/AnimatedSkyBackground";
        private const string ForestCosmosBackground = "Visuals/Arenas/Backgrounds/Cosmos/cosmos";
        private const string CityBackground = "Visuals/Arenas/Backgrounds/City/city";
        private const string GribBackground = "Visuals/Arenas/Backgrounds/Grib/grib";
        private const string LagunaBackground = "Visuals/Arenas/Backgrounds/Laguna/laguna";
        private const string TrainingGround = "Visuals/Arenas/Training/ground";
        private const string TrainingShip = "Visuals/Arenas/Training/ship";
        private const string TrainingBoxes = "Visuals/Arenas/Training/boxes";
        private const string TrainingRock = "Visuals/Arenas/Training/rock";
        private const string PlayerCharacter = "";
        private const string OpponentCharacter = "";
        private const string PlayerRig = "Visuals/Characters/DroidRig";
        private const string OpponentRig = "Visuals/Characters/SigmaRig";
        // Frames are deliberately only about 30% larger than their terrain.
        // ResolveCameraFrame expands the short side for the actual device aspect.
        private static readonly Rect DesertCameraFrame = Rect.MinMaxRect(-23.955f, -11.25f, 23.955f, 15.7f);
        // The authored canyon arena and animated sky share this offset. It places the
        // animated background root at Y = 2.0 on the current desert camera frame.
        private static readonly Vector2 DesertWorldOffset = new(0f, -1.225f);
        private static readonly Vector2 DesertArenaScale = new(1.37f, 1.37f);
        private static readonly Vector2 DesertArenaScalePivot = new(0f, DesertCameraFrame.yMin);
        private static readonly Rect ForestCameraFrame = Rect.MinMaxRect(-15.925f, -7.61f, 15.925f, 10.31f);
        private static readonly Rect SceneCameraFrame = Rect.MinMaxRect(-17.25f, -8.1f, 17.25f, 11.31f);
        private static readonly Rect TrainingCameraFrame = Rect.MinMaxRect(-10.665f, -5.05f, 10.665f, 6.95f);

        public static BattleVisualTheme ForArena(ArenaDefinition arena)
        {
            if (arena == null)
            {
                return null;
            }

            if (arena.ArenaId == DesertArenaId)
            {
                return new BattleVisualTheme(
                    default,
                    default,
                    PlayerCharacter,
                    OpponentCharacter,
                    new Vector2(0.18f, 0.18f),
                    new Vector2(0f, -0.08f),
                    true,
                    false,
                    playerSkinFacingDirectionX: 1,
                    opponentSkinFacingDirectionX: 1,
                    playerRigResourcePath: PlayerRig,
                    opponentRigResourcePath: OpponentRig,
                    backgroundPrefabResourcePath: AnimatedSkyBackgroundPrefab,
                    cameraFrame: DesertCameraFrame,
                    worldOffset: DesertWorldOffset,
                    arenaScale: DesertArenaScale,
                    arenaScalePivot: DesertArenaScalePivot,
                    cameraTopPanAllowancePercent: 0.2f,
                    killZoneOffsetY: 1.225f);
            }

            if (arena.ArenaId == ForestArenaId)
            {
                return new BattleVisualTheme(
                    StaticBackground(ForestCosmosBackground, ForestCameraFrame),
                    default,
                    PlayerCharacter,
                    OpponentCharacter,
                    new Vector2(0.18f, 0.18f),
                    new Vector2(0f, -0.08f),
                    true,
                    false,
                    playerSkinFacingDirectionX: 1,
                    opponentSkinFacingDirectionX: 1,
                    playerRigResourcePath: PlayerRig,
                    opponentRigResourcePath: OpponentRig,
                    cameraFrame: ForestCameraFrame);
            }

            var background = arena.ArenaId switch { "arena.city" => CityBackground, "arena.grib" => GribBackground, "arena.laguna" => LagunaBackground, _ => string.Empty };
            if (!string.IsNullOrEmpty(background))
            {
                return new BattleVisualTheme(StaticBackground(background, SceneCameraFrame), default, PlayerCharacter, OpponentCharacter, new Vector2(0.18f, 0.18f), new Vector2(0f, -0.08f), true, false, playerRigResourcePath: PlayerRig, opponentRigResourcePath: OpponentRig, cameraFrame: SceneCameraFrame);
            }

            if (arena.ArenaId == "arena.vines")
            {
                return new BattleVisualTheme(StaticBackground(ForestCosmosBackground, SceneCameraFrame), default, PlayerCharacter, OpponentCharacter, new Vector2(0.18f, 0.18f), new Vector2(0f, -0.08f), true, false, playerRigResourcePath: PlayerRig, opponentRigResourcePath: OpponentRig, cameraFrame: SceneCameraFrame);
            }

            if (arena.ArenaId == "arena.monument" || arena.ArenaId == "arena.kraken" || arena.ArenaId == "arena.robots")
            {
                return new BattleVisualTheme(default, default, PlayerCharacter, OpponentCharacter, new Vector2(0.18f, 0.18f), new Vector2(0f, -0.08f), true, false, playerRigResourcePath: PlayerRig, opponentRigResourcePath: OpponentRig, backgroundPrefabResourcePath: AnimatedSkyBackgroundPrefab, cameraFrame: SceneCameraFrame);
            }

            if (arena.ArenaId != FirstArenaId)
            {
                return null;
            }

            return new BattleVisualTheme(
                default,
                new BattleVisualLayer(TrainingGround, new Vector2(-0.09862f, -4.93959f), new Vector2(3.47552f, 3.91558f), -8, true),
                PlayerCharacter,
                OpponentCharacter,
                new Vector2(0.18f, 0.18f),
                new Vector2(0f, -0.08f),
                true,
                false,
                playerSkinFacingDirectionX: 1,
                opponentSkinFacingDirectionX: 1,
                    playerRigResourcePath: PlayerRig,
                    opponentRigResourcePath: OpponentRig,
                    backgroundPrefabResourcePath: AnimatedSkyBackgroundPrefab,
                    cameraFrame: TrainingCameraFrame,
                    additionalDestructibleLayers: new[]
                {
                    new BattleVisualLayer(TrainingShip, new Vector2(-9.66498f, -0.53610f), new Vector2(2.05578f, 1.88287f), -7, true),
                    new BattleVisualLayer(TrainingBoxes, new Vector2(0.83379f, -2.39658f), new Vector2(0.97620f, 1.00449f), -7, true),
                    new BattleVisualLayer(TrainingRock, new Vector2(9.25f, -2.35f), new Vector2(1.85961f, 1.80162f), -7, true)
                    });
        }

        private static BattleVisualLayer StaticBackground(string resourcePath, Rect frame)
        {
            return new BattleVisualLayer(resourcePath, frame.center, Vector2.one, -30);
        }
    }
}
