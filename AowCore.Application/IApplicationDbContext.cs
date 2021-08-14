using System.Threading;
using System.Threading.Tasks;
using AowCore.Domain;
using AowCore.Domain.Common;
using AowCore.Domain.Payroll;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AowCore.Application
{
    public interface IApplicationDbContext
    {
        DbSet<Company> Companies { get; set; }
        DbSet<FinancialYear> FinancialYears { get; set; }
        DbSet<AppUserCompany> AppUserCompanies { get; set; }
        DbSet<Ledger> Ledgers { get; set; }
        DbSet<LedgerCategory> LedgerCategories { get; set; }
        DbSet<ProductCategory> ProductCategories { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ItemUnit> ItemUnits { get; set; }
        DbSet<ProductImage> ProductImages { get; set; }
        DbSet<ProductGroup> ProductGroups { get; set; }
        DbSet<ItemGroupProduct> ItemGroupProducts { get; set; }
        DbSet<ProductAttribute> ProductAttributes { get; set; }
        DbSet<ProductAttributeOptions> ProductAttributeOptions { get; set; }
        DbSet<ProductVariant> ProductVariants { get; set; }
        DbSet<JournalEntry> JournalEntries { get; set; }
        DbSet<Voucher> Vouchers { get; set; }
        DbSet<VoucherItem> VoucherItems { get; set; }
        DbSet<VoucherItemVariant> VoucherItemVariants { get; set; }
        DbSet<VoucherSundryItem> VoucherSundryItems { get; set; }
        DbSet<TransporterDetail> TransporterDetails { get; set; }
        DbSet<ProductVariantProductAttributeOption> ProductVariantProductAttributeOptions { get; set; }
        DbSet<EmpAttendence> EmpAttendences { get; set; }
        DbSet<EmployeeDetail> EmployeeDetails { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry Add(object entity);
        EntityEntry Update(object entity);
        ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);
    }
}
