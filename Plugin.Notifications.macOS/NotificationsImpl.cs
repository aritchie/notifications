using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Plugin.Notifications
{
    public class NotificationsImpl : AbstractNotificationsImpl
    {
        public override Task Cancel(int notificationId)
        {
            throw new NotImplementedException();
        }


        public override Task Send(Notification notification)
        {
            throw new NotImplementedException();
        }


        public override Task<int> GetBadge()
        {
            throw new NotImplementedException();
        }


        public override Task SetBadge(int value)
        {
            throw new NotImplementedException();
        }


        public override void Vibrate(int ms)
        {
            throw new NotImplementedException();
        }


        public override Task<IEnumerable<Notification>> GetScheduledNotifications()
        {
            throw new NotImplementedException();
        }


        public override Task<bool> RequestPermission()
        {
            throw new NotImplementedException();
        }
    }
}
