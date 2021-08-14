using AowCore.Domain.Common;
using System;

namespace AowCore.Domain
{
    public class VoucherItemVariant : Entity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }        
        public int? SrNo { get; set; }
        public decimal? DiscountRatePerUnit { get; set; }
        public decimal? MRPPerUnit { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? ItemAmount { get; set; }

        public Guid? ProductVariantId { get; set; }
        public virtual ProductVariant ProductVariant { get; set; }
        public Guid VoucherItemId { get; set; }
        public virtual VoucherItem VoucherItem { get; set; }      
    }
}
