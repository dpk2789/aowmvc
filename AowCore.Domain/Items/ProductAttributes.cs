using AowCore.Domain.Common;
using System;
using System.Collections.Generic;

namespace AowCore.Domain
{
    public class ProductAttribute : AuditableEntity<Guid>
    {
        public ProductAttribute()
        {
           
        }
        public string Name { get; set; }
       
        public Guid ProductCategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual IList<ProductAttributeOptions> ProductAttributeOptions { get; set; }
     

    }

   
}
