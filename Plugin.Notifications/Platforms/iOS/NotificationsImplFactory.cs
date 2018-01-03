using UIKit;

namespace Plugin.Notifications
{
    public class NotificationsImplFactory
    {
        public static AbstractNotificationsImpl GetImplementation()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                return new UNNotificationsImpl();
            else
                return new UILocalNotificationsImpl();
        }
    }
}