using System.ComponentModel.DataAnnotations;

namespace API.Models.V1.Request
{
    public class UserRegisterRequest
    {
        [EmailAddress]
        public string email { get; set; }
        public string password { get; set; }
    }
}
