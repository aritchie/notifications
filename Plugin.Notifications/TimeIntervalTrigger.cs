using System;


namespace Plugin.Notifications
{
    public class TimeIntervalTrigger
    {
        public TimeSpan Interval { get; set; }
        public bool Repeats { get; set; }
    }
}
