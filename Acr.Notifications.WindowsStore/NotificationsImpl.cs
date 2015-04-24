using System;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;


namespace Acr.Notifications {

    public class NotificationsImpl : INotifications {
        private readonly BadgeUpdater badgeUpdater;
        private readonly ToastNotifier toastNotifier;


        public NotificationsImpl() {
            this.badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
            this.toastNotifier = ToastNotificationManager.CreateToastNotifier();
//badgeXml = Notifications.BadgeUpdateManager.getTemplateContent(Notifications.BadgeTemplateType.badgeNumber);
//    badgeAttributes = badgeXml.getElementsByTagName("badge");
//    badgeAttributes[0].setAttribute("value", "7");
        }


        public void Send(string title, string message, string sound = null, TimeSpan? when = null) {
            var xmlData = String.Format("CONST", title, message);

            var xml = new XmlDocument();
            xml.LoadXml(xmlData);

            if (when == null) {
                var toast = new ToastNotification(xml);
                this.toastNotifier.Show(toast);
            }
            else {
                //new ScheduledToastNotification(xml, DateTimeOffset.Now.Add(when))
                //this.toastNotifier.AddToSchedule();
            }
        }


        public int Badge {
            get { return 0; }
            set {
                if (value == 0)
                    this.badgeUpdater.Clear();
                //else
                    //this.badgeUpdater.Update(new BadgeNotification());
            }
        }


        public void CancelAll() {
            
        }
    }
}
/*
<badge value="1"/>


<toast>
  <audio src="ms-appx:///Assets/sound.wav"/>

  <visual>
    <binding template="ToastText02">
      <text id="1">headlineText</text>
      <text id="2">bodyText</text>
    </binding>  
  </visual>
</toast>


function sendBadgeNotification() {
    var Notifications = Windows.UI.Notifications;
    var badgeXml;
    var badgeAttributes;

    // Get an XML DOM version of a specific template by using getTemplateContent.
    badgeXml = Notifications.BadgeUpdateManager.getTemplateContent(Notifications.BadgeTemplateType.badgeNumber);
    badgeAttributes = badgeXml.getElementsByTagName("badge");
    badgeAttributes[0].setAttribute("value", "7");

    // Create a badge notification from the XML content.
    var badgeNotification = new Notifications.BadgeNotification(badgeXml);

    // Send the badge notification to the app's tile.
    Notifications.BadgeUpdateManager.createBadgeUpdaterForApplication().update(badgeNotification);
}
*/