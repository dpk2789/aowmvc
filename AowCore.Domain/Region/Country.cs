using AowCore.Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AowCore.Domain.Region
{
    public class Country : Entity<int>
    {        
        [Required(ErrorMessage = "Name is required")]
        public string CountryName { get; set; }
        public string Code { get; set; }
        public string NiceName { get; set; }
        public string ISO { get; set; }

        public int? NumCode { get; set; }
        public int PhoneCode { get; set; }
        public virtual IList<State> States { get; set; }

    }
}
