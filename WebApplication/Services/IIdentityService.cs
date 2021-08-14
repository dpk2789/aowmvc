using API.Models;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IIdentityService
    {
        Task<AuthResult> RegisterAsync(string Email , string Password);
        Task<AuthResult> LoginAsync(string Email, string Password);
    }
}
