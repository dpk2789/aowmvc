using System;

namespace AowCore.Domain
{
    public class ProductCategoryProductAttribute
    {
        public Guid Id { get; set; }
        public Guid ProductAttributeId { get; set; }
        public ProductAttribute ProductAttribute { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

    }
}
