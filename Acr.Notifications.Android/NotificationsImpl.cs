using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Java.IO;


namespace Acr.Notifications
{

    public class NotificationsImpl : AbstractNotificationsImpl
    {
        readonly NotificationManagerCompat notificationManager;
        readonly AlarmManager alarmManager;
        public int AppIconResourceId { get; set; }


        public NotificationsImpl()
        {
            this.AppIconResourceId = Application.Context.Resources.GetIdentifier("icon", "drawable", Application.Context.PackageName);
            this.notificationManager = NotificationManagerCompat.From(Application.Context);
            this.alarmManager = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);
        }


        public override string Send(Notification notification)
        {
            var id = NotificationSettings.Instance.CreateScheduleId();

            if (notification.IsScheduled)
            {
                var triggerMs = this.GetEpochMills(notification.SendTime);
                var pending = notification.ToPendingIntent(id);

                this.alarmManager.Set(
                    AlarmType.RtcWakeup,
                    Convert.ToInt64(triggerMs),
                    pending
                );

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
                .SetContentIntent(TaskStackBuilder
                    .Create(Application.Context)
                    .AddNextIntent(launchIntent)
                    .GetPendingIntent(id, (int)PendingIntentFlags.OneShot)
                );

            if (notification.Vibrate)
            {
                builder.SetVibrate(new long[] { 500, 500});
            }

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
            foreach (var id in NotificationSettings.Instance.ScheduleIds)
                this.CancelInternal(id);

            NotificationSettings.Instance.ClearScheduled();
            this.notificationManager.CancelAll();
        }


        public override bool Cancel(string id)
        {
            var @int = 0;
            if (!Int32.TryParse(id, out @int))
                return false;

            this.CancelInternal(@int);
            NotificationSettings.Instance.RemoveScheduledId(@int);
            return true;
        }


        public override int Badge
        {
            get { return NotificationSettings.Instance.CurrentBadge; }
            set
            {
                NotificationSettings.Instance.CurrentBadge = value;
                if (value <= 0)
                    ME.Leolin.Shortcutbadger.ShortcutBadger.RemoveCount(Application.Context);
                else
                    ME.Leolin.Shortcutbadger.ShortcutBadger.ApplyCount(Application.Context, value);
            }
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


        void CancelInternal(int notificationId)
        {
            var pending = Helpers.GetNotificationPendingIntent(notificationId);
            pending.Cancel();
            this.alarmManager.Cancel(pending);
            this.notificationManager.Cancel(notificationId);
        }
    }
}
