using DevicesHub.Application.External;
using DevicesHub.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace DevicesHub.Web.SeedAdmin
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Use constants for role names
            var roles = new[] { SD.AdminRole, SD.EditorRole, SD.CustomerRole };
            foreach (var roleName in roles)
            {
                if (await roleManager.FindByNameAsync(roleName) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Seed admin user with role
            string adminEmail = "admin@gmail.com";
            string adminPassword = "Admin@123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "Sharawy Mohamed",
                    Address = "El-Monsha, Sohag, Egypt",
                    City = "Sohag"
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    // Assign the Admin role to the admin user
                    await userManager.AddToRoleAsync(adminUser, SD.AdminRole);
                }
            }
        }
    }
}