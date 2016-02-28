using System;
using System.Collections.Generic;
using Acr.Settings;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Java.IO;


namespace Acr.Notifications
{

    public class NotificationsImpl : AbstractNotificationsImpl
    {
        const string SCHEDULED_IDS = "ScheduledIds";
        const string NOTIFICATION_ID = "NotificationId";

        readonly NotificationManagerCompat notificationManager;
        readonly AlarmManager alarmManager;
        readonly ISettings settings;
        public int AppIconResourceId { get; set; }


        public NotificationsImpl()
        {
            this.AppIconResourceId = Application.Context.Resources.GetIdentifier("icon", "drawable", Application.Context.PackageName);
            this.notificationManager = NotificationManagerCompat.From(Application.Context);
            this.alarmManager = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);
            this.settings = Settings.Settings.Local;
            this.settings.KeysNotToClear.AddRange(new[]
            {
                SCHEDULED_IDS,
                NOTIFICATION_ID
            });
        }


        public override string Send(Notification notification)
        {
            var id = this.GetNextNotificationId();

            if (notification.IsScheduled)
            {
                var ts = notification.SendTime.Subtract(DateTime.Now);
                var triggerMs = SystemClock.ElapsedRealtime() + ts.TotalMilliseconds;
                var pending = notification.ToPendingIntent(id);

                this.alarmManager.Set(
                    AlarmType.RtcWakeup,
                    Convert.ToInt64(triggerMs),
                    pending
                );
                this.AddScheduledId(id);
                return id.ToString();
            }

            var launchIntent = Application.Context.PackageManager.GetLaunchIntentForPackage(Application.Context.PackageName);
            launchIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);

            var builder = new NotificationCompat
                .Builder(Application.Context)
                .SetAutoCancel(true)
                .SetContentTitle(notification.Title)
                .SetContentText(notification.Message)
                .SetSmallIcon(this.AppIconResourceId)
                .SetContentIntent(Android.Support.V4.App.TaskStackBuilder
                    .Create(Application.Context)
                    .AddNextIntent(launchIntent)
                    .GetPendingIntent(id, (int)PendingIntentFlags.OneShot)
                );

            if (notification.Sound != null)
            {
                var file = new File(notification.Sound);
                var uri = Android.Net.Uri.FromFile(file);
                builder.SetSound(uri);
            }
            var not = builder.Build();
            this.notificationManager.Notify(id, not);
            return id.ToString();
        }


        public override void CancelAll()
        {
            var scheduleIds = this.GetScheduledIds();
            foreach (var id in scheduleIds)
                this.CancelInternal(id);

            this.settings.Remove(SCHEDULED_IDS);
            this.notificationManager.CancelAll();
        }


        public override bool Cancel(string id)
        {
            var @int = 0;
            if (!Int32.TryParse(id, out @int))
                return false;

            this.CancelInternal(@int);
            this.RemoveScheduledId(@int);
            return true;
        }


        public override void Vibrate(int ms)
        {
            using (var vibrate = (Vibrator)Application.Context.GetSystemService(Context.VibratorService))
            {
                if (!vibrate.HasVibrator)
                    return;

                vibrate.Vibrate(ms);
            }
        }


        readonly object syncLock = new object();
        protected virtual int GetNextNotificationId()
        {
            var id = 0;

            lock (this.syncLock)
            {
                id = this.settings.Get(NOTIFICATION_ID, 0);
                id++;
                this.settings.Set(NOTIFICATION_ID, id);
            }
            return id;
        }


        protected virtual IList<int> GetScheduledIds()
        {
            return this.settings.Get(SCHEDULED_IDS, new List<int>());
        }


        protected virtual void AddScheduledId(int id)
        {
            var ids = this.GetScheduledIds();
            ids.Add(id);
            this.settings.Set(SCHEDULED_IDS, ids);
        }


        protected virtual void RemoveScheduledId(int id)
        {
            var ids = this.GetScheduledIds();
            if (ids.Remove(id))
                this.settings.Set(SCHEDULED_IDS, ids);
        }


        protected virtual long GetEpochMills(DateTime sendTime)
        {
            var utc = sendTime.ToUniversalTime();
            var epochDiff = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;
            var utcAlarmTimeInMillis = utc.AddSeconds(-epochDiff).Ticks / 10000;
            return utcAlarmTimeInMillis;
        }


        void CancelInternal(int notificationId)
        {
            var pending = Helpers.GetNotificationPendingIntent(notificationId);
            this.alarmManager.Cancel(pending);
            this.notificationManager.Cancel(notificationId);
        }
    }
}
