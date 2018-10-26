using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Geofencing;
using Plugin.Jobs;
using Plugin.Notifications.Data;


namespace Plugin.Notifications
{
    public abstract class AbstractPlatformNotificationManagerImpl : AbstractNotificationManagerImpl
    {
        readonly IGeofenceManager geofenceMgr;
        readonly INotificationRepository repository;


        protected AbstractPlatformNotificationManagerImpl(IGeofenceManager geofenceMgr,
                                                          IJobManager jobManager,
                                                          INotificationRepository repository)
        {
            this.geofenceMgr = geofenceMgr ?? CrossGeofences.Current;
            this.repository = repository ?? new SqliteNotificationRepository();

            this.geofenceMgr.RegionStatusChanged += (sender, args) =>
            {
                var notification = repository.GetById(args.Region.Identifier);
                if (notification != null)
                    this.NativeSend(notification);
            };
            // (jobManager ?? CrossJobs.Current).Schedule(new JobInfo
            // {
            //     
            // });
        }


        protected abstract void NativeSend(Notification notification);
        

        public override Task<IEnumerable<Notification>> GetPendingNotifications()
            => Task.FromResult(this.repository.GetPending());


        public override Task CancelAll()
        {
            this.repository.DeleteAll();
            return Task.CompletedTask;
        }


        public override Task Cancel(string notificationId)
        {
            this.repository.Delete(notificationId);
            return Task.CompletedTask;
        }


        public override async Task Send(Notification notification)
        {
            this.repository.Insert(notification);
            if (notification.Trigger is LocationNotificationTrigger lt)
            {
                var permission = await this.geofenceMgr.RequestPermission().ConfigureAwait(false);

                this.geofenceMgr.StartMonitoring(new GeofenceRegion(
                    notification.Id,
                    new Position(lt.GpsLatitude, lt.GpsLongitude),
                    Distance.FromMeters(lt.RadiusInMeters)
                ));
            }
        }


        public override int Badge { get; set; }


        protected virtual void OnPostNotificationFired(Notification fired)
        {
            if (!fired.Trigger.Repeats)
            {
                this.repository.Delete(fired.Id);
                this.geofenceMgr.StopMonitoring(fired.Id);
                return;
            }

            // if time based, calc next fire date and update record
            if (fired.Trigger is TimeIntervalNotificationTrigger interval)
            {

            }
            else if (fired.Trigger is CalendarNotificationTrigger calendar)
            {
                // what if this was a specific date, I don't want to resend
            }
        }
    }
}
