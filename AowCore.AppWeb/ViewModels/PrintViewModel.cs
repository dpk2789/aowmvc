using System;
using System.Collections.Generic;

namespace AowCore.AppWeb.ViewModels
{
    public class PrintViewModel
    {
        public Guid? Id { get; set; }
        public string Head { get; set; }
        public decimal Invoice { get; set; }

        public DateTime? Date { get; set; }
        public int? TermsDays { get; set; }
        public DateTime? TermsEndDate { get; set; }
        public string CurrecyName { get; set; }
        public string CurrecySymbol { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Net { get; set; }
        public decimal? AmountDue { get; set; }
        public decimal? AmountPaid { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TotalTaxAmount { get; set; }
        public decimal? ItemsTotal { get; set; }
        public decimal? SundryTotal { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? HdnDue { get; set; }
        public decimal? OutStandingAmount { get; set; }
        public decimal? Total { get; set; }
        public string CrDrType { get; set; }

        public Guid ParticularAccountId { get; set; }
        public string AccountParticularName { get; set; }
        public Guid AccountId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerGSTIN { get; set; }
        public string CustomerAddressLine1 { get; set; }
        public string CustomerAddressLine2 { get; set; }
        public string CustomerLandMark { get; set; }
        public string CustomerColony { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerState { get; set; }
        public string CustomerZipCode { get; set; }


        public string RefId { get; set; }
        public decimal? TaxWithExtendedPrice { get; set; }

        public string Description { get; set; }
        public decimal? Freight { get; set; }
        public decimal? Packaging { get; set; }
        public decimal? OtherExpenses { get; set; }


        public Guid VoucherTypesId { get; set; }

        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string RegTaxNumber { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Mobile { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public int? CityId { get; set; }
        public string State { get; set; }
        public int? StateId { get; set; }
        public string Country { get; set; }
        public int? CountryId { get; set; }
        public string PinCode { get; set; }
        public virtual List<VoucherItemsViewModel> VoucherItemsViewModels { get; set; }
        public virtual List<VoucherSundryItemsViewModel> VoucherSundryItemsViewModels { get; set; }
    }
}
