using AowCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AowCore.Infrastructure.Config
{
    public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            //  builder.HasIndex(p => new { p.CompanyName, p.UserId }).IsUnique();
            //builder.Property(a => a.PinCode)
            //     .HasMaxLength(18)
            //     .IsRequired();
         
            builder.Property(p => p.CostPrice).HasColumnType("decimal(18,4)");
            builder.Property(p => p.SalePrice).HasColumnType("decimal(18,4)");
            builder.Property(p => p.MRP).HasColumnType("decimal(18,4)");
        }
    }
}
