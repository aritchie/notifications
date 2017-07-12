using System;
using SQLite;


namespace Plugin.Notifications
{
    public class DbNotification
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
        public string Sound { get; set; }
        public bool Vibrate { get; set; }

        public DateTime DateScheduled { get; set; }
    }
}