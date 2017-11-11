using System;
using Android.App;
using Plugin.Notifications.Data;


namespace Plugin.Notifications
{
    public static class AndroidConfig
    {
        public static int AppIconResourceId { get; set; } = GetResourceIdByName("icon");
        public static INotificationRepository Repository { get; set; }


        public static int GetResourceIdByName(string iconName) => Application
            .Context
            .Resources
            .GetIdentifier(
                iconName,
                "drawable",
                Application.Context.PackageName
            );
    }
}