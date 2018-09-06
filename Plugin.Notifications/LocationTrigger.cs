using System;


namespace Plugin.Notifications
{
    public class LocationTrigger
    {
        public LocationTrigger(double radiusKm, double latitude, double longitude)
        {
            this.RadiusInKm = radiusKm;
            this.GpsLatitude = latitude;
            this.GpsLongitude = longitude;
        }


        public double RadiusInKm { get; }
        public double GpsLatitude { get; }
        public double GpsLongitude { get; }
        public bool Repeats { get; set; }
    }
}