using AowCore.AppWeb.Interfaces;
using AowCore.AppWeb.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AowCore.AppWeb.Configuration
{
    public static class ConfigureWebServices
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
        {           
            services.AddScoped<IVoucherViewModelService, VoucherViewModelService>();        
            return services;
        }
    }
}
