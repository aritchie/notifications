using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserNotifications;


namespace Plugin.Notifications
{
#if __IOS__
    public class UNNotificationsImpl : AbstractAppleNotificationsImpl
#else
    public class NotificationsImpl : AbstractAppleNotificationsImpl
#endif
    {
        public override Task Cancel(int notificationId)
        {
            throw new NotImplementedException();
        }


        public override Task Send(Notification notification)
        {
            throw new NotImplementedException();
        }


        public override Task<int> GetBadge()
        {
            throw new NotImplementedException();
        }


        public override Task SetBadge(int value)
        {
            throw new NotImplementedException();
        }


        public override void Vibrate(int ms)
        {
            throw new NotImplementedException();
        }


        public override Task<IEnumerable<Notification>> GetScheduledNotifications()
        {
            throw new NotImplementedException();
        }


        public override Task<bool> RequestPermission()
        {
            var tcs = new TaskCompletionSource<bool>();

            UNUserNotificationCenter.Current.RequestAuthorization(
                UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
                (approved, error) => tcs.TrySetResult(approved)
            );
            return tcs.Task;
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
        }
    }
}
