using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Plugin.Notifications
{

    public abstract class AbstractNotificationManagerImpl : INotificationManager, IDisposable
    {
        ~AbstractNotificationManagerImpl() => this.Dispose(false);


        public event EventHandler<Notification> Activated;
        public abstract Task Cancel(string notificationId);
        public abstract Task Send(Notification notification);
        public abstract Task<IEnumerable<Notification>> GetPendingNotifications();
        public abstract Task<bool> RequestPermission();
        public abstract void Vibrate(int ms);
        public abstract int Badge { get; set; }


        public virtual async Task CancelAll()
        {
            var notifications = await this.GetPendingNotifications();
            foreach (var notification in notifications)
                await this.Cancel(notification.Id).ConfigureAwait(false);
        }


        protected virtual void OnActivated(Notification notification)
            => this.Activated?.Invoke(this, notification);


        public void Dispose() => this.Dispose(true);
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
