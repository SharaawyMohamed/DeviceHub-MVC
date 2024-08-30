using AutoMapper;
using DevicesHub.Application.External;
using DevicesHub.Application.Settings;
using DevicesHub.Application.ViewModels;
using DevicesHub.Domain.Interfaces;
using DevicesHub.Domain.Models;
using DevicesHub.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevicesHub.Web.Areas.Admin.Controllers
{
    [Area(SD.AdminRole)]
    public class ProductController : Controller
    {
        private readonly IMapper mapper;
        private readonly IProductService _productService;

        //private readonly IWebHostEnvironment webHostEnvironment

        public ProductController(IMapper _mapper, IProductService productService)
        {
            mapper = _mapper;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            //var Products = await _orderDetailsService.Product.GetAllAsync(IncludeWord: "Category"); // 
            var products = await _productService.GetAllProductsAsync(IncludeWord: "Category");
            var MappedProduct = mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(products);
            return View(MappedProduct);
        }
        public async Task<IActionResult> GetData()
        {
            // var Products = await _orderDetailsService.Product.GetAllAsync(IncludeWord: "Category"); // 
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
        public async Task<IActionResult> Create(ProductViewModel productVM)
        {
            if (ModelState.IsValid)
            {
                productVM.ImageName = DocumentSettings.UploadFile(productVM.Image, "Products");
                var MappedProduct = mapper.Map<ProductViewModel, Product>(productVM);
                //await _orderDetailsService.Product.AddAsync(MappedProduct);
                //await _orderDetailsService.CompleteAsync();
                var Created = await _productService.AddProductAsync(MappedProduct);
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
            return View(productVM);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var product = await _productService.GetFirstProductAsync(IncludeWord: "Category");//_orderDetailsService.Product.GetFirstAsync(C => C.Id == id, "Category");
            if (product == null) return NotFound();

            var MappedProduct = mapper.Map<Product, ProductViewModel>(product);
            TempData["ImageName"] = MappedProduct.ImageName;
            return View(MappedProduct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                product.ImageName = (string)TempData["ImageName"];
                if (product.Image != null)
                {
                    DocumentSettings.DeleteFile(product.ImageName, "Products");
                    product.ImageName = DocumentSettings.UploadFile(product.Image, "Products");
                }
                var MappedProduct = mapper.Map<ProductViewModel, Product>(product);
                //_orderDetailsService.Product.Update(MappedProduct);
                //await _orderDetailsService.CompleteAsync();
                var Updated = await _productService.UpdateProductAsync(MappedProduct);
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
            var product = await _productService.GetFirstProductAsync(x => x.Id == id);//_orderDetailsService.Product.GetFirstAsync(X => X.Id == id);
            if (product == null)
                return Json(new { success = false, message = "Error while deleting the product" });
            //_orderDetailsService.Product.Remove(product);
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
            //_orderDetailsService.CompleteAsync();
        }
    }

}
