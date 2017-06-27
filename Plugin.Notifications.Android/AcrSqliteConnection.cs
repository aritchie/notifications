using System;
using System.IO;
using SQLite;


namespace Plugin.Notifications
{
    public class AcrSqliteConnection : SQLiteConnectionWithLock
    {
        public AcrSqliteConnection() :
            base(new SQLiteConnectionString(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "notifications.db"),
                true
            ), SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex | SQLiteOpenFlags.ReadWrite)
        {
            this.CreateTable<DbNotification>();
            this.CreateTable<DbNotificationMetadata>();
            this.CreateTable<DbSettings>();
        }


        public TableQuery<DbNotification> Notifications => this.Table<DbNotification>();
        public TableQuery<DbNotificationMetadata> NotificationMetadata => this.Table<DbNotificationMetadata>();
        public TableQuery<DbSettings> Settings => this.Table<DbSettings>();
    }
}