using AowCore.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TreeUtility;

namespace AowCore.Domain
{
    public class LedgerCategory : Entity<Guid>, ITreeNode<LedgerCategory>
    {
        //public Guid Id { get; set; }

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
        public string Code { get; set; }
        public string Type { get; set; }
        public virtual LedgerCategory Parent { get; set; }
        public IList<LedgerCategory> Children { get; set; }
        public virtual IList<Ledger> Ledgers { get; set; }
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}
