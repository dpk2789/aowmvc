using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Dependency_Injection
{
    public interface IInstaller
    {
        void InstallerServices(IServiceCollection services, IConfiguration Configuration);
    }
}
