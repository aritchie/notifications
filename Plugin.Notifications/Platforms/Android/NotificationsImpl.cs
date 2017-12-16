using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;


namespace Plugin.Notifications
{
    //https://stackoverflow.com/questions/6422319/start-service-from-notification
    public class NotificationsImpl : AbstractNotificationsImpl, IAndroidNotificationReceiver
    {
        readonly AlarmManager alarmManager;

        public NotificationsImpl()
        {
            this.alarmManager = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);
        }


        public override Task Send(Notification notification)
        {
            if (notification.Id == null)
            {
                AndroidConfig.Repository.CurrentScheduleId++;
                notification.Id = AndroidConfig.Repository.CurrentScheduleId;
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
                AndroidConfig.Repository.Insert(notification);
            }
            else
            {
                var launchIntent = new Intent(Application.Context, typeof(NotificationActionService));
                launchIntent.PutExtra("acrnotification", "0");
                //var launchIntent = Application
                //    .Context
                //    .PackageManager
                //    .GetLaunchIntentForPackage(Application.Context.PackageName);
                //launchIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);

                foreach (var pair in notification.Metadata)
                {
                    launchIntent.PutExtra(pair.Key, pair.Value);
                }

                var builder = new NotificationCompat.Builder(Application.Context)
                    .SetAutoCancel(true)
                    .SetContentTitle(notification.Title)
                    .SetContentText(notification.Message)
                    .SetSmallIcon(AndroidConfig.AppIconResourceId)
                    .SetContentIntent(TaskStackBuilder
                        .Create(Application.Context)
                        .AddNextIntent(launchIntent)
                        .GetPendingIntent(notification.Id.Value, (int)PendingIntentFlags.OneShot)
                    );

                if (notification.Vibrate)
                {
                    builder.SetVibrate(new long[] {500, 500});
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
            }
            return Task.CompletedTask;
        }


        public override Task CancelAll()
        {
            var notifications = AndroidConfig.Repository.GetScheduled();
            foreach (var notification in notifications)
                this.CancelInternal(notification.Id.Value);

            AndroidConfig.Repository.DeleteAll();

            NotificationManagerCompat
                .From(Application.Context)
                .CancelAll();

            return Task.CompletedTask;
        }


        public override Task Cancel(int notificationId)
        {
            AndroidConfig.Repository.Delete(notificationId);
            this.CancelInternal(notificationId);
            return Task.FromResult(true);
        }


        public override Task<IEnumerable<Notification>> GetScheduledNotifications()
            => Task.FromResult(AndroidConfig.Repository.GetScheduled());


        public override Task<bool> RequestPermission() => Task.FromResult(true);
        public override Task<int> GetBadge() => Task.FromResult(AndroidConfig.Repository.CurrentBadge);
        public override Task SetBadge(int value)
        {
            try
            {
                AndroidConfig.Repository.CurrentBadge = value;
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


        public void TriggerNotification(int id)
        {
            var notification = AndroidConfig.Repository.GetById(id);
            if (notification != null)
                this.OnActivated(notification);
        }


        public void TriggerScheduledNotification(int notificationId)
        {
            var notification = AndroidConfig.Repository.GetById(notificationId);
            if (notification == null)
                return;

            AndroidConfig.Repository.Delete(notificationId);

            // resend without schedule so it goes through normal mechanism
            notification.When = null;
            notification.Date = null;
            this.Send(notification);
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
            pending.Cancel();

            this.alarmManager.Cancel(pending);
            NotificationManagerCompat
                .From(Application.Context)
                .Cancel(notificationId);
        }
    }
}
