using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.System.Profile;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;


namespace Plugin.Notifications
{
    //https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/07/08/quickstart-sending-a-local-toast-notification-and-handling-activations-from-it-windows-10/
    public class NotificationManagerImpl : AbstractPlatformNotificationManagerImpl
    {
        readonly BadgeUpdater badgeUpdater;
        readonly ToastNotifier toastNotifier;


        public NotificationManagerImpl() : base(null, null, null)
        {
            this.badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
            this.toastNotifier = ToastNotificationManager.CreateToastNotifier();
        }
        

        protected override void NativeSend(Notification notification)
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

            return Task.CompletedTask;            
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