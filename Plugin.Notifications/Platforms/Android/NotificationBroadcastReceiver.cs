using System;
using Android.App;
using Android.Content;


namespace Plugin.Notifications
{
    [BroadcastReceiver]
    public class NotificationBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.HasExtra(Constants.ACTION_KEY))
            {
                var notificationId = intent.GetIntExtra(Constants.NOTIFICATION_ID, 0);
                (CrossNotifications.Current as IAndroidNotificationReceiver)?.TriggerNotification(notificationId);
            }
        }


        static NotificationBroadcastReceiver instance;
        public static void Register()
        {
            if (instance != null)
                return;

            var filter = new IntentFilter();
            //filter.AddAction($"{Application.Context.PackageName}.{Constants.ACTION_KEY}");
            filter.AddAction(Constants.ACTION_KEY);
            instance = new NotificationBroadcastReceiver();
            Application.Context.RegisterReceiver(instance, filter);
        }


        public static void UnRegister()
        {
            if (instance == null)
                return;

            Application.Context.UnregisterReceiver(instance);
            instance = null;
        }
    }
}