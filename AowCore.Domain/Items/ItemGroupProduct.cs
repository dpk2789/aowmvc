using AowCore.Domain.Common;
using System;

namespace AowCore.Domain
{
    public class ItemGroupProduct 
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; }
    }
}
