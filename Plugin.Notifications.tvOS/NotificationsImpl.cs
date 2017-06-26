using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Plugin.Notifications
{
    public class NotificationsImpl : AbstractNotificationsImpl
    {
        public override Task Cancel(string messageId)
        {
            throw new NotImplementedException();
        }


        public override Task Send(Notification notification)
        {
            throw new NotImplementedException();
        }


        public override Task<IEnumerable<Notification>> GetScheduledNotifications()
        {
            throw new NotImplementedException();
        }
    }
}
