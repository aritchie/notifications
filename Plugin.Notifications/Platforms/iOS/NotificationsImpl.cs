using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;


namespace Plugin.Notifications
{

    public class NotificationsImpl : AbstractNotificationsImpl
    {
        readonly INotifications impl;


        public NotificationsImpl()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                this.impl = new UNNotificationsImpl();
            else
                this.impl = new UILocalNotificationsImpl();
        }


        public override Task Cancel(int notificationId) => this.impl.Cancel(notificationId);
        public override Task Send(Notification notification) => this.impl.Send(notification);
        public override Task<int> GetBadge() => this.impl.GetBadge();
        public override Task SetBadge(int value) => this.impl.SetBadge(value);
        public override void Vibrate(int ms) => this.impl.Vibrate(ms);
        public override Task<IEnumerable<Notification>> GetScheduledNotifications() => this.impl.GetScheduledNotifications();
        public override Task<bool> RequestPermission() => this.impl.RequestPermission();
    }
}