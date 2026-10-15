using Game.Gameplay.Battle.Arenas;
using UnityEngine;

namespace Game.Gameplay.Battle.Visuals
{
    public static class BattleVisualThemeLibrary
    {
        private const string FirstArenaId = "arena.training-meadow";
        private const string DesertArenaId = "arena.desert-sci-fi";
        private const string ForestArenaId = "arena.forest";
        private const string LagunaArenaId = "arena.laguna";
        private const string AnimatedSkyBackgroundPrefab = "Visuals/Arenas/Backgrounds/Sky1/AnimatedSkyBackground";
        private const string BonesBackground = "Visuals/Arenas/Backgrounds/Bones/bones";
        private static readonly Color BonesArenaTint = new(0.84f, 0.84f, 0.84f, 1f);
        private static readonly Color ForestArenaTint = new(0.84f, 0.84f, 0.84f, 1f);
        private const string ForestBackground = "Visuals/Arenas/Backgrounds/Forest/forest";
        private const string CityBackground = "Visuals/Arenas/Backgrounds/City/city";
        private const string GribBackground = "Visuals/Arenas/Backgrounds/Grib/grib_new";
        private const string LagunaBackground = "Visuals/Arenas/Backgrounds/Laguna/laguna_new";
        private const string KrakenBackground = "Visuals/Arenas/Backgrounds/Kraken/kraken";
        private const string MonumentBackground = "Visuals/Arenas/Backgrounds/Monument/monument";
        private const string RobotsBackground = "Visuals/Arenas/Backgrounds/Robots/robots";
        private const string VinesBackground = "Visuals/Arenas/Backgrounds/Vines/vines";
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
        // The imported training arena is intentionally left at its authored world scale.
        // This frame exposes the whole terrain at about 80% of a standard battle viewport.
        private static readonly Rect TrainingCameraFrame = Rect.MinMaxRect(-26.2f, -11.265f, 26.2f, 13.165f);
        private static readonly Vector2 LagunaArenaContentOffset = new(0f, -150f / 256f);
        private static readonly Vector2 MonumentArenaContentOffset = new(0f, -200f / 256f);
        private static readonly Vector2 RobotsArenaContentOffset = new(0f, -50f / 256f);

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
                    StaticBackground(ForestBackground, ForestCameraFrame),
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
                    cameraFrame: ForestCameraFrame,
                    arenaContentOffset: new Vector2(0f, -3.16f),
                    // The rebuilt Forest terrain extends lower than its authored
                    // camera frame. Keep its current artwork untouched, but let
                    // intermediate zoom levels pan down to the lower roots.
                    cameraBottomPanAllowance: 3.2f,
                    arenaTint: ForestArenaTint);
            }

            if (arena.ArenaId == "arena.bones")
            {
                return StaticSceneTheme(BonesBackground, arenaTint: BonesArenaTint);
            }

            var background = arena.ArenaId switch { "arena.city" => CityBackground, "arena.grib" => GribBackground, "arena.laguna" => LagunaBackground, _ => string.Empty };
            if (!string.IsNullOrEmpty(background))
            {
                var arenaContentOffset = arena.ArenaId == LagunaArenaId
                    ? LagunaArenaContentOffset
                    : Vector2.zero;
                return new BattleVisualTheme(
                    StaticBackground(background, SceneCameraFrame),
                    default,
                    PlayerCharacter,
                    OpponentCharacter,
                    new Vector2(0.18f, 0.18f),
                    new Vector2(0f, -0.08f),
                    true,
                    false,
                    playerRigResourcePath: PlayerRig,
                    opponentRigResourcePath: OpponentRig,
                    cameraFrame: SceneCameraFrame,
                    arenaContentOffset: arenaContentOffset,
                    hasKillZone: arena.ArenaId != "arena.grib");
            }

            if (arena.ArenaId == "arena.vines")
            {
                return StaticSceneTheme(VinesBackground, hasKillZone: false);
            }

            if (arena.ArenaId == "arena.monument")
            {
                return StaticSceneTheme(MonumentBackground, MonumentArenaContentOffset);
            }

            if (arena.ArenaId == "arena.robots")
            {
                return StaticSceneTheme(RobotsBackground, RobotsArenaContentOffset);
            }

            if (arena.ArenaId == "arena.kraken")
            {
                return StaticSceneTheme(KrakenBackground);
            }

            if (arena.ArenaId != FirstArenaId)
            {
                return null;
            }

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
                    cameraFrame: TrainingCameraFrame);
        }

        private static BattleVisualLayer StaticBackground(string resourcePath, Rect frame)
        {
            return new BattleVisualLayer(resourcePath, frame.center, Vector2.one, -30);
        }

        private static BattleVisualTheme StaticSceneTheme(
            string backgroundResource,
            Vector2? arenaContentOffset = null,
            bool hasKillZone = true,
            Color? arenaTint = null)
        {
            return new BattleVisualTheme(
                StaticBackground(backgroundResource, SceneCameraFrame),
                default,
                PlayerCharacter,
                OpponentCharacter,
                new Vector2(0.18f, 0.18f),
                new Vector2(0f, -0.08f),
                true,
                false,
                playerRigResourcePath: PlayerRig,
                opponentRigResourcePath: OpponentRig,
                cameraFrame: SceneCameraFrame,
                arenaContentOffset: arenaContentOffset,
                hasKillZone: hasKillZone,
                arenaTint: arenaTint);
        }
    }
}
