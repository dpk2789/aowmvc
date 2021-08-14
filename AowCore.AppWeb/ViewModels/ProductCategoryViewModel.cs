using System;
using System.ComponentModel.DataAnnotations;

namespace AowCore.AppWeb.ViewModels
{
    public class ProductCategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int TabIndex { get; set; }
        public string AttributeName { get; set; }
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
    }
}
