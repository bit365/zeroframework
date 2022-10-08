using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate;

namespace ZeroFramework.DeviceCenter.Infrastructure.EntityConfigurations.Ordering
{
    public class OrderStatusEntityTypeConfiguration : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            builder.ToTable("OrderStatus", Constants.DbConstants.DefaultTableSchema);
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).UseHiLo();
            builder.Property(o => o.Id).IsRequired();
            builder.Property(o => o.Name).HasMaxLength(200).IsRequired();
        }
    }
}
