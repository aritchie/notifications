using System;
using System.Collections.Generic;


namespace Plugin.Notifications.Data.EfCore
{
    public class EfCoreNotificationRepository : INotificationRepository
    {
        public Notification GetById(int id)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<Notification> GetScheduled()
        {
            throw new NotImplementedException();
        }


        public void Insert(Notification notification)
        {
            throw new NotImplementedException();
        }


        public void Delete(int id)
        {
            throw new NotImplementedException();
        }


        public void DeleteAll()
        {
            throw new NotImplementedException();
        }


        public int CurrentScheduleId { get; set; }
        public int CurrentBadge { get; set; }
    }
}
