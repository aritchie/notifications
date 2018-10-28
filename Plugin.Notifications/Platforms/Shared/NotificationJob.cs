using System;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Jobs;
using Plugin.Notifications.Data;


namespace Plugin.Notifications
{
    public class NotificationJob : IJob
    {
        readonly INotificationRepository repository;
        public NotificationJob(INotificationRepository repository)
            => this.repository = repository;


        public async Task Run(JobInfo jobInfo, CancellationToken cancelToken)
        {
            var pending = this.repository.GetPending();

            foreach (var notification in pending)
                this.Process(notification);
        }


        void Process(Notification notification)
        {
            var dateTime = DateTime.MinValue;
            if (notification.Trigger is TimeIntervalNotificationTrigger interval)
            {
                dateTime = interval.CalculateNextTriggerDateFromNow();
            }
            else if (notification.Trigger is CalendarNotificationTrigger calendar)
            {
                // what if this was a specific date, I don't want to resend
                dateTime = calendar.CalculateNextTriggerDateFromNow();
            }
        }
    }
}
