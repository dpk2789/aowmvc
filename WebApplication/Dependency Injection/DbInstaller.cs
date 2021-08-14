using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AowCore.Infrastructure.Data;
using AowCore.Domain;
using AowCore.Application;

namespace API.Dependency_Injection
{
    public class DbInstaller : IInstaller
    {
        public void InstallerServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(
                     Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
          

        }
    }
}
