using System;
using UserNotifications;


namespace Plugin.Notifications
{
    public class AcrUserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        readonly Action<UNNotificationResponse> action;


        public AcrUserNotificationCenterDelegate(Action<UNNotificationResponse> action)
        {
            this.action = action;
        }


        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            this.action(response);
            base.DidReceiveNotificationResponse(center, response, completionHandler);
        }
    }
}