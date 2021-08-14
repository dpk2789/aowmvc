

namespace AowCore.Domain.Region
{
    public class City 
    {
        public int Id { get; set; }
        public string cityName { get; set; }
        public string Code { get; set; }
        public string ZipCode { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string countryID { get; set; }
        public int StateId { get; set; }
        public virtual State State { get; set; }
    }
}
