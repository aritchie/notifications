using System;


namespace Plugin.Notifications
{

    public class Notification
    {

        public static string DefaultTitle { get; set; }
        public static string DefaultSound { get; set; }


        public string Id { get; set; }
        public string Title { get; set; } = DefaultTitle;
        public string Message { get; set; } = DefaultSound;
        public string Sound { get; set; }


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
    }
}