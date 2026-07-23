using System;
using Game.App;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

namespace Game.Infrastructure.Ugs
{
    /// <summary>
    /// Самодостатній онлайн-бутстрап: піднімає UGS після старту гри і, якщо
    /// вдалось увійти, підміняє no-op сервіси живими адаптерами. Будь-яка
    /// помилка (проєкт не залінковано, немає мережі) лишає гру в офлайні —
    /// жодних падінь чи блокувань.
    /// </summary>
    internal static class UgsOnlineBootstrap
    {
        private const string DevelopmentEnvironment = "development";
        private const string ProductionEnvironment = "production";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            BootstrapAsync();
        }

        private static async void BootstrapAsync()
        {
            try
            {
                var environment = Debug.isDebugBuild || Application.isEditor
                    ? DevelopmentEnvironment
                    : ProductionEnvironment;
                var options = new InitializationOptions();
                options.SetEnvironmentName(environment);
                await UnityServices.InitializeAsync(options);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"[UGS] Init skipped, staying offline: {exception.Message}");
                return;
            }

            var auth = new UgsAuthService();
            auth.SignIn(success =>
            {
                if (!success)
                {
                    return;
                }

                var remoteConfig = new UgsRemoteConfigService();
                remoteConfig.FetchInBackground();
                AppServices.AttachOnlineServices(auth, new UgsCloudSaveSyncService(), remoteConfig);
                Debug.Log($"[UGS] Online services attached. PlayerId: {auth.PlayerId}");
            });
        }
    }
}
