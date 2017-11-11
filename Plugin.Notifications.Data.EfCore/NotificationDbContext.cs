using System;
using Microsoft.EntityFrameworkCore;
using Plugin.Notifications.Data.EfCore.Maps;
using Plugin.Notifications.Infrastructure;


namespace Plugin.Notifications.Data.EfCore
{
    public class NotificationDbContext : DbContext
    {
        public DbSet<DbNotification> Notifications { get; set; }
        public DbSet<DbNotificationMetadata> Metadata { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite("Data Source=AcrNotifications.db");
            base.OnConfiguring(builder);
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new DbNotificationMap());
            builder.ApplyConfiguration(new DbNotificationMetadataMap());
            base.OnModelCreating(builder);
        }
    }
}
