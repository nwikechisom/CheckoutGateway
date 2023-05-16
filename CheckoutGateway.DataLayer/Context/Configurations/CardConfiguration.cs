
using CheckoutGateway.DataLayer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckoutGateway.DataLayer.Context.Configurations;

internal class CardConfiguration : BaseConfiguration<Card>
{
    public override void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.Property(a => a.CardNumber).IsRequired().HasMaxLength(25);
        builder.Property(a => a.Cvv).IsRequired().HasMaxLength(25);
        builder.Property(a => a.HolderName).IsRequired().HasMaxLength(25);
        builder.Property(a => a.ExpiryMonth).IsRequired().HasMaxLength(25);
        builder.Property(a => a.ExpiryYear).IsRequired().HasMaxLength(25);
        builder.HasIndex(a => a.HolderName);
        base.Configure(builder);
    }
}
