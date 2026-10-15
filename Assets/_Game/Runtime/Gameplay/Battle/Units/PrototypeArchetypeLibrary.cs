using Game.Gameplay.Battle.AI;

namespace Game.Gameplay.Battle.Units
{
    /// <summary>
    /// The MVP character archetypes and the rules that pick one for a battle.
    /// The player fields a balanced striker; the opponent scales with AI difficulty.
    /// </summary>
    public static class PrototypeArchetypeLibrary
    {
        public static CharacterArchetype Striker => new("archetype.striker", "\u0428\u0442\u0443\u0440\u043c\u043e\u0432\u0438\u043a", 100, 0.95f, 2.4f, 2.9f, 0.42f, 1.95f, 1.15f, 1.15f, 1.4f, 60f);
        public static CharacterArchetype Skirmisher => new("archetype.skirmisher", "\u0420\u043e\u0437\u0432\u0456\u0434\u043d\u0438\u043a", 80, 0.82f, 2.9f, 3.3f, 0.46f, 2.2f, 1.22f, 1.25f, 1.48f, 64f);
        public static CharacterArchetype Bulwark => new("archetype.bulwark", "\u0411\u0430\u0441\u0442\u0456\u043e\u043d", 135, 1.12f, 2.05f, 2.45f, 0.34f, 1.6f, 0.95f, 0.95f, 1.2f, 54f);

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

        public static bool TryGetById(string archetypeId, out CharacterArchetype archetype)
        {
            switch (archetypeId)
            {
                case "archetype.striker":
                    archetype = Striker;
                    return true;
                case "archetype.skirmisher":
                    archetype = Skirmisher;
                    return true;
                case "archetype.bulwark":
                    archetype = Bulwark;
                    return true;
                default:
                    archetype = default;
                    return false;
            }
        }
    }
}
