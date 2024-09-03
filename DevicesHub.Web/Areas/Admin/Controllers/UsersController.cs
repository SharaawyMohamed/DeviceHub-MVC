using System.Collections.Specialized;
using System.Security.Claims;
using DevicesHub.Application.External;
using DevicesHub.Domain.Models;
using DevicesHub.Infrastructure.Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;

namespace DevicesHub.Web.Areas.Admin.Controllers
{
    [Area(SD.AdminRole)]
    [Authorize(Roles = SD.AdminRole)]
    public class UsersController : Controller
    {
        private int lockstate = 0;
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;
            var Users = await _userManager.Users.Where(U => U.Id != userId).ToListAsync();
            return View(Users);
        }

        public async Task<IActionResult> LockUnlock(string? Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
                return NotFound();

            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddHours(4);
                TempData["Delete"] = "User Has Been Locked Successfully";
            }
            else
            {
                user.LockoutEnd = DateTime.Now;
                TempData["Edit"] = "User UnLocked Successfully";

            }
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index), new { area = SD.AdminRole });
        }
        public async Task<IActionResult> Delete(string? Id)
        {
            if (Id is null)
            {
                return RedirectToAction(nameof(Index));
            }
            var user = await _userManager.FindByIdAsync(Id);
            if (user is not null)
            {
                await _userManager.DeleteAsync(user);
                TempData["Delete"] = "User Has Been Delete Successfully";
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
