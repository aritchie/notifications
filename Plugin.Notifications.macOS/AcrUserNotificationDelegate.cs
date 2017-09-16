using System;
using Foundation;


namespace Plugin.Notifications
{
    public class AcrUserNotificationDelegate : NSUserNotificationCenterDelegate
    {
        readonly Action<NSUserNotification> action;


        public AcrUserNotificationDelegate(Action<NSUserNotification> action)
        {
            this.action = action;
        }


        public override void DidActivateNotification(NSUserNotificationCenter center, NSUserNotification notification)
        {
            this.action(notification);
        }
    }
}
