using System;


namespace Plugin.Notifications
{
    public class CalendarTrigger
    {
        //init(calendar: Calendar?, timeZone: TimeZone?, era: Int?, year: Int?, month: Int?, day: Int?, hour: Int?, minute: Int?, second: Int?, nanosecond: Int?, weekday: Int?, weekdayOrdinal: Int?, quarter: Int?, weekOfMonth: Int?, weekOfYear: Int?, yearForWeekOfYear: Int?)
        //Initializes a date components value, optionally specifying values for its fields.

        public bool Repeats { get; set; }


        //public static CalendarTrigger FromDate(DateTime dt) =>                 UNCalendarNotificationTrigger.CreateTrigger(new NSDateComponents
        //{
        //    Year = dt.Year,
        //    Month = dt.Month,
        //    Day = dt.Day,
        //    Hour = dt.Hour,
        //    Minute = dt.Minute,
        //    Second = dt.Second
        //}, false)
    }
}
