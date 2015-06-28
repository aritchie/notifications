using System;
using System.Collections.Generic;


namespace Acr.Notifications {

    public class Notification {

        public static string DefaultTitle { get; set; }
        public static string DefaultSound { get; set; }


        public Notification() {
            this.Title = DefaultTitle;
            this.Sound = DefaultSound;
            //this.Actions = new List<NotificationAction>();
        }


        //public string LaunchUri { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Sound { get; set; }

        public TimeSpan? When { get; set; }
        public DateTime? Date { get; set; }

        //public IList<NotificationAction> Actions { get; set; }

        #region Helpers

        public Notification SetTitle(string title) {
            this.Title = title;
            return this;
        }


        public Notification SetMessage(string message) {
            this.Message = message;
            return this;
        }


        //public Notification AddAction(NotificationAction action) {
        //    this.Actions.Add(action);
        //    return this;
        //}


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


        public bool IsScheduled { get { return this.Date != null || this.When != null; }}


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

        #endregion
    }
}