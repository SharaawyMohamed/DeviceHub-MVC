using System.Security.Claims;
using DevicesHub.Application.External;
using DevicesHub.Infrastructure.Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevicesHub.Web.Areas.Admin.Controllers
{
    [Area(SD.AdminRole)]
    [Authorize(Roles = SD.AdminRole)]
    public class UsersController : Controller
    {
        private readonly DeviceHubDbContext context;

        public UsersController(DeviceHubDbContext _context)
        {
            context = _context;
        }
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;
            var Users = await context.applicationUsers.Where(U => U.Id != userId).ToListAsync();
            return View(Users);
        }

        public async Task<IActionResult> LockUnlock(string? id)
        {
            var user = await context.applicationUsers.FirstOrDefaultAsync(U => U.Id == id);
            if (user == null)
                return NotFound();

            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddHours(4);
            }
            else
            {
                user.LockoutEnd = DateTime.Now;
            }
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index),nameof(UsersController), new { area = "Admin" });
        }

    }
}
