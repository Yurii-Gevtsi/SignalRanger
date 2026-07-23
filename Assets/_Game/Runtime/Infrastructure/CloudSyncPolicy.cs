using System;
using Game.Data.Save;

namespace Game.Infrastructure
{
    public enum CloudSyncDecision
    {
        PushLocal,
        AdoptRemote,
        AskPlayer,
        NoOp
    }

    /// <summary>
    /// Правило зведення локального і хмарного сейву при вході
    /// (див. docs/architecture/backend-online-services.md, «Синхронізація сейву»).
    /// </summary>
    public static class CloudSyncPolicy
    {
        /// <summary>
        /// Розбіжність, більша за цей поріг, вважається суттєвою:
        /// автоматичний вибір поступається місцем вибору гравця.
        /// </summary>
        public static readonly TimeSpan SignificantDivergence = TimeSpan.FromHours(24);

        public static CloudSyncDecision Decide(SaveData local, SaveData remote, bool localIsFreshInstall)
        {
            if (remote == null)
            {
                return local == null ? CloudSyncDecision.NoOp : CloudSyncDecision.PushLocal;
            }

            if (local == null || localIsFreshInstall)
            {
                return CloudSyncDecision.AdoptRemote;
            }

            if (remote.schemaVersion > SaveData.CurrentSchemaVersion)
            {
                // Хмарний сейв із новішої версії гри: не чіпати і не перезаписувати.
                return CloudSyncDecision.NoOp;
            }

            var localTicks = local.lastWriteUtcTicks;
            var remoteTicks = remote.lastWriteUtcTicks;
            if (remoteTicks == localTicks)
            {
                return CloudSyncDecision.NoOp;
            }

            var divergence = TimeSpan.FromTicks(Math.Abs(remoteTicks - localTicks));
            if (remoteTicks > localTicks)
            {
                return divergence > SignificantDivergence
                    ? CloudSyncDecision.AskPlayer
                    : CloudSyncDecision.AdoptRemote;
            }

            return CloudSyncDecision.PushLocal;
        }
    }
}
