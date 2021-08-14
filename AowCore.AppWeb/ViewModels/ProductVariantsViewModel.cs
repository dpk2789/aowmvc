using System;
using System.Collections.Generic;

namespace AowCore.AppWeb.ViewModels
{
    public class ProductVariantsViewModel
    {
        public Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? PurchaseAccountId { get; set; }
        public Guid? InventoryAccountId { get; set; }

        public Guid? ProductId { get; set; }
        public Guid? StoreProductId { get; set; }

        public string BatchName { get; set; }

        public string Name { get; set; }
        public string Size { get; set; }
        public string Code { get; set; }
        public string ModelNumber { get; set; }
        public string Title { get; set; }

        public string Option1Name { get; set; }
        public Guid? Option1Id { get; set; }
        public string Option2Name { get; set; }
        public Guid? Option2Id { get; set; }

        public string Percent { get; set; }
        public string ProductTaxCode { get; set; }
        public string DiscountType { get; set; } //dynamic amount or %
        public string ItemType { get; set; }
        public string TaxType { get; set; }
        public int ItemTypeId { get; set; }

        public string AccountCategoryName { get; set; }
        public string CategoryName { get; set; }

        public string AutoGenerateName { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? MRP { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal? DiscountPerUnitInPercent { get; set; }
        public string Discription { get; set; }
        public string Specifications { get; set; }      
        public string ThumbMainImageName { get; set; }
        public string ThumbMainImagePath { get; set; }
        public string ImagePath { get; set; }
        public decimal? ImageSize { get; set; }
        public string ImageExtention { get; set; }
        public int TabIndex { get; set; }
        public bool IsAvailableForSale { get; set; }
        public bool ShowOnWebsite { get; set; }
        public bool Is_NewArrival { get; set; }
        public bool Is_FeaturedProduct { get; set; }
        public bool Is_BestSeller { get; set; }
        public IList<AttributesViewModel> AttributesViewModels { get; set; }
    
    }

    public class AttributesViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }       
        public IEnumerable<AttributesOptionsViewModel> AttributesOptionsViewModels { get; set; }
        public bool IsChecked { get; set; }
    }

    public class AttributesOptionsViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }
}
