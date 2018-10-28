using System;
using System.Collections.Generic;


namespace Plugin.Notifications.Data
{
    public interface INotificationRepository
    {
        Notification GetById(int id);
        IEnumerable<Notification> GetPending();
        void Insert(Notification notification);
        void Delete(int id);
        void DeleteAll();
    }
}