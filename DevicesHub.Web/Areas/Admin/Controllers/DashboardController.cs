using DevicesHub.Application.External;
using DevicesHub.Domain.Interfaces;
using DevicesHub.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevicesHub.Web.Areas.Admin.Controllers
{
    [Area(SD.AdminRole)]
    [Authorize(Roles = SD.AdminRole)]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public DashboardController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.AllOrders = (await unitOfWork.Repository<OrderHeader>().GetAllAsync()).Count();
            ViewBag.ApprovedOrders = (await unitOfWork.Repository<OrderHeader>().GetAllAsync(O => O.OrderStatus == SD.Approve)).Count();
            ViewBag.Users = (await unitOfWork.Repository<ApplicationUser>().GetAllAsync()).Count();
            ViewBag.Products = (await unitOfWork.Repository<Product>().GetAllAsync()).Count();
            return View();
        }
    }
}
