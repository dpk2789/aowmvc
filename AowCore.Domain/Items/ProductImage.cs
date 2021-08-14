using AowCore.Domain.Common;
using System;

namespace AowCore.Domain
{
    public class ProductImage : AuditableEntity<Guid>
    {
        public string FullName { get; set; }
        public string ImageName { get; set; }
        public string Path { get; set; }
        public int Size { get; set; }
        public string Extention { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
