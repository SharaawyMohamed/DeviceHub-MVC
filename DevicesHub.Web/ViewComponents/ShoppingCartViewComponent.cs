using System.Security.Claims;
using DevicesHub.Application.External;
using DevicesHub.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevicesHub.Web.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartViewComponent(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                if (HttpContext.Session.GetInt32(SD.SessionKey) != null)
                {
                    return View(HttpContext.Session.GetInt32(SD.SessionKey));
                }
                HttpContext.Session.SetInt32(SD.SessionKey,(await _shoppingCartService.GetAllShoppingCartsAsync(X => X.ApplicationUserId == claim.Value)).ToList().Count());
                return View(HttpContext.Session.GetInt32(SD.SessionKey));
            }
            HttpContext.Session.Clear();
            return View(0);
        }
    }
}
