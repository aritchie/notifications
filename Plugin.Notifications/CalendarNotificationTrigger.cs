using System;


namespace Plugin.Notifications
{
    public class CalendarNotificationTrigger : INotificationTrigger
    {

        public CalendarNotificationTrigger(DateParts dateParts, bool repeats)
        {
            this.DateParts = dateParts;
            this.Repeats = repeats;
        }


        public DateParts DateParts { get; }
        public bool Repeats { get; }


        public static CalendarNotificationTrigger CreateFromSpecificDateTime(DateTime dateTime)
            => new CalendarNotificationTrigger(new DateParts
            {
                Year = dateTime.Year,
                Month = dateTime.Month,
                Day = dateTime.Day,
                TimeOfDay = dateTime.TimeOfDay
            }, false);
    }
}
