using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZeroFramework.DeviceCenter.Domain.Aggregates.BuyerAggregate;

namespace ZeroFramework.DeviceCenter.Infrastructure.EntityConfigurations.Buyers
{
    public class PaymentMethodEntityTypeConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.ToTable("PaymentMethods", Constants.DbConstants.DefaultTableSchema);
            builder.HasKey(pm => pm.Id);
            builder.Property(pm => pm.Id).UseHiLo();
            builder.Property(pm => pm.CardNumber).IsRequired().HasMaxLength(25);
            builder.Property(pm => pm.CardType).HasConversion<string>();
        }
    }
}
