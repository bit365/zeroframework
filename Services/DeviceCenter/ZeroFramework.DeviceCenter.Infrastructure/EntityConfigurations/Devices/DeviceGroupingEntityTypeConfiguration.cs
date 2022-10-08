using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;

namespace ZeroFramework.DeviceCenter.Infrastructure.EntityConfigurations.Devices
{
    public class DeviceGroupingEntityTypeConfiguration : IEntityTypeConfiguration<DeviceGrouping>
    {
        public void Configure(EntityTypeBuilder<DeviceGrouping> builder)
        {
            builder.ToTable("DeviceGroupings", Constants.DbConstants.DefaultTableSchema);

            builder.HasKey(e => new { e.DeviceId, e.DeviceGroupId });

            builder.HasOne(e => e.Device).WithMany().HasForeignKey(e => e.DeviceId);
            builder.HasOne(e => e.DeviceGroup).WithMany().HasForeignKey(e => e.DeviceGroupId);
        }
    }
}