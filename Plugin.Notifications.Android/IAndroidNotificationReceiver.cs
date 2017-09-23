using System;


namespace Plugin.Notifications
{
    public interface IAndroidNotificationReceiver
    {
        void TriggerNotification(int notificationId);
        void TriggerScheduledNotification(int notificationId);
    }
}