using AowCore.Application;
using AowCore.Domain.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using AowCore.Domain;
using AowCore.Domain.Models;
using AowCore.Infrastructure.Config;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AowCore.Domain.Payroll;
using AowCore.Domain.Region;
using Microsoft.AspNetCore.Identity;

namespace AowCore.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

          
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseMySQL("Server=mysql5012.site4now.net;user id=a6120d_aowmys1;password=aowKcn2b35;Database=db_a6120d_aowmys1;MultipleActiveResultSets=true");
        //}

        public DbSet<Company> Companies { get; set; }
        public DbSet<AppUserCompany> AppUserCompanies { get; set; }
        public DbSet<FinancialYear> FinancialYears { get; set; }

        public DbSet<Ledger> Ledgers { get; set; }
        public DbSet<LedgerCategory> LedgerCategories { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ItemUnit> ItemUnits { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<ItemGroupProduct> ItemGroupProducts { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductAttributeOptions> ProductAttributeOptions { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }      
        public DbSet<ProductVariantProductAttributeOption> ProductVariantProductAttributeOptions { get; set; }

        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherItem> VoucherItems { get; set; }
        public DbSet<VoucherItemVariant> VoucherItemVariants { get; set; }
        public DbSet<VoucherSundryItem> VoucherSundryItems { get; set; }
        public DbSet<TransporterDetail> TransporterDetails { get; set; }

        public DbSet<EmployeeDetail> EmployeeDetails { get; set; }
        public DbSet<EmpAttendence> EmpAttendences { get; set; }

        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }

        public override EntityEntry Add(object entity)
        {
            return base.Add(entity);
        }
        public override ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default)
        {
            return base.AddAsync(entity, cancellationToken);
        }
        public override EntityEntry Update(object entity)
        {
            return base.Update(entity);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            builder.Entity<ApplicationUser>(entity => entity.Property(m => m.NormalizedEmail).HasMaxLength(85));
            builder.Entity<ApplicationUser>(entity => entity.Property(m => m.NormalizedUserName).HasMaxLength(85));

            builder.Entity<ApplicationRole>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            builder.Entity<ApplicationRole>(entity => entity.Property(m => m.NormalizedName).HasMaxLength(85));

            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(85));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.ProviderKey).HasMaxLength(85));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            builder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));

            builder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(85));

            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(85));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.Name).HasMaxLength(85));

            builder.Entity<IdentityUserClaim<string>>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            builder.Entity<IdentityUserClaim<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(85));

            builder.ApplyConfiguration(new CompanyConfiguration());
            builder.ApplyConfiguration(new FyrConfiguration());
            builder.ApplyConfiguration(new UserCompanyConfiguration());
            builder.ApplyConfiguration(new JournalEntryConfiguration());
            builder.ApplyConfiguration(new VoucherConfiguration());
            builder.ApplyConfiguration(new VoucherItemConfiguration());
            builder.ApplyConfiguration(new VoucherItemVariantConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new ProductVariantConfiguration());
            builder.Entity<ItemUnit>().Property(p => p.RelationalUnit).HasColumnType("decimal(18,4)");
            builder.Entity<Ledger>().Property(p => p.OpeningBalance).HasColumnType("decimal(18,4)");
            builder.Entity<VoucherSundryItem>().Property(p => p.ItemAmount).HasColumnType("decimal(18,4)");
            builder.Entity<EmployeeDetail>().Property(p => p.DaySalary).HasColumnType("decimal(18,4)");
            builder.Entity<EmployeeDetail>().Property(p => p.OverTimeHours).HasColumnType("decimal(18,4)");
            builder.Entity<EmployeeDetail>().Property(p => p.OverTimeTotal).HasColumnType("decimal(18,4)");
            builder.Entity<EmployeeDetail>().Property(p => p.RatePerHour).HasColumnType("decimal(18,4)");
            builder.Entity<EmployeeDetail>().Property(p => p.RatePerHourOvertime).HasColumnType("decimal(18,4)");
            builder.Entity<EmployeeDetail>().Property(p => p.WorkHourTotal).HasColumnType("decimal(18,4)");
            builder.Entity<EmployeeDetail>().Property(p => p.WorkHours).HasColumnType("decimal(18,4)");
            base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in base.ChangeTracker.Entries<IAuditableEntity>())
            {
                IAuditableEntity entity = entry.Entity as IAuditableEntity;

                if (entity != null)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                             
                            entry.Entity.CreatedDate = DateTime.UtcNow;
                            break;
                        case EntityState.Modified:
                             
                            entry.Entity.UpdatedDate = DateTime.UtcNow;
                            break;
                    }
                }

            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
