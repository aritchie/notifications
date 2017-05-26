using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Plugin.Notifications
{

    public abstract class AbstractNotificationsImpl : INotifications
    {
        public abstract Task Cancel(string messageId);
        public abstract Task Send(Notification notification);
        public abstract Task<IEnumerable<Notification>> GetScheduledNotifications();


        public virtual async Task CancelAll()
        {
            var notifications = await this.GetScheduledNotifications();
            foreach (var notification in notifications)
            {
                await this.Cancel(notification.Id);
            }
        }


        public virtual Task<bool> RequestPermission() => Task.FromResult(true);
        public virtual Task<int> GetBadge() => Task.FromResult(-1);
        public virtual Task SetBadge(int value) => Task.FromResult(new object());
        public virtual void Vibrate(int ms) {}
    }
}
