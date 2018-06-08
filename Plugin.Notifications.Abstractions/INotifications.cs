using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Plugin.Notifications
{
    public interface INotifications
    {
        /// <summary>
        /// This is required on iOS to trigger permission request
        /// </summary>
        Task<bool> RequestPermission();


        /// <summary>
        ///
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
        /// Get or set the current badge count
        /// </summary>
        Task<int> GetBadge();


        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetBadge(int value);


        /// <summary>
        /// Vibrate the device
        /// </summary>
        /// <param name="ms"></param>
        void Vibrate(int ms = 300);

        /// <summary>
        /// The priority of the notification. Only applies to Android
        /// </summary>
        /// <param name="priority"></param>
        void SetPriority(int priority);
    }
}
