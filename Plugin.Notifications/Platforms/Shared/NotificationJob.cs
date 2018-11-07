using System;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Jobs;


namespace Plugin.Notifications
{
    public class NotificationJob : IJob
    {
        public Task Run(JobInfo jobInfo, CancellationToken cancelToken)
        {
            var pending = Internals.Repository.GetPending();

            foreach (var notification in pending)
                this.Process(notification);

            return Task.CompletedTask;
        }


        void Process(NotificationInfo notification)
        {
            Internals.NativeSend.Invoke(notification.Request);

            if (notification.Request.Trigger.Repeats && notification.Request.Trigger is ITimeBasedNotificationTrigger trigger)
            {
                var next = trigger.CalculateNextTriggerDateFromNow();
                Internals.Repository.Update(notification.Id, next);
            }
            else
            {
                Internals.Repository.Delete(notification.Id);
            }
        }
    }
}
