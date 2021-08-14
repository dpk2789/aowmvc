using System;

namespace AowCore.AppWeb.ViewModels
{
    public class DashboardViewModel
    {
        public Guid CompanyId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string FyrName { get; set; }
    }
}
