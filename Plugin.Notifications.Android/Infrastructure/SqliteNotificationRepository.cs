using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


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
            throw new NotImplementedException();
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
                Date = x.DateScheduled,
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
            throw new NotImplementedException();
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