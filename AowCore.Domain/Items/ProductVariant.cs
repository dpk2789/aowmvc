using AowCore.Domain.Common;
using System;
using System.Collections.Generic;

namespace AowCore.Domain
{
    public class ProductVariant : AuditableEntity<Guid>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string ModelNumber { get; set; }
        public string AutoGenerateName { get; set; }
        public string Discription { get; set; }
        public string Size { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? MRP { get; set; }
        public decimal? SalePrice { get; set; }
        public string ItemType { get; set; }
        public string TaxType { get; set; }

        public bool IsAvailableForSale { get; set; }
        public bool ShowOnWebsite { get; set; }
        public bool Is_NewArrival { get; set; }
        public bool Is_FeaturedProduct { get; set; }
        public bool Is_BestSeller { get; set; }
        public bool Is_FreeShipping { get; set; }
        public bool Is_Taxable { get; set; }


        public string ThumbMainImageName { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Products { get; set; }
        public Guid ProductAttributeOptions1Id { get; set; }
        public Guid ProductAttributeOptions2Id { get; set; }
        public Guid ProductAttributeOptionsId { get; set; }
        //  public virtual ProductAttributeOptions ProductAttributeOptions { get; set; }
        //  public virtual IList<ProductAttributeOptions> ProductAttributeOptions { get; set; }
        public List<ProductVariantProductAttributeOption> ProductVariantProductAttributeOptions { get; set; }

    }
}
