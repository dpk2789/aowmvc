using AowCore.Domain.Common;
using System;

namespace AowCore.Domain
{
    public class TransporterDetail : Entity<Guid>
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string GRNumber { get; set; }
        public string TaxNumber { get; set; }
        public string VechicleNumber { get; set; }
        public string Station { get; set; }
        public string Destination { get; set; }
        public Guid VouchersId { get; set; }
        public virtual Voucher Vouchers { get; set; }
    }
}
