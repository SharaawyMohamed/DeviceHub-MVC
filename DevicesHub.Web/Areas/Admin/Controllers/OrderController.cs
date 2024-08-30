using DevicesHub.Application.External;
using DevicesHub.Application.ViewModels;
using DevicesHub.Domain.Interfaces;
using DevicesHub.Domain.Models;
using DevicesHub.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace DevicesHub.Web.Areas.Admin.Controllers
{
    [Area(SD.AdminRole)]
    [Authorize(Roles = SD.AdminRole)]
    public class OrderController : Controller
    {
        private readonly IOrderDetailsService _orderDetailsService;
        private readonly IOrderHeaderService _orderHeaderService;
        [BindProperty]
        public OrderViewModel orderViewModel { get; set; }
        public OrderController(IOrderDetailsService _unitOfWork, IOrderHeaderService orderHeaderService)
        {
            _orderDetailsService = _unitOfWork;
            _orderHeaderService = orderHeaderService;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var orderHeaders = await _orderDetailsService.GetAllOrdersDetailsAsync(IncludeWord: "ApplicationUser");
            return Json(new { data = orderHeaders });
        }

        public async Task<IActionResult> Details(int orderid)
        {
            var orderViewModel = new OrderViewModel()
            {
                OrderHeader = await _orderHeaderService.GetFirstOrderHeadersAsync(O => O.Id == orderid, "ApplicationUser"),
                OrdersDetails = await _orderDetailsService.GetAllOrdersDetailsAsync(O => O.OrderHeaderId == orderid, "Product")
            };
            return View(orderViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderDetails()
        {
            var order = await _orderHeaderService.GetFirstOrderHeadersAsync(O => O.Id == orderViewModel.OrderHeader.Id);
            if (order == null) return NotFound();
            order.Name = orderViewModel.OrderHeader.Name;
            order.Phone = orderViewModel.OrderHeader.Phone;
            order.Address = orderViewModel.OrderHeader.Address;
            order.City = orderViewModel.OrderHeader.City;
            if (orderViewModel.OrderHeader.Carrier != null)
            {
                order.Carrier = orderViewModel.OrderHeader.Carrier;
            }
            if (orderViewModel.OrderHeader.TrackingNumber != null)
            {
                order.Carrier = orderViewModel.OrderHeader.TrackingNumber;
            }
            var Edited = await _orderHeaderService.UpdateOrderHeaderAsync(order);
            if (Edited > 0)
            {
                TempData["Edit"] = "Data has been updated successfully";
            }
            else
            {
                TempData["Edit"] = "Oops...,Order hasn't updated.";
            }
            return RedirectToAction(nameof(Details), nameof(OrderController), new { orderid = order.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Proccess()
        {
            var Status = await _orderHeaderService.UpdateOrderStatus(orderViewModel.OrderHeader.Id, SD.Proccessing, null);
            if (Status > 0)
            {
                TempData["Edit"] = "Order status has been updated successfully";
            }
            else
            {
                TempData["Edit"] = "Oops...., Order status hasn't updated.";

            }
            return RedirectToAction(nameof(Details), nameof(OrderController), new { orderid = orderViewModel.OrderHeader.Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Fulfill()
        {
            var order = await _orderHeaderService.GetFirstOrderHeadersAsync(O => O.Id == orderViewModel.OrderHeader.Id);
            order.TrackingNumber = orderViewModel.OrderHeader.TrackingNumber;
            order.Carrier = orderViewModel.OrderHeader.Carrier;
            order.OrderStatus = SD.Shipped;
            order.ShippingDate = DateTime.Now;

            var Updated = await _orderHeaderService.UpdateOrderHeaderAsync(order);
            if (Updated > 0)
            {
                TempData["Edit"] = "Order has shipped successfully";
            }
            else
            {
                TempData["Edit"] = "Oops...., Order hasn't shipped ";
            }
            return RedirectToAction(nameof(Details),nameof(OrderController), new { orderid = orderViewModel.OrderHeader.Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel()
        {
            var order = await _orderHeaderService.GetFirstOrderHeadersAsync(O => O.Id == orderViewModel.OrderHeader.Id);
            if (order.OrderStatus == SD.Approve)
            {
                var option = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = order.PaymentIntenId
                };
                var service = new RefundService();
                Refund refund = service.Create(option);
                await _orderHeaderService.UpdateOrderStatus(order.Id, SD.Canceled, SD.Refund);
            }
            else
            {
                await _orderHeaderService.UpdateOrderStatus(order.Id, SD.Canceled, SD.Canceled);
            }

            TempData["Edit"] = "Order  has been cancelled successfully";
            return RedirectToAction(nameof(Details),nameof(OrderController), new { orderid = orderViewModel.OrderHeader.Id });
        }
    }
}
