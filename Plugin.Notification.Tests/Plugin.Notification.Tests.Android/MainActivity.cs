using System;
using System.Reflection;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Xunit.Runners.UI;
using Xunit.Sdk;


namespace Plugin.Notification.Tests.Droid
{
    [Activity(
        Label = "Plugin.Notification.Tests",
        Icon = "@mipmap/icon", Theme = "@style/MainTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
    )]
    public class MainActivity : RunnerActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            this.AddExecutionAssembly(typeof(ExtensibilityPointFactory).Assembly);
            this.AddTestAssembly(Assembly.GetExecutingAssembly());
            this.AddTestAssembly(typeof(NotificationManagerTests).Assembly);

			this.AutoStart = false;
			this.TerminateAfterExecution = false;

            base.OnCreate(bundle);
        }
    }
}