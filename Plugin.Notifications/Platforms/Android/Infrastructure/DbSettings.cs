using System;
using SQLite;


namespace Plugin.Notifications.Infrastructure
{
    public class DbSettings
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public int CurrentBadge { get; set; }
        public int CurrentScheduleId { get; set; }
    }
}