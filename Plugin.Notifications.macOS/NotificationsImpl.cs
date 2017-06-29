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
            // TODO: cancel delivered and scheduled?
        });


        public override Task Send(Notification notification) => this.Invoke(() =>
        {
            //new     UNUserNotificationCenter.Current.RequestAuthorization (UNAuthorizationOptions.Alert, (approved, err) => {
            //    // Handle approval
            //});
            var not = new NSUserNotification
            {
                Title = notification.Title,
                InformativeText = notification.Message,
                Identifier = notification.Id.Value.ToString(),
                DeliveryDate = (NSDate) notification.SendTime
            };
            NSUserNotificationCenter
                .DefaultUserNotificationCenter
                .ScheduleNotification(not);
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
                        // TODO
                    });
                tcs.TrySetResult(natives);
            });
            return tcs.Task;
        }


        public override Task<bool> RequestPermission() => Task.FromResult(true);


        async Task Invoke(Action action)
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
