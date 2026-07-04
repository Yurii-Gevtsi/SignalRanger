using Game.Gameplay.Battle.Arenas;
using UnityEngine;

namespace Game.Gameplay.Battle.Visuals
{
    public static class BattleVisualThemeLibrary
    {
        private const string FirstArenaId = "arena.training-meadow";
        private const string FruitGardenBackground = "Visuals/Arenas/fruit-garden-background";
        private const string FruitGardenForeground = "Visuals/Arenas/fruit-garden-foreground";
        private const string PlayerCharacter = "Visuals/Characters/raccoon-agent-clean";
        private const string OpponentCharacter = "Visuals/Characters/octopus-enemy-clean";

        public static BattleVisualTheme ForArena(ArenaDefinition arena)
        {
            if (arena == null || arena.ArenaId != FirstArenaId)
            {
                return null;
            }

            return new BattleVisualTheme(
                new BattleVisualLayer(FruitGardenBackground, new Vector2(0f, 1.2f), new Vector2(2.08f, 2.08f), -30, false, true),
                new BattleVisualLayer(FruitGardenForeground, new Vector2(0f, 1.5f), new Vector2(1.76f, 1.76f), -8, true),
                PlayerCharacter,
                OpponentCharacter,
                new Vector2(0.18f, 0.18f),
                new Vector2(0f, 0.23f),
                true,
                false);
        }
    }
}
