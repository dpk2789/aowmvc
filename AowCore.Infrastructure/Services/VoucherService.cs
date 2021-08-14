using AowCore.Application;
using AowCore.Application.IRepository;
using AowCore.Application.Services;
using AowCore.Domain;
using AowCore.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AowCore.Infrastructure.Services
{
    public class VoucherService : BaseManager, IVoucherService
    {       
        private readonly ILedgerRepository _ledgerRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VoucherService(IApplicationDbContext context, IUnitOfWork unitOfWork, ILedgerRepository ledgerRepository, IVoucherRepository voucherRepository) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _ledgerRepository = ledgerRepository;
            _voucherRepository = voucherRepository;
        }

        public async Task<IEnumerable<Ledger>> GetAllLedgers(Guid cmpidG)
        {
            var ledgers = await _ledgerRepository.GetLedgers(cmpidG);
            return ledgers;
        }
        public async Task<IReadOnlyList<Voucher>> GetVouchers(Guid fYrId, string voucherName)
        {
            var vouchers = await _voucherRepository.GetVouchersByVoucherName(fYrId, voucherName);
            return vouchers;
        }      

    }
}
