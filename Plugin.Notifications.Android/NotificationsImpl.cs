using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;


namespace Plugin.Notifications
{
    public class NotificationsImpl : AbstractNotificationsImpl
    {
        readonly AcrSqliteConnection conn;
        readonly DbSettings settings;
        readonly AlarmManager alarmManager;
        public static int AppIconResourceId { get; set; }


        static NotificationsImpl()
        {
            AppIconResourceId = Application.Context.Resources.GetIdentifier("icon", "drawable", Application.Context.PackageName);
        }


        public NotificationsImpl()
        {
            this.conn = new AcrSqliteConnection();
            this.alarmManager = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);

            this.settings = this.conn.Settings.SingleOrDefault();
            if (this.settings == null)
            {
                this.settings = new DbSettings();
                this.conn.Insert(this.settings);
            }
        }


        public override Task Send(Notification notification)
        {
            if (notification.Id == null)
            {
                notification.Id = ++this.settings.CurrentScheduleId;
                this.conn.Update(this.settings);
            }

            if (notification.IsScheduled)
            {
                var triggerMs = this.GetEpochMills(notification.SendTime);
                var pending = notification.ToPendingIntent(notification.Id.Value);

                this.alarmManager.Set(
                    AlarmType.RtcWakeup,
                    Convert.ToInt64(triggerMs),
                    pending
                );
            }
            var launchIntent = Application.Context.PackageManager.GetLaunchIntentForPackage(Application.Context.PackageName);
            launchIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            foreach (var pair in notification.Metadata)
            {
                launchIntent.PutExtra(pair.Key, pair.Value);
            }

            var builder = new NotificationCompat
                .Builder(Application.Context)
                .SetAutoCancel(true)
                .SetContentTitle(notification.Title)
                .SetContentText(notification.Message)
                .SetSmallIcon(AppIconResourceId)
                .SetContentIntent(TaskStackBuilder
                    .Create(Application.Context)
                    .AddNextIntent(launchIntent)
                    .GetPendingIntent(notification.Id.Value, PendingIntentFlags.OneShot)
                );

            if (notification.Vibrate)
            {
                builder.SetVibrate(new long[] { 500, 500 });
            }

            if (notification.Sound != null)
            {
                if (!notification.Sound.Contains("://"))
                {
                    notification.Sound = $"{ContentResolver.SchemeAndroidResource}://{Application.Context.PackageName}/raw/{notification.Sound}";
                }
                var uri = Android.Net.Uri.Parse(notification.Sound);
                builder.SetSound(uri);
            }
            var not = builder.Build();
            NotificationManagerCompat
                .From(Application.Context)
                .Notify(notification.Id.Value, not);

            return Task.CompletedTask;
        }


        public override Task CancelAll()
        {
            var notifications = this.conn.Notifications.ToList();
            foreach (var notification in notifications)
                this.CancelInternal(notification.Id);

            this.conn.DeleteAll<DbNotificationMetadata>();
            this.conn.DeleteAll<DbNotification>();

            NotificationManagerCompat
                .From(Application.Context)
                .CancelAll();

            return Task.CompletedTask;
        }


        public override Task Cancel(int notificationId)
        {
            this.CancelInternal(notificationId);
            return Task.FromResult(true);
        }


        public override Task<IEnumerable<Notification>> GetScheduledNotifications()
        {
            var nots = this.conn.Notifications.ToList();
            var mds = this.conn.NotificationMetadata.ToList();
            return Task.FromResult(nots.Select(x => new Notification
            {
                Id = x.Id,
                Title = x.Title,
                Message = x.Message,
                Sound = x.Sound,
                Vibrate = x.Vibrate,
                Date = x.DateScheduled,
                Metadata = mds
                    .Where(y => y.NotificationId == x.Id)
                    .ToDictionary(
                        y => y.Key,
                        y => y.Value
                    )
            }));
        }


        public override Task<bool> RequestPermission() => Task.FromResult(true);
        public override Task<int> GetBadge() => Task.FromResult(this.settings.CurrentBadge);
        public override Task SetBadge(int value)
        {
            try
            {
                this.settings.CurrentBadge = value;
                this.conn.Update(this.settings);
                if (value <= 0)
                {
                    ME.Leolin.Shortcutbadger.ShortcutBadger.RemoveCount(Application.Context);
                }
                else
                {
                    ME.Leolin.Shortcutbadger.ShortcutBadger.ApplyCount(Application.Context, value);
                }
            }
            catch
            {
            }
            return Task.CompletedTask;
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


        protected virtual long GetEpochMills(DateTime sendTime)
        {
            var utc = sendTime.ToUniversalTime();
            var epochDiff = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;
            var utcAlarmTimeInMillis = utc.AddSeconds(-epochDiff).Ticks / 10000;
            return utcAlarmTimeInMillis;
        }


        protected virtual void CancelInternal(int notificationId)
        {
            var pending = Helpers.GetNotificationPendingIntent(notificationId);
            this.conn.Notifications.Delete(x => x.Id == notificationId);
            pending.Cancel();
            this.alarmManager.Cancel(pending);
            NotificationManagerCompat
                .From(Application.Context)
                .Cancel(notificationId);
        }
    }
}
