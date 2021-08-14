using System.Collections.Generic;

namespace AowCore.AppWeb.ViewModels
{
    public class ItemVarientsSearchViewModel
    {
        public IList<AttributesViewModel> AttributesViewModel { get; set; }
        public IList<ProductVariantsViewModel> ProductVariantsViewModel { get; set; }
    }
}
