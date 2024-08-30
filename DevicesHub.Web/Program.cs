using DevicesHub.Application.External;
using DevicesHub.Application.MappingProfiles;
using DevicesHub.Application.Services;
using DevicesHub.Domain.Interfaces;
using DevicesHub.Domain.Services;
using DevicesHub.Infrastructure.Data.Contexts;
using DevicesHub.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace DevicesHub.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // AddCategoryAsync services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DeviceHubDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });

            builder.Services.Configure<StripeData>(builder.Configuration.GetSection("stripe"));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(
                             options => options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(4))
                            .AddDefaultTokenProviders()
                            .AddDefaultUI()
                            .AddEntityFrameworkStores<DeviceHubDbContext>();

            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddAutoMapper(M => M.AddProfile(new ProductProfile()));
            builder.Services.AddSingleton<IEmailSender, EmailSender>();

            builder.Services.AddScoped<IOrderDetailsService, OrderDetailsService>();
            builder.Services.AddScoped<IOrderHeaderService, OrderHeaderService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductServices>();
            builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

            builder.Services.AddMemoryCache();
            builder.Services.AddSession();
            var app = builder.Build();

            // To Apply Migrations
            await ApplyMigrations(app);
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            StripeConfiguration.ApiKey = builder.Configuration.GetSection("stripe:Secretkey").Get<string>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();
            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                 name: "Customer",
                 pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
        private static async Task ApplyMigrations(WebApplication app)
        {
            using(var scope = app.Services.CreateScope())
            {
                var service = scope.ServiceProvider;
                var loggerFactory= service.GetRequiredService<ILoggerFactory>();
                try
                {
                    var _context = service.GetRequiredService<DeviceHubDbContext>();
                    var unApplyingMigrations = await _context.Database.GetPendingMigrationsAsync();
                    if (unApplyingMigrations.Any())
                    {
                        await _context.Database.MigrateAsync();
                    }
                }
                catch(Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex.Message);
                }
            }
        }
    }
}
