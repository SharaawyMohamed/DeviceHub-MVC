using AutoMapper;
using DevicesHub.Application.External;
using DevicesHub.Application.Settings;
using DevicesHub.Domain.Interfaces;
using DevicesHub.Domain.Models;
using DevicesHub.Domain.Services;
using DevicesHub.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevicesHub.Web.Areas.Admin.Controllers
{
    [Area(SD.AdminRole)]
    [Authorize(Roles = SD.AdminRole)]

    public class ProductController : Controller
    {
        private readonly IMapper mapper;
        private readonly IProductService _productService;


        public ProductController(IMapper _mapper, IProductService productService)
        {
            mapper = _mapper;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync(IncludeWord: "Category");
            return View(products);
        }
        public async Task<IActionResult> GetData()
        {
            var Products = await _productService.GetAllProductsAsync(IncludeWord: "Category");
            var TotalRecords = Products.Count();
            var JsonFile = new { data = Products };
            return Ok(JsonFile);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVM entity)
        {
            if (ModelState.IsValid)
            {
                
                var Created = await _productService.AddProductAsync(entity);
                if (Created > 0)
                {
                    TempData["Create"] = "Data has been created successfully";
                }
                else
                {
                    TempData["Create"] = "Oops..., Product hasn't created.";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var product = await _productService.GetFirstProductAsync(p=>p.Id==id,IncludeWord: "Category");//_orderDetailsService.Product.GetFirstAsync(C => C.Id == id, "Category");
            if (product == null) return NotFound();

            TempData["ImageName"] = product.ImageName;
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(ProductVM product)
        {
            if (ModelState.IsValid)
            {
                product.ImageName = (string)TempData["ImageName"];

                var Updated = await _productService.UpdateProductAsync(product);
                if (Updated > 0)
                {
                    TempData["Edit"] = "Data has been Updated successfully";
                }
                else
                {
                    TempData["Edit"] = "Oops...,Product hasn't Updated.";

                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpDelete]

        public async Task<IActionResult> Delete(int? id)
        {
            var product = await _productService.GetFirstProductAsync(x => x.Id == id);
            if (product == null)
                return Json(new { success = false, message = "Error while deleting the product" });

            var Removed = await _productService.RemoveProductAsync(product);
            if (Removed > 0)
            {
                DocumentSettings.DeleteFile(product.ImageName, "Products");
                return Json(new { success = true, message = "product has been deleted Successfully" });
            }
            else
            {
                return Json(new { fail = true, message = "product hasn't deleted." });
            }
        }
    }

}
