using System;
using System.Threading.Tasks;


namespace Acr.Notifications
{

    public interface INotifications
    {
        /// <summary>
        /// This is required on iOS to trigger permission request
        /// </summary>
        Task<bool> RequestPermission();


        /// <summary>
        /// Cancel all scheduled notifications
        /// </summary>
        Task CancelAll();


        /// <summary>
        /// Cancel a specific notification
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>Returns true if message found and cancelled successfully</returns>
        Task Cancel(string messageId);


        /// <summary>
        /// Send a notification
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="sound"></param>
        /// <param name="when"></param>
        /// <returns>The messageID that you can use to cancel with</returns>
        Task Send(string title, string message, string sound = null, TimeSpan? when = null);


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
