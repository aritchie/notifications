using System;
using System.Drawing;


namespace Plugin.Notifications
{
    public class AndroidOptions
    {
        public static string DefaultChannel { get; set; } = "pluginnotifications";
        public static string DefaultSmallIconResourceName { get; set; }
        public static string DefaultChannelDescription { get; set; }
        public static Color? DefaultColor { get; set; }
        public static bool DefaultVibrate { get; set; }
        public static AndroidActivityFlags DefaultLaunchActivityFlags { get; set; } = AndroidActivityFlags.NewTask | AndroidActivityFlags.ClearTask;

        public AndroidActivityFlags LaunchActivityFlags { get; set; } = DefaultLaunchActivityFlags;
        public bool Vibrate { get; set; } = DefaultVibrate;
        public int? Priority { get; set; }
        public Color? Color { get; set; } = DefaultColor; // API 21+
        public string Channel { get; set; } = DefaultChannel;
        public string ChannelDescription { get; set; } = DefaultChannelDescription;
        public string SmallIconResourceName { get; set; } = DefaultSmallIconResourceName;
    }
}
