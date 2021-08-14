using AowCore.Domain.Common;
using System;

namespace AowCore.Domain
{
    public class VoucherSundryItem : Entity<Guid>
    {
        public int? SrNo { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Percent { get; set; }
        public string Description { get; set; }
        public decimal? ItemAmount { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
        public Guid VoucherId { get; set; }
        public virtual Voucher Vouchers { get; set; }

    }
}
