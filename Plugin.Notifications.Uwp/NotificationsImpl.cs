using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;


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


        const string CFG_KEY = "acr.notifications";
        string GetMessageId()
        {
            var id = 0;
            var s = ApplicationData.Current.LocalSettings.Values;
            if (s.ContainsKey(CFG_KEY))
            {
                id = Int32.Parse((string)s[CFG_KEY]);
            }
            id++;
            s[CFG_KEY] = id.ToString();
            return id.ToString();
        }


        public override Task Send(Notification notification)
        {
            var id = this.GetMessageId();

            var soundXml = notification.Sound == null
                ? String.Empty
                : $"<audio src=\"ms-appx:///Assets/{notification.Sound}.wav\"/>";

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
                    Id = id
                };
                this.toastNotifier.AddToSchedule(schedule);
            }
            //return id;
            return Task.CompletedTask;
        }


        //public override int Badge
        //{
        //    get { return 0; }
        //    set
        //    {
        //        if (value == 0)
        //        {
        //            this.badgeUpdater.Clear();
        //        }
        //        else
        //        {
        //            this.badgeEl.SetAttribute("value", value.ToString());
        //            this.badgeUpdater.Update(new BadgeNotification(this.badgeXml));
        //        }
        //    }
        //}


        public override Task<bool> Cancel(string id)
        {
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
            //this.Badge = 0;
            var list = this.toastNotifier
                .GetScheduledToastNotifications()
                .ToList();

            foreach (var item in list)
                this.toastNotifier.RemoveFromSchedule(item);

            return Task.CompletedTask;
        }


        public override void Vibrate(int ms) => Windows
                .Phone
                .Devices
                .Notification
                .VibrationDevice
                .GetDefault()
                .Vibrate(TimeSpan.FromMilliseconds(ms));
    }
}