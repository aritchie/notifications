using System;
using System.Threading.Tasks;


namespace Acr.Notifications
{

    public abstract class AbstractNotificationsImpl : INotifications
    {

        public virtual Task<bool> RequestPermission() => Task.FromResult(true);
        public abstract void CancelAll();
        public abstract bool Cancel(string messageId);
        public abstract string Send(Notification notification);

        public virtual int Badge { get; set; }
        public virtual void Vibrate(int ms = 300) { }
        public virtual string Send(string title, string message, string sound = null, TimeSpan? when = null)
        {
            return this.Send(new Notification
            {
                Title = title,
                Message = message,
                Sound = sound,
                When = when
            });
        }
    }
}
