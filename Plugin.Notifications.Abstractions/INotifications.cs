using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Plugin.Notifications
{
    public interface INotifications
    {
        /// <summary>
        /// Fires when a notification selection or action is performed
        /// </summary>
        event EventHandler<Notification> Selected;


        /// <summary>
        /// This is required on iOS to trigger permission request
        /// </summary>
        Task<bool> RequestPermission();


        /// <summary>
        /// Get scheduled notifications
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Notification>> GetScheduledNotifications();


        /// <summary>
        /// Cancel all scheduled notifications
        /// </summary>
        Task CancelAll();


        /// <summary>
        /// Cancel a specific notification
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns>Returns true if message found and cancelled successfully</returns>
        Task Cancel(int notificationId);


        /// <summary>
        /// Send a notification
        /// </summary>
        /// <param name="notification"></param>
        /// <returns>The messageID that you can use to cancel with</returns>
        Task Send(Notification notification);


        /// <summary>
        /// Get the current badge count
        /// </summary>
        Task<int> GetBadge();


        /// <summary>
        /// Set the current badge
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetBadge(int value);


        /// <summary>
        /// Vibrate the device
        /// </summary>
        /// <param name="ms"></param>
        void Vibrate(int ms = 300);
    }
}
