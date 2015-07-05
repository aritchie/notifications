using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Java.IO;
using Java.Lang;


namespace Acr.Notifications {

    public class NotificationsImpl : AbstractNotificationsImpl {
        private readonly NotificationManager notificationManager;
        private readonly AlarmManager alarmManager;
        private readonly int appIconId;


        // IconId = Resource.Drawable.ic_notification
        //builder.SetLargeIcon (BitmapFactory.DecodeResource (Resources, Resource.Drawable.monkey_icon));
        //builder.SetSound (RingtoneManager.GetDefaultUri(RingtoneType.Alarm));

        public NotificationsImpl() {
            this.appIconId = Application.Context.Resources.GetIdentifier("icon", "drawable", Application.Context.PackageName);
            this.notificationManager = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);
            this.alarmManager = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);
        }


        public override string Send(Notification notification) {
            var id = this.GetNextNotificationId();

            if (notification.IsScheduled) {
                var pending = notification.ToPendingIntent(id);
                var ts = notification.SendTime.Subtract(DateTime.Now);
                var triggerMs = Convert.ToInt64(JavaSystem.CurrentTimeMillis() + ts.TotalMilliseconds);

                this.alarmManager.Set(
                    AlarmType.RtcWakeup,
                    triggerMs,
                    pending
                );
                return id.ToString();
            }

            var builder = new global::Android.App.Notification
                .Builder(Application.Context)
                .SetAutoCancel(true)
                .SetContentTitle(notification.Title)
                .SetContentText(notification.Message)
                .SetSmallIcon(this.appIconId);

            if (notification.Sound != null) {
                var file = new File(notification.Sound);
                var uri = Android.Net.Uri.FromFile(file);
                builder.SetSound(uri);
            }

            notification.Actions.ToList().ForEach(x => {
                var intent = new Intent(x.Identifier);
                var pending = PendingIntent.GetActivity(Application.Context, 0, intent, PendingIntentFlags.OneShot);
                builder.AddAction(0, x.Title, pending);
            });
            var not = builder.Build();
            this.notificationManager.Notify(id, not);
            return id.ToString();
        }


        public override void CancelAll() {

            //this.alarmManager.Cancel()
            this.notificationManager.CancelAll();
        }


        public override bool Cancel(string id) {
            var @int = 0;
            if (!Int32.TryParse(id, out @int))
                return false;

            var pending = Helpers.GetNotificationPendingIntent(@int);
            this.alarmManager.Cancel(pending);
            this.notificationManager.Cancel(@int);
            return true;
        }


        public override void Vibrate(int ms) {
            using (var vibrate = (Vibrator)Application.Context.GetSystemService(Context.VibratorService)) {
                if (!vibrate.HasVibrator)
                    return;

                vibrate.Vibrate(ms);
            }
        }


        private readonly object syncLock = new object();

        protected virtual int GetNextNotificationId() {
            var ctx = Application.Context.ApplicationContext;
            var id = 0;

            lock (this.syncLock) {
                using (var prefs = PreferenceManager.GetDefaultSharedPreferences(ctx)) {
                    id = prefs.GetInt("NotificationId", 0);
                    id++;
                    using (var editor = prefs.Edit()) {
                        editor.PutInt("NotificationId", id);
                        editor.Commit();
                    }
                }
            }
            return id;
        }
    }
}
