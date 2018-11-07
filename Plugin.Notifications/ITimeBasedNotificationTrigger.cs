using System;


namespace Plugin.Notifications
{
    public interface ITimeBasedNotificationTrigger : INotificationTrigger
    {
        DateTime CalculateNextTriggerDateFromNow();
    }
}
