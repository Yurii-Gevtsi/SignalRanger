using UnityEngine;

namespace Game.App.Content
{
    [CreateAssetMenu(menuName = "Game/Content/Difficulty Profile Definition", fileName = "DifficultyProfileDefinition")]
    public sealed class DifficultyProfileDefinition : ScriptableObject
    {
        [SerializeField] private string _profileId = "difficulty.easy";
        [SerializeField] private int _aiDifficultyTier;
        [SerializeField] private string _aiProfileId = "ai.easy";
        [SerializeField] private string _opponentArchetypeId = "archetype.skirmisher";

        public string ProfileId => _profileId;
        public int AiDifficultyTier => Mathf.Clamp(_aiDifficultyTier, 0, 2);
        public string AiProfileId => _aiProfileId;
        public string OpponentArchetypeId => _opponentArchetypeId;

        public static DifficultyProfileDefinition CreateRuntime(
            string profileId,
            int aiDifficultyTier,
            string aiProfileId,
            string opponentArchetypeId)
        {
            var definition = CreateInstance<DifficultyProfileDefinition>();
            definition._profileId = profileId ?? string.Empty;
            definition._aiDifficultyTier = Mathf.Clamp(aiDifficultyTier, 0, 2);
            definition._aiProfileId = aiProfileId ?? string.Empty;
            definition._opponentArchetypeId = opponentArchetypeId ?? string.Empty;
            return definition;
        }
    }
}
