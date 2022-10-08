using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZeroFramework.DeviceCenter.Domain.Aggregates.MonitoringAggregate;

namespace ZeroFramework.DeviceCenter.Infrastructure.EntityConfigurations.Monitoring
{
    public class MonitoringFactorEntityTypeConfiguration : IEntityTypeConfiguration<MonitoringFactor>
    {
        public void Configure(EntityTypeBuilder<MonitoringFactor> builder)
        {
            builder.ToTable("MonitoringFactors", Constants.DbConstants.DefaultTableSchema);

            foreach (IMutableEntityType entityType in builder.Metadata.Model.GetEntityTypes())
            {
                if (entityType.ClrType == typeof(MonitoringFactor))
                {
                    foreach (IMutableProperty property in entityType.GetProperties().Where(p => p.ClrType == typeof(string)))
                    {
                        property.SetMaxLength(36);
                        builder.Property(property.Name).IsRequired(true);
                    }
                }
            }

            builder.Property(e => e.Unit).IsRequired(false);
            builder.Property(e => e.Remarks).IsRequired(false);

            builder.HasIndex(e => new { e.FactorCode }).IsUnique();
        }
    }
}