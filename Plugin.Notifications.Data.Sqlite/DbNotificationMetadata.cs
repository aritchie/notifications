using System;
using SQLite;


namespace Plugin.Notifications.Data.Sqlite
{
    public class DbNotificationMetadata
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public int NotificationId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}