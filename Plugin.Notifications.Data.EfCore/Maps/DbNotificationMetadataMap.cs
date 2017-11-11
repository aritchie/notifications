using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Plugin.Notifications.Infrastructure;


namespace Plugin.Notifications.Data.EfCore.Maps
{
    public class DbNotificationMetadataMap : IEntityTypeConfiguration<DbNotificationMetadata>
    {
        public void Configure(EntityTypeBuilder<DbNotificationMetadata> builder)
        {
        }
    }
}
