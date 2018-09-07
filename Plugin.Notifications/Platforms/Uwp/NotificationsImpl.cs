using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.System.Profile;
using Windows.UI.Notifications;
//using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using Plugin.Geofencing;


namespace Plugin.Notifications
{
    //https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/07/08/quickstart-sending-a-local-toast-notification-and-handling-activations-from-it-windows-10/
    public class NotificationsImpl : AbstractNotificationsImpl
    {
        readonly IGeofenceManager geofenceManager;
        readonly BadgeUpdater badgeUpdater;
        readonly ToastNotifier toastNotifier;


        public NotificationsImpl(IGeofenceManager geofenceManager = null)
        {
            this.geofenceManager = geofenceManager ?? CrossGeofences.Current;
            this.badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
            this.toastNotifier = ToastNotificationManager.CreateToastNotifier();

            this.geofenceManager.RegionStatusChanged += this.OnRegionStatusChanged;
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.geofenceManager.RegionStatusChanged -= this.OnRegionStatusChanged;
        }


        public override Task Cancel(string notificationId)
        {
            var notification = this.toastNotifier
                .GetScheduledToastNotifications()
                .FirstOrDefault(x => x.Id.Equals(notificationId, StringComparison.OrdinalIgnoreCase));

            if (notification == null)
                return Task.FromResult(false);

            this.toastNotifier.RemoveFromSchedule(notification);
            return Task.FromResult(true);
        }


        public override Task CancelAll()
        {
            this.Badge = 0;

            var list = this.toastNotifier
                .GetScheduledToastNotifications()
                .ToList();

            foreach (var item in list)
                this.toastNotifier.RemoveFromSchedule(item);

            return Task.CompletedTask;
        }


        public override Task Send(Notification notification)
        {
            var toastContent = new ToastContent
            {
                //Launch = this.ToQueryString(notification.Metadata),
                Visual = new ToastVisual
                {
                    BindingGeneric = new ToastBindingGeneric
                    {
                        Children =
                        {
                            new AdaptiveText
                            {
                                Text = notification.Title
                            },
                            new AdaptiveText
                            {
                                Text = notification.Message
                            }
                        }
                    }
                }
            };
            if (!String.IsNullOrWhiteSpace(notification.Sound) && this.IsAudioSupported)
            {
                if (!notification.Sound.StartsWith("ms-appx:"))
                    notification.Sound = $"ms-appx:///Assets/Audio/{notification.Sound}.m4a";

                toastContent.Audio = new ToastAudio
                {
                    Src = new Uri(notification.Sound)
                };
            }


            //if (notification.ScheduledDate == null)
            //{
            //    var toast = new ToastNotification(toastContent.GetXml());
            //    toast.Activated += (sender, args) => this.OnActivated(notification);
            //    this.toastNotifier.Show(toast);
            //}
            //else
            //{
            //    //https://msdn.microsoft.com/library/74ba3513-0a52-46a0-8769-ed58abe7c05a
            //    //var schedule = new ScheduledToastNotification(toastContent.GetXml(), notification.ScheduledDate.Value)
            //    //{
            //    //    Id = notification.Id
            //    //};
            //    this.toastNotifier.AddToSchedule(schedule);
            //}
            return Task.CompletedTask;
        }


        public override Task<IEnumerable<Notification>> GetScheduledNotifications()
        {
            var list = this.toastNotifier
                .GetScheduledToastNotifications()
                .Select(x => new Notification());

            return Task.FromResult(list);
        }


        public override Task<bool> RequestPermission() => Task.FromResult(true);


        public override void Vibrate(int ms)
        {
            //if (!ApiInformation.IsTypePresent("Windows.Phone.Devices.Notification.VibrationDevice"))
            //    return;

            //Windows
            //    .Phone
            //    .Devices
            //    .Notification
            //    .VibrationDevice
            //    .GetDefault()?
            //    .Vibrate(TimeSpan.FromMilliseconds(ms));
        }


        const string BADGE_KEY = "acr.notifications.badge";
        public override int Badge
        {
            get
            {
                var values = ApplicationData.Current.LocalSettings.Values;
                var id = 0;
                if (values.ContainsKey(BADGE_KEY))
                    Int32.TryParse(values[BADGE_KEY] as string, out id);

                return id;
            }
            set => ApplicationData.Current.LocalSettings.Values[BADGE_KEY] = value.ToString();
        }


        void OnRegionStatusChanged(object sender, GeofenceStatusChangedEventArgs e)
        {

        }


        protected virtual string ToQueryString(IDictionary<string, string> dict)
        {
            //var qs = new QueryString();
            //foreach (var pair in dict)
            //    qs.Add(pair.Key, pair.Value);

            //var r = qs.ToString();
            //return r;
            return String.Empty;
        }


        protected virtual IDictionary<string, string> FromQueryString(string queryString)
        {
            var dict = new Dictionary <string, string>();
            //var qs = QueryString.Parse(queryString);
            //foreach (var pair in qs)
            //{
            //    dict.Add(pair.Name, pair.Value);
            //}
            return dict;
        }


        protected virtual bool IsAudioSupported =>
            AnalyticsInfo.VersionInfo.DeviceFamily.Equals("Windows.Desktop") &&
            !ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 2);
    }
}