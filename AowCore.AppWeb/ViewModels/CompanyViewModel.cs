using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AowCore.AppWeb.ViewModels
{
    public class CompanyUserList
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool Is_Selected { get; set; }
    }
    public class CompanyViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "You must enter an Company Name.")]
        //[StringLength(20, ErrorMessage = "The account number must be 20 characters or shorter.")]
        public string CompanyName { get; set; }
        public string PANNumber { get; set; }
        public string TaxNumber { get; set; }
        public string TaxType { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string UserId { get; set; }
        public string Mobile { get; set; }
      //  public HttpPostedFileBase MainImageNameFile { get; set; }

        [Display(Name = "Company Logo")]
        public string MainImageName { get; set; }
        public string ImageExtention { get; set; }

        public string ShippingName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PlotNumber { get; set; }      
        public string City { get; set; }
        public int? CityId { get; set; }
        public string State { get; set; }
        public int? StateId { get; set; }
        
        public string Country { get; set; }
        public int? CountryId { get; set; }
        public string PinCode { get; set; }

        //[Required(ErrorMessage = "You must Select an TimeZone.")]
        public string TimeZone { get; set; }

        //  [Required(ErrorMessage = "You must Select an IndustryType.")]
        public string IndustryType { get; set; }

        ///[Required(ErrorMessage = "You must Select an BussinessType.")]
        public string BussinessType { get; set; }

        //[Required(ErrorMessage = "You must Select an Do My Accouting.")]
        public string DoMyAccoutingUsing { get; set; }

        //[Required(ErrorMessage = "You must Select an Currency.")]
        public string Currency { get; set; }


        public DateTime CreatedDate { get; set; }
        public Guid FinancialYearName { get; set; }
        public bool IsActive { get; set; }
        public bool IsPaid { get; set; }
        public List<SelectListItem> CurrencyList { get; set; }
     //   public virtual IList<ApplicationUserViewModel> UserList { get; set; }
        public IEnumerable<CompanyUserList> CompanyUserList { get; set; }

      
        public string NameShipping { get; set; }
        public string AddressShippingLine1 { get; set; }
        public string AddressShippingLine2 { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingCountry { get; set; }
        public string ShippingZipCode { get; set; }
        public string ShippingMobile { get; set; }
        public string ShippingNearestLandMark { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }
    }
}
