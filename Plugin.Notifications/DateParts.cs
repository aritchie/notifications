using System;


namespace Plugin.Notifications
{
    public class DateParts
    {
        int? Year { get; set; }
        int? Month { get; set; }
        int? Day { get; set; }

        public int? WeekOfYear { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }
        public TimeSpan? TimeOfDay { get; set; }
    }
}
