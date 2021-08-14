
using AowCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AowCore.Infrastructure.Config
{
   public class FyrConfiguration : IEntityTypeConfiguration<FinancialYear>
    {
        public void Configure(EntityTypeBuilder<FinancialYear> builder)
        {
            builder.HasOne(i => i.Company)
              .WithMany(c => c.FinancialYears)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
