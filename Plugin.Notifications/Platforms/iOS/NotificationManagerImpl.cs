using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AudioToolbox;
using UIKit;
using UserNotifications;


namespace Plugin.Notifications
{
    public class NotificationManagerImpl : AbstractNotificationManagerImpl
    {
        public NotificationManagerImpl()
        {
            UNUserNotificationCenter
                .Current
                .Delegate = new AcrUserNotificationCenterDelegate(response =>
                {
                    var notification = response.Notification.Request.FromNative();
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

            var request = UNNotificationRequest.FromIdentifier(
                notification.Id ,
                content,
                notification.Trigger.ToNative()
            );
            await UNUserNotificationCenter.Current.AddNotificationRequestAsync(request);
        });


        public override Task<IEnumerable<Notification>> GetPendingNotifications()
        {
            var tcs = new TaskCompletionSource<IEnumerable<Notification>>();
            UIApplication.SharedApplication.BeginInvokeOnMainThread(async () =>
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


        public override int Badge
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


        protected Task Invoke(Action action)
        {
            var tcs = new TaskCompletionSource<object>();
            var app = UIApplication.SharedApplication;
            app.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    action();
                    tcs.TrySetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });
            return tcs.Task;
        }
    }
}