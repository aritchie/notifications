using System;
using CoreLocation;
using Foundation;
using UserNotifications;


namespace Plugin.Notifications
{
    public static class iOSExtensions
    {
        public static UNCalendarNotificationTrigger ToNative(this CalendarTrigger trigger)
            => UNCalendarNotificationTrigger.CreateTrigger(new NSDateComponents
            {
                //Year = dt.Year,
                //Month = dt.Month,
                //Day = dt.Day,
                //Hour = dt.Hour,
                //Minute = dt.Minute,
                //Second = dt.Second
            }, trigger.Repeats);


        public static UNLocationNotificationTrigger ToNative(this LocationTrigger trigger, string identifier) => UNLocationNotificationTrigger
            .CreateTrigger(
                new CLCircularRegion(
                    new CLLocationCoordinate2D(
                        trigger.GpsLatitude,
                        trigger.GpsLongitude
                    ),
                    trigger.RadiusInKm,
                    identifier
                )
                {
                    NotifyOnEntry = true
                },
                trigger.Repeats
            );


        public static UNTimeIntervalNotificationTrigger ToNative(this TimeIntervalTrigger trigger)
            => UNTimeIntervalNotificationTrigger.CreateTrigger(trigger.Interval.TotalMilliseconds, trigger.Repeats);
    }
}
