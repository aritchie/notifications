using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Java.IO;


namespace Acr.Notifications {

    public class NotificationsImpl : INotifications {
        private readonly NotificationManager notificationManager;
        private readonly int appIconId;

        // IconId = Resource.Drawable.ic_notification
        //builder.SetLargeIcon (BitmapFactory.DecodeResource (Resources, Resource.Drawable.monkey_icon));

        public NotificationsImpl() {
            this.appIconId = Application.Context.Resources.GetIdentifier("icon", "drawable", Application.Context.PackageName);
        }


        public virtual string Send(string title, string message, string sound = null, TimeSpan? when = null) {
            var id = this.GetNextNotificationId();

            var builder = new Notification
                .Builder(Application.Context)
                .SetAutoCancel(true)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(this.appIconId);

            if (sound != null) {
                //builder.SetSound (RingtoneManager.GetDefaultUri(RingtoneType.Alarm));
                var file = new File(sound);
                var uri = Android.Net.Uri.FromFile(file);
                builder.SetSound(uri);
            }

            if (when != null)
                builder.SetWhen(when.Value.Ticks);

            var notification = builder.Build();
            this.notificationManager.Notify(id, notification);

            return id.ToString();
        }


        public virtual void CancelAll() {
            this.notificationManager.CancelAll();
        }


        public virtual bool Cancel(string id) {
            var @int = 0;
            if (!Int32.TryParse(id, out @int))
                return false;

            this.notificationManager.Cancel(@int);
            return true;
        }


        public int Badge { get; set; }


        public void Vibrate(int ms) {
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
