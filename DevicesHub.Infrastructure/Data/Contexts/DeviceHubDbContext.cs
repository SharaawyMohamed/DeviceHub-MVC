

using DevicesHub.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevicesHub.Infrastructure.Data.Contexts
{
    public class DeviceHubDbContext : IdentityDbContext<ApplicationUser>
	{
		public DeviceHubDbContext(DbContextOptions<DeviceHubDbContext> options) : base(options)
		{
		}
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ShoppingCart> ShoppingCarts { get; set; }
		public DbSet<OrderHeader> OrderHeaders { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }
		public DbSet<ApplicationUser> applicationUsers { get; set; }
	}
}
