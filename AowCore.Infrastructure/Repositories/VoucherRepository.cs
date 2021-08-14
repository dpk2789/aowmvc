using AowCore.Application.IRepository;
using AowCore.Domain;
using AowCore.Infrastructure.Common;
using AowCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AowCore.Infrastructure.Repositories
{
    public class VoucherRepository : GenericRepository<Voucher>, IVoucherRepository
    {
        public VoucherRepository(ApplicationDbContext context) : base(context) { }
        public async Task<IReadOnlyList<Voucher>> GetVouchersByVoucherName(Guid fYrId, string voucherName)
        {
            var vouchers = await context.Vouchers.Include(x => x.JournalEntries).Include(j => j.VoucherItems).
                Where(x => x.FinancialYearId == fYrId && x.VoucherName == voucherName).OrderBy(x => x.VoucherNumber).ToListAsync();
            return vouchers;
        }
        public async Task<IEnumerable<Voucher>> GetAllVouchers(Guid fYrId)
        {
            var vouchers = await context.Vouchers.Include(x => x.JournalEntries).Include(j => j.VoucherItems).
                Where(x => x.FinancialYearId == fYrId).OrderBy(x => x.VoucherNumber).ToListAsync();
            return vouchers;
        }

        public bool VoucherExistsAny(Guid id)
        {
            return context.Vouchers.Any(e => e.Id == id);
        }
        public async Task<Voucher> GetVoucherById(Guid id)
        {
            var voucher = await context.Vouchers
                .Include(j => j.JournalEntries)
                .FirstOrDefaultAsync(m => m.Id == id);
            return voucher;
        }

        public async Task<Voucher> GetVoucherByIdIncludeItems(Guid id)
        {
            var voucher = await context.Vouchers
                .Include(j => j.JournalEntries).Include(j => j.VoucherItems).Include(j => j.VoucherSundryItems).Include(j => j.FinancialYear.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            return voucher;
        }

    }
}
