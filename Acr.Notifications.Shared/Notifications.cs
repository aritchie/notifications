using System;


namespace Acr.Notifications {

    public static class Notifications {
        public static INotifications Instance { get; set; }

#if __PLATFORM__

        public static void Init() {
            if (Instance == null)
                Instance = new NotificationsImpl();
        }
#else

        [Obsolete("ERROR: You are calling the PCL version of Init")]
        public static void Init() {
            throw new ArgumentException("You are calling the PCL version of Init");
        }
#endif
    }
}
