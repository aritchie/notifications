using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using UserNotifications;


namespace Plugin.Notifications
{
#if __IOS__
    public class UNNotificationsImpl : AbstractAppleNotificationsImpl
#else
    public class NotificationsImpl : AbstractAppleNotificationsImpl
#endif
    {
        public override Task CancelAll() => this.Invoke(() =>
        {
            UNUserNotificationCenter.Current.RemoveAllPendingNotificationRequests();
            UNUserNotificationCenter.Current.RemoveAllDeliveredNotifications();
        });


        public override Task Cancel(int notificationId) => this.Invoke(() =>
        {
            var ids = new [] {notificationId.ToString()};
            UNUserNotificationCenter.Current.RemovePendingNotificationRequests(ids);
            UNUserNotificationCenter.Current.RemoveDeliveredNotifications(ids);
        });


        public override Task Send(Notification notification) => this.Invoke(async () =>
        {
            ////request.Subtitle = "";
            //var trigger =  UNTimeIntervalNotificationTrigger.CreateTrigger (5, false);
            var request = UNNotificationRequest.FromIdentifier(
                notification.Id.Value.ToString(),
                new UNMutableNotificationContent
                {
                    Title = notification.Title,
                    Body = notification.Message,
                    Sound = UNNotificationSound.GetSound(notification.Sound) // TODO
                },
                UNCalendarNotificationTrigger.CreateTrigger(new NSDateComponents(), false)
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
    }
}
