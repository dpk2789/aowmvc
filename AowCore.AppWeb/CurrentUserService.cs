using System.Security.Claims;
using AowCore.Application;
using Microsoft.AspNetCore.Http;

namespace AowCore.AppWeb
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _accessor;

        public CurrentUserService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            //UserId = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            ClaimsPrincipal currentUser = _accessor?.HttpContext?.User;
            if (currentUser != null)
            {
                var currentUserName = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                UserId = currentUserName;
            }
        }

        public ClaimsPrincipal GetUser()
        {
            return _accessor?.HttpContext?.User as ClaimsPrincipal;
        }

        public string UserId { get; }
    }
}
