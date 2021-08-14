using AowCore.Domain.Common;
using System;
using System.Collections.Generic;

namespace AowCore.Domain
{
    public class ProductAttributeOptions : AuditableEntity<Guid>
    {
        public string Name { get; set; }
        public Guid ProductAttributeId { get; set; }
        public virtual ProductAttribute ProductAttributes { get; set; }
        // public virtual List<ProductVariant> ProductVariants { get; set; }
        public virtual IList<ProductVariantProductAttributeOption> ProductVariantProductAttributeOptions { get; set; }
    }
}
