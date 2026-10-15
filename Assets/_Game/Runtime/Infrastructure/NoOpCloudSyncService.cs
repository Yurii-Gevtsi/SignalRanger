using System;
using Game.Data.Save;

namespace Game.Infrastructure
{
    public sealed class NoOpCloudSyncService : ICloudSyncService
    {
        public bool IsAvailable => false;

        public void PushSnapshot(SaveData saveData, Action<bool> onCompleted = null)
        {
            onCompleted?.Invoke(false);
        }

        public void PullSnapshot(Action<SaveData> onCompleted)
        {
            onCompleted?.Invoke(null);
        }
    }
}
