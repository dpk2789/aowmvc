using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AowCore.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public IList<AppUserCompany> AppUserCompanies { get; set; }
    }
}
