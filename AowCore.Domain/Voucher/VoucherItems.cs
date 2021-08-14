using AowCore.Domain.Common;
using System;
using System.Collections.Generic;

namespace AowCore.Domain
{
    public class VoucherItem : AuditableEntity<Guid>
    {    
        public int? SrNo { get; set; }
        public decimal? DiscountRatePerUnit { get; set; }
        public decimal? MRPPerUnit { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? ItemAmount { get; set; }
        public string Description { get; set; }       
        public decimal Price { get; set; }


        //public Guid LedgerId { get; set; }
        //public virtual Ledger Ledger { get; set; }

        //public Guid? StoreProductId { get; set; }
        //public virtual StoreProduct StoreProduct { get; set; }
        //public Guid? BatchId { get; set; }
        //public Guid? DiscountDailyItemId { get; set; }

        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }     

        public Guid VoucherId { get; set; }
        public virtual Voucher Voucher { get; set; }
        public virtual IList<VoucherItemVariant> VoucherItemVariants { get; set; }
    }
}

