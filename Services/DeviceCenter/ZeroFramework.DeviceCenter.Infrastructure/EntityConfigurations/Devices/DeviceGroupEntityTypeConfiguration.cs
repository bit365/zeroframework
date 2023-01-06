using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;

namespace ZeroFramework.DeviceCenter.Infrastructure.EntityConfigurations.Devices
{
    public class DeviceGroupEntityTypeConfiguration : IEntityTypeConfiguration<DeviceGroup>
    {
        public void Configure(EntityTypeBuilder<DeviceGroup> builder)
        {
            builder.ToTable("DeviceGroups", Constants.DbConstants.DefaultTableSchema);

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn(100000, 1);
            builder.Property(e => e.Name).HasMaxLength(20);
            builder.Property(e => e.Remark).HasMaxLength(256);
            builder.Ignore(e => e.DomainEvents);

            builder.HasMany(e => e.Devices).WithMany(e => e.DeviceGroups).UsingEntity<DeviceGrouping>(configureRight => configureRight.HasOne(e => e.Device).WithMany(), configureLeft => configureLeft.HasOne(e => e.DeviceGroup).WithMany()).HasKey(e => new { e.DeviceId, e.DeviceGroupId });
        }
    }
}