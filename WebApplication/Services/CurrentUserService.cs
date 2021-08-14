using AowCore.Application;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _accessor;       

        public CurrentUserService(IHttpContextAccessor accessor)
        {
            this._accessor = accessor;           
            //UserId = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
           // UserId = _userManager.GetUserId(GetUser());
        }

        public ClaimsPrincipal GetUser()
        {
            return _accessor?.HttpContext?.User as ClaimsPrincipal;
        }

        public string UserId { get; }
    }
}
