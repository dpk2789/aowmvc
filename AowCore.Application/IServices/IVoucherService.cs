using AowCore.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AowCore.Application.Services
{
    public interface IVoucherService
    {
        Task<IEnumerable<Ledger>> GetAllLedgers(Guid cmpidG);
        Task<IReadOnlyList<Voucher>> GetVouchers(Guid fYrId, string voucherName);

    }
}
