
using AowCore.Domain.Common;
using System;
using System.Collections.Generic;

namespace AowCore.Domain
{
    public class Product : AuditableEntity<Guid>
    {
        public Product()
        {
            //ProductAttributeOptions = new List<ProductAttributeOptions>();
            ProductImages = new List<ProductImage>();
        }

        public string ItemType { get; set; }
        public string TaxType { get; set; }
        public string Name { get; set; }

        public string ProductTaxCode { get; set; }
        public string Percent { get; set; }
        public string NatureAddSub { get; set; } //dynamic amount or %

        public string Code { get; set; }
        public string ModelNumber { get; set; }
        public string AutoGenerateName { get; set; }
        public string Title { get; set; }
        public string Discription { get; set; }
        public string Specifications { get; set; }
        public decimal SalePrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal StandardPrice { get; set; }
        //public decimal? DiscountPerUnitInPercent { get; set; }
        //public decimal StandardShippingCharges { get; set; }
        //public decimal StandardShippingTime { get; set; }
        //public decimal TwoDaysDeliveryCharges { get; set; }
        //public decimal TwoDaysDeliveryTime { get; set; }
        //public bool IsAvailableForSale { get; set; }
        //public bool ShowOnWebsite { get; set; }
        //public bool Is_NewArrival { get; set; }
        //public bool Is_FeaturedProduct { get; set; }
        //public bool Is_BestSeller { get; set; }
        //public bool Is_FreeShipping { get; set; }
        public bool Is_Taxable { get; set; }


        public string ThumbMainImageName { get; set; }
        public string ImagePath { get; set; }
        public decimal? ImageSize { get; set; }
        public string ImageExtention { get; set; }


        public Guid ProductCategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual IList<ProductImage> ProductImages { get; set; }
       
        public IList<ItemGroupProduct> ItemGroupProducts { get; set; }
        public virtual IList<ProductVariant> ProductVariants { get; set; }
        //public virtual IList<ProductAttributeOptions> ProductAttributeOptions { get; set; }
        public Guid? LedgerId { get; set; }
        public virtual Ledger Ledger { get; set; }
        public Guid? PurchaseLedgerId { get; set; }
        public virtual Ledger PurchaseLedger { get; set; }
    }
}
