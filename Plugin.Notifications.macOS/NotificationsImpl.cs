using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppKit;
using Foundation;


namespace Plugin.Notifications
{
    public class NotificationsImpl : AbstractNotificationsImpl
    {
        public override Task CancelAll() => this.Invoke(() =>
        {
            var dunc = NSUserNotificationCenter.DefaultUserNotificationCenter;
            foreach (var native in dunc.ScheduledNotifications)
                dunc.RemoveScheduledNotification(native);

            dunc.RemoveAllDeliveredNotifications();
        });


        public override Task Cancel(int notificationId) => this.Invoke(() =>
        {
            var dnc = NSUserNotificationCenter.DefaultUserNotificationCenter;
            var native = dnc.ScheduledNotifications.FirstOrDefault(x => x.Identifier == notificationId.ToString());
            if (native != null)
                dnc.RemoveScheduledNotification(native);

            native = dnc.DeliveredNotifications.FirstOrDefault(x => x.Identifier == notificationId.ToString());
            if (native != null)
                dnc.RemoveDeliveredNotification(native);
        });


        public override Task Send(Notification notification) => this.Invoke(() =>
        {
            if (notification.Id == null)
                notification.GeneratedNotificationId();

            var native = new NSUserNotification
            {
                Identifier = notification.Id.Value.ToString(),
                Title = notification.Title,
                InformativeText = notification.Message,
                SoundName = notification.Sound,
                DeliveryDate = notification.SendTime.ToNSDate(),
                UserInfo = notification.Metadata.ToNsDictionary()
            };
            NSUserNotificationCenter
                .DefaultUserNotificationCenter
                .ScheduleNotification(native);
        });


        public override async Task<int> GetBadge()
        {
            var tcs = new TaskCompletionSource<int>();
            NSApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                Int32.TryParse(NSApplication.SharedApplication.DockTile.BadgeLabel, out var i);
                tcs.TrySetResult(i);
            });
            return await tcs.Task;
        }


        public override Task SetBadge(int value) => this.Invoke(() =>
            NSApplication.SharedApplication.DockTile.BadgeLabel = value.ToString()
        );


        public override void Vibrate(int ms)
        {
        }


        public override Task<IEnumerable<Notification>> GetScheduledNotifications()
        {
            var tcs = new TaskCompletionSource<IEnumerable<Notification>>();
            NSApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var natives = NSUserNotificationCenter
                    .DefaultUserNotificationCenter
                    .ScheduledNotifications
                    .Select(x => new Notification
                    {
                        Id = this.ToNotificationId(x.Identifier),
                        Title = x.Title,
                        Message = x.InformativeText,
                        Sound = x.SoundName,
                        Date = x.DeliveryDate.ToDateTime(),
                        Metadata = x.UserInfo.FromNsDictionary()
                    });

                tcs.TrySetResult(natives);
            });
            return tcs.Task;
        }


        public override Task<bool> RequestPermission() => Task.FromResult(true);


        protected int ToNotificationId(string value)
        {
            if (!Int32.TryParse(value, out var i))
                return -1;

            return i;
        }


        protected async Task Invoke(Action action)
        {
            var tcs = new TaskCompletionSource<object>();
            NSApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                action();
                tcs.TrySetResult(null);
            });
            await tcs.Task;
        }
    }
}
