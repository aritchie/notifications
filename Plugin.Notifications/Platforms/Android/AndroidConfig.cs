using System;
using Android.App;
using Android.Content;
using Plugin.Notifications.Infrastructure;


namespace Plugin.Notifications
{
    public static class AndroidConfig
    {
        public static int AppIconResourceId { get; set; } = GetResourceIdByName("icon");
        public static INotificationRepository Repository { get; set; } = new SqliteNotificationRepository();
        public static ActivityFlags LaunchIntentFlags { get; set; } = ActivityFlags.NewTask | ActivityFlags.ClearTask;


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