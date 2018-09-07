using System;


namespace Plugin.Notifications
{
    public interface INotificationTrigger
    {
        bool Repeats { get; }
    }
}
