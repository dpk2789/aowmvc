
using AowCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AowCore.Infrastructure.Config
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            //  builder.HasIndex(p => new { p.CompanyName, p.UserId }).IsUnique();
            builder.Property(a => a.PinCode)
                 .HasMaxLength(18);


            builder.Property(a => a.AddressLine1)
                .HasMaxLength(90);
                

            builder.Property(a => a.AddressLine2)
                .HasMaxLength(90);

            builder.Property(a => a.CompanyName)
                .HasMaxLength(20);


            builder.Property(a => a.TaxNumber)
                .HasMaxLength(20);
              
        }
       
    }
}
