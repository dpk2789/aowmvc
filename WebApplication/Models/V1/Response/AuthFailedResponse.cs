using System.Collections.Generic;

namespace API.Models.V1.Response
{
    public class AuthFailedResponse
    {
        public IEnumerable< string> ErrorMessages { get; set; }
    }
}
