using System;


namespace Plugin.Notifications
{
    public class DateParts
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }

        //public int? WeekOfYear { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }
        public TimeSpan? TimeOfDay { get; set; }
    }
}
