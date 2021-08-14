using System.Collections.Generic;

namespace AowCore.Domain.Region
{
    public class State 
    {
        public int Id { get; set; }
        public string stateName { get; set; }
        public string Code { get; set; }
        public float? latitude { get; set; }
        public float? longitude { get; set; }
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public virtual IList<City> Cities { get; set; }
    }
}
