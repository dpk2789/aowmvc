using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AowCore.AppWeb.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public Guid ProductCategoryId { get; set; }
        public Guid? LedgerId { get; set; }
        public string LedgerName { get; set; }
        public Guid? PurchaseAccountId { get; set; }
        public Guid? InventoryAccountId { get; set; }

        public Guid? ProductId { get; set; }
        public Guid? StoreProductId { get; set; }

        public string BatchName { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
        public string ModelNumber { get; set; }
        public string Title { get; set; }
        public string Percent { get; set; }
        public string ProductTaxCode { get; set; }
        public string DiscountType { get; set; } //dynamic amount or %
        public string ItemType { get; set; }
        public string TaxType { get; set; }
        public string ItemTypeId { get; set; }

        public string AccountCategoryName { get; set; }
        public string CategoryName { get; set; }

        public string AutoGenerateName { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? DiscountPerUnitInPercent { get; set; }
        public string Discription { get; set; }
        public string Specifications { get; set; }       
        public string ThumbMainImageName { get; set; }
        public string ThumbMainImagePath { get; set; }
        public string ImagePath { get; set; }
        public decimal? ImageSize { get; set; }
        public string ImageExtention { get; set; }
        public int? TabIndex { get; set; }
        public bool IsAvailableForSale { get; set; }
        public bool ShowOnWebsite { get; set; }
        public bool Is_NewArrival { get; set; }
        public bool Is_FeaturedProduct { get; set; }
        public bool Is_BestSeller { get; set; }
        public SelectList ProductCategorySelectList { get; set; }
        public List<SelectListItem> getItemTypeList()
        {
            List<SelectListItem> myList = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value="Inventory Item",Text="Inventory Item"},
                 new SelectListItem{ Value="Non Inventory Item",Text="Non Inventory Item"},
                  new SelectListItem{ Value="Service",Text="Service"}

             };
            myList = data.ToList();
            return myList;
        }
    }
}
