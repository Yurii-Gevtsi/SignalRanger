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
        private const string LaserKillZone = "Visuals/Arenas/KillZones/LaserLine/laser_line";
        private const float DesertKillZoneY = -9.8f;
        private const string PlayerCharacter = "";
        private const string OpponentCharacter = "";
        private const string PlayerRig = "Visuals/Characters/DroidRig";
        private const string OpponentRig = "Visuals/Characters/SigmaRig";

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
                    overlayLayers: new[]
                    {
                        KillZoneLayer(DesertKillZoneY)
                    },
                    killZoneY: DesertKillZoneY);
            }

            if (arena.ArenaId == ForestArenaId)
            {
                return new BattleVisualTheme(
                    new BattleVisualLayer(ForestCosmosBackground, new Vector2(0f, 1.1f), Vector2.one, -30, coverCamera: true),
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
                    overlayLayers: new[]
                    {
                        KillZoneLayer(-7.35f)
                    },
                    killZoneY: -7.35f);
            }

            var background = arena.ArenaId switch { "arena.city" => CityBackground, "arena.grib" => GribBackground, "arena.laguna" => LagunaBackground, _ => string.Empty };
            if (!string.IsNullOrEmpty(background))
            {
                return new BattleVisualTheme(new BattleVisualLayer(background, Vector2.zero, Vector2.one, -30, coverCamera: true), default, PlayerCharacter, OpponentCharacter, new Vector2(0.18f, 0.18f), new Vector2(0f, -0.08f), true, false, playerRigResourcePath: PlayerRig, opponentRigResourcePath: OpponentRig);
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
                additionalDestructibleLayers: new[]
                {
                    new BattleVisualLayer(TrainingShip, new Vector2(-9.66498f, -0.53610f), new Vector2(2.05578f, 1.88287f), -7, true),
                    new BattleVisualLayer(TrainingBoxes, new Vector2(0.83379f, -2.39658f), new Vector2(0.97620f, 1.00449f), -7, true),
                    new BattleVisualLayer(TrainingRock, new Vector2(9.25f, -2.35f), new Vector2(1.85961f, 1.80162f), -7, true)
                },
                overlayLayers: new[]
                {
                    KillZoneLayer(-5.05f)
                },
                // The bright core of the laser_line art (sprite top ~-4.87,
                // beam ~0.2 of the band below it). Feet below this line = death.
                killZoneY: -5.05f);
        }

        private static BattleVisualLayer KillZoneLayer(float lethalY)
        {
            // The laser core is 0.7 world units above this wide overlay's centre.
            return new BattleVisualLayer(LaserKillZone, new Vector2(0f, lethalY - 0.7f), new Vector2(5.7f, 4.55f), 45);
        }
    }
}
