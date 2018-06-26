using System;
using System.Collections.Generic;
using System.Linq;


namespace Plugin.Notifications.Infrastructure
{
    public class SqliteNotificationRepository : INotificationRepository
    {
        readonly AcrSqliteConnection conn;
        readonly DbSettings settings;


        public SqliteNotificationRepository()
        {
            this.conn = new AcrSqliteConnection();
            this.settings = this.conn.Settings.SingleOrDefault();
            if (this.settings == null)
            {
                this.settings = new DbSettings();
                this.conn.Insert(this.settings);
            }
        }


        public Notification GetById(int id)
        {
            var db = this.conn.Notifications.FirstOrDefault(x => x.Id == id);
            if (db == null)
                return null;

            var mds = this.conn
                .NotificationMetadata
                .Where(x => x.NotificationId == id)
                .ToList();

            return new Notification
            {
                Id = db.Id,
                Title = db.Title,
                Message = db.Message,
                Sound = db.Sound,
                Vibrate = db.Vibrate,
                ScheduledDate = db.DateScheduled,
                Metadata = mds
                    .ToDictionary(
                        y => y.Key,
                        y => y.Value
                    )
            };
        }


        public IEnumerable<Notification> GetScheduled()
        {
            var nots = this.conn.Notifications.ToList();
            var mds = this.conn.NotificationMetadata.ToList();
            return nots.Select(x => new Notification
            {
                Id = x.Id,
                Title = x.Title,
                Message = x.Message,
                Sound = x.Sound,
                Vibrate = x.Vibrate,
                ScheduledDate = x.DateScheduled,
                Metadata = mds
                    .Where(y => y.NotificationId == x.Id)
                    .ToDictionary(
                        y => y.Key,
                        y => y.Value
                    )
            });
        }


        public void Insert(Notification notification)
        {
            try
            {

                this.conn.BeginTransaction();
                var db = new DbNotification
                {
                    Id = notification.Id.Value,
                    Title = notification.Title,
                    Message = notification.Message,
                    Sound = notification.Sound,
                    Vibrate = notification.Vibrate,
                    DateScheduled = notification.ScheduledDate
                };
                this.conn.Insert(db);

                foreach (var pair in notification.Metadata)
                {
                    this.conn.Insert(new DbNotificationMetadata
                    {
                        NotificationId = db.Id,
                        Key = pair.Key,
                        Value = pair.Value
                    });
                }
                this.conn.Commit();
            }
            catch
            {
                this.conn.Rollback();
                throw;
            }
        }


        public void Delete(int id)
        {
            this.conn.Notifications.Delete(x => x.Id == id);
            this.conn.NotificationMetadata.Delete(x => x.NotificationId == id);
        }


        public void DeleteAll()
        {
            this.conn.DeleteAll<DbNotification>();
            this.conn.DeleteAll<DbNotificationMetadata>();
        }


        public int CurrentScheduleId
        {
            get => this.settings.CurrentScheduleId;
            set
            {
                if (this.settings.CurrentScheduleId.Equals(value))
                    return;

                this.settings.CurrentScheduleId = value;
                this.conn.Update(this.settings);
            }
        }


        public int CurrentBadge
        {
            get => this.settings.CurrentBadge;
            set
            {
                if (this.settings.CurrentBadge.Equals(value))
                    return;

                this.settings.CurrentBadge = value;
                this.conn.Update(this.settings);
            }
        }
    }
}