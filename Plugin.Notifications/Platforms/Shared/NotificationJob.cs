using System;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Jobs;
using Plugin.Notifications.Data;


namespace Plugin.Notifications.Platforms.Shared
{
    public class NotificationJob : IJob
    {
        readonly INotificationRepository repository;
        public NotificationJob(INotificationRepository repository)
            => this.repository = repository;


        public async Task Run(JobInfo jobInfo, CancellationToken cancelToken)
        {
            var pending = this.repository.GetPending();

            // TODO: calculate next notification date for each being sent
        }
    }
}
