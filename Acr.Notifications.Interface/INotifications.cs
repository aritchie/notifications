using System;


namespace Acr.Notifications {

    public interface INotifications {

        /// <summary>
        /// Cancel all scheduled notifications
        /// </summary>
        void CancelAll();

        /// <summary>
        /// Cancel a specific notification
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>Returns true if message found and cancelled successfully</returns>
        bool Cancel(string messageId);

        /// <summary>
        /// Send a notification
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="sound"></param>
        /// <param name="when"></param>
        /// <returns>The messageID that you can use to cancel with</returns>
        string Send(string title, string message, string sound = null, TimeSpan? when = null);


        /// <summary>
        /// Send a notification
        /// </summary>
        /// <param name="notification"></param>
        /// <returns>The messageID that you can use to cancel with</returns>
        string Send(Notification notification);


        /// <summary>
        /// Get or set the current badge count
        /// </summary>
        int Badge { get; set; }


        /// <summary>
        /// Vibrate the device
        /// </summary>
        /// <param name="ms"></param>
        void Vibrate(int ms = 300);
    }
}
