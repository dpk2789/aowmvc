using System;
using System.Collections.Generic;

namespace AowCore.AppWeb.ViewModels
{
    public class ProductsAttributeViewModel
    {
        public Guid Id { get; set; }
        public Guid ProductCategoryId { get; set; }
        public string Name { get; set; }
        public string ProductName { get; set; }
        public string OptionValue { get; set; }
        public IList<AttributesOptions> AttributesOptions { get; set; }
    }

    public class AttributesOptions
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }
}
