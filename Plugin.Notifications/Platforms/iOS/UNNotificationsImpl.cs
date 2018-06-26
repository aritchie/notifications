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
        public UNNotificationsImpl()
        {
            UNUserNotificationCenter
                .Current
                .Delegate = new AcrUserNotificationCenterDelegate(response =>
                {
                    var notification = this.FromNative(response.Notification.Request);
                    this.OnActivated(notification);
                });
        }


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
            if (notification.Id == null)
                notification.GeneratedNotificationId();

            var content = new UNMutableNotificationContent
            {
                Title = notification.Title,
                Body = notification.Message,
                UserInfo = notification.MetadataToNsDictionary()
            };
            if (!String.IsNullOrWhiteSpace(notification.Sound))
                content.Sound = UNNotificationSound.GetSound(notification.Sound);

            var dt = notification.ScheduledDate ?? DateTime.Now;
            var request = UNNotificationRequest.FromIdentifier(
                notification.Id.Value.ToString(),
                content,
                UNCalendarNotificationTrigger.CreateTrigger(new NSDateComponents
                {
                    Year = dt.Year,
                    Month = dt.Month,
                    Day = dt.Day,
                    Hour = dt.Hour,
                    Minute = dt.Minute,
                    Second = dt.Second
                }, false)
            );
            await UNUserNotificationCenter.Current.AddNotificationRequestAsync(request);
        });


        public override Task<IEnumerable<Notification>> GetScheduledNotifications()
        {
            var tcs = new TaskCompletionSource<IEnumerable<Notification>>();
            UIApplication.SharedApplication.InvokeOnMainThread(async () =>
            {
                var requests = await UNUserNotificationCenter
                    .Current
                    .GetPendingNotificationRequestsAsync();
                var notifications = requests.Select(this.FromNative);
                tcs.TrySetResult(notifications);
            });
            return tcs.Task;
        }


        public override Task<bool> RequestPermission()
        {
            var tcs = new TaskCompletionSource<bool>();

            //UNUserNotificationCenter.Current.Delegate.DidReceiveNotificationResponse();
            //UNUserNotificationCenter.Current.Delegate.WillPresentNotification();
            UNUserNotificationCenter.Current.RequestAuthorization(
                UNAuthorizationOptions.Alert |
                UNAuthorizationOptions.Badge |
                UNAuthorizationOptions.Sound,
                (approved, error) => tcs.TrySetResult(approved)
            );
            return tcs.Task;
        }


        protected virtual Notification FromNative(UNNotificationRequest native)
        {
            if (!Int32.TryParse(native.Identifier, out var i))
                return null;

            var date =
                (native.Trigger as UNCalendarNotificationTrigger)?
                    .DateComponents?
                    .Date?
                    .ToDateTime()
                ?? DateTime.Now;

            var plugin = new Notification
            {
                Id = i,
                Title = native.Content.Title,
                Message = native.Content.Body,
                Sound = native.Content.Sound?.ToString(),
                Date = date,
                Metadata = native.Content.UserInfo.FromNsDictionary()
            };

            return plugin;
        }
    }
}
