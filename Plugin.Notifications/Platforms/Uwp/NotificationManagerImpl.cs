using System;
using System.IO;
using Windows.Foundation.Metadata;
using Windows.System.Profile;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using Plugin.Geofencing;
using Plugin.Jobs;
using Plugin.Notifications.Data;


namespace Plugin.Notifications
{
    //https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/07/08/quickstart-sending-a-local-toast-notification-and-handling-activations-from-it-windows-10/
    public class NotificationManagerImpl : AbstractPlatformNotificationManagerImpl
    {
        readonly BadgeUpdater badgeUpdater;
        readonly ToastNotifier toastNotifier;


        public NotificationManagerImpl(IGeofenceManager geofenceMgr = null,
                                       IJobManager jobManager = null,
                                       INotificationRepository repository = null) : base(geofenceMgr, jobManager, repository)
        {
            this.badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
            this.toastNotifier = ToastNotificationManager.CreateToastNotifier();
        }


        protected override void NativeSend(Notification notification)
        {
            var toastContent = new ToastContent
            {
                Duration = notification.Windows.UseLongDuration ? ToastDuration.Long : ToastDuration.Short,
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
                var sound = this.BuildSoundPath(notification.Sound);
                toastContent.Audio = new ToastAudio
                {
                    Src = new Uri(sound)
                };
            }
            //toastContent.Actions
            //toastContent.AdditionalProperties.Ad
            //toastContent.Launch = "";
            var native = new ToastNotification(toastContent.GetXml());
            this.toastNotifier.Show(native);
        }


        string BuildSoundPath(string sound)
        {
            var ext = Path.GetExtension(sound);
            if (String.IsNullOrWhiteSpace(ext))
                sound += ".mp4";

            if (sound.StartsWith("ms-appx://"))
                sound = "ms-appx://" + sound;

            return sound;
        }


        //public override void Vibrate(int ms)
        //{
            //if (!ApiInformation.IsTypePresent("Windows.Phone.Devices.Notification.VibrationDevice"))
            //    return;

            //Windows
            //    .Phone
            //    .Devices
            //    .Notification
            //    .VibrationDevice
            //    .GetDefault()?
            //    .Vibrate(TimeSpan.FromMilliseconds(ms));
        //}


        //const string BADGE_KEY = "acr.notifications.badge";
        //public override int Badge
        //{
        //    get
        //    {
        //        var values = ApplicationData.Current.LocalSettings.Values;
        //        var id = 0;
        //        if (values.ContainsKey(BADGE_KEY))
        //            Int32.TryParse(values[BADGE_KEY] as string, out id);

        //        return id;
        //    }
        //    set => ApplicationData.Current.LocalSettings.Values[BADGE_KEY] = value.ToString();
        //}


        protected virtual bool IsAudioSupported =>
            AnalyticsInfo.VersionInfo.DeviceFamily.Equals("Windows.Desktop") &&
            !ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 2);
    }
}