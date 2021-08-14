using AowCore.Application;
using AowCore.Application.IRepository;
using AowCore.Application.Services;
using AowCore.Domain;
using AowCore.Domain.Models;
using AowCore.Infrastructure.Common;
using AowCore.Infrastructure.Data;
using AowCore.Infrastructure.Repositories;
using AowCore.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AowCore.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        configuration.GetConnectionString("DefaultConnection"),
            //        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddDbContextPool<ApplicationDbContext>(options =>
                options.UseMySQL(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());


            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            //  services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<ILedgerCategoryRepository, LedgerCategoryRepository>();
            services.AddTransient<ILedgerRepository, LedgerRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IVoucherRepository, VoucherRepository>();
            services.AddTransient<IJournalEntryRepository, JournalEntryRepository>();
            services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IVoucherService, VoucherService>();
            services.AddTransient<IVoucherItemRepository, VoucherItemRepository>();
            services.AddTransient<IVoucherSundryItemRepository, VoucherSundryItemRepository>();
            services.AddTransient<ICountryRepository, CountryRepository>();
            //services.AddDefaultIdentity<ApplicationUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.AddIdentityServer()
            //    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            //services.AddAuthentication()
            //    .AddIdentityServerJwt();

            return services;
        }
    }
}
