using System;


namespace Plugin.Notifications
{
    public class TimeIntervalNotificationTrigger : INotificationTrigger
    {

        public TimeIntervalNotificationTrigger(TimeSpan interval, bool repeats)
        {
            this.Interval = interval;
            this.Repeats = repeats;
        }


        public TimeSpan Interval { get; }
        public bool Repeats { get; }
    }
}
