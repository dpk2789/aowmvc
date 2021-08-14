
using AowCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AowCore.Infrastructure.Config
{
    public class UserCompanyConfiguration : IEntityTypeConfiguration<AppUserCompany>
    {
        public void Configure(EntityTypeBuilder<AppUserCompany> builder)
        {
            builder.HasKey(bc => new { bc.CompanyId, bc.ApplicationUserId });

            builder.HasOne(bc => bc.Company)
                .WithMany(b => b.AppUserCompanies)
                .HasForeignKey(bc => bc.CompanyId);

            builder.HasOne(bc => bc.ApplicationUser)
                .WithMany(c => c.AppUserCompanies)
                .HasForeignKey(bc => bc.ApplicationUserId);
        }
    }
}
