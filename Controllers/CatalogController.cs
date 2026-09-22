using Microsoft.AspNetCore.Mvc;
using ProductManagementSystem.Models;
using ProductManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ProductManagementSystem.Controllers
{
    [Authorize]
    public class CatalogController : Controller
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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



        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Categories = _categoryService.GetAllCategories();
            ViewBag.Brands = _brandService.GetAllBrands();
            ViewBag.Suppliers = _supplierService.GetAllSuppliers();
            ViewBag.Tags = _tagService.GetAllTags();

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(Category category)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            category.UserId = userId;
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                TempData["Error"] = "Category name is required.";
                return RedirectToAction(nameof(Index));
            }

            _categoryService.AddCategory(category);

            TempData["Success"] = "Category created successfully.";

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(Category category)
        {
            if (category.Id <= 0)
            {
                TempData["Error"] = "Invalid category.";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                TempData["Error"] = "Category name is required.";
                return RedirectToAction(nameof(Index));
            }

            var existingCategory = _categoryService.GetCategoryById(category.Id);

            if (existingCategory == null)
            {
                TempData["Error"] = "Category not found.";
                return RedirectToAction(nameof(Index));
            }

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            _categoryService.UpdateCategory(existingCategory);

            TempData["Success"] = "Category updated successfully.";

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Invalid category.";
                return RedirectToAction(nameof(Index));
            }

            var category = _categoryService.GetCategoryById(id);

            if (category == null)
            {
                TempData["Error"] = "Category not found.";
                return RedirectToAction(nameof(Index));
            }

            _categoryService.DeleteCategory(id);

            TempData["Success"] = "Category deleted successfully.";

            return RedirectToAction(nameof(Index));
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBrand(Brand brand)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            category.UserId = userId;
            if (string.IsNullOrWhiteSpace(brand.Name))
            {
                TempData["Error"] = "Brand name is required.";
                return RedirectToAction(nameof(Index));
            }

            _brandService.AddBrand(brand);

            TempData["Success"] = "Brand created successfully.";

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditBrand(Brand brand)
        {
            if (brand.Id <= 0)
            {
                TempData["Error"] = "Invalid brand.";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(brand.Name))
            {
                TempData["Error"] = "Brand name is required.";
                return RedirectToAction(nameof(Index));
            }

            var existingBrand = _brandService.GetBrandById(brand.Id);

            if (existingBrand == null)
            {
                TempData["Error"] = "Brand not found.";
                return RedirectToAction(nameof(Index));
            }

            existingBrand.Name = brand.Name;

            _brandService.UpdateBrand(existingBrand);

            TempData["Success"] = "Brand updated successfully.";

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBrand(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Invalid brand.";
                return RedirectToAction(nameof(Index));
            }

            var brand = _brandService.GetBrandById(id);

            if (brand == null)
            {
                TempData["Error"] = "Brand not found.";
                return RedirectToAction(nameof(Index));
            }

            _brandService.DeleteBrand(id);

            TempData["Success"] = "Brand deleted successfully.";

            return RedirectToAction(nameof(Index));
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateSupplier(Supplier supplier)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            category.UserId = userId;
            if (string.IsNullOrWhiteSpace(supplier.Name))
            {
                TempData["Error"] = "Supplier name is required.";
                return RedirectToAction(nameof(Index));
            }

            _supplierService.AddSupplier(supplier);

            TempData["Success"] = "Supplier created successfully.";

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSupplier(Supplier supplier)
        {
            if (supplier.Id <= 0)
            {
                TempData["Error"] = "Invalid supplier.";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(supplier.Name))
            {
                TempData["Error"] = "Supplier name is required.";
                return RedirectToAction(nameof(Index));
            }

            var existingSupplier = _supplierService.GetSupplierById(supplier.Id);

            if (existingSupplier == null)
            {
                TempData["Error"] = "Supplier not found.";
                return RedirectToAction(nameof(Index));
            }

            existingSupplier.Name = supplier.Name;
            existingSupplier.Country = supplier.Country;

            _supplierService.UpdateSupplier(existingSupplier);

            TempData["Success"] = "Supplier updated successfully.";

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSupplier(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Invalid supplier.";
                return RedirectToAction(nameof(Index));
            }

            var supplier = _supplierService.GetSupplierById(id);

            if (supplier == null)
            {
                TempData["Error"] = "Supplier not found.";
                return RedirectToAction(nameof(Index));
            }

            _supplierService.DeleteSupplier(id);

            TempData["Success"] = "Supplier deleted successfully.";

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTag(Tag tag)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            category.UserId = userId;
            if (string.IsNullOrWhiteSpace(tag.Name))
            {
                TempData["Error"] = "Tag name is required.";
                return RedirectToAction(nameof(Index));
            }

            _tagService.AddTag(tag);

            TempData["Success"] = "Tag created successfully.";

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTag(Tag tag)
        {
            if (tag.Id <= 0)
            {
                TempData["Error"] = "Invalid tag.";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(tag.Name))
            {
                TempData["Error"] = "Tag name is required.";
                return RedirectToAction(nameof(Index));
            }

            var existingTag = _tagService.GetTagById(tag.Id);

            if (existingTag == null)
            {
                TempData["Error"] = "Tag not found.";
                return RedirectToAction(nameof(Index));
            }

            existingTag.Name = tag.Name;

            _tagService.UpdateTag(existingTag);

            TempData["Success"] = "Tag updated successfully.";

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTag(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Invalid tag.";
                return RedirectToAction(nameof(Index));
            }

            var tag = _tagService.GetTagById(id);

            if (tag == null)
            {
                TempData["Error"] = "Tag not found.";
                return RedirectToAction(nameof(Index));
            }

            _tagService.DeleteTag(id);

            TempData["Success"] = "Tag deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}