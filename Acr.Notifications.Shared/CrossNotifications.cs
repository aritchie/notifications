using System;


namespace Acr.Notifications
{

    public static class CrossNotifications
    {
        static INotifications current;
        public static INotifications Current
        {
            get {
#if PCL
                throw new ArgumentException("Platform implementation not found.  Did you reference the nuget package in your main project as well?");
#else
                current = current ?? new NotificationsImpl();
                return current;
#endif
            }
            set { current = value; }
        }
    }
}
