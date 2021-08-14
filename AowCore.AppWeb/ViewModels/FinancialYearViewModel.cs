using System;

namespace AowCore.AppWeb.ViewModels
{
    public class FinancialYearViewModel
    {
        public string Name { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}
