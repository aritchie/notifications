using System;
using System.Collections.Generic;
using System.Linq;


namespace Plugin.Notifications.Data
{
    public class SqliteNotificationRepository : INotificationRepository
    {
        readonly PluginSqliteConnection conn = new PluginSqliteConnection();


        public Notification GetById(int id)
        {
            var db = this.conn.Notifications.FirstOrDefault(x => x.Id == id);
            if (db == null)
                return null;

            return new Notification
            {
                Id = db.Id,
                Title = db.Title,
                Message = db.Message,
                Sound = db.Sound
                //Vibrate = db.Vibrate
            };
        }


        public IEnumerable<Notification> GetPending()
        {
            var nots = this.conn.Notifications.ToList();

            return nots.Select(x => new Notification
            {
                Id = x.Id,
                Title = x.Title,
                Message = x.Message,
                Sound = x.Sound
                //Vibrate = x.Vibrate
            });
        }


        public void Insert(Notification notification) => this.conn.Insert(new DbNotification
        {
            Id = notification.Id,
            Title = notification.Title,
            Message = notification.Message,
            Sound = notification.Sound
            //Vibrate = notification.Vibrate
        });


        public void Delete(string id)
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
    }
}