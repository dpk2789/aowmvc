using AowCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AowCore.Infrastructure.Config
{
    public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            //  builder.HasIndex(p => new { p.CompanyName, p.UserId }).IsUnique();
            //builder.Property(a => a.PinCode)
            //     .HasMaxLength(18)
            //     .IsRequired();
            builder.Property(p => p.Total).HasColumnType("decimal(18,4)");
            builder.Property(p => p.VoucherNumber).HasColumnType("decimal(18,4)");           
        }
    }
}
