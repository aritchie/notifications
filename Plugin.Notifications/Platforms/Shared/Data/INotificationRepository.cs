using System;
using System.Collections.Generic;


namespace Plugin.Notifications.Data
{
    public interface INotificationRepository
    {
        NotificationInfo GetById(int id);
        IEnumerable<NotificationInfo> GetPending();
        int Insert(NotificationRequest notification, DateTime? nextRun);
        void Update(int notificationId, DateTime nextRun);
        void Delete(int id);
        void DeleteAll();
    }
}