using System;
using System.Linq;
using AudioToolbox;
using Foundation;
using UIKit;


namespace Acr.Notifications {

    public class NotificationsImpl : AbstractNotificationsImpl {

        public NotificationsImpl() {
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0)) {
                var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }
        }


        public override int Badge {
            get { return (int)UIApplication.SharedApplication.ApplicationIconBadgeNumber; }
            set { UIApplication.SharedApplication.ApplicationIconBadgeNumber = value; }
        }


        public override string Send(Notification notification) {
            var not = new UILocalNotification {
                FireDate = (NSDate)notification.SendTime,
                AlertAction = notification.Title,
                AlertBody = notification.Message,
                SoundName = notification.Sound
            };
            var msgId = Guid.NewGuid().ToString();

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0) && notification.Actions.Any()) {
                var category = new UIMutableUserNotificationCategory {
                    Identifier = Guid.NewGuid().ToString()
                };
                var actions = notification
                    .Actions
                    .Select(x => new UIMutableUserNotificationAction {
                        Title = x.Title,
                        Identifier = x.Identifier,
                        Destructive = x.IsDestructive,
                        AuthenticationRequired = false,
                        ActivationMode = x.IsBackgroundAction
                            ? UIUserNotificationActivationMode.Background
                            : UIUserNotificationActivationMode.Foreground
                    })
                    .ToArray();

                category.SetActions(actions, UIUserNotificationActionContext.Default);
                var catSet = new NSSet(category);
                var settings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                    catSet
                );
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
                not.Category = category.Identifier;
            }

            UIApplication.SharedApplication.ScheduleLocalNotification(not);
            return msgId;
        }


        public override bool Cancel(string messageId) {
            var key = new NSString("MessageID");
            var keyValue = new NSString(messageId);

            var notification = UIApplication.SharedApplication.ScheduledLocalNotifications.FirstOrDefault(x =>
                x.UserInfo.ContainsKey(key) &&
                x.UserInfo[key].Equals(keyValue)
            );
            if (notification == null)
                return false;

            UIApplication.SharedApplication.CancelLocalNotification(notification);
            return true;
        }


        public override void CancelAll() {
            this.Badge = 0;
            UIApplication.SharedApplication.CancelAllLocalNotifications();
        }


        public override void Vibrate(int ms) {
            SystemSound.Vibrate.PlaySystemSound();
        }
    }
}
/*
    public override void HandleAction (UIApplication application, string actionIdentifier, UILocalNotification localNotification, Action completionHandler)
    {
// if (actionIdentifier == "SaveActionString") {
//
// }
nint tskID = UIApplication.SharedApplication.BeginBackgroundTask(() => {});
new Task ( () => {
//Start monitoring for beacon region when a significant location change is detected.
UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
SendError ("It Worked!", "I Handled the Action");
completionHandler ();
UIApplication.SharedApplication.EndBackgroundTask(tskID);
}).Start();

    }
    public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
    {
        try{
            // show an alert if app is in Foreground
            if(application.ApplicationState == UIApplicationState.Active){
                //Create Alert
                var textInputAlertController = UIAlertController.Create(notification.AlertAction, notification.AlertBody, UIAlertControllerStyle.Alert);

                //Add Actions
                var shareAction = UIAlertAction.Create ("Share", UIAlertActionStyle.Default, alertAction => ShareNotification());
                var saveAction = UIAlertAction.Create("Save", UIAlertActionStyle.Default, alertAction => SaveNotification());
                textInputAlertController.AddAction(shareAction);
                textInputAlertController.AddAction(saveAction);

                //Present Alert
                initialViewController.PresentViewController(textInputAlertController, true, null);
            }

        }catch(Exception ex){
            SendError (ex.Message.ToString (), "ReceivedLocalNotification Error");

        }

    }
*/