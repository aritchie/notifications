using System;


namespace Acr.Notifications {

    public class NotificationAction {

        public string Title { get; set; }
        public string Identifier { get; set; }
        public bool IsDestructive { get; set; }
        public bool IsBackgroundAction { get; set; }


        public NotificationAction() { }
        public NotificationAction(string title, string identifier, bool destructive = false, bool isBackgroundAction = true) {
            this.Title = title;
            this.Identifier = identifier;
            this.IsDestructive = destructive;
            this.IsBackgroundAction = isBackgroundAction;
        }
    }
}