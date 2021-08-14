using AowCore.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TreeUtility;

namespace AowCore.Domain
{
    public class ProductCategory : Entity<Guid>, ITreeNode<ProductCategory>
    {
        private Guid? _parentCategoryId;

        [Display(Name = "Parent Category")]
        public Guid? ParentCategoryId
        {
            get { return _parentCategoryId; }
            set
            {
                if (Id == value)
                    throw new InvalidOperationException("A category cannot have itself as its parent.");

                _parentCategoryId = value;
            }
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public virtual ProductCategory Parent { get; set; }
        public IList<ProductCategory> Children { get; set; }
        public virtual IList<Product> Products { get; set; }
        public virtual IList<ProductAttribute> ProductAttributes { get; set; }
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
