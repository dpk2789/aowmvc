using System;

namespace AowCore.Domain
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        public string City { get; set; }
        public int? CityId { get; set; }
        public string State { get; set; }
        public int? StateId { get; set; }
        public string Country { get; set; }
        public int? CountryId { get; set; }
        public string PinCode { get; set; }

        public Guid? CompanyId { get; set; }
        public virtual Company Company { get; set; }

        //public Address(string toWhom, string nearestLandMark, string street, string city, string state, string country, string zipcode)
        //{
        //    ToWhom = toWhom;
        //    NearestLandMark = nearestLandMark;
        //    Street = street;
        //    City = city;
        //    State = state;
        //    Country = country;
        //    ZipCode = zipcode;
        //}
    }
}
