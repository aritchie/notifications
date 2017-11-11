using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Plugin.Notifications.Infrastructure;


namespace Plugin.Notifications.Data.EfCore.Maps
{
    public class DbNotificationMap : IEntityTypeConfiguration<DbNotification>
    {
        public void Configure(EntityTypeBuilder<DbNotification> builder)
        {
            builder.ToTable("Notifications");
            builder.HasKey(x => x.Id);
        }
    }
}
