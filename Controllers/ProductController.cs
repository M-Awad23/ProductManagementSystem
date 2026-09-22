using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagementSystem.Models;
using ProductManagementSystem.Services;
using System.Security.Claims;

namespace ProductManagementSystem.Controllers;

[Authorize]
public class ProductController : Controller
{
    private readonly IPdfService _pdfService;
    private readonly IProductImageService _productImageService;
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IBrandService _brandService;
    private readonly ISupplierService _supplierService;
    private readonly ITagService _tagService;

    public ProductController(
        IProductImageService productImageService,
        IProductService productService,
        ICategoryService categoryService,
        IBrandService brandService,
        ISupplierService supplierService,
        ITagService tagService,
        IPdfService pdfService)
    {
        _productImageService = productImageService;
        _productService = productService;
        _categoryService = categoryService;
        _brandService = brandService;
        _supplierService = supplierService;
        _tagService = tagService;
        _pdfService = pdfService;
    }


    private void LoadProductFormData()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        ViewBag.Categories = _categoryService.GetAllCategories(userId);
        ViewBag.Brands = _brandService.GetAllBrands(userId);
        ViewBag.Suppliers = _supplierService.GetAllSuppliers(userId);
        ViewBag.Tags = _tagService.GetAllTags(userId);
    }

    public async Task<IActionResult> Index(
        string? search,
        string? sortOrder,
        int? categoryId,
        int? brandId,
        int? supplierId,
        decimal? minPrice,
        decimal? maxPrice,
        int? minQuantity,
        int? maxQuantity,
        int page = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        const int pageSize = 8;

        var products = await _productService.GetProductsAsync(
            userId,
            search,
            sortOrder,
            categoryId,
            brandId,
            supplierId,
            minPrice,
            maxPrice,
            minQuantity,
            maxQuantity,
            page,
            pageSize);

        var totalProducts = await _productService.GetProductCountAsync(
            userId,
            search,
            categoryId,
            brandId,
            supplierId,
            minPrice,
            maxPrice,
            minQuantity,
            maxQuantity);

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(
            totalProducts / (double)pageSize);

        ViewBag.Categories = _categoryService.GetAllCategories(userId);
        ViewBag.Brands = _brandService.GetAllBrands(userId);
        ViewBag.Suppliers = _supplierService.GetAllSuppliers(userId);

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView("_ProductList", products);
        }

        return View(products);
    }

    public IActionResult Create()
    {
        LoadProductFormData();

        return PartialView("_ProductForm", new Product());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Name,Description,Price,Quantity,CategoryId,BrandId,SupplierId")]
        Product product,
        List<IFormFile>? images,
        List<int>? tagIds)
    {
        ModelState.Remove(nameof(Product.UserId));
        ModelState.Remove(nameof(Product.User));
        ModelState.Remove(nameof(Product.ProductTags));
        ModelState.Remove(nameof(Product.ProductImages));

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var result = await _productService.CreateProductAsync(
            product,
            userId,
            images,
            tagIds);

        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage!);
            return BadRequest(ModelState);
        }

        var products = await _productService.GetProductsAsync(
            userId,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            1,
            8);

        return PartialView("_ProductList", products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var product = await _productService.GetByIdAsync(
            id,
            userId);

        if (product == null)
        {
            return NotFound();
        }

        return PartialView("_Details", product);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var product = await _productService.GetByIdAsync(
            id,
            userId);

        if (product == null)
        {
            return NotFound();
        }

        LoadProductFormData();

        return PartialView("_ProductForm", product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Name,Description,Price,Quantity,CategoryId,BrandId,SupplierId")]
        Product product,
        List<IFormFile>? images,
        List<int>? tagIds)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        ModelState.Remove(nameof(Product.UserId));
        ModelState.Remove(nameof(Product.User));

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var result = await _productService.UpdateProductAsync(
            product,
            userId,
            images,
            tagIds);

        if (!result.Success)
        {
            if (result.ErrorMessage == "Product not found.")
            {
                return NotFound();
            }

            ModelState.AddModelError(string.Empty, result.ErrorMessage!);
            return BadRequest(ModelState);
        }

        var products = await _productService.GetProductsAsync(
            userId,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            1,
            8);

        return PartialView("_ProductList", products);
    }

    public async Task<IActionResult> DownloadPdf(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var product = await _productService.GetByIdAsync(id, userId);

        if (product == null)
        {
            return NotFound();
        }

        var pdf = _pdfService.GenerateProductPdf(product);

        return File(
            pdf,
            "application/pdf",
            $"Product-{product.Id}.pdf");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var deleted = await _productService.DeleteAsync(id, userId);

        if (!deleted)
        {
            return NotFound();
        }

        var products = await _productService.GetProductsAsync(
            userId,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            1,
            8);

        return PartialView("_ProductList", products);
    }

    public async Task<IActionResult> Deleted()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var products = await _productService.GetDeletedProductsAsync(userId);

        return View(products);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var restored = await _productService.RestoreAsync(id, userId);

        if (!restored)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Deleted));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> P    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PermanentDelete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var deleted = await _productService.PermanentDeleteAsync(id, userId);

        if (!deleted)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Deleted));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteImage(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var deleted = await _productImageService.DeleteProductImageAsync(
            id,
            userId);

        if (!deleted)
        {
            return NotFound();
        }

        return Json(new
        {
            success = true
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReplaceImage(
        int id,
        IFormFile? image)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _productImageService.ReplaceProductImageAsync(
            id,
            userId,
            image);

        if (!result.Success)
        {
            return BadRequest(new
            {
                success = false,
                message = result.ErrorMessage
            });
        }

        return Json(new
        {
            success = true
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetPrimaryImage(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _productImageService.SetPrimaryImageAsync(
            id,
            userId);

        if (!result)
        {
            return NotFound();
        }

        return Json(new
        {
            success = true
        });
    }

    public async Task<IActionResult> DownloadReportPdf(
        string? search,
        string? sortOrder,
        int? categoryId,
        int? brandId,
        int? supplierId,
        decimal? minPrice,
        decimal? maxPrice,
        int? minQuantity,
        int? maxQuantity)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var products = await _productService.GetProductsAsync(
            userId,
            search,
            sortOrder,
            categoryId,
            brandId,
            supplierId,
            minPrice,
            maxPrice,
            minQuantity,
            maxQuantity,
            1,
            int.MaxValue);

        var pdf = _pdfService.GenerateProductsPdf(products);

        return File(
            pdf,
            "application/pdf",
            "Products-Report.pdf");
    }
}
