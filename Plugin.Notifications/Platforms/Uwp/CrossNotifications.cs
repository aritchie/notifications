using System;
using Plugin.Jobs;


namespace Plugin.Notifications
{
    public static partial class CrossNotifications
    {
        public static void Init()
        {
            CrossJobs.Init();
            Current = new NotificationManagerImpl();
        }
    }
}
