using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Plugin.Notifications
{

    public abstract class AbstractNotificationsImpl : INotifications
    {
        public event EventHandler<Notification> Selected;
        public abstract Task Cancel(int notificationId);
        public abstract Task Send(Notification notification);
        public abstract Task<IEnumerable<Notification>> GetScheduledNotifications();
        public abstract Task<bool> RequestPermission();
        public abstract Task<int> GetBadge();
        public abstract Task SetBadge(int value);
        public abstract void Vibrate(int ms);


        public virtual async Task CancelAll()
        {
            var notifications = await this.GetScheduledNotifications();
            foreach (var notification in notifications)
            {
                await this.Cancel(notification.Id.Value);
            }
        }


        protected virtual void OnSelected(Notification notification)
            => this.Selected?.Invoke(this, notification);
    }
}
