using DevicesHub.Application.External;
using DevicesHub.Domain.Interfaces;
using DevicesHub.Domain.Models;
using DevicesHub.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevicesHub.Web.Areas.Admin.Controllers
{
    [Area(SD.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            // var categories = await _orderDetailsService.Category.GetAllAsync();
            var categories = await _categoryService.GetAllCategoryAsync();
            return View(categories);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                var done = await _categoryService.AddCategoryAsync(category);
                if (done > 0)
                {
                    //await _orderDetailsService.Category.AddCategoryAsync(category);
                    //await _orderDetailsService.CompleteAsync();
                    TempData["Create"] = "Data has been created successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(category);
        }
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || id == 0) return NotFound();
            var category = await _categoryService.GetFirstCategoryAsync(C => C.Id == id);
            // var category = await _orderDetailsService.Category.GetFirstAsync(C => C.Id == id);
            if (category is null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                //_orderDetailsService.Category.Update(category);
                //await _orderDetailsService.CompleteAsync();
                var done = await _categoryService.UpdateCategoryAsync(category);
                if (done > 0)
                {
                    TempData["Edit"] = "Category has been Updated successfully";
                }
                else
                {
                    TempData["Edit"] = "Oops..., Category Is Not Updated";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            // var category = await _orderDetailsService.Category.GetFirstAsync(C => C.Id == id);
            var category = await _categoryService.GetFirstCategoryAsync(c => c.Id == id);
            if (category is null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Category category)
        {
            if (ModelState.IsValid)
            {

                //_orderDetailsService.Category.Remove(category);
                //await _orderDetailsService.CompleteAsync();
                var done = await _categoryService.RemoveCategoryAsync(category);
                if (done > 0)
                {
                    TempData["Delete"] = "Data has been deleted successfully";
                }
                else
                {
                    TempData["Delete"] = "Oops Category, hasn't deleted.";

                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
    }

}
