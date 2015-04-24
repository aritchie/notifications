using System;


namespace Acr.Notifications {

    public interface INotifications {

        void CancelAll();
        void Send(string title, string message, string sound = null, TimeSpan? when = null);
        int Badge { get; set; }
    }
}
