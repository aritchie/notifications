using System;


namespace Plugin.Notifications
{
    public class LocationNotificationTrigger : INotificationTrigger
    {
        public LocationNotificationTrigger(double radiusMeters, double latitude, double longitude, bool repeats)
        {
            this.RadiusInMeters = radiusMeters;
            this.GpsLatitude = latitude;
            this.GpsLongitude = longitude;
            this.Repeats = repeats;
        }


        public double RadiusInMeters { get; }
        public double GpsLatitude { get; }
        public double GpsLongitude { get; }
        public bool Repeats { get; }
    }
}