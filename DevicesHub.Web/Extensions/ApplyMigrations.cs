using DevicesHub.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DevicesHub.Web.Extensions
{
    public static class ApplyMigrations
    {
        public static async Task UpdatePendingMigrations(WebApplication app)
        {
            using var Scope = app.Services.CreateScope();
            var Service = Scope.ServiceProvider;
            var LoggerFactory = Service.GetRequiredService<ILoggerFactory>();
            try
            {
                var _context = Service.GetRequiredService<DeviceHubDbContext>();
                var pendingMigrations = _context.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex.Message, "An Error Occurred During Update Database.");
            }
        }
    }
}
