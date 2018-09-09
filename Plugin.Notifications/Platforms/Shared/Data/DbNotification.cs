using System;
using SQLite;


namespace Plugin.Notifications.Data
{
    public class DbNotification
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
        public string Sound { get; set; }
        public bool Vibrate { get; set; }
        public string Payload { get; set; }

        public int TriggerType { get; set; }
        public bool Repeats { get; set; }

        public double? TimeIntervalMs { get; set; }

        public double? LocationCenterGpsLatitude { get; set; }
        public double? LocationCenterGpsLongitude { get; set; }
        public double? LocationRadiusInMeters { get; set; }

        public DateTime? CalendarSpecificDate { get; set; }
        public int? CalendarWeekOfYear { get; set; }
        public int? CalendarDayOfWeek { get; set; }
        public int? CalendarTimeOfDayMs { get; set; }
    }
}