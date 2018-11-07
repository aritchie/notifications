using System;


namespace Plugin.Notifications
{
    public class CalendarNotificationTrigger : ITimeBasedNotificationTrigger
    {

        public CalendarNotificationTrigger(DateParts dateParts, bool repeats)
        {
            this.DateParts = dateParts;
            this.Repeats = repeats;
        }


        public DateParts DateParts { get; }
        public bool Repeats { get; }


        public DateTime CalculateNextTriggerDateFromNow()
        {
            //// use original start time as job trigger may be delayed
            //var dt = DateTime.Now.Date.Add(this.StartDate.TimeOfDay);

            //switch (this.RepeatType)
            //{
            //    case CalendarRepeatType.Daily:
            //        dt = dt.AddDays(1);
            //        break;

            //    case CalendarRepeatType.Monthly:
            //        dt = dt.AddMonths(1);
            //        break;

            //    case CalendarRepeatType.Weekly:
            //        dt = dt.AddDays(7);
            //        break;

            //    case CalendarRepeatType.Yearly:
            //        dt = dt.AddYears(1);
            //        break;
            //}

            //return dt;
            var dt = DateTime.Now;
            var dp = this.DateParts;

            if (dp.TimeOfDay != null)
                dt = dt.Date.Add(dp.TimeOfDay.Value);

            if (dp.Year != null)
            {
                while (dt.Year < dp.Year.Value)
                    dt = dt.AddYears(1);
            }

            if (dp.Month != null)
            {
                while (dt.Month != dp.Month)
                    dt = dt.AddMonths(1);
            }

            if (dp.DayOfWeek != null)
            {
                while (dt.DayOfWeek != dp.DayOfWeek.Value)
                    dt = dt.AddDays(1);
            }
            return dt;
        }

        public static CalendarNotificationTrigger Immediate()
            => new CalendarNotificationTrigger(DateTime.Now, CalendarRepeatType.None);
    }
}
