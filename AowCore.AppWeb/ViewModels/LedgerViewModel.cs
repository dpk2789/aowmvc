using System;
using System.Collections.Generic;

using TreeUtility;

namespace AowCore.AppWeb.ViewModels
{
    public class LedgerViewModel : ITreeNode<LedgerViewModel>
    {

        public Guid Id { get; set; }
        private Guid? _parentCategoryId;
        
        public Guid? ParentCategoryId
        {
            get { return _parentCategoryId; }
            set
            {
                if (Id == value)
                    throw new InvalidOperationException("A category cannot have itself as its parent.");

                _parentCategoryId = value;
            }
        }

        public virtual LedgerViewModel Parent { get; set; }
        public IList<LedgerViewModel> Children { get; set; }
        public string Name { get; set; }
        public string PrintName { get; set; }
        public string CategoryName { get; set; }
        public string RootCategory { get; set; }
        public string ParentName { get; set; }
        public string TINNumber { get; set; }
        public string RegTaxNumber { get; set; }
        public string PANNumber { get; set; }

        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }
        public decimal? OpeningBalance { get; set; }
        public decimal? CurrentBalance { get; set; }
        public decimal? ClosingBalance { get; set; }
      


        public string CompanyNameShipping { get; set; }
        public string ShippingAddressLine1 { get; set; }
        public string ShippingAddressLine2 { get; set; }
        public string PlotNumber { get; set; }
        public string StreetName { get; set; }
        public string LandMark { get; set; }
        public string Colony { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        public int? AttendenceMachineNumber { get; set; }
        public int? Age { get; set; }

        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string FathersName { get; set; }


        public string Anniversary { get; set; }
        public string DateOfResignation { get; set; }

        public string Email { get; set; }
        public string Mobile { get; set; }
        public string EmpergencyContact { get; set; }

        public string CustomerType { get; set; }
        public string AccountType { get; set; }

        public string MainImageName { get; set; }
        public string ImageExtention { get; set; }
        public string AddressBlock
        {
            get
            {
                string addressBlock = string.Format("{0}<br/>{1}, {2}, {3} ,{4} </br> {5} ,{6} ,{7}", Name, PlotNumber, StreetName, LandMark, Colony, City, State, ZipCode).Trim();
                return addressBlock == "<br/>," ? string.Empty : addressBlock;
            }
        }

        public Guid? LedgerCategoryId { get; set; }    
    }
}