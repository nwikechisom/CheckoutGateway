using CheckoutGateway.DataLayer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckoutGateway.DataLayer.Context.Configurations;

public class MerchantConfiguration : BaseConfiguration<Merchant>
{
    public override void Configure(EntityTypeBuilder<Merchant> builder)
    {
        builder.Property(a => a.Name).HasMaxLength(300);
        builder.Property(a => a.Description).HasMaxLength(1000);
        //builder.OwnsOne(a => a.Charge, c =>
        //{
        //    c.Property(p => p.Amount).HasPrecision(14, 2);
        //    c.Property(p => p.Currency).HasMaxLength(10);
        //});
        base.Configure(builder);
    }
}
