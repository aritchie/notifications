using System;


namespace Plugin.Notifications
{
    public static class TriggerExtensions
    {
        public static DateTime CalculateNextTriggerDateFromNow(this CalendarNotificationTrigger trigger)
        {
            if (trigger.DateParts.Date != null)
            {

            }
            return DateTime.Now;
        }


        public static DateTime CalculateNextTriggerDateFromNow(this TimeIntervalNotificationTrigger trigger)
            => DateTime.Now.Add(trigger.Interval);
    }
}
