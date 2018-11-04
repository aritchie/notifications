using System;
using SQLite;


namespace Plugin.Notifications.Data
{
    public class DbNotification
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Sound { get; set; }
        public string Payload { get; set; }

        public string TriggerType { get; set; }
        public DateTime? NextTriggerDate { get; set; }
        public string AndroidJson { get; set; }
        public string RequestJson { get; set; }
    }
}