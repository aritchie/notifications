using System;


namespace Plugin.Notifications
{
    public class NotificationInfo
    {
        public NotificationInfo(int id, DateTime? nextDate, NotificationRequest request)
        {
            this.Id = id;
            this.NextScheduledDate = nextDate;
            this.Request = request;
        }


        public int Id { get; }
        public DateTime? NextScheduledDate { get; }
        public NotificationRequest Request { get; }
    }
}
