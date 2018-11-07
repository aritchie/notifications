using System;


namespace Plugin.Notifications
{
    public class TimeIntervalNotificationTrigger : ITimeBasedNotificationTrigger
    {

        public TimeIntervalNotificationTrigger(TimeSpan interval, bool repeats)
        {
            this.Interval = interval;
            this.Repeats = repeats;
        }


        public TimeSpan Interval { get; }
        public bool Repeats { get; }

        public DateTime CalculateNextTriggerDateFromNow() => DateTime.Now.Add(this.Interval);
    }
}
