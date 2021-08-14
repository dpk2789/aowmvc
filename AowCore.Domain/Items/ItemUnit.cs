using AowCore.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TreeUtility;

namespace AowCore.Domain
{
    public class ItemUnit : Entity<Guid>, ITreeNode<ItemUnit>
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
        public decimal? RelationalUnit { get; set; }
        public virtual ItemUnit Parent { get; set; }
        public IList<ItemUnit> Children { get; set; }
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
