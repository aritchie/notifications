using System;
using System.IO;
using SQLite;


namespace Plugin.Notifications
{
    public class AcrSqliteConnection : SQLiteConnection
    {
        public AcrSqliteConnection() : base(Path.Combine(Environment.Documents.GetPersonalPath(), "notifications.db"), true)
        {
            this.CreateTable<DbNotification>();
        }


        public TableQuery<DbNotification> Notifications => this.Table<DbNotification>();
    }
}