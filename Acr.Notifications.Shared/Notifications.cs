using System;


namespace Acr.Notifications {

    public static class Notifications {
        static readonly Lazy<INotifications> instanceInit = new Lazy<INotifications>(() => {
#if PCL
            throw new ArgumentException("Platform implementation not found.  Did you reference the nuget package in your main project as well?");
#else
            return new NotificationsImpl();
#endif
        }, false);


        static INotifications customInstance;
        public static INotifications Instance {
            get { return customInstance ?? instanceInit.Value; }
            set { customInstance = value; }
        }
    }
}
