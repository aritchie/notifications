using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using UserNotifications;


namespace Plugin.Notifications
{
    public class UNNotificationsImpl : AbstractAppleNotificationsImpl
    {
        public override Task CancelAll() => this.Invoke(() =>
        {
            UNUserNotificationCenter.Current.RemoveAllPendingNotificationRequests();
            UNUserNotificationCenter.Current.RemoveAllDeliveredNotifications();
        });


        public override Task Cancel(int notificationId) => this.Invoke(() =>
        {
            var ids = new [] { notificationId.ToString() };
            UNUserNotificationCenter.Current.RemovePendingNotificationRequests(ids);
            UNUserNotificationCenter.Current.RemoveDeliveredNotifications(ids);
        });


        public override Task Send(Notification notification) => this.Invoke(async () =>
        {
            // TODO: set ID
            var content = new UNMutableNotificationContent
            {
                Title = notification.Title,
                Body = notification.Message
            };
            if (!String.IsNullOrWhiteSpace(notification.Sound))
                content.Sound = UNNotificationSound.GetSound(notification.Sound);

            var request = UNNotificationRequest.FromIdentifier(
                notification.Id.Value.ToString(),
                content,
                null
            );
            await UNUserNotificationCenter.Current.AddNotificationRequestAsync(request);
        });


        public override Task<IEnumerable<Notification>> GetScheduledNotifications()
        {
            var tcs = new TaskCompletionSource<IEnumerable<Notification>>();
            UIApplication.SharedApplication.InvokeOnMainThread(async () =>
            {
                var requests = await UNUserNotificationCenter.Current.GetPendingNotificationRequestsAsync();
                var notifications = requests.Select(x => new Notification());
                tcs.TrySetResult(notifications);
            });
            return tcs.Task;
        }


        public override Task<bool> RequestPermission()
        {
            var tcs = new TaskCompletionSource<bool>();

            UNUserNotificationCenter.Current.RequestAuthorization(
                UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
                (approved, error) => tcs.TrySetResult(approved)
            );
            return tcs.Task;
        }


        protected virtual Notification FromNative(UNNotificationRequest native)
        {
            if (!Int32.TryParse(native.Identifier, out var i))
                return null;

            var plugin = new Notification
            {
                Id = i,
                Title = native.Content.Title,
                Message = native.Content.Body,
                Sound = native.Content.Sound.ToString(),
                Date = (native.Trigger as UNCalendarNotificationTrigger)?.NextTriggerDate.ToDateTime() ?? DateTime.MinValue
            };

            foreach (var pair in native.Content.UserInfo)
            {
                var key = pair.Key as NSString;
                if (key != null)
                {
                    var value = pair.Value as NSString;
                    if (value != null)
                    {
                        plugin.Metadata.Add(key, value);
                    }
                }
            }

            return plugin;
        }
    }
}
