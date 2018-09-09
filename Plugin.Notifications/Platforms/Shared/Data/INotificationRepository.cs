using System;
using System.Collections.Generic;


namespace Plugin.Notifications.Data
{
    public interface INotificationRepository
    {
        Notification GetById(string id);
        IEnumerable<Notification> GetPending();
        void Insert(Notification notification);
        void Delete(string id);
        void DeleteAll();
    }
}