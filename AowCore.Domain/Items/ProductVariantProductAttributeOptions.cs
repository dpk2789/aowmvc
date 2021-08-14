using System;

namespace AowCore.Domain
{
    public class ProductVariantProductAttributeOption
    {
        public Guid Id { get; set; }
        public Guid? ProductAttributeOptionsId { get; set; }
        public virtual ProductAttributeOptions ProductAttributeOptions { get; set; }
        public Guid? ProductVariantId { get; set; }
        public virtual ProductVariant ProductVariant { get; set; }
    }
}
