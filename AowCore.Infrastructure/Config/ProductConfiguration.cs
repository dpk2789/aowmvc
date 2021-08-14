using AowCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AowCore.Infrastructure.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            //  builder.HasIndex(p => new { p.CompanyName, p.UserId }).IsUnique();
            //builder.Property(a => a.PinCode)
            //     .HasMaxLength(18)
            //     .IsRequired();
            builder.Property(p => p.ImageSize).HasColumnType("decimal(18,4)");         
            builder.Property(p => p.PurchasePrice).HasColumnType("decimal(18,4)");
            builder.Property(p => p.SalePrice).HasColumnType("decimal(18,4)");
            builder.Property(p => p.StandardPrice).HasColumnType("decimal(18,4)");
        }
    }
}
