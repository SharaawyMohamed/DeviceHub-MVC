using System.Security.Claims;
using DevicesHub.Application.External;
using DevicesHub.Application.ViewModels;
using DevicesHub.Domain.Interfaces;
using DevicesHub.Domain.Models;
using DevicesHub.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Session = Stripe.Checkout.Session;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;

namespace DevicesHub.Web.Areas.Customer.Controllers
{
    [Area(SD.CustomerRole)]
   // [Authorize]
    public class CartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderHeaderService _orderHeader;
        private readonly IOrderDetailsService _orderDetailsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public ShoppingCartViewModel shoppingCartViewModel { get; set; }
        public int TotalCart { get; set; }
        public CartController(IShoppingCartService shoppingCartService, IOrderHeaderService orderHeader, IOrderDetailsService orderDetailsService, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _shoppingCartService = shoppingCartService;
            _orderHeader = orderHeader;
            _orderDetailsService = orderDetailsService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartViewModel = new ShoppingCartViewModel()
            {
                CartsList = await _shoppingCartService.GetAllShoppingCartsAsync(U => U.ApplicationUserId == claim.Value, "Product"),
                OrderHeader = new()
            };

            foreach (var item in shoppingCartViewModel.CartsList)
            {
                shoppingCartViewModel.OrderHeader.TotalPrice += item.Count * item.Product.Price;
            }
            return View(shoppingCartViewModel);
        }

        public async Task<IActionResult> Plus(int cartId)
        {
            var shoppingCart = await _shoppingCartService.GetFirstShoppingCartAsync(S => S.Id == cartId);
            await _shoppingCartService.IncreaseCountAsync(shoppingCart, 1);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Minus(int cartId)
        {
            var shoppingCart = await _shoppingCartService.GetFirstShoppingCartAsync(S => S.Id == cartId);
            if (shoppingCart.Count > 1)
            {
                await _shoppingCartService.DecreaseCountAsync(shoppingCart, 1);
            }
            else
            {
                await _shoppingCartService.RemoveShoppingCartAsync(shoppingCart);
                var count = (await _shoppingCartService.GetAllShoppingCartsAsync(U => U.ApplicationUserId == shoppingCart.ApplicationUserId)).ToList().Count() - 1;
                HttpContext.Session.SetInt32(SD.SessionKey, count);
            }
            //unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int cartId)
        {
            var shoppingCart = await _shoppingCartService.GetFirstShoppingCartAsync(S => S.Id == cartId);
            await _shoppingCartService.RemoveShoppingCartAsync(shoppingCart);

            var count = (await _shoppingCartService.GetAllShoppingCartsAsync(U => U.ApplicationUserId == shoppingCart.ApplicationUserId)).ToList().Count();
            HttpContext.Session.SetInt32(SD.SessionKey, count);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartViewModel = new ShoppingCartViewModel()
            {
                CartsList = await _shoppingCartService.GetAllShoppingCartsAsync(U => U.ApplicationUserId == claim.Value, "Product"),
                OrderHeader = new()
            };

            // you can use utility or mapper here
            shoppingCartViewModel.OrderHeader.ApplicationUser = await _unitOfWork.Repository<ApplicationUser>().GetFirstAsync(x => x.Id == claim.Value);
            shoppingCartViewModel.OrderHeader.Name = shoppingCartViewModel.OrderHeader.ApplicationUser.Name;
            shoppingCartViewModel.OrderHeader.Address = shoppingCartViewModel.OrderHeader.ApplicationUser.Address;
            shoppingCartViewModel.OrderHeader.City = shoppingCartViewModel.OrderHeader.ApplicationUser.City;
            shoppingCartViewModel.OrderHeader.Phone = shoppingCartViewModel.OrderHeader.ApplicationUser.PhoneNumber;

            foreach (var item in shoppingCartViewModel.CartsList)
            {
                shoppingCartViewModel.TotalPrices += item.Count * item.Product.Price;
            }
            return View(shoppingCartViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Summary(ShoppingCartViewModel shoppingCartViewModel)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCartViewModel.CartsList = await _shoppingCartService.GetAllShoppingCartsAsync(U => U.ApplicationUserId == claim.Value, "Product");

            shoppingCartViewModel.OrderHeader.OrderStatus = SD.Pending;
            shoppingCartViewModel.OrderHeader.PaymentStatus = SD.Pending;
            shoppingCartViewModel.OrderHeader.OrderDate = DateTime.Now;
            shoppingCartViewModel.OrderHeader.ApplicationUserId = claim.Value;

            foreach (var item in shoppingCartViewModel.CartsList)
            {
                shoppingCartViewModel.OrderHeader.TotalPrice += item.Count * item.Product.Price;
            }

            await _orderHeader.AddOrderHeaderAsync(shoppingCartViewModel.OrderHeader);

            foreach (var item in shoppingCartViewModel.CartsList)
            {
                OrderDetails orderDetails = new OrderDetails()
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = shoppingCartViewModel.OrderHeader.Id,
                    Price = item.Product.Price,
                    Count = item.Count
                };

                await _orderDetailsService.AddOrderDetailsAsync(orderDetails);
            }

            var domain = _configuration["DomainSettings:BaseUrl"];//"https://localhost:7157/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/orderconfirmation?id={shoppingCartViewModel.OrderHeader.Id}",
                CancelUrl = domain + $"customer/cart/index"
            };

            foreach (var item in shoppingCartViewModel.CartsList)
            {
                var sessionlineoption = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionlineoption);
            }

            var service = new SessionService();
            Session session = service.Create(options);
            shoppingCartViewModel.OrderHeader.SessionId = session.Id;
            await _unitOfWork.CompleteAsync();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public async Task<IActionResult> OrderConfirmation(int id)
        {
            OrderHeader orderHeader = await _orderHeader.GetFirstOrderHeadersAsync(O => O.Id == id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                await _orderHeader.UpdateOrderStatus(id, SD.Approve, SD.Approve);
                orderHeader.PaymentIntenId = session.PaymentIntentId;
                await _unitOfWork.CompleteAsync();
            }
            List<ShoppingCart> shoppingCarts = (await _shoppingCartService.GetAllShoppingCartsAsync(U => U.ApplicationUserId == orderHeader.ApplicationUserId)).ToList();
            await _shoppingCartService.RemoveRangeOfShoppingCartsAsync(shoppingCarts);
            return View();
        }

    }
}
