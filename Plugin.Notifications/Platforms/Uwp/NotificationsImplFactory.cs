namespace Plugin.Notifications
{
    public class NotificationsImplFactory
    {
        public static AbstractNotificationsImpl GetImplementation()
        {
            return new NotificationsImpl();
        }
    }
}