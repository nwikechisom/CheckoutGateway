using CheckoutGateway.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckoutGateway.DataLayer.Context.Configurations;

internal class BankConfiguration : IEntityTypeConfiguration<Bank>
{
    public void Configure(EntityTypeBuilder<Bank> builder)
    {
        builder.Property(a => a.Name).HasMaxLength(50).IsRequired();
    }
}
