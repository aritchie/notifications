using System;


namespace Plugin.Notifications
{
    public class UwpOptions
    {
        public static bool DefaultUseLongDuration { get; set; }
        public bool UseLongDuration { get; set; } = DefaultUseLongDuration;
    }
}
