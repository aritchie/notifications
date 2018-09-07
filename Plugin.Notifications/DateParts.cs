using System;


namespace Plugin.Notifications
{
    public class DateParts
    {
        public DateTime? Date { get; set; }

        public int? WeekOfYear { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }
        public TimeSpan? TimeOfDay { get; set; }
    }
}
