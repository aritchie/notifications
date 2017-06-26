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

    public class NotificationsImpl : AbstractNotificationsImpl
    {
        public override async Task<bool> RequestPermission()
        {
            var result = false;

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var settings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                    null
                );
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
                result = true;
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                var tcs = new TaskCompletionSource<bool>();

                UNUserNotificationCenter.Current.RequestAuthorization(
                    UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
                    (approved, error) => tcs.TrySetResult(approved)
                );
                result = await tcs.Task;
            }
            return result;
        }


        public override Task<IEnumerable<Notification>> GetScheduledNotifications()
        {
            throw new NotImplementedException();
        }


        public override Task<int> GetBadge()
        {
            var tcs = new TaskCompletionSource<int>();
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var value = (int)UIApplication.SharedApplication.ApplicationIconBadgeNumber;
                tcs.TrySetResult(value);
            });
            return tcs.Task;
        }


        public override Task SetBadge(int value) => this.Invoke(() =>
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = value
        );


        public override Task Send(Notification notification)
        {
            var msgId = Guid.NewGuid().ToString();
            var userInfo = new NSMutableDictionary();
            userInfo.Add(new NSString("MessageID"), new NSString(msgId));

            //UNUserNotificationCenter.Current.RequestAuthorization (UNAuthorizationOptions.Alert, (approved, err) => {
            //    // Handle approval
            //});
            //https://developer.xamarin.com/guides/ios/platform_features/introduction-to-ios10/user-notifications/enhanced-user-notifications/#About-Local-Notifications
            //UNUserNotificationCenter.Current.RemoveAllPendingNotificationRequests();
            //UNUserNotificationCenter.Current.RemoveAllDeliveredNotifications();
            //UNUserNotificationCenter.Current.GetNotificationSettings(settings => {});
            //var request = new UNMutableNotificationContent();
            //request.ThreadIdentifier = "";
            //request.Title = "";
            //request.Badge = 0;
            //request.Sound = UNNotificationSound.Default;
            ////request.UserInfo = userInfo;
            ////request.Subtitle = "";
            //var trigger =  UNTimeIntervalNotificationTrigger.CreateTrigger (5, false);

            //var requestID = "sampleRequest";
            //var request = UNNotificationRequest.FromIdentifier (requestID, content, trigger);

            //UNUserNotificationCenter.Current.AddNotificationRequest (request, (err) => {
            //    if (err != null) {
            //        // Do something with error...
            //    }
            //});


            var not = new UILocalNotification
            {
                FireDate = notification.SendTime.ToNSDate(),
                AlertTitle = notification.Title,
                AlertBody = notification.Message,
                SoundName = notification.Sound,
                UserInfo = userInfo
            };
            return this.Invoke(() => UIApplication.SharedApplication.ScheduleLocalNotification(not));
        }


        public override Task Cancel(string messageId)
        {
            var key = new NSString("MessageID");
            var keyValue = new NSString(messageId);

            var notification = UIApplication.SharedApplication.ScheduledLocalNotifications.FirstOrDefault(x =>
                x.UserInfo.ContainsKey(key) &&
                x.UserInfo[key].Equals(keyValue)
            );
            if (notification == null)
            {
            }
            return this.Invoke(() =>
                UIApplication.SharedApplication.CancelLocalNotification(notification)
            );
        }


        public override Task CancelAll() => this.Invoke(() =>
            UIApplication.SharedApplication.CancelAllLocalNotifications()
        );


        public override void Vibrate(int ms) => this.Invoke(() => SystemSound.Vibrate.PlaySystemSound());


        Task Invoke(Action action)
        {
            var tcs = new TaskCompletionSource<object>();
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                action();
                tcs.TrySetResult(null);
            });
            return tcs.Task;
        }
    }
}