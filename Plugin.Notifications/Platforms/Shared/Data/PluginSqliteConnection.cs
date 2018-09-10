using System;
using System.IO;
using SQLite;


namespace Plugin.Notifications.Data
{
    public class PluginSqliteConnection : SQLiteConnectionWithLock
    {
        public PluginSqliteConnection() :
            base(new SQLiteConnectionString(
                Path.Combine(
#if __ANDROID__
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal),
#elif WINDOWS_UWP
                    Windows.Storage.ApplicationData.Current.LocalFolder.Path,
#endif
                    "acrnotifications.db"
                ),
                true
            ), SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex | SQLiteOpenFlags.ReadWrite)
        {
            this.CreateTable<DbNotification>();
        }


        public TableQuery<DbNotification> Notifications => this.Table<DbNotification>();
    }
}