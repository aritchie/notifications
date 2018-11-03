using System;
using Plugin.Notifications.Data;


namespace Plugin.Notifications
{
    static class Internals
    {
        public static INotificationRepository Repository { get; set; }
        public static Action<NotificationRequest> NativeSend { get; set; }
    }
}
