using System;
using System.Threading.Tasks;
using Acr;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Plugin.Geofencing;
using Plugin.Jobs;
using Plugin.Notifications.Data;
using TaskStackBuilder = Android.App.TaskStackBuilder;


namespace Plugin.Notifications
{
    public class NotificationManagerImpl : AbstractPlatformNotificationManagerImpl
    {
        public NotificationManagerImpl(IGeofenceManager geofenceManager = null,
                                       IJobManager jobManager = null,
                                       INotificationRepository repository = null)
            : base(geofenceManager, jobManager, repository) {}


        protected override void NativeSend(Notification notification)
        {

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
            var launchIntent = Application
                .Context
                .PackageManager
                .GetLaunchIntentForPackage(Application.Context.PackageName)
                .SetFlags(notification.Android.LaunchActivityFlags.ToNative());

            if (!notification.Payload.IsEmpty())
                launchIntent.PutExtra("Payload", notification.Payload);

            var pendingIntent = TaskStackBuilder
                .Create(Application.Context)
                .AddNextIntent(launchIntent)
                .GetPendingIntent(0, PendingIntentFlags.OneShot | PendingIntentFlags.CancelCurrent);// TODO

            var smallIconResourceId = notification.Android.SmallIconResourceName.IsEmpty()
                ? Helpers.AppIconResourceId
                : Helpers.GetResourceIdByName(notification.Android.SmallIconResourceName);

            var builder = new NotificationCompat.Builder(Application.Context)
                .SetAutoCancel(true)
                .SetContentTitle(notification.Title)
                .SetContentText(notification.Message)
                .SetSmallIcon(smallIconResourceId)
                .SetContentIntent(pendingIntent);

            if (notification.Android.Vibrate)
                builder.SetVibrate(new long[] { 500, 500 });

            if (notification.Sound != null)
            {
                if (!notification.Sound.Contains("://"))
                    notification.Sound = $"{ContentResolver.SchemeAndroidResource}://{Application.Context.PackageName}/raw/{notification.Sound}";

                var uri = Android.Net.Uri.Parse(notification.Sound);
                builder.SetSound(uri);
            }
            var not = builder.Build();
            NotificationManagerCompat
                .From(Application.Context)
                .Notify(0, not); // TODO: id
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
    }
}
