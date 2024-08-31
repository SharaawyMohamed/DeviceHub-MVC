using DevicesHub.Application.External;
using DevicesHub.Application.MappingProfiles;
using DevicesHub.Application.Services;
using DevicesHub.Domain.Interfaces;
using DevicesHub.Domain.Services;
using DevicesHub.Infrastructure.Data.Contexts;
using DevicesHub.Infrastructure.Repositories;
using DevicesHub.Web.Extensions;
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

            // Service Extension
            builder.Services.AddAppService(builder.Configuration);

            builder.Services.AddMemoryCache();
            builder.Services.AddSession();
            var app = builder.Build();

            // To Apply Migrations
            await ApplyMigrations.UpdatePendingMigrations(app);

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
    }
}
