using System;
using Android.App;
using Android.Content;
using Newtonsoft.Json;


namespace Acr.Notifications {

    public static class Helpers {
        private const string DATA_KEY = "data";


        public static PendingIntent ToPendingIntent(this Notification notification, int id) {
            var json = JsonConvert.SerializeObject(notification);
            var intent = new Intent(Application.Context.ApplicationContext, typeof(AlarmBroadcastReceiver)).PutExtra(DATA_KEY, json);
            var pending = PendingIntent.GetBroadcast(Application.Context, 0, intent, PendingIntentFlags.CancelCurrent);
            return pending;
        }


        public static PendingIntent GetNotificationPendingIntent(int id) {
            var intent = new Intent(Application.Context.ApplicationContext, typeof(AlarmBroadcastReceiver));
            intent.SetData(Android.Net.Uri.Parse("notification://" + id));

            var pending = PendingIntent.GetBroadcast(Application.Context, 0, intent, PendingIntentFlags.CancelCurrent);
            return pending;
        }


        public static Notification FromIntent(this Intent intent) {
            if (!intent.HasExtra(DATA_KEY))
                throw new ArgumentException("Invalid intent package");

            var data = intent.GetStringExtra(DATA_KEY);
            var notification = JsonConvert.DeserializeObject<Notification>(data);

            // nullify date so it doesn't just reschedule
            notification.Date = null;
            notification.When = null;
            return notification;
        }
    }
}