using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.System.Profile;
using Windows.UI.Notifications;
using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;


namespace Plugin.Notifications
{
    //https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/07/08/quickstart-sending-a-local-toast-notification-and-handling-activations-from-it-windows-10/
    public class NotificationsImpl : AbstractNotificationsImpl
    {
        readonly BadgeUpdater badgeUpdater;
        readonly ToastNotifier toastNotifier;


        public NotificationsImpl()
        {
            this.badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
            this.toastNotifier = ToastNotificationManager.CreateToastNotifier();
        }


        public override Task Cancel(int notificationId)
        {
            var id = notificationId.ToString();

            var notification = this.toastNotifier
                .GetScheduledToastNotifications()
                .FirstOrDefault(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (notification == null)
                return Task.FromResult(false);

            this.toastNotifier.RemoveFromSchedule(notification);
            return Task.FromResult(true);
        }


        public override async Task CancelAll()
        {
            await this.SetBadge(0);

            var list = this.toastNotifier
                .GetScheduledToastNotifications()
                .ToList();

            foreach (var item in list)
                this.toastNotifier.RemoveFromSchedule(item);
        }


        public override Task Send(Notification notification)
        {
            if (notification.Id == null)
                notification.Id = this.GetNotificationId();

            var toastContent = new ToastContent
            {
                Launch = this.ToQueryString(notification.Metadata),
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

            if (notification.Date == null && notification.When == null)
            {
                var toast = new ToastNotification(toastContent.GetXml());
                this.toastNotifier.Show(toast);
            }
            else
            {
                var schedule = new ScheduledToastNotification(toastContent.GetXml(), notification.SendTime)
                {
                    Id = notification.Id.Value.ToString()
                };
                this.toastNotifier.AddToSchedule(schedule);
            }
            return Task.CompletedTask;
        }


        public override Task<int> GetBadge() => Task.FromResult(this.CurrentBadge);


        public override Task SetBadge(int value)
        {
            this.CurrentBadge = value;
            if (value == 0)
            {
                this.badgeUpdater.Clear();
            }
            else
            {
                this.badgeUpdater.Update(new BadgeNotification(new BadgeNumericContent((uint)value).GetXml()));
            }
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
            if (!ApiInformation.IsTypePresent("Windows.Phone.Devices.Notification.VibrationDevice"))
                return;

            Windows
                .Phone
                .Devices
                .Notification
                .VibrationDevice
                .GetDefault()?
                .Vibrate(TimeSpan.FromMilliseconds(ms));
        }


        const string BADGE_KEY = "acr.notifications.badge";
        protected int CurrentBadge
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


        const string CFG_KEY = "acr.notifications";
        protected virtual int GetNotificationId()
        {
            var id = 0;
            var s = ApplicationData.Current.LocalSettings.Values;
            if (s.ContainsKey(CFG_KEY))
            {
                id = Int32.Parse((string)s[CFG_KEY]);
            }
            id++;
            s[CFG_KEY] = id.ToString();
            return id;
        }


        protected virtual string ToQueryString(IDictionary<string, string> dict)
        {
            var qs = new QueryString();
            foreach (var pair in dict)
                qs.Add(pair.Key, pair.Value);

            var r = qs.ToString();
            return r;
        }


        protected virtual IDictionary<string, string> FromQueryString(string queryString)
        {
            var dict = new Dictionary<string, string>();
            var qs = QueryString.Parse(queryString);
            foreach (var pair in qs)
            {
                dict.Add(pair.Name, pair.Value);
            }
            return dict;
        }


        protected virtual bool IsAudioSupported =>
            AnalyticsInfo.VersionInfo.DeviceFamily.Equals("Windows.Desktop") &&
            !ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 2);
    }
}