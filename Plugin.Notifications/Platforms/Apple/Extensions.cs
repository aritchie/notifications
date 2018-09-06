using System;
using System.Collections.Generic;
using Foundation;


namespace Plugin.Notifications
{
    public static class Extensions
    {
        static DateTime reference = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);


        public static DateTime ToDateTime(this NSDate date)
        {
            var utcDateTime = reference.AddSeconds(date.SecondsSinceReferenceDate);
            var dateTime = utcDateTime.ToLocalTime();
            return dateTime;
        }


        public static NSDate ToNSDate(this DateTime datetime)
        {
            var utcDateTime = datetime.ToUniversalTime();
            var date = NSDate.FromTimeIntervalSinceReferenceDate((utcDateTime - reference).TotalSeconds);
            return date;
        }


        public static NSDictionary MetadataToNsDictionary(this Notification notification)
        {
            var ns = new NSMutableDictionary();
            if (notification.Metadata != null)
            {
                foreach (var pair in notification.Metadata)
                {
                    ns.SetValueForKey(new NSString(pair.Value), new NSString(pair.Key));
                }
            }
            return ns;
        }


        public static IDictionary<string, string> FromNsDictionary(this NSDictionary ns)
        {
            var dict = new Dictionary<string, string>();
            if (ns != null)
                foreach (var pair in ns)
                    dict.Add(pair.Key.ToString(), pair.Value.ToString());

            return dict;
        }
    }
}