using System;
using Android.App;
using Android.Content;


namespace Plugin.Notifications
{
    public static class Helpers
    {
        public static int AppIconResourceId { get; set; } = GetResourceIdByName("icon");


        public static ActivityFlags ToNative(this AndroidActivityFlags flags)
        {
            var intValue = (int) flags;
            var native = (ActivityFlags) intValue;
            return native;
        }


        public static NotificationImportance ToNative(this AndroidNotificationImportance import)
        {
            var intValue = (int) import;
            var native = (NotificationImportance) intValue;
            return native;
        }


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