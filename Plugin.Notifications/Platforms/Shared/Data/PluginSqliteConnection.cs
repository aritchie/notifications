using System;
using System.IO;
using SQLite;


namespace Plugin.Notifications.Data
{
    public class PluginSqliteConnection : SQLiteConnectionWithLock
    {
        public PluginSqliteConnection() :
            base(new SQLiteConnectionString(
#if DEBUG
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "notifications.db"),
#else
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "notifications.db"),
#endif
                true
            ), SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex | SQLiteOpenFlags.ReadWrite)
        {
            this.CreateTable<DbNotification>();
        }


        public TableQuery<DbNotification> Notifications => this.Table<DbNotification>();
    }
}