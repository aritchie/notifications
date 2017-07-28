using System;
using Plugin.Notifications.Infrastructure;


namespace Plugin.Notifications
{
    public static class Services
    {
        public static INotificationRepository Repository { get; set; } = new SqliteNotificationRepository();
    }
}