using AowCore.Domain.Common;
using System;
using System.Collections.Generic;

namespace AowCore.Domain
{
    public class ProductGroup : Entity<Guid>
    {
        public string Name { get; set; }
        public string Type { get; set; } 
        public IList<ItemGroupProduct> ItemGroupProducts { get; set; }

    }
}
