﻿using AowCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AowCore.Infrastructure.Config
{
   public class VoucherItemConfiguration : IEntityTypeConfiguration<VoucherItem>
    {
        public void Configure(EntityTypeBuilder<VoucherItem> builder)
        {
            //  builder.HasIndex(p => new { p.CompanyName, p.UserId }).IsUnique();
            //builder.Property(a => a.PinCode)
            //     .HasMaxLength(18)
            //     .IsRequired();
            builder.Property(p => p.DiscountRatePerUnit).HasColumnType("decimal(18,4)");
            builder.Property(p => p.ItemAmount).HasColumnType("decimal(18,4)");
            builder.Property(p => p.MRPPerUnit).HasColumnType("decimal(18,4)");
            builder.Property(p => p.Price).HasColumnType("decimal(18,4)");
            builder.Property(p => p.Quantity).HasColumnType("decimal(18,4)");            
        }
    }
}
