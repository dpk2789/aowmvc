using AowCore.AppWeb.Helpers;
using AowCore.AppWeb.ViewModels;
using AowCore.Domain;

namespace AowCore.AppWeb.Mapping
{
    public class CompanyMapping
    {       
        public Company ViewModelToDomain(Company cmp, CompanyViewModel viewModel)
        {           
            cmp.CompanyName = viewModel.CompanyName;
            cmp.TaxNumber = viewModel.TaxNumber;
            cmp.Currency = viewModel.Currency;
            cmp.CountryId = viewModel.CountryId;
            cmp.Country = viewModel.Country;
            cmp.PrintName = viewModel.ShippingName;
            cmp.Mobile = viewModel.Mobile;
            cmp.AddressLine1 = viewModel.AddressLine1;
            cmp.AddressLine2 = viewModel.AddressLine2;
            cmp.Country = viewModel.Country;
            cmp.State = viewModel.State;
            cmp.City = viewModel.City;
            cmp.PinCode = viewModel.PinCode;
            cmp.Currency = viewModel.Currency;
            return cmp;
        }

        public CompanyViewModel DomainToResponse(Company company)
        {
            var viewModel = new CompanyViewModel
            {
                Id = company.Id,
                CompanyName = company.CompanyName,
                TaxNumber = company.TaxNumber,
                Email = company.Email,
                Mobile = company.Mobile,
                Currency = company.Currency,
                ShippingName = company.PrintName,
                AddressLine1 = company.AddressLine1,
                AddressLine2 = company.AddressLine2,
                Country = company.Country,
                CountryId = company.CountryId,
                State = company.State,
                City = company.City,
                PinCode = company.PinCode
                //FileUser = item.dispatch_user == aapuser.Id ? "Self" : "Received"
            };
            var selectLIstItems = new SelectListItemsDropdown();
            if (company.Currency == null)
            {
                viewModel.Currency = "INR";
                viewModel.CurrencyList = selectLIstItems.getCurrencyList();
            }
            else
            {               
                viewModel.CurrencyList = selectLIstItems.getCurrencyList();
            }
            return viewModel;
        }
    }
}
