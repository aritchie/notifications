using System;
using System.Diagnostics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


namespace Samples.iOS {

    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate {

        public override bool FinishedLaunching(UIApplication app, NSDictionary options) {
            Forms.Init();
            this.LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }


        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification) {
            Debug.WriteLine("Location Notification: {0}:{1}", notification.AlertAction, notification.AlertBody);
            //Debug.WriteLine("Location Notification: " + notification.AlertBody);

            if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active) {
                new UIAlertView(notification.AlertAction, notification.AlertBody, null, "OK", null).Show();

                //var alert = UIAlertController.Create(notification.AlertAction, notification.AlertBody, UIAlertControllerStyle.Alert);
                //UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
            }
        }


        public override void HandleAction(UIApplication application, string actionIdentifier, NSDictionary remoteNotificationInfo, Action completionHandler) {
            if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active)
                new UIAlertView("Notification Action", actionIdentifier, null, "OK", null).Show();
            else
                Console.WriteLine("Notification Action {0}", actionIdentifier);

            base.HandleAction(application, actionIdentifier, remoteNotificationInfo, completionHandler);
        }
    }
}
