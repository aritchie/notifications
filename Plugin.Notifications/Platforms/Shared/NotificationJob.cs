using System;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Jobs;


namespace Plugin.Notifications
{
    public class NotificationJob : IJob
    {
        public async Task Run(JobInfo jobInfo, CancellationToken cancelToken)
        {
            var pending = Internals.Repository.GetPending();

            foreach (var notification in pending)
                this.Process(notification);
        }


        void Process(NotificationInfo notification)
        {
            Internals.NativeSend.Invoke(notification.Request);

            var dateTime = DateTime.MinValue;
            //if (notification.Trigger is TimeIntervalNotificationTrigger interval)
            //{
            //    dateTime = interval.CalculateNextTriggerDateFromNow();
            //}
            //else if (notification.Trigger is CalendarNotificationTrigger calendar)
            //{
            //    // what if this was a specific date, I don't want to resend
            //    dateTime = calendar.CalculateNextTriggerDateFromNow();
            //}

            // TODO: update or delete if not further repeats
        }
    }
}
