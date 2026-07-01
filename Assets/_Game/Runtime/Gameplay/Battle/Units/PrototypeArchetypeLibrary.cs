using Game.Gameplay.Battle.AI;

namespace Game.Gameplay.Battle.Units
{
    /// <summary>
    /// The MVP character archetypes and the rules that pick one for a battle.
    /// The player fields a balanced striker; the opponent scales with AI difficulty.
    /// </summary>
    public static class PrototypeArchetypeLibrary
    {
        public static CharacterArchetype Striker => new("archetype.striker", "Штурмовик", 100, 0.95f);
        public static CharacterArchetype Skirmisher => new("archetype.skirmisher", "Розвідник", 80, 0.82f);
        public static CharacterArchetype Bulwark => new("archetype.bulwark", "Бастіон", 135, 1.12f);

        public static CharacterArchetype[] CreateAll()
        {
            return new[] { Striker, Skirmisher, Bulwark };
        }

        public static CharacterArchetype PlayerArchetype => Striker;

        public static CharacterArchetype ForDifficulty(AiDifficulty difficulty)
        {
            return difficulty switch
            {
                AiDifficulty.Hard => Bulwark,
                AiDifficulty.Normal => Striker,
                _ => Skirmisher
            };
        }
    }
}
