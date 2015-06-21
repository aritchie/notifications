using System;
using System.Linq;
using AudioToolbox;
using Foundation;
using UIKit;


namespace Acr.Notifications {

    public class NotificationsImpl : INotifications {

        public NotificationsImpl() {
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0)) {
                var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }
        }


        public virtual int Badge {
            get { return (int)UIApplication.SharedApplication.ApplicationIconBadgeNumber; }
            set { UIApplication.SharedApplication.ApplicationIconBadgeNumber = value; }
        }


        public virtual string Send(string title, string message, string sound = null, TimeSpan? when = null) {
            var msgId = Guid.NewGuid().ToString();
            var dt = DateTime.Now;
            if (when != null)
                dt = dt.Add(when.Value);

            var notification = new UILocalNotification {
                FireDate = (NSDate)dt,
                AlertAction = title,
                AlertBody = message
            };
            if (sound != null)
                notification.SoundName = sound;

            notification.UserInfo.SetValueForKey(new NSString(msgId), new NSString("MessageID"));
            UIApplication.SharedApplication.ScheduleLocalNotification(notification);
            return msgId;
        }


        public virtual bool Cancel(string messageId) {
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


        public virtual void CancelAll() {
            this.Badge = 0;
            UIApplication.SharedApplication.CancelAllLocalNotifications();
        }


        public virtual void Vibrate(int ms) {
            SystemSound.Vibrate.PlaySystemSound();
        }
    }
}
