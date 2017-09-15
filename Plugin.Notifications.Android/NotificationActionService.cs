using System;
using Android.App;
using Android.Content;
using Android.OS;


namespace Plugin.Notifications
{
    [Service]
    public class NotificationActionService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            if (intent.HasExtra(Constants.ACTION_KEY))
            {
                var notificationId = intent.GetIntExtra(Constants.NOTIFICATION_ID, 0);
                ((NotificationsImpl)CrossNotifications.Current).TriggerNotification(notificationId);
            }
            return null;
        }
    }
}