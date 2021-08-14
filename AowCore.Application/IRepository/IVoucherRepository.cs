using AowCore.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AowCore.Application.IRepository
{
    public interface IVoucherRepository : IRepository<Voucher>
    {
        Task<IReadOnlyList<Voucher>> GetVouchersByVoucherName(Guid fYrId, string voucherName);
        Task<IEnumerable<Voucher>> GetAllVouchers(Guid fYrId);
        bool VoucherExistsAny(Guid id);
        Task<Voucher> GetVoucherById(Guid id);
        Task<Voucher> GetVoucherByIdIncludeItems(Guid id);
    }
}
