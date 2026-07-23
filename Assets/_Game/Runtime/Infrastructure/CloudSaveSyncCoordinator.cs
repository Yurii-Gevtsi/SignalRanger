using System;
using Game.Data.Save;

namespace Game.Infrastructure
{
    /// <summary>
    /// Оркеструє синхронізацію локального сейву з хмарою поверх абстракцій
    /// ISaveService/ICloudSyncService: пуш після кожного локального збереження,
    /// зведення за CloudSyncPolicy при вході. Не знає про конкретний бекенд.
    /// </summary>
    public sealed class CloudSaveSyncCoordinator : IDisposable
    {
        private readonly ISaveService _saveService;
        private readonly ICloudSyncService _cloudSync;
        private readonly bool _localIsFreshInstall;

        public CloudSaveSyncCoordinator(ISaveService saveService, ICloudSyncService cloudSync, bool localIsFreshInstall)
        {
            _saveService = saveService ?? throw new ArgumentNullException(nameof(saveService));
            _cloudSync = cloudSync ?? throw new ArgumentNullException(nameof(cloudSync));
            _localIsFreshInstall = localIsFreshInstall;
            _saveService.Saved += OnLocalSaved;
        }

        public CloudSyncDecision LastDecision { get; private set; } = CloudSyncDecision.NoOp;

        /// <summary>
        /// Викликається після успішного входу: тягне хмарний знімок і зводить
        /// його з локальним. AskPlayer поки що консервативно нічого не змінює
        /// (UI вибору гравця з'явиться на етапі 1; жоден бік не перезаписується).
        /// </summary>
        public void OnSignedIn(Action<CloudSyncDecision> onResolved = null)
        {
            if (!_cloudSync.IsAvailable)
            {
                onResolved?.Invoke(CloudSyncDecision.NoOp);
                return;
            }

            _cloudSync.PullSnapshot(remote =>
            {
                var local = _saveService.Current;
                var decision = CloudSyncPolicy.Decide(local, remote, _localIsFreshInstall);
                LastDecision = decision;

                switch (decision)
                {
                    case CloudSyncDecision.PushLocal:
                        _cloudSync.PushSnapshot(local);
                        break;
                    case CloudSyncDecision.AdoptRemote:
                        SaveDataSerializer.OverwriteInto(remote, local);
                        _saveService.Save(local);
                        break;
                }

                onResolved?.Invoke(decision);
            });
        }

        private void OnLocalSaved(SaveData data)
        {
            if (_cloudSync.IsAvailable)
            {
                _cloudSync.PushSnapshot(data);
            }
        }

        public void Dispose()
        {
            _saveService.Saved -= OnLocalSaved;
        }
    }
}
