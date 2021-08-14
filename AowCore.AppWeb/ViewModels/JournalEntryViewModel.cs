using System;
using System.ComponentModel.DataAnnotations;

namespace AowCore.AppWeb.ViewModels
{
    public class JournalEntryViewModel
    {
        public Guid? Id { get; set; }
        public Guid? VoucherId { get; set; }
        public string VoucherName { get; set; }
        [Required]
        public decimal VoucherInvoice { get; set; }
        [Required]
        public DateTime VoucherDate { get; set; }
        public decimal? VoucherTotal { get; set; }
        public decimal? OutstandingAmount { get; set; }
        public bool Paid { get; set; }
        public int? SrNo { get; set; }
        public string CrDrType { get; set; }
        public Guid LedgerId { get; set; }       
        public string InvoiceAccountName { get; set; }
        public string AccountName { get; set; }
        public string RootCategory { get; set; }
        public decimal? CreditAmount { get; set; }
        public decimal? DebitAmount { get; set; }       
        public string RefId { get; set; }
        public decimal? MRPPerUnit { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? ItemAmount { get; set; }
       
       
        public string Note { get; set; }
        public string AccountType { get; set; }
        public decimal? TaxWithExtendedPrice { get; set; }

        public string Description { get; set; }
        public decimal? Freight { get; set; }
        public decimal? Packaging { get; set; }
        public decimal? OtherExpenses { get; set; }
    }
}