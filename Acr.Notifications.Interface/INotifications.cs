using System;


namespace Acr.Notifications {

    public interface INotifications {

        void CancelAll();

        bool Cancel(string messageId);

        string Send(string title, string message, string sound = null, TimeSpan? when = null);
        string Send(Notification notification);

        int Badge { get; set; }

        void Vibrate(int ms = 300);
    }
}
