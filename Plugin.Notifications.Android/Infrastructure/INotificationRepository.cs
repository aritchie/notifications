using System;
using System.Collections.Generic;


namespace Plugin.Notifications.Infrastructure
{
    public interface INotificationRepository
    {
        Notification GetById(int id);
        IEnumerable<Notification> GetScheduled();
        void Insert(Notification notification);
        void Delete(int id);
        void DeleteAll();

        int CurrentScheduleId { get; set; }
        int CurrentBadge { get; set; }
    }
}