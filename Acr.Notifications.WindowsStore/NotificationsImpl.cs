using System;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;


namespace Acr.Notifications {

    public class NotificationsImpl : INotifications {
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


        public void Send(string title, string message, string sound = null, TimeSpan? when = null) {
            var soundXml = sound == null
                ? String.Empty
                : String.Format("<audio src=\"ms-appx:///Assets/{0}.wav\"/>", sound);

            var xmlData = String.Format(TOAST_TEMPLATE, soundXml, title, message);
            var xml = new XmlDocument();
            xml.LoadXml(xmlData);

            if (when == null) {
                var toast = new ToastNotification(xml);
                this.toastNotifier.Show(toast);
            }
            else {
                var date = DateTimeOffset.Now.Add(when.Value);
                var schedule = new ScheduledToastNotification(xml, date);
                this.toastNotifier.AddToSchedule(schedule);
            }
        }


        public int Badge {
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


        public void CancelAll() {
            this.Badge = 0;
            var list = this.toastNotifier
                .GetScheduledToastNotifications()
                .ToList();

            foreach (var item in list)
                this.toastNotifier.RemoveFromSchedule(item);
        }
    }
}