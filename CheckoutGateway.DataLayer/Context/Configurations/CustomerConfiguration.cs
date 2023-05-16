using CheckoutGateway.DataLayer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckoutGateway.DataLayer.Context.Configurations;

internal class CustomerConfiguration : BaseConfiguration<Customer>
{
    public override void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(a => a.Email).HasMaxLength(64);
        builder.Property(a => a.PhoneNumber).HasMaxLength(64);
        builder.Property(a => a.Name).HasMaxLength(255);
        base.Configure(builder);
    }
}
