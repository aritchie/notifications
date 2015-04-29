using System;


namespace Acr.Notifications {

    public static class Notifications {
        private static readonly Lazy<INotifications> instanceInit = new Lazy<INotifications>(() => {
#if __PLATFORM__
            return new NotificationsImpl();
#else
            throw new ArgumentException("Platform implementation not found.  Did you reference the nuget package in your main project as well?");
#endif
        }, false);


        private static INotifications customInstance;
        public static INotifications Instance {
            get { return customInstance ?? instanceInit.Value; }
            set { customInstance = value; }
        }
    }
}
