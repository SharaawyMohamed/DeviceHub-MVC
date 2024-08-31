using DevicesHub.Application.External;
using DevicesHub.Application.MappingProfiles;
using DevicesHub.Application.Services;
using DevicesHub.Domain.Interfaces;
using DevicesHub.Domain.Services;
using DevicesHub.Infrastructure.Data.Contexts;
using DevicesHub.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace DevicesHub.Web.Extensions
{
    public static class AppServiceExtension
    {
        public static IServiceCollection AddAppService(this IServiceCollection Services,IConfiguration Configuration)
        {
            
            Services.AddDbContext<DeviceHubDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            Services.Configure<StripeData>(Configuration.GetSection("stripe"));

           Services.AddIdentity<IdentityUser, IdentityRole>(
                             options => options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(4))
                            .AddDefaultTokenProviders()
                            .AddDefaultUI()
                            .AddEntityFrameworkStores<DeviceHubDbContext>();

            Services.AddRazorPages().AddRazorRuntimeCompilation();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<ICategoryRepository, CategoryRepository>();
            Services.AddAutoMapper(M => M.AddProfile(new ProductProfile()));
            Services.AddSingleton<IEmailSender, EmailSender>();

            Services.AddScoped<IOrderDetailsService, OrderDetailsService>();
            Services.AddScoped<IOrderHeaderService, OrderHeaderService>();
            Services.AddScoped<ICategoryService, CategoryService>();
            Services.AddScoped<IProductService, ProductServices>();
            Services.AddScoped<IShoppingCartService, ShoppingCartService>();

            return  Services;
        }
    }
}
