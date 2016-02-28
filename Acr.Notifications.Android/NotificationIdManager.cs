using System;
using System.Collections.Generic;
using Acr.Settings;


namespace Acr.Notifications
{
    public class NotificationIdManager
    {
        const string SCHEDULED_IDS = "ScheduledIds";
        const string NOTIFICATION_ID = "NotificationId";


        public static NotificationIdManager Instance { get; } = new NotificationIdManager();
        readonly ISettings settings;


        NotificationIdManager()
        {
            this.settings = Settings.Settings.Local;
            this.settings.KeysNotToClear.AddRange(new[]
            {
                SCHEDULED_IDS,
                NOTIFICATION_ID
            });
        }


        public void AddScheduledId(int id)
        {
            var ids = this.GetScheduledIds();
            ids.Add(id);
            this.settings.Set(SCHEDULED_IDS, ids);
        }


        public void RemoveScheduledId(int id)
        {
            var ids = this.GetScheduledIds();
            if (ids.Remove(id))
                this.settings.Set(SCHEDULED_IDS, ids);
        }


        public IList<int> GetScheduledIds()
        {
            return this.settings.Get(SCHEDULED_IDS, new List<int>());
        }


        public void ClearAllScheduled()
        {
            this.settings.Remove(SCHEDULED_IDS);
        }


        readonly object syncLock = new object();
        public int GetNextId()
        {
            var id = 0;

            lock (this.syncLock)
            {
                id = this.settings.Get(NOTIFICATION_ID, 0);
                id++;
                this.settings.Set(NOTIFICATION_ID, id);
            }
            return id;
        }
    }
}