using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate;

namespace ZeroFramework.DeviceCenter.Infrastructure.EntityConfigurations.Ordering
{
    public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems", Constants.DbConstants.DefaultTableSchema);
            builder.Property(e => e.UnitPrice).HasPrecision(18, 2);
            builder.HasKey(o => o.Id);
        }
    }
}
