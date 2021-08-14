using AowCore.Domain.Common;
using System;

namespace AowCore.Domain
{
    public class SundryItems : Entity<Guid>
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Percent { get; set; }
        public string Nature { get; set; }
        public string ItemType { get; set; }
        public Guid LedgerId { get; set; }
        public virtual Ledger Ledger { get; set; }
      
    }
}
