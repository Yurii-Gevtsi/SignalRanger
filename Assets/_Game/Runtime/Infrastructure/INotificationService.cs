using System;

namespace Game.Infrastructure
{
    public interface INotificationService
    {
        void ScheduleLocal(string notificationId, string title, string body, DateTime utcTime);
        void Cancel(string notificationId);
        void CancelAll();
    }
}
