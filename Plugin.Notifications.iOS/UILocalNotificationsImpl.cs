using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using UIKit;


namespace Plugin.Notifications
{
    public class UILocalNotificationsImpl : AbstractAppleNotificationsImpl
    {
        const string NOTIFICATION_ID_KEY = "NotificationID";


        public override Task Cancel(int notificationId)
        {
            var key = new NSString(NOTIFICATION_ID_KEY);
            var keyValue = new NSString(notificationId.ToString());

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


        public override Task Send(Notification notification)
        {
            var msgId = Guid.NewGuid().ToString();
            var userInfo = new NSMutableDictionary();
            userInfo.Add(new NSString(NOTIFICATION_ID_KEY), new NSString(msgId));

            foreach (var pair in notification.Metadata)
                userInfo.Add(new NSString(pair.Key), new NSString(pair.Value));

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


        public override Task<IEnumerable<Notification>> GetScheduledNotifications()
        {
            var tcs = new TaskCompletionSource<IEnumerable<Notification>>();
            UIApplication.SharedApplication.InvokeOnMainThread(() => tcs.TrySetResult(
                UIApplication
                    .SharedApplication
                    .ScheduledLocalNotifications
                    .Select(x => new Notification
                    {
                        // TODO
                    })
            ));
            return tcs.Task;
        }


        public override Task<bool> RequestPermission()
        {
            var settings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                null
            );
            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            return Task.FromResult(true);
        }
    }
}
