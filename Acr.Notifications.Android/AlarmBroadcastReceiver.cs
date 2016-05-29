using System;
using Android.App;
using Android.Content;


namespace Acr.Notifications
{
    [BroadcastReceiver]
    [IntentFilter(new[] { Intent.ActionBootCompleted }, Priority = Int32.MaxValue)]
    public class AlarmBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var notification = intent.ToNotification();
            var notificationId = intent.NotificationId();
            NotificationSettings.Instance.RemoveScheduledId(notificationId);

            // resend without schedule so it goes through normal mechanism
            notification.When = null;
            notification.Date = null;
            Notifications.Instance.Send(notification);
        }
    }
}