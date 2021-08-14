using AowCore.AppWeb.Helpers;
using AowCore.AppWeb.ViewModels;
using AowCore.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AowCore.AppWeb.Interfaces
{
    public interface IVoucherViewModelService
    {
        Task<IReadOnlyList<LedgerViewModel>> GetAllLedgers(Guid cmpidG);
        Task<IEnumerable<VoucherViewModel>> JEntryListViewModel(IEnumerable<Voucher> vouchers, Guid cmpidG);
        Task<IEnumerable<VoucherInvoiceViewModel>> PurchaseListToViewModel(IEnumerable<Voucher> vouchers, Guid cmpidG);
        Task<VoucherViewModel> GetVoucherForViewModel(Guid id, Guid cmpidG);
        Task<VoucherInvoiceViewModel> GetVoucherInvoiceForViewModel(Guid id, Guid cmpidG);
        Task<PrintViewModel> GetVoucherInvoiceForBillPrint(Guid id, Guid cmpidG);
        Task<List<LedgerViewModel>> GetLedgersForVouchers(Guid cmpidG, string term, string HeadName, string crdr);
        Task CreateVourcherAsync(string voucherName, string data, decimal Invoice, DateTime Date, Guid fyrId);
        Task EditVourcherAsync(string voucherName, string voucherData, decimal Invoice, string data, Guid fyrId);
        Task<JsonResultClientSide> EditVoucherInvoice(string data, string data2, string voucherName, decimal Invoice, DateTime Date, int? termsDays, Guid AccountId, Guid VoucherId,
           decimal? Total, string Note, Voucher voucher, Guid cmpidG, Guid fyrId, string actionName);
        bool VoucherExists(Guid id);
        Task<JsonResultClientSide> DeleteConfirmed(Guid id);
    }
}
