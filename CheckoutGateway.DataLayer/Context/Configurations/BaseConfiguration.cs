
using CheckoutGateway.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckoutGateway.DataLayer.Context.Configurations;

public class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : Auditable
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(a => a.Modified).HasMaxLength(50);
        builder.Property(a => a.Modifier).HasMaxLength(300);
        builder.Property(a => a.Created).HasMaxLength(50);
        builder.Property(a => a.Creator).HasMaxLength(300);
    }
}
