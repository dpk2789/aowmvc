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
    public class LedgerRepository : GenericRepository<Ledger>, ILedgerRepository
    {
        public LedgerRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Ledger>> GetLedgers(Guid cmpidG)
        {
            var ledgers = await context.Ledgers.Include(x => x.LedgerCategory).Where(c => c.LedgerCategory.CompanyId == cmpidG).OrderBy(x => x.Name).ToListAsync();
            return ledgers;
        }

        public async Task<IEnumerable<Ledger>> GetLedgersByTerm(Guid cmpidG, string term)
        {
            var ledgers = await context.Ledgers.Include(x => x.LedgerCategory).Where(c => c.LedgerCategory.CompanyId == cmpidG).
                Where(ii => ii.Name.Contains(term)).OrderBy(x => x.Name).ToListAsync();
            return ledgers;
        }

        public async Task<Ledger> GetLedgerFirstOrDefault(Guid cmpidG, string term)
        {
            var ledger = await context.Ledgers.Include(x => x.LedgerCategory).Where(c => c.LedgerCategory.CompanyId == cmpidG && c.Name == term).FirstOrDefaultAsync();
            return ledger;
        }

    }
}
