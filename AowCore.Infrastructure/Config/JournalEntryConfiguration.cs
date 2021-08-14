using AowCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AowCore.Infrastructure.Config
{
    public class JournalEntryConfiguration : IEntityTypeConfiguration<JournalEntry>
    {
        public void Configure(EntityTypeBuilder<JournalEntry> builder)
        {
            //  builder.HasIndex(p => new { p.CompanyName, p.UserId }).IsUnique();
            //builder.Property(a => a.PinCode)
            //     .HasMaxLength(18)
            //     .IsRequired();
            builder.Property(p => p.CreditAmount).HasColumnType("decimal(18,4)");
            builder.Property(p => p.DebitAmount).HasColumnType("decimal(18,4)");
            builder.Property(p => p.VoucherNumber).HasColumnType("decimal(18,4)");
        }

    }
}
