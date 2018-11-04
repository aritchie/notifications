using System;
using System.Collections.Generic;
using System.Linq;
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

            Internals.NativeSend = this.NativeSend;
            Internals.Repository = this.repository;

            this.geofenceMgr.RegionStatusChanged += (sender, args) =>
            {
                if (Int32.TryParse(args.Region.Identifier, out int id))
                {
                    var notification = this.repository.GetById(id);
                    if (notification != null)
                    {
                        this.NativeSend(notification.Request);
                        if (!notification.Request.Trigger.Repeats)
                            this.repository.Delete(notification.Id);
                    }
                }
            };
            (jobManager ?? CrossJobs.Current).Schedule(new JobInfo
            {
                Name = nameof(NotificationJob),
                Type = typeof(NotificationJob)
            });
        }


        protected abstract void NativeSend(NotificationRequest notification);


        public override Task<IEnumerable<NotificationInfo>> GetPendingNotifications()
            => Task.FromResult(this.repository.GetPending());


        public override async Task CancelAll()
        {
            var nots = this.geofenceMgr.MonitoredRegions.Where(x => x.Identifier.StartsWith("notification"));
            foreach (var not in nots)
                await this.geofenceMgr.StopMonitoring(not);
            this.repository.DeleteAll();
        }


        public override async Task Cancel(int notificationId)
        {
            await this.geofenceMgr.StopMonitoring("notification-" + notificationId);
            this.repository.Delete(notificationId);
        }


        public override async Task<NotificationInfo> Send(NotificationRequest notification)
        {
            if (notification.Trigger == null)
            {
                this.NativeSend(notification);
                return null; // if no trigger, ship it now
            }

            DateTime? nextTriggerDate = null;
            if (notification.Trigger is LocationNotificationTrigger lt)
            {
                await this.geofenceMgr.StartMonitoring(new GeofenceRegion(
                    notification.Id.ToString(),
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
                nextTriggerDate = null;
                // TODO: calculate next execution date
            }
            var id = this.repository.Insert(notification, nextTriggerDate);
            return new NotificationInfo(id, nextTriggerDate, notification);
        }
    }
}
