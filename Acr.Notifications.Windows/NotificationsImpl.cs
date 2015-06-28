using System;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;


namespace Acr.Notifications {

    public class NotificationsImpl : AbstractNotificationsImpl {
        private const string TOAST_TEMPLATE = @"
<toast>
{0}
  <visual>
    <binding template=""ToastText02"">
      <text id=""1"">{1}</text>
      <text id=""2"">{2}</text>
    </binding>
  </visual>
</toast>";

        private readonly BadgeUpdater badgeUpdater;
        private readonly ToastNotifier toastNotifier;
        private readonly XmlDocument badgeXml;
        private readonly XmlElement badgeEl;


        public NotificationsImpl() {
            this.badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
            this.toastNotifier = ToastNotificationManager.CreateToastNotifier();

            this.badgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);
            this.badgeEl = (XmlElement)this.badgeXml.SelectSingleNode("/badge");
        }


        public override string Send(Notification notification) {
            var id = Guid.NewGuid().ToString();

            var soundXml = notification.Sound == null
                ? String.Empty
                : String.Format("<audio src=\"ms-appx:///Assets/{0}.wav\"/>", notification.Sound);

            var xmlData = String.Format(TOAST_TEMPLATE, soundXml, notification.Title, notification.Message);
            var xml = new XmlDocument();
            xml.LoadXml(xmlData);

            if (notification.Date == null && notification.When == null) {
                var toast = new ToastNotification(xml);
                this.toastNotifier.Show(toast);
            }
            else {
                var schedule = new ScheduledToastNotification(xml, notification.SendTime) { Id = id };
                this.toastNotifier.AddToSchedule(schedule);
            }
            return id;
        }


        public override int Badge {
            get { return 0; }
            set {
                if (value == 0)
                    this.badgeUpdater.Clear();
                else {
                    this.badgeEl.SetAttribute("value", value.ToString());
                    this.badgeUpdater.Update(new BadgeNotification(this.badgeXml));
                }
            }
        }


        public override bool Cancel(string id) {
            var notification = this.toastNotifier
                .GetScheduledToastNotifications()
                .FirstOrDefault(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (notification == null)
                return false;

            this.toastNotifier.RemoveFromSchedule(notification);
            return true;
        }


        public override void CancelAll() {
            this.Badge = 0;
            var list = this.toastNotifier
                .GetScheduledToastNotifications()
                .ToList();

            foreach (var item in list)
                this.toastNotifier.RemoveFromSchedule(item);
        }



#if WINDOWS_PHONE
        public override void Vibrate(int ms) {
            var ts = TimeSpan.FromMilliseconds(ms);
            Windows
                .Phone
                .Devices
                .Notification
                .VibrationDevice
                .GetDefault()
                .Vibrate(ts);
        }
#endif
    }
}