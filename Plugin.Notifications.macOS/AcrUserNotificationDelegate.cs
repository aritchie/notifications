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


        public override void DidDeliverNotification(NSUserNotificationCenter center, NSUserNotification notification)
        {

            base.DidDeliverNotification(center, notification);
        }
    }
}
