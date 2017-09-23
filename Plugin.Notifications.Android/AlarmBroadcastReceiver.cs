using System;
using Android.Content;


namespace Plugin.Notifications
{
    [BroadcastReceiver(Enabled = true, Label = "Notifications Broadcast Receiver")]
    public class AlarmBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var notificationId = intent.NotificationId();
            (CrossNotifications.Current as IAndroidNotificationReceiver)?.TriggerScheduledNotification(notificationId);
        }
    }
}