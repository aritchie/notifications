using System;
using System.Collections;
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


        public static NSDictionary ToNsDictionary(this IDictionary<string, string> dict)
        {
            var ns = new NSDictionary();
            foreach (var pair in dict)
                ns.SetValueForKey(new NSString(pair.Value), new NSString(pair.Key));

            return ns;
        }


        const string NOTIFICATION_ID_KEY = "notificationid";
        static readonly object syncLock = new object();
        public static int GeneratedNotificationId(this Notification notification)
        {
            var id = 0;
            var p = NSUserDefaults.StandardUserDefaults;

            lock (syncLock)
            {
                var value = p.ValueForKey(new NSString(NOTIFICATION_ID_KEY));
                if (value != null)
                    id = (int)p.IntForKey(NOTIFICATION_ID_KEY);

                id++;
                p.SetInt(id, NOTIFICATION_ID_KEY);
                p.Synchronize();

                notification.Id = id;
            }
            return id;
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