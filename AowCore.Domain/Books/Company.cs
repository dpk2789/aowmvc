using System;
using System.Collections.Generic;
using AowCore.Domain.Common;

namespace AowCore.Domain
{
    public class Company : AuditableEntity<Guid>
    {      
        public string CompanyName { get; set; }
        public string TaxNumber { get; set; }
        public string TaxType { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Currency { get; set; }
        public int? CurrencyId { get; set; }
        public string UserId { get; set; }
        public string PrintName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public int? CityId { get; set; }
        public string State { get; set; }
        public int? StateId { get; set; }
        public string Country { get; set; }
        public int? CountryId { get; set; }
        public string PinCode { get; set; }
        public virtual IList<FinancialYear> FinancialYears { get; set; }
        public IList<AppUserCompany> AppUserCompanies { get; set; }

    }
}