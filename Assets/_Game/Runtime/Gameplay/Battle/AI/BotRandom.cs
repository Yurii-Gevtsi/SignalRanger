namespace Game.Gameplay.Battle.AI
{
    /// <summary>
    /// A tiny random source the bot draws from, wrapped behind an interface so
    /// planning stays deterministic under a seed and is trivially fakeable in
    /// tests. The bot must never touch UnityEngine.Random directly.
    /// </summary>
    public interface IBotRandom
    {
        /// <summary>Uniform value in [0, 1).</summary>
        float NextFloat();
    }

    /// <summary>Seeded <see cref="System.Random"/>-backed implementation.</summary>
    public sealed class SystemBotRandom : IBotRandom
    {
        private readonly System.Random _random;

        public SystemBotRandom(int seed)
        {
            _random = new System.Random(seed);
        }

        public float NextFloat()
        {
            return (float)_random.NextDouble();
        }
    }
}
