using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Plugin.Geofencing;
using Plugin.Jobs;
using Plugin.Notifications.Data;


namespace Plugin.Notifications
{
    public class NotificationManagerImpl : AbstractPlatformNotificationManagerImpl
    {
        public NotificationManagerImpl(IGeofenceManager geofenceManager = null,
                                       IJobManager jobManager = null,
                                       INotificationRepository repository = null)
            : base(geofenceManager, jobManager, repository) {}


        public override Task Send(Notification notification)
        {
            //if (notification.IsScheduled)
            //{
            //    var triggerMs = this.GetEpochMills(notification.ScheduledDate.Value);
            //    var pending = notification.ToPendingIntent(notification.Id.Value);

            //    var alarmMgr = (AlarmManager) Application.Context.GetSystemService(Context.AlarmService);
            //    alarmMgr.Set(
            //        AlarmType.RtcWakeup,
            //        Convert.ToInt64(triggerMs),
            //        pending
            //    );
            //    AndroidConfig.Repository.Insert(notification);
            //}
            //else
            //{
            //    /*
            //          Intent stopIntent = new Intent(this, typeof(DownloadsBroadcastReceiver));
            //            stopIntent.PutExtra("action", "actionName");
            //            PendingIntent stopPi = PendingIntent.GetBroadcast(this, 4, stopIntent, PendingIntentFlags.UpdateCurrent);
            //            Intent intent = new Intent(this, typeof(MainActivity));
            //            TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);
            //            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(MainActivity)));
            //            stackBuilder.AddNextIntent(intent);
            //            PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);
            //            Notification.Action pauseAction = new Notification.Action.Builder(Resource.Drawable.Pause, "WSTRZYMAJ", stopPi).Build();
            //            notificationBuilder = new Notification.Builder(this)
            //                .SetSmallIcon(Resource.Drawable.Icon)
            //                .SetContentIntent(resultPendingIntent)
            //                .SetContentTitle(title)
            //                .SetContentText(content)
            //                .AddAction(pauseAction);
            //            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            //            notificationManager.Notify(uniqueNumber, notificationBuilder.Build());
            //     */
            //    var launchIntent = Application
            //        .Context
            //        .PackageManager
            //        .GetLaunchIntentForPackage(Application.Context.PackageName)
            //        .SetAction(Constants.ACTION_KEY)
            //        .SetFlags(AndroidConfig.LaunchActivityFlags);

            //    foreach (var pair in notification.Metadata)
            //        launchIntent.PutExtra(pair.Key, pair.Value);

            //    var pendingIntent = TaskStackBuilder
            //        .Create(Application.Context)
            //        .AddNextIntent(launchIntent)
            //        .GetPendingIntent(notification.Id.Value, (int)(PendingIntentFlags.OneShot | PendingIntentFlags.CancelCurrent));

            //    var builder = new NotificationCompat.Builder(Application.Context)
            //        .SetAutoCancel(true)
            //        .SetContentTitle(notification.Title)
            //        .SetContentText(notification.Message)
            //        .SetSmallIcon(AndroidConfig.AppIconResourceId)
            //        .SetContentIntent(pendingIntent);

            //    if (notification.Vibrate)
            //        builder.SetVibrate(new long[] {500, 500});

            //    if (notification.Sound != null)
            //    {
            //        if (!notification.Sound.Contains("://"))
            //            notification.Sound = $"{ContentResolver.SchemeAndroidResource}://{Application.Context.PackageName}/raw/{notification.Sound}";

            //        var uri = Android.Net.Uri.Parse(notification.Sound);
            //        builder.SetSound(uri);
            //    }
            //    var not = builder.Build();
            //    NotificationManagerCompat
            //        .From(Application.Context)
            //        .Notify(notification.Id.Value, not);
            //}
            return Task.CompletedTask;
        }


        public override Task CancelAll()
        {
            //var notifications = AndroidConfig.Repository.GetScheduled();
            //foreach (var notification in notifications)
            //    this.CancelInternal(notification.Id.Value);

            //AndroidConfig.Repository.DeleteAll();

            //NotificationManagerCompat
            //    .From(MediaTypeNames.Application.Context)
            //    .CancelAll();

            return Task.CompletedTask;
        }


        public override async Task Cancel(string notificationId)
        {
            //AndroidConfig.Repository.Delete(notificationId);
            //this.CancelInternal(notificationId);
            //return Task.FromResult(true);
        }


        public override Task<bool> RequestPermission() => Task.FromResult(true);


        public override int Badge
        {
            //get => AndroidConfig.Repository.CurrentBadge;
            get => 0;
            set
            {
                //AndroidConfig.Repository.CurrentBadge = value;
                //if (value <= 0)
                //{
                //    ME.Leolin.Shortcutbadger.ShortcutBadger.RemoveCount(MediaTypeNames.Application.Context);
                //}
                //else
                //{
                //    ME.Leolin.Shortcutbadger.ShortcutBadger.ApplyCount(MediaTypeNames.Application.Context, value);
                //}
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



        //async Task RegisterTrigger(Notification notification)
        //{
        //    if (notification.Trigger is LocationNotificationTrigger cast1)
        //    {
        //        var status = await this.geofenceManager.RequestPermission().ConfigureAwait(false);
        //        if (status != PermissionStatus.Granted)
        //            throw new ArgumentException("");

        //        this.geofenceManager.StartMonitoring(new GeofenceRegion(
        //            notification.Id,
        //            new Position(cast1.GpsLatitude, cast1.GpsLongitude),
        //            Distance.FromMeters(cast1.RadiusInMeters))
        //        );
        //    }
        //    else if (notification.Trigger is CalendarNotificationTrigger cast2)
        //    {

        //    }
        //    else if (notification.Trigger is TimeIntervalNotificationTrigger cast3)
        //    {

        //    }
        //}


        void OnRegionStatusChanged(object sender, GeofenceStatusChangedEventArgs args)
        {
            // TODO find all incompleted notifications for this region
        }
    }
}
