using System;
using Android.App;
using Android.Content;


namespace Acr.Notifications {

    public static class Helpers {

        public static Intent TryPut(this Intent intent, string key, string value) {
            if (value == null)
                return intent;

            intent.PutExtra(key, value);
            return intent;
        }


        public static string TryGet(this Intent intent, string key) {
            return intent.HasExtra(key)
                ? intent.GetStringExtra(key)
                : null;
        }


        public static PendingIntent ToPendingIntent(this Notification notification, int id) {
            var intent = new Intent(Application.Context.ApplicationContext, typeof(AlarmBroadcastReceiver))
                .TryPut("title", notification.Title)
                .TryPut("message", notification.Message)
                .TryPut("sound", notification.Sound);

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
            return new Notification {
                Title = intent.TryGet("title"),
                Message = intent.TryGet("message"),
                Sound = intent.TryGet("sound")
            };
        }
    }
}