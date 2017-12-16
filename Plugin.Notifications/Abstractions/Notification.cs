using System;
using System.Collections.Generic;


namespace Plugin.Notifications
{

    public class Notification
    {
        public static string DefaultTitle { get; set; }
        public static string DefaultSound { get; set; }


        public int? Id { get; set; }
        public string Title { get; set; } = DefaultTitle;
        public string Message { get; set; }
        public string Sound { get; set; } = DefaultSound;
        public IDictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();


        public Notification SetMetadata(string key, string value)
        {
            this.Metadata.Add(key, value);
            return this;
        }


        /// <summary>
        /// Only works with Android
        /// </summary>
        public bool Vibrate { get; set; }

        public TimeSpan? When { get; set; }
        public DateTime? Date { get; set; }

        public bool IsScheduled => this.Date != null || this.When != null;


        public DateTime SendTime
        {
            get
            {
                if (this.Date != null)
                    return this.Date.Value;

                var dt = DateTime.Now;
                if (this.When != null)
                    dt = dt.Add(this.When.Value);

                return dt;
            }
        }

        /*
         * TODO: Repeat Intervals
if (notification.Interval != NotificationInterval.None) {
				not.RepeatInterval = notification.Interval == NotificationInterval.Weekly ? NSCalendarUnit.Week : NSCalendarUnit.Day;
			}
         */
    }
}