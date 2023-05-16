using CheckoutGateway.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckoutGateway.DataLayer.Context.Configurations;

internal class TransactionConfiguration : BaseConfiguration<Transaction>
{
    public override void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.Property(a => a.Amount).IsRequired().HasColumnName(nameof(Transaction.Amount).ToLower()).HasMaxLength(255);
        builder.Property(a => a.Charge).IsRequired().HasColumnName(nameof(Transaction.Charge).ToLower()).HasMaxLength(64);
        builder.Property(a => a.Currency).IsRequired().HasColumnName(nameof(Transaction.Currency).ToLower()).HasMaxLength(20);
        builder.Property(a => a.Merchant).IsRequired().HasColumnName(nameof(Transaction.Merchant).ToLower()).HasMaxLength(255);
        builder.Property(a => a.CallBackUrl).HasColumnName("callback").HasMaxLength(255);
        builder.HasIndex(a => new { a.CallBackUrl, a.Merchant, a.Currency, a.Amount });
        base.Configure(builder);
    }
}
