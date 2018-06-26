using System;


namespace Plugin.Notifications.Platforms.Android
{
    public static partial class CrossNotifications
    {
        static CrossNotifications()
        {
            Current = new NotificationsImpl();
        }
    }
}
