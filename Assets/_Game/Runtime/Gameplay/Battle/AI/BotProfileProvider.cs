using UnityEngine;

namespace Game.Gameplay.Battle.AI
{
    /// <summary>
    /// Resolves the bot profile for a difficulty. Designers can drop an authored
    /// <see cref="BotProfile"/> asset at <c>Resources/Bot/BotProfile.&lt;Difficulty&gt;</c>
    /// to override balance without code; when none is present the runtime
    /// defaults from <see cref="BotProfileLibrary"/> keep the game playable.
    /// </summary>
    public static class BotProfileProvider
    {
        public static string ResourcePath(AiDifficulty difficulty) => $"Bot/BotProfile.{difficulty}";

        public static BotProfile Resolve(AiDifficulty difficulty)
        {
            var authored = Resources.Load<BotProfile>(ResourcePath(difficulty));
            return authored != null ? authored : BotProfileLibrary.For(difficulty);
        }
    }
}
