using System;
using Game.Data.Save;

namespace Game.Infrastructure
{
    public interface ICloudSyncService
    {
        bool IsAvailable { get; }
        void PushSnapshot(SaveData saveData, Action<bool> onCompleted = null);
        void PullSnapshot(Action<SaveData> onCompleted);
    }
}
