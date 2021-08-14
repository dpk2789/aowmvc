using Microsoft.AspNetCore.Http;
using System.Linq;

namespace API.Helpers
{
    public static class UserExtention
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            return httpContext.User.Claims.Single(x => x.Type == "Id").Value;
        }
    }
}
