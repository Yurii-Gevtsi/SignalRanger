using System;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Game.Infrastructure.Ugs
{
    public sealed class UgsRemoteConfigService : IRemoteConfigService
    {
        private struct UserAttributes
        {
        }

        private struct AppAttributes
        {
        }

        public async void FetchInBackground(Action<bool> onCompleted = null)
        {
            try
            {
                await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
                onCompleted?.Invoke(true);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"[UGS] Remote config fetch failed: {exception.Message}");
                onCompleted?.Invoke(false);
            }
        }

        public bool GetBool(string key, bool fallback)
        {
            var config = RemoteConfigService.Instance.appConfig;
            return config != null && config.HasKey(key) ? config.GetBool(key) : fallback;
        }

        public int GetInt(string key, int fallback)
        {
            var config = RemoteConfigService.Instance.appConfig;
            return config != null && config.HasKey(key) ? config.GetInt(key) : fallback;
        }

        public float GetFloat(string key, float fallback)
        {
            var config = RemoteConfigService.Instance.appConfig;
            return config != null && config.HasKey(key) ? config.GetFloat(key) : fallback;
        }

        public string GetString(string key, string fallback)
        {
            var config = RemoteConfigService.Instance.appConfig;
            return config != null && config.HasKey(key) ? config.GetString(key) : fallback;
        }
    }
}
