using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;


namespace Plugin.Notifications.Data
{
    public class SqliteNotificationRepository : INotificationRepository
    {
        readonly PluginSqliteConnection conn = new PluginSqliteConnection();


        public NotificationInfo GetById(int id)
        {
            var db = this.conn.Notifications.FirstOrDefault(x => x.Id == id);
            if (db == null)
                return null;

            return this.ToInfo(db);
        }


        public IEnumerable<NotificationInfo> GetPending() => this
            .conn
            .Notifications
            .ToList()
            .Select(this.ToInfo);


        public IEnumerable<NotificationInfo> GetDueToBeTriggered() => this
            .conn
            .Notifications
            .Where(x => x.NextTriggerDate != null && x.NextTriggerDate <= DateTime.Now)
            .ToList()
            .Select(this.ToInfo);


        public int Insert(NotificationRequest notification, DateTime? nextRun)
        {
            var db = new DbNotification
            {
                Title = notification.Title,
                Message = notification.Message,
                Payload = notification.Payload,
                AndroidJson = JsonConvert.SerializeObject(notification.Android),
                RequestJson = JsonConvert.SerializeObject(notification.Trigger),
                NextTriggerDate = nextRun,
                TriggerType = notification.Trigger.GetType().AssemblyQualifiedName
            };
            this.conn.Insert(db);
            return db.Id;
        }


        public void Update(int notificationId, DateTime nextRun)
        {
            var db = this.conn.Get<DbNotification>(notificationId);
            if (db != null)
            {
                db.NextTriggerDate = nextRun;
                this.conn.Update(db);
            }
        }


        public void Delete(int id)
            => this.conn.Notifications.Delete(x => x.Id == id);


        public void DeleteAll()
            => this.conn.DeleteAll<DbNotification>();

        //public int CurrentBadge
        //{
        //    get => this.settings.CurrentBadge;
        //    set
        //    {
        //        if (this.settings.CurrentBadge.Equals(value))
        //            return;

        //        this.settings.CurrentBadge = value;
        //        this.conn.Update(this.settings);
        //    }
        //}


        public NotificationInfo ToInfo(DbNotification db)
        {
            var triggerType = Type.GetType(db.TriggerType);
            var trigger = (INotificationTrigger)JsonConvert.DeserializeObject(db.RequestJson, triggerType);
            var droidOpts = JsonConvert.DeserializeObject<AndroidOptions>(db.AndroidJson);

            var request = new NotificationRequest
            {
                Title = db.Title,
                Message = db.Message,
                Payload = db.Payload,
                Sound = db.Sound,
                Trigger = trigger,
                Android = droidOpts
            };

            return new NotificationInfo(db.Id, db.NextTriggerDate, request);
        }
    }
}