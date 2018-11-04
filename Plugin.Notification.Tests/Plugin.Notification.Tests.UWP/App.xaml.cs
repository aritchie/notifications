using System;
using System.Reflection;
using Xunit.Runners.UI;


namespace Plugin.Notification.Tests.UWP
{
    sealed partial class App : RunnerApplication
    {
        protected override void OnInitializeRunner()
        {
            this.AddTestAssembly(this.GetType().GetTypeInfo().Assembly);
            this.AddTestAssembly(typeof(NotificationManagerTests).Assembly);
        }
    }
}
