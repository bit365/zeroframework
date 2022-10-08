using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;

namespace ZeroFramework.DeviceCenter.Infrastructure.EntityConfigurations.Devices
{
    public class DeviceEntityTypeConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.ToTable("Devices", Constants.DbConstants.DefaultTableSchema);

            builder.HasKey(e => e.Id);
            builder.Ignore(e => e.DomainEvents);
            builder.Property(e => e.Id).UseIdentityColumn(100000, 1);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(20);
            builder.Property(e => e.Remark).HasMaxLength(100);
            builder.Property(e => e.Coordinate).HasMaxLength(30);

            var converter = new ValueConverter<GeoCoordinate?, string?>(v => v != null ? v.ToString() : null, v => v != null ? (GeoCoordinate?)v : null);

            builder.Property(e => e.Coordinate).HasConversion(converter);

            builder.Property(e => e.Status).HasMaxLength(20).HasConversion(new EnumToStringConverter<DeviceStatus>());
        }
    }
}