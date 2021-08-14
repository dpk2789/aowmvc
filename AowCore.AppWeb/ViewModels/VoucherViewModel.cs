using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AowCore.AppWeb.ViewModels
{
    public class VoucherViewModel
    {
        public Guid Id { get; set; }
        public decimal VoucherNumber { get; set; }
        public int VoucherTypeId { get; set; }
        public string VoucherName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd-MM-yyyy")]
        public DateTime Date { get; set; }
        public string RefId { get; set; }
        public string Note { get; set; }
        public decimal Total { get; set; }
        public bool? Type { get; set; }
        public virtual List<JournalEntryViewModel> JournalEntryViewModel { get; set; }
    }

    public class VoucherInvoiceViewModel
    {
        public Guid? Id { get; set; }
        public decimal VoucherNumber { get; set; }
        public int VoucherTypeId { get; set; }
        public string VoucherName { get; set; }
        public Guid LedgerId { get; set; }
        public string LedgerName { get; set; }

      //  [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }
        public string RefId { get; set; }
        public string Note { get; set; }
        public decimal Total { get; set; }
        public decimal ItemsTotal { get; set; }
        public decimal SundryTotal { get; set; }
        public bool? Type { get; set; }
        public virtual List<JournalEntryViewModel> JournalEntryViewModel { get; set; }
        public virtual List<VoucherSundryItemsViewModel> VoucherSundryItemsViewModels { get; set; }
        public virtual List<VoucherItemsViewModel> VoucherItemsViewModels { get; set; }
    }


    public class VoucherSundryItemsViewModel
    {
        public Guid Id { get; set; }
        public int? SrNo { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Percent { get; set; }
        public string Description { get; set; }
        public decimal? ItemAmount { get; set; }
        public Guid LedgerId { get; set; }
        public Guid ProductId { get; set; }
    }

    public class VoucherItemsViewModel
    {
        public Guid Id { get; set; }
        public int? SrNo { get; set; }
        public string Name { get; set; }
        public string ItemType { get; set; }
        public string ItemTaxCode { get; set; }
        public string Percent { get; set; }
        public string Description { get; set; }
        public decimal? ItemAmount { get; set; }
        public decimal? MRPPerUnit { get; set; }
        public decimal? Quantity { get; set; }
        public decimal Price { get; set; }
        public Guid LedgerId { get; set; }
        public Guid ProductId { get; set; }
    }

    public class VoucherItemVariantViewModel
    {
        public Guid Id { get; set; }
        public int? SrNo { get; set; }
        public string Name { get; set; }
        public string ItemType { get; set; }
        public string Percent { get; set; }
        public string Description { get; set; }
        public decimal? ItemAmount { get; set; }
        public decimal? MRPPerUnit { get; set; }
        public decimal? Quantity { get; set; }
        public Guid VarientProductId { get; set; }
    }


    public class TransporterDetailViewModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string GRNumber { get; set; }
        public string TINNumber { get; set; }
        public string VechicleNumber { get; set; }
        public string Station { get; set; }
        public string Destination { get; set; }
        public Guid VouchersId { get; set; }

    }



}