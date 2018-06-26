using System;


namespace Plugin.Notifications
{
    public static partial class CrossNotifications
    {
        static CrossNotifications()
        {
            Current = new NotificationsImpl();
        }
    }
}
