using System.Security.Claims;
using DevicesHub.Application.External;
using DevicesHub.Application.Services;
using DevicesHub.Application.ViewModels;
using DevicesHub.Domain.Interfaces;
using DevicesHub.Domain.Models;
using DevicesHub.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace DevicesHub.Web.Areas.Customer.Controllers
{
    [Area(SD.CustomerRole)]
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderHeaderService _orderHeaderService;
        private readonly IOrderDetailsService _orderDetailsService;
		public HomeController(IUnitOfWork _unitOfWork, IProductService productService, IShoppingCartService shoppingCartService, IOrderHeaderService orderService, IOrderDetailsService orderDetailsService)
		{
			unitOfWork = _unitOfWork;
			_productService = productService;
			_shoppingCartService = shoppingCartService;
			_orderHeaderService = orderService;
			_orderDetailsService = orderDetailsService;
		}
		public async Task<IActionResult> Index(int? page)
        {
            var PageNumber = page ?? 1;
            var PageSize = 4;
            var products = await (await _productService.GetAllProductsAsync()).ToPagedListAsync(PageNumber, PageSize);
            return View(products);
        }

        public async Task<IActionResult> Details(int ProductId)
        {
            ShoppingCart obj = new ShoppingCart()
            {
                ProductId = ProductId,
                Product = await _productService.GetFirstProductAsync(P => P.Id == ProductId, "Category"),
                Count = 1
            };
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;
            ShoppingCart cart = await _shoppingCartService.GetFirstShoppingCartAsync(
                U => U.ApplicationUserId == claim.Value
                && U.ProductId == shoppingCart.ProductId
                );

            if (cart == null)
            {
                await _shoppingCartService.AddShoppingCartAsync(shoppingCart);
                HttpContext.Session.SetInt32(SD.SessionKey,
                    (await _shoppingCartService.GetAllShoppingCartsAsync(U => U.ApplicationUserId == claim.Value))
                    .ToList().Count);
            }
            else
            {
                await _shoppingCartService.IncreaseCountAsync(cart, shoppingCart.Count);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UserOrders()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            var orderHeader = await _orderHeaderService.GetAllOrderHeadersAsync(O => O.ApplicationUserId == claim.Value);

            
			return View(orderHeader);
		}
    }
}
