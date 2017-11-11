using System;


namespace Plugin.Notifications.Data.EfCore
{
    public class DbSettings
    {
        public int Id { get; set; }

        public int CurrentBadge { get; set; }
        public int CurrentScheduleId { get; set; }
    }
}