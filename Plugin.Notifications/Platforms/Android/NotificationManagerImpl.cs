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

/*
Intent stopIntent = new Intent(this, typeof(DownloadsBroadcastReceiver));
stopIntent.PutExtra("action", "actionName");
PendingIntent stopPi = PendingIntent.GetBroadcast(this, 4, stopIntent, PendingIntentFlags.UpdateCurrent);
Intent intent = new Intent(this, typeof(MainActivity));
TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);
stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(MainActivity)));
stackBuilder.AddNextIntent(intent);
PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);
Notification.Action pauseAction = new Notification.Action.Builder(Resource.Drawable.Pause, "WSTRZYMAJ", stopPi).Build();
notificationBuilder = new Notification.Builder(this)
    .SetSmallIcon(Resource.Drawable.Icon)
    .SetContentIntent(resultPendingIntent)
    .SetContentTitle(title)
    .SetContentText(content)
    .AddAction(pauseAction);
var notificationManager = (NotificationManager)GetSystemService(NotificationService);
notificationManager.Notify(uniqueNumber, notificationBuilder.Build());
*/
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
                .GetPendingIntent(notification.Id, PendingIntentFlags.CancelCurrent);
                //.GetPendingIntent(notification.Id, PendingIntentFlags.OneShot | PendingIntentFlags.CancelCurrent);

            var smallIconResourceId = notification.Android.SmallIconResourceName.IsEmpty()
                ? Helpers.AppIconResourceId
                : Helpers.GetResourceIdByName(notification.Android.SmallIconResourceName);

            var builder = new NotificationCompat.Builder(Application.Context)
                .SetAutoCancel(true)
                .SetContentTitle(notification.Title)
                .SetContentText(notification.Message)
                .SetSmallIcon(smallIconResourceId)
                .SetContentIntent(pendingIntent);

            // TODO
            //if ((int)Build.VERSION.SdkInt >= 21 && notification.Android.Color != null)
            //    builder.SetColor(notification.Android.Color.Value)

            if (notification.Android.Priority != null)
                builder.SetPriority(notification.Android.Priority.Value);

            if (notification.Android.Vibrate)
                builder.SetVibrate(new long[] { 500, 500 });

            if (notification.Sound != null)
            {
                if (!notification.Sound.Contains("://"))
                    notification.Sound = $"{ContentResolver.SchemeAndroidResource}://{Application.Context.PackageName}/raw/{notification.Sound}";

                var uri = Android.Net.Uri.Parse(notification.Sound);
                builder.SetSound(uri);
            }

            if ((int)Build.VERSION.SdkInt >= 26)
            {
                var channelId = notification.Android.ChannelId;
                var manager = NotificationManager.FromContext(Application.Context);
                if (manager.GetNotificationChannel(channelId) == null)
                {
                    var channel = new NotificationChannel(
                        channelId,
                        notification.Android.Channel,
                        notification.Android.NotificationImportance.ToNative()
                    );
                    var d = notification.Android.ChannelDescription;
                    if (!d.IsEmpty())
                        channel.Description = d;

                    manager.CreateNotificationChannel(channel);
                }
                builder.SetChannelId(channelId);
                manager.Notify(notification.Id, builder.Build());
            }
            else
            {
                NotificationManagerCompat
                    .From(Application.Context)
                    .Notify(notification.Id, builder.Build());
            }
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
