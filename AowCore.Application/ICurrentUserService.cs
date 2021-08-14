using System.Security.Claims;

namespace AowCore.Application
{
    public interface ICurrentUserService
    {
        ClaimsPrincipal GetUser();
        string UserId { get; }
    }
}
