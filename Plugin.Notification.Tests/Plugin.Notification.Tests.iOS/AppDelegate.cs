using System;
using System.Reflection;
using Foundation;
using UIKit;
using Xunit.Runner;
using Xunit.Sdk;


namespace Plugin.Notification.Tests.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : RunnerAppDelegate
    {
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            this.AddExecutionAssembly(typeof(ExtensibilityPointFactory).Assembly);
            this.AddTestAssembly(Assembly.GetExecutingAssembly());
            this.AddTestAssembly(typeof(NotificationManagerTests).Assembly);

            this.AutoStart = false;
            this.TerminateAfterExecution = false;

            return base.FinishedLaunching(application, launchOptions);
        }
    }
}
