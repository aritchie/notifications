using System;
using Android.App;
using Android.Content;
using Android.OS;


namespace Acr.Notifications {

    public class NotificationsImpl : INotifications {
        private readonly NotificationManager notificationManager;
        private readonly int appIconId;

        // IconId = Resource.Drawable.ic_notification
        //builder.SetLargeIcon (BitmapFactory.DecodeResource (Resources, Resource.Drawable.monkey_icon));

        public NotificationsImpl() {
            this.notificationManager = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);
            //this.alarmManager = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);
            this.appIconId = Application.Context.Resources.GetIdentifier("icon", "drawable", Application.Context.PackageName);
        }


        public void Send(string title, string message, string sound = null, TimeSpan? when = null) {
            var builder = new Notification
                .Builder(Application.Context)
                .SetContentTitle(title)
                .SetContentText(message)
                //.SetSound() // TODO: Uri:  builder.SetSound (RingtoneManager.GetDefaultUri(RingtoneType.Alarm));
                .SetSmallIcon(this.appIconId);

            //if (when != null) {
            //    builder.SetWhen(when.Value.Ticks);
            //}

            var notification = builder.Build();
            this.notificationManager.Notify(0, notification);

    //Intent alarmIntent = new Intent(Forms.Context, typeof(AlarmReceiver));
    //alarmIntent.PutExtra ("message", message);
    //alarmIntent.PutExtra ("title", title);

    //PendingIntent pendingIntent = PendingIntent.GetBroadcast(Forms.Context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
    //

    ////TODO: For demo set after 5 seconds.
    //alarmManager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime () + 5 * 1000, pendingIntent);
        }


        public void CancelAll() {
            this.notificationManager.CancelAll();
        }


        public int Badge { get; set; }


        public void Vibrate(int ms) {
            using (var vibrate = (Vibrator)Application.Context.GetSystemService(Context.VibratorService)) {
                if (!vibrate.HasVibrator)
                    return;

                vibrate.Vibrate(ms);
            }
        }
    }
}
