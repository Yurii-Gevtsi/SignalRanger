using Game.Gameplay.Battle.Arenas;
using UnityEngine;

namespace Game.Gameplay.Battle.Visuals
{
    public static class BattleVisualThemeLibrary
    {
        private const string FirstArenaId = "arena.training-meadow";
        private const string FruitGardenBackground = "Visuals/Arenas/Background2";
        private const string FruitGardenForeground = "Visuals/Arenas/train2";
        private const string PlayerCharacter = "Visuals/Characters/racoon-full";
        private const string OpponentCharacter = "Visuals/Characters/oct_full";
        private const string PlayerRig = "Visuals/Characters/RaccoonRig";
        private const string OceanKillZone = "Visuals/Arenas/KillZones/Ocean/Prefabs/KillZone_Ocean";

        public static BattleVisualTheme ForArena(ArenaDefinition arena)
        {
            if (arena == null || arena.ArenaId != FirstArenaId)
            {
                return null;
            }

            return new BattleVisualTheme(
                new BattleVisualLayer(FruitGardenBackground, new Vector2(0f, 1.45f), Vector2.one, -30, false, true),
                new BattleVisualLayer(FruitGardenForeground, new Vector2(0f, -2f), new Vector2(1.8f, 1.8f), -8, true),
                PlayerCharacter,
                OpponentCharacter,
                new Vector2(0.18f, 0.18f),
                new Vector2(0f, 0.23f),
                true,
                false,
                playerSkinFacingDirectionX: -1,
                opponentSkinFacingDirectionX: 1,
                playerRigResourcePath: PlayerRig,
                killZonePrefabResourcePath: OceanKillZone,
                killZoneDeathY: -5.75f);
        }
    }
}
