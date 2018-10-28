using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AudioToolbox;
using Foundation;
using UIKit;
using UserNotifications;


namespace Plugin.Notifications
{
    public class NotificationManagerImpl : AbstractNotificationManagerImpl
    {
        public NotificationManagerImpl()
        {
            //UNUserNotificationCenter.Current.SetNotificationCategories(
            //    UNNotificationCategory.FromIdentifier(
            //        "",
            //        new UNNotificationAction[]
            //        {
            //            UNNotificationAction.FromIdentifier(
            //                "id",
            //                "title",
            //                UNNotificationActionOptions.AuthenticationRequired
            //            )
            //        },
            //        new string[] { "" },
            //        "hiddenPreviewsBodyPlaceholder",
            //        new NSString(""),
            //        UNNotificationCategoryOptions.None
            //    )
            //);

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


        public override Task Cancel(int notificationId) => this.Invoke(() =>
        {
            var ids = new [] { notificationId.ToString() };

            UNUserNotificationCenter.Current.RemovePendingNotificationRequests(ids);
            UNUserNotificationCenter.Current.RemoveDeliveredNotifications(ids);
        });


        public override Task Send(Notification notification) => this.Invoke(async () =>
        {
            var permission = await this.RequestPermission();
            if (!permission)
                return;

            var content = new UNMutableNotificationContent
            {
                Title = notification.Title,
                Body = notification.Message,
                //Badge=
                //LaunchImageName = ""
                //Subtitle = ""
            };
            //UNNotificationAttachment.FromIdentifier("", NSUrl.FromString(""), new UNNotificationAttachmentOptions().)
            if (!String.IsNullOrWhiteSpace(notification.Payload))
                content.UserInfo.SetValueForKey(new NSString(notification.Payload), new NSString("Payload"));

            if (!String.IsNullOrWhiteSpace(notification.Sound))
                content.Sound = UNNotificationSound.GetSound(notification.Sound);

            var request = UNNotificationRequest.FromIdentifier(
                notification.Id.ToString(),
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
                try
                {
                    var requests = await UNUserNotificationCenter
                        .Current
                        .GetPendingNotificationRequestsAsync();

                    //var notifications = requests.Select(x => this.FromNative(x));
                    // tcs.TrySetResult(notifications);
                    tcs.TrySetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
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