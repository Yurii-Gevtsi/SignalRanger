using System;
using System.Collections.Generic;
using Game.Data.Save;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;

namespace Game.Infrastructure.Ugs
{
    /// <summary>
    /// Дзеркало SaveData в Cloud Save під одним ключем. Джерело істини для
    /// геймплею лишається локальним; зведення виконує CloudSaveSyncCoordinator.
    /// </summary>
    public sealed class UgsCloudSaveSyncService : ICloudSyncService
    {
        private const string SaveKey = "save";

        public bool IsAvailable =>
            UnityServices.State == ServicesInitializationState.Initialized
            && AuthenticationService.Instance.IsSignedIn;

        public async void PushSnapshot(SaveData saveData, Action<bool> onCompleted = null)
        {
            if (saveData == null)
            {
                onCompleted?.Invoke(false);
                return;
            }

            try
            {
                var json = SaveDataSerializer.ToJson(saveData, false);
                await CloudSaveService.Instance.Data.Player.SaveAsync(
                    new Dictionary<string, object> { [SaveKey] = json });
                onCompleted?.Invoke(true);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"[UGS] Cloud save push failed: {exception.Message}");
                onCompleted?.Invoke(false);
            }
        }

        public async void PullSnapshot(Action<SaveData> onCompleted)
        {
            try
            {
                var items = await CloudSaveService.Instance.Data.Player.LoadAsync(
                    new HashSet<string> { SaveKey });
                if (!items.TryGetValue(SaveKey, out var item))
                {
                    onCompleted?.Invoke(null);
                    return;
                }

                var json = item.Value.GetAs<string>();
                onCompleted?.Invoke(string.IsNullOrWhiteSpace(json) ? null : SaveDataSerializer.FromJson(json));
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"[UGS] Cloud save pull failed: {exception.Message}");
                onCompleted?.Invoke(null);
            }
        }
    }
}
