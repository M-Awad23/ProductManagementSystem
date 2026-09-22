using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagementSystem.Models;
using ProductManagementSystem.Services;
using System.Security.Claims;

namespace ProductManagementSystem.Controllers
{
    [Authorize]
    public class CatalogController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly ISupplierService _supplierService;
        private readonly ITagService _tagService;

        public CatalogController(
            ICategoryService categoryService,
            IBrandService brandService,
            ISupplierService supplierService,
            ITagService tagService)
        {
            _categoryService = categoryService;
            _brandService = brandService;
            _supplierService = supplierService;
            _tagService = tagService;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public IActionResult Index()
        {
            var userId = GetUserId();

            ViewBag.Categories = _categoryService.GetAllCategories(userId);
            ViewBag.Brands = _brandService.GetAllBrands(userId);
            ViewBag.Suppliers = _supplierService.GetAllSuppliers(userId);
            ViewBag.Tags = _tagService.GetAllTags(userId);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(Category category)
        {
            var result = _categoryService.CreateCategory(
                category.Name,
                category.Description,
                GetUserId());

            if (!result.Success)
            {
                TempData["Error"] = result.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Category created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(Category category)
        {
            var result = _categoryService.UpdateCategory(
                category.Id,
                category.Name,
                category.Description,
                GetUserId());

            TempData[result.Success ? "Success" : "Error"] =
                result.Success
                    ? "Category updated successfully."
                    : result.ErrorMessage;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int id)
        {
            var deleted = _categoryService.DeleteCategory(
                id,
                GetUserId());

            TempData[deleted ? "Success" : "Error"] =
                deleted
                    ? "Category deleted successfully."
                    : "Category not found.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBrand(Brand brand)
        {
            var result = _brandService.CreateBrand(
                brand.Name,
                GetUserId());

            if (!result.Success)
            {
                TempData["Error"] = result.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Brand created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditBrand(Brand brand)
        {
            var result = _brandService.UpdateBrand(
                brand.Id,
                brand.Name,
                GetUserId());

            TempData[result.Success ? "Success" : "Error"] =
                result.Success
                    ? "Brand updated successfully."
                    : result.ErrorMessage;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBrand(int id)
        {
            var deleted = _brandService.DeleteBrand(
                id,
                GetUserId());

            TempData[deleted ? "Success" : "Error"] =
                deleted
                    ? "Brand deleted successfully."
                    : "Brand not found.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateSupplier(Supplier supplier)
        {
            var result = _supplierService.CreateSupplier(
                supplier.Name,
                supplier.Country,
                GetUserId());

            if (!result.Success)
            {
                TempData["Error"] = result.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Supplier created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSupplier(Supplier supplier)
        {
            var result = _supplierService.UpdateSupplier(
                supplier.Id,
                supplier.Name,
                supplier.Country,
                GetUserId());

            TempData[result.Success ? "Success" : "Error"] =
                result.Success
                    ? "Supplier updated successfully."
                    : result.ErrorMessage;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSupplier(int id)
        {
            var deleted = _supplierService.DeleteSupplier(
                id,
                GetUserId());

            TempData[deleted ? "Success" : "Error"] =
                deleted
                    ? "Supplier deleted successfully."
                    : "Supplier not found.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTag(Tag tag)
        {
            var result = _tagService.CreateTag(
                tag.Name,
                GetUserId());

            if (!result.Success)
            {
                TempData["Error"] = result.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Tag created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTag(Tag tag)
        {
            var result = _tagService.UpdateTag(
                tag.Id,
                tag.Name,
                GetUserId());

            TempData[result.Success ? "Success" : "Error"] =
                result.Success
                    ? "Tag updated successfully."
                    : result.ErrorMessage;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTag(int id)
        {
            var deleted = _tagService.DeleteTag(
                id,
                GetUserId());

            TempData[deleted ? "Success" : "Error"] =
                deleted
                    ? "Tag deleted successfully."
                    : "Tag not found.";

            return RedirectToAction(nameof(Index));
        }
    }
}