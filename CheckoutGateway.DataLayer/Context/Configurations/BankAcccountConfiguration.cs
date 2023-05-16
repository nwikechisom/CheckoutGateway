
using CheckoutGateway.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckoutGateway.DataLayer.Context.Configurations;

public class BankAcccountConfiguration : BaseConfiguration<BankAccount>
{
    public override void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.Property(a => a.AccountName).HasMaxLength(300).HasColumnName(nameof(BankAccount.AccountName).ToLower());
        builder.Property(a => a.AccountNumber).HasMaxLength(1000).HasColumnName(nameof(BankAccount.AccountNumber).ToLower());
        base.Configure(builder);
    }
}
