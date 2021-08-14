using System;
using System.Collections.Generic;

namespace AowCore.AppWeb.ViewModels
{
    public class LedgerReportViewModel
    {
        public Guid Id { get; set; }
        public Guid LedgerId { get; set; }
        public decimal VoucherNumber { get; set; }
        public int VoucherTypeId { get; set; }
        public string VoucherName { get; set; }     
        public string LedgerName { get; set; }
        public DateTime Date { get; set; }
        public string RefId { get; set; }
        public string Note { get; set; }
        public decimal Total { get; set; }
        public decimal ItemsTotal { get; set; }
        public decimal SundryTotal { get; set; }
        public int? SrNo { get; set; }
        public string CrDrType { get; set; }
        public decimal? CreditAmount { get; set; }
        public decimal? DebitAmount { get; set; }
        public decimal? Amount { get; set; }
        public IEnumerable<LedgerViewModel> Files { get; set; }
    }
}
