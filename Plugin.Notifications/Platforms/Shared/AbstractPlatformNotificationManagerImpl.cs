using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Geofencing;
using Plugin.Jobs;
using Plugin.Notifications.Data;
using Plugin.Permissions.Abstractions;


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
                var notification = this.repository.GetById(args.Region.Identifier);
                if (notification != null)
                {
                    this.NativeSend(notification);
                    if (!notification.Trigger.Repeats)
                        this.repository.Delete(notification.Id);
                }
            };
            (jobManager ?? CrossJobs.Current).Schedule(new JobInfo
            {
                Name = nameof(NotificationJob),
                Type = typeof(NotificationJob)
            });
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
            if (notification.Trigger == null)
            {
                this.NativeSend(notification);
                return; // if no trigger, ship it now
            }

            this.repository.Insert(notification);
            if (notification.Trigger is LocationNotificationTrigger lt)
            {
                var permission = await this.geofenceMgr.RequestPermission().ConfigureAwait(false);
                if (permission != PermissionStatus.Granted)
                    throw new ArgumentException("Permission request failed - " + permission);

                this.geofenceMgr.StartMonitoring(new GeofenceRegion(
                    notification.Id,
                    new Position(lt.GpsLatitude, lt.GpsLongitude),
                    Distance.FromMeters(lt.RadiusInMeters)
                )
                {
                    NotifyOnEntry = lt.NotifyOnEntry,
                    NotifyOnExit = lt.NotifyOnExit,
                    SingleUse = !lt.Repeats
                });
            }
            else
            {
                // TODO: calculate next execution date
            }
        }


    }
}
