using System;

namespace AowCore.AppWeb.ViewModels
{
    public class SundryItemViewModel
    {
        public Guid Id { get; set; }       
        public Guid ProductCategoryId { get; set; }
        public Guid? LedgerId { get; set; }
        public string Name { get; set; }
        public string Percent { get; set; }
        public string NatureAddSub { get; set; } //dynamic amount or %
    }
}
