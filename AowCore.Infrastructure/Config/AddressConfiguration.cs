
using AowCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AowCore.Infrastructure.Config
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(a => a.PinCode)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(a => a.AddressLine1)
                .HasMaxLength(180)
                .IsRequired();

            builder.Property(a => a.AddressLine1)
                .HasMaxLength(60);

            builder.Property(a => a.Country)
                .HasMaxLength(90)
                .IsRequired();

            builder.Property(a => a.City)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
