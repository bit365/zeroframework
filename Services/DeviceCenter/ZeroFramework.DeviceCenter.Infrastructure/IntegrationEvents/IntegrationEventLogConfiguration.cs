using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ZeroFramework.DeviceCenter.Infrastructure.IntegrationEvents
{
    public class IntegrationEventLogConfiguration : IEntityTypeConfiguration<IntegrationEventLog>
    {
        public void Configure(EntityTypeBuilder<IntegrationEventLog> builder)
        {
            builder.ToTable("IntegrationEventLogs", Constants.DbConstants.DefaultTableSchema);
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).IsRequired();
            builder.Property(e => e.Content).IsRequired();
            builder.Property(e => e.CreationTime).IsRequired();
            builder.Property(e => e.Status).IsRequired();
            builder.Property(e => e.TimesSent).IsRequired();
            builder.Property(e => e.EventTypeName).IsRequired();
        }
    }
}