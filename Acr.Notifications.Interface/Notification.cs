using System;


namespace Acr.Notifications {

    public class Notification {

        public static string DefaultTitle { get; set; }
        public static string DefaultSound { get; set; }


        public string Title { get; set; } = DefaultTitle;
        public string Message { get; set; } = DefaultSound;
        public string Sound { get; set; }

        public TimeSpan? When { get; set; }
        public DateTime? Date { get; set; }


        public Notification SetTitle(string title) {
            this.Title = title;
            return this;
        }


        public Notification SetMessage(string message) {
            this.Message = message;
            return this;
        }


        public Notification SetSound(string sound) {
            this.Sound = sound;
            return this;
        }


        public Notification SetSchedule(TimeSpan timeSpan) {
            this.When = timeSpan;
            return this;
        }


        public Notification SetSchedule(DateTime dateTime) {
            this.Date = dateTime;
            return this;
        }


        public bool IsScheduled => this.Date != null || this.When != null;


        public DateTime SendTime {
            get {
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