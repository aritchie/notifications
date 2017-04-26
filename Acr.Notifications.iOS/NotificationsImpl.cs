using System;
using System.Linq;
using AudioToolbox;
using Foundation;
using UIKit;


namespace Acr.Notifications
{

    public class NotificationsImpl : AbstractNotificationsImpl
    {

        public NotificationsImpl()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }
        }


        public override int Badge
        {
            get { return (int)UIApplication.SharedApplication.ApplicationIconBadgeNumber; }
            set { UIApplication.SharedApplication.ApplicationIconBadgeNumber = value; }
        }


        public override string Send(Notification notification)
        {
            var msgId = Guid.NewGuid().ToString();
            var userInfo = new NSMutableDictionary();
            userInfo.Add(new NSString("MessageID"), new NSString(msgId));

            var not = new UILocalNotification
            {
                FireDate = (NSDate)notification.SendTime,
                AlertTitle = notification.Title,
                AlertBody = notification.Message,
                SoundName = notification.Sound,
                UserInfo = userInfo
            };
            UIApplication.SharedApplication.ScheduleLocalNotification(not);
            return msgId;
        }


        public override bool Cancel(string messageId)
        {
            var key = new NSString("MessageID");
            var keyValue = new NSString(messageId);

            var notification = UIApplication.SharedApplication.ScheduledLocalNotifications.FirstOrDefault(x =>
                x.UserInfo.ContainsKey(key) &&
                x.UserInfo[key].Equals(keyValue)
            );
            if (notification == null)
                return false;

            UIApplication.SharedApplication.CancelLocalNotification(notification);
            return true;
        }


        public override void CancelAll()
        {
            this.Badge = 0;
            UIApplication.SharedApplication.CancelAllLocalNotifications();
        }


        public override void Vibrate(int ms)
        {
            SystemSound.Vibrate.PlaySystemSound();
        }
    }
}