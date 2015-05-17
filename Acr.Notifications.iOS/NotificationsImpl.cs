using System;
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


        public int Badge {
            get { return (int)UIApplication.SharedApplication.ApplicationIconBadgeNumber; }
            set { UIApplication.SharedApplication.ApplicationIconBadgeNumber = value; }
        }


        public void Send(string title, string message, string sound = null, TimeSpan? when = null) {
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

            UIApplication.SharedApplication.ScheduleLocalNotification(notification);
        }


        public void CancelAll() {
            this.Badge = 0;
            UIApplication.SharedApplication.CancelAllLocalNotifications();
        }


        public void Vibrate(int ms) {
            SystemSound.Vibrate.PlaySystemSound();
        }
    }
}
