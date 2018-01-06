using System;
using Android.Content;


namespace Plugin.Notifications
{
    [BroadcastReceiver]
    public class NotificationActionService : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.HasExtra(Constants.ACTION_KEY))
            {
                var notificationId = intent.GetIntExtra(Constants.NOTIFICATION_ID, 0);
                (CrossNotifications.Current as IAndroidNotificationReceiver)?.TriggerNotification(notificationId);
            }
        }
    }
}