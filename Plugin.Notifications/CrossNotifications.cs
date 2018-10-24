using System;


namespace Plugin.Notifications
{

    public static partial class CrossNotifications
    {
        static INotificationManager current;
        public static INotificationManager Current
        {
            get
            {
                if (current == null)
                    throw new ArgumentException("[Plugin.Notifications] Platform implementation not found.  Did you reference the nuget package in your main project as well?");

                return current;
            }
            set => current = value;
        }
    }
}
