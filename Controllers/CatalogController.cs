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
            var userId = GetUserId();

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                TempData["Error"] = "Category name is required.";
                return RedirectToAction(nameof(Index));
            }

            category.UserId = userId;

            _categoryService.AddCategory(category);

            TempData["Success"] = "Category created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(Category category)
        {
            var userId = GetUserId();

            if (category.Id <= 0 || string.IsNullOrWhiteSpace(category.Name))
            {
                TempData["Error"] = "Invalid category.";
                return RedirectToAction(nameof(Index));
            }

            var existing = _categoryService.GetCategoryById(category.Id, userId);

            if (existing == null)
            {
                TempData["Error"] = "Category not found.";
                return RedirectToAction(nameof(Index));
            }

            existing.Name = category.Name;
            existing.Description = category.Description;

            _categoryService.UpdateCategory(existing, userId);

            TempData["Success"] = "Category updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int id)
        {
            var userId = GetUserId();

            if (_categoryService.GetCategoryById(id, userId) == null)
            {
                TempData["Error"] = "Category not found.";
                return RedirectToAction(nameof(Index));
            }

            _categoryService.DeleteCategory(id, userId);

            TempData["Success"] = "Category deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBrand(Brand brand)
        {
            var userId = GetUserId();

            if (string.IsNullOrWhiteSpace(brand.Name))
            {
                TempData["Error"] = "Brand name is required.";
                return RedirectToAction(nameof(Index));
            }

            brand.UserId = userId;

            _brandService.AddBrand(brand);

            TempData["Success"] = "Brand created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditBrand(Brand brand)
        {
            var userId = GetUserId();

            if (brand.Id <= 0 || string.IsNullOrWhiteSpace(brand.Name))
            {
                TempData["Error"] = "Invalid brand.";
                return RedirectToAction(nameof(Index));
            }

            var existing = _brandService.GetBrandById(brand.Id, userId);

            if (existing == null)
            {
                TempData["Error"] = "Brand not found.";
                return RedirectToAction(nameof(Index));
            }

            existing.Name = brand.Name;

            _brandService.UpdateBrand(existing, userId);

            TempData["Success"] = "Brand updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBrand(int id)
        {
            var userId = GetUserId();

            if (_brandService.GetBrandById(id, userId) == null)
            {
                TempData["Error"] = "Brand not found.";
                return RedirectToAction(nameof(Index));
            }

            _brandService.DeleteBrand(id, userId);

            TempData["Success"] = "Brand deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateSupplier(Supplier supplier)
        {
            var userId = GetUserId();

            if (string.IsNullOrWhiteSpace(supplier.Name))
            {
                TempData["Error"] = "Supplier name is required.";
                return RedirectToAction(nameof(Index));
            }

            supplier.UserId = userId;

            _supplierService.AddSupplier(supplier);

            TempData["Success"] = "Supplier created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSupplier(Supplier supplier)
        {
            var userId = GetUserId();

            if (supplier.Id <= 0 || string.IsNullOrWhiteSpace(supplier.Name))
            {
                TempData["Error"] = "Invalid supplier.";
                return RedirectToAction(nameof(Index));
            }

            var existing = _supplierService.GetSupplierById(supplier.Id, userId);

            if (existing == null)
            {
                TempData["Error"] = "Supplier not found.";
                return RedirectToAction(nameof(Index));
            }

            existing.Name = supplier.Name;
            existing.Country = supplier.Country;

            _supplierService.UpdateSupplier(existing, userId);

            TempData["Success"] = "Supplier updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSupplier(int id)
        {
            var userId = GetUserId();

            if (_supplierService.GetSupplierById(id, userId) == null)
            {
                TempData["Error"] = "Supplier not found.";
                return RedirectToAction(nameof(Index));
            }

            _supplierService.DeleteSupplier(id, userId);

            TempData["Success"] = "Supplier deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTag(Tag tag)
        {
            var userId = GetUserId();

            if (string.IsNullOrWhiteSpace(tag.Name))
            {
                TempData["Error"] = "Tag name is required.";
                return RedirectToAction(nameof(Index));
            }

            tag.UserId = userId;

            _tagService.AddTag(tag);

            TempData["Success"] = "Tag created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTag(Tag tag)
        {
            var userId = GetUserId();

            if (tag.Id <= 0 || string.IsNullOrWhiteSpace(tag.Name))
            {
                TempData["Error"] = "Invalid tag.";
                return RedirectToAction(nameof(Index));
            }

            var existing = _tagService.GetTagById(tag.Id, userId);

            if (existing == null)
            {
                TempData["Error"] = "Tag not found.";
                return RedirectToAction(nameof(Index));
            }

            existing.Name = tag.Name;

            _tagService.UpdateTag(existing, userId);

            TempData["Success"] = "Tag updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTag(int id)
        {
            var userId = GetUserId();

            if (_tagService.GetTagById(id, userId) == null)
            {
                TempData["Error"] = "Tag not found.";
                return RedirectToAction(nameof(Index));
            }

            _tagService.DeleteTag(id, userId);

            TempData["Success"] = "Tag deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}