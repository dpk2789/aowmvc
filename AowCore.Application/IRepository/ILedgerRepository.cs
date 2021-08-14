using AowCore.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AowCore.Application.IRepository
{
    public interface ILedgerRepository : IRepository<Ledger>
    {
        Task<IEnumerable<Ledger>> GetLedgers(Guid cmpidG);
        Task<IEnumerable<Ledger>> GetLedgersByTerm(Guid cmpidG, string term);
        Task<Ledger> GetLedgerFirstOrDefault(Guid cmpidG, string term);
    }
}
