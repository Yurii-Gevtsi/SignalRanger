namespace Game.Infrastructure
{
    public interface IRemoteConfigService
    {
        bool GetBool(string key, bool fallback);
        int GetInt(string key, int fallback);
        float GetFloat(string key, float fallback);
        string GetString(string key, string fallback);
    }
}
