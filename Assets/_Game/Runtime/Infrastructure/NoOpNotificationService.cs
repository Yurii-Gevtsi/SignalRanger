using System;

namespace Game.Infrastructure
{
    public sealed class NoOpNotificationService : INotificationService
    {
        public void ScheduleLocal(string notificationId, string title, string body, DateTime utcTime)
        {
        }

        public void Cancel(string notificationId)
        {
        }

        public void CancelAll()
        {
        }
    }
}
