using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;


namespace Plugin.Notifications
{

    public class NotificationsImpl : AbstractNotificationsImpl
    {

        const string TOAST_TEMPLATE = @"
<toast>
{0}
  <visual>
    <binding template=""ToastText02"">
      <text id=""1"">{1}</text>
      <text id=""2"">{2}</text>
    </binding>
  </visual>
</toast>";

        readonly BadgeUpdater badgeUpdater;
        readonly ToastNotifier toastNotifier;
        readonly XmlDocument badgeXml;
        readonly XmlElement badgeEl;


        public NotificationsImpl()
        {
            this.badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
            this.toastNotifier = ToastNotificationManager.CreateToastNotifier();

            this.badgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);
            this.badgeEl = (XmlElement)this.badgeXml.SelectSingleNode("/badge");
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


        public override Task CancelAll()
        {
            this.SetBadge(0);

            var list = this.toastNotifier
                .GetScheduledToastNotifications()
                .ToList();

            foreach (var item in list)
                this.toastNotifier.RemoveFromSchedule(item);

            return Task.CompletedTask;
        }


        public override Task Send(Notification notification)
        {
            var content = new ToastContent
            {
                Launch = ""
            };

            if (notification.Id == null)
                notification.Id = this.GetMessageId();

            var soundXml = notification.Sound == null
                ? String.Empty
                : $"<audio src=\"ms-appx:///Assets/{notification.Sound}.wav\"/>"; // TODO: sound type

            var xmlData = String.Format(TOAST_TEMPLATE, soundXml, notification.Title, notification.Message);
            var xml = new XmlDocument();
            xml.LoadXml(xmlData);

            if (notification.Date == null && notification.When == null)
            {
                var toast = new ToastNotification(xml);
                this.toastNotifier.Show(toast);
            }
            else
            {
                var schedule = new ScheduledToastNotification(xml, notification.SendTime)
                {
                    Id = notification.Id.Value.ToString()
                };
                this.toastNotifier.AddToSchedule(schedule);
            }
            return Task.CompletedTask;
        }


        public override Task<int> GetBadge()
        {
            var attr = this.badgeEl.GetAttribute("value");
            if (attr == null)
                return Task.FromResult(0);

            Int32.TryParse(attr, out var count);
            return Task.FromResult(count);
        }


        public override Task SetBadge(int value)
        {
            if (value == 0)
            {
                this.badgeUpdater.Clear();
            }
            else
            {
                this.badgeEl.SetAttribute("value", value.ToString());
                this.badgeUpdater.Update(new BadgeNotification(this.badgeXml));
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


        public override void Vibrate(int ms) => Windows
                .Phone
                .Devices
                .Notification
                .VibrationDevice
                .GetDefault()
                .Vibrate(TimeSpan.FromMilliseconds(ms));


        const string CFG_KEY = "acr.notifications";
        int GetMessageId()
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
    }
}
/*

// Now we can construct the final toast content
ToastContent toastContent = new ToastContent()
{
    Visual = visual,
    Actions = actions,

    // Arguments when the user taps body of toast
    Launch = new QueryString()
    {
        { "action", "viewConversation" },
        { "conversationId", conversationId.ToString() }

    }.ToString()
};

// And create the toast notification
var toast = new ToastNotification(toastContent.GetXml());




// Now we can construct the final toast content
string argsLaunch = $"action=viewConversation&conversationId={conversationId}";

// TODO: all args need to be XML escaped

string toastXmlString =
$@"<toast launch='{argsLaunch}'>
    {toastVisual}
    {toastActions}
</toast>";

// Parse to XML
XmlDocument toastXml = new XmlDocument();
toastXml.LoadXml(toastXmlString);

// Generate toast
var toast = new ToastNotification(toastXml);

bool supportsCustomAudio = true;

// If we're running on Desktop before Version 1511, do NOT include custom audio
// since it was not supported until Version 1511, and would result in a silent toast.
if (AnalyticsInfo.VersionInfo.DeviceFamily.Equals("Windows.Desktop")
    && !ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 2))
{
    supportsCustomAudio = false;
}

if (supportsCustomAudio)
{
    toastContent.Audio = new ToastAudio()
    {
        Src = new Uri("ms-appx:///Assets/Audio/CustomToastAudio.m4a")
    };
}*/