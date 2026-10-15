namespace Game.Infrastructure
{
    public sealed class NoOpRemoteConfigService : IRemoteConfigService
    {
        public bool GetBool(string key, bool fallback)
        {
            return fallback;
        }

        public int GetInt(string key, int fallback)
        {
            return fallback;
        }

        public float GetFloat(string key, float fallback)
        {
            return fallback;
        }

        public string GetString(string key, string fallback)
        {
            return fallback;
        }
    }
}
