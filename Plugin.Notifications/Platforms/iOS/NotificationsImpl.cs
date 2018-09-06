using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using AudioToolbox;
using UserNotifications;


namespace Plugin.Notifications
{

    public class NotificationsImpl : AbstractNotificationsImpl
    {
        public NotificationsImpl()
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


        public override Task Cancel(string notificationId) => this.Invoke(() =>
        {
            var ids = new [] { notificationId };

            UNUserNotificationCenter.Current.RemovePendingNotificationRequests(ids);
            UNUserNotificationCenter.Current.RemoveDeliveredNotifications(ids);
        });


        public override Task Send(Notification notification) => this.Invoke(async () =>
        {
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
                notification.Id ,
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
            var date =
                (native.Trigger as UNCalendarNotificationTrigger)?
                    .DateComponents?
                    .Date?
                    .ToDateTime()
                ?? DateTime.Now;

            var plugin = new Notification
            {
                Id = native.Identifier,
                Title = native.Content.Title,
                Message = native.Content.Body,
                Sound = native.Content.Sound?.ToString(),
                ScheduledDate = date,
                Metadata = native.Content.UserInfo.FromNsDictionary()
            };

            return plugin;
        }


        public int Badge
        {
            get
            {
                var value = 0;
                var app = UIApplication.SharedApplication;
                app.InvokeOnMainThread(() => value = (int)app.ApplicationIconBadgeNumber);
                return value;
            }
            set
            {
                var app = UIApplication.SharedApplication;
                app.InvokeOnMainThread(() => app.ApplicationIconBadgeNumber = value);
            }
        }


        public override void Vibrate(int ms) => SystemSound.Vibrate.PlaySystemSound();
    }
}