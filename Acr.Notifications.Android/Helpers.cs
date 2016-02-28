using System;
using Android.App;
using Android.Content;
using Newtonsoft.Json;


namespace Acr.Notifications {

    public static class Helpers {
        const string DATA_KEY = "data";
        const string NOTIFICATION_ID = "id";


        public static PendingIntent ToPendingIntent(this Notification notification, int id) {
            var json = JsonConvert.SerializeObject(notification);
            var intent = new Intent(Application.Context, typeof(AlarmBroadcastReceiver))
                .PutExtra(NOTIFICATION_ID, id)
                .PutExtra(DATA_KEY, json);
            var pending = PendingIntent.GetBroadcast(Application.Context, id, intent, PendingIntentFlags.OneShot);
            return pending;
        }


        public static PendingIntent GetNotificationPendingIntent(int id) {
            var intent = new Intent(Application.Context, typeof(AlarmBroadcastReceiver))
                .PutExtra(NOTIFICATION_ID, id)
                .PutExtra(DATA_KEY, "hack");
            var pending = PendingIntent.GetBroadcast(Application.Context, id, intent, PendingIntentFlags.OneShot);
            return pending;
        }


        public static Notification ToNotification(this Intent intent) {
            if (!intent.HasExtra(DATA_KEY))
                throw new ArgumentException("Invalid intent package");

            var data = intent.GetStringExtra(DATA_KEY);
            var notification = JsonConvert.DeserializeObject<Notification>(data);

            // nullify date so it doesn't just reschedule
            notification.Date = null;
            notification.When = null;
            return notification;
        }


        public static int NotificationId(this Intent intent)
        {
            return intent.GetIntExtra(NOTIFICATION_ID, 0);
        }
    }
}