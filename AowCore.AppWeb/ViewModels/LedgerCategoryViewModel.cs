using System;
using System.Collections.Generic;
using TreeUtility;

namespace AowCore.AppWeb.ViewModels
{
    public class LedgerCategoryViewModel : ITreeNode<LedgerCategoryViewModel>
    {
        public Guid Id { get; set; }
        private Guid? _parentCategoryId;

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

        public virtual LedgerCategoryViewModel Parent { get; set; }
        public IList<LedgerCategoryViewModel> Children { get; set; }
        public virtual IList<LedgerViewModel> LedgerViewModels { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
    }
}