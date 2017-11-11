using System;


namespace Plugin.Notifications.Data.EfCore
{
    public class DbNotificationMetadata
    {
        public int Id { get; set; }

        public int NotificationId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}