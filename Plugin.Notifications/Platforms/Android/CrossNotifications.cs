using System;
using Android.App;
using Android.OS;
using Plugin.Jobs;


namespace Plugin.Notifications
{
    public static partial class CrossNotifications
    {
        public static void Init(Activity activity, Bundle savedInstanceState)
        {
            CrossJobs.Init(activity, savedInstanceState);
            Current = new NotificationManagerImpl();
        }
    }
}
