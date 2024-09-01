using DevicesHub.Web.Extensions;
using DevicesHub.Web.SeedAdmin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;

namespace DevicesHub.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Service Extension for additional services
            builder.Services.AddAppService(builder.Configuration);

            // Additional services
            builder.Services.AddMemoryCache();
            builder.Services.AddSession();

            // Build the app
            var app = builder.Build();

            // Apply pending migrations
            await ApplyMigrations.UpdatePendingMigrations(app);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Set Stripe configuration
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe")["SecretKey"]?.Trim();

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

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // Initialize seed data
                    await SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the database.");
                }
            }

            app.Run();
        }
    }
}
