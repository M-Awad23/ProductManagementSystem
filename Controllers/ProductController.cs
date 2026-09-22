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
    private readonly IProductTagService _productTagService;

    public ProductController(
        IProductImageService productImageService,
        IProductService productService,
        ICategoryService categoryService,
        IBrandService brandService,
        ISupplierService supplierService,
        ITagService tagService,
        IProductTagService productTagService,
        IPdfService pdfService)
    {
        _productImageService = productImageService;
        _productService = productService;
        _categoryService = categoryService;
        _brandService = brandService;
        _supplierService = supplierService;
        _tagService = tagService;
        _productTagService = productTagService;
        _pdfService = pdfService;
    }


    private void ValidateCatalogOwnership(Product product, string userId)
    {
        if (product.CategoryId.HasValue && _categoryService.GetCategoryById(product.CategoryId.Value, userId) == null)
            ModelState.AddModelError(nameof(Product.CategoryId), "Invalid category.");

        if (product.BrandId.HasValue && _brandService.GetBrandById(product.BrandId.Value, userId) == null)
            ModelState.AddModelError(nameof(Product.BrandId), "Invalid brand.");

        if (product.SupplierId.HasValue && _supplierService.GetSupplierById(product.SupplierId.Value, userId) == null)
            ModelState.AddModelError(nameof(Product.SupplierId), "Invalid supplier.");
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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        product.UserId = userId;
        product.CreatedAt = DateTime.Now;

        ModelState.Remove(nameof(Product.UserId));
        ModelState.Remove(nameof(Product.User));
        ModelState.Remove(nameof(Product.ProductTags));
        ModelState.Remove(nameof(Product.ProductImages));

        ValidateCatalogOwnership(product, userId);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _productService.AddAsync(product);

        if (tagIds != null && tagIds.Any())
        {
            _productTagService.AddProductTags(
                product.Id,
                tagIds,
                userId);
        }

        var uploadResult = await _productImageService.AddProductImagesAsync(
            product.Id,
            userId,
            images);

        if (!uploadResult.Success)
        {
            return BadRequest(new
            {
                success = false,
                message = uploadResult.ErrorMessage
            });
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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var existingProduct = await _productService.GetByIdAsync(
            id,
            userId);

        if (existingProduct == null)
        {
            return NotFound();
        }

        product.UserId = userId;
        product.CreatedAt = existingProduct.CreatedAt;

        ModelState.Remove(nameof(Product.UserId));
        ModelState.Remove(nameof(Product.User));

        ValidateCatalogOwnership(product, userId);

        if (id != product.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.Quantity = product.Quantity;
        existingProduct.CategoryId = product.CategoryId;
        existingProduct.BrandId = product.BrandId;
        existingProduct.SupplierId = product.SupplierId;

        await _productService.UpdateAsync(existingProduct);

        _productTagService.ReplaceProductTags(
            product.Id,
            tagIds ?? new List<int>(),
            userId);

        var uploadResult = await _productImageService.AddProductImagesAsync(
            product.Id,
            userId,
            images);

        if (!uploadResult.Success)
        {
            return BadRequest(new
            {
                success = false,
                message = uploadResult.ErrorMessage
            });
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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var product = await _productService.GetByIdAsync(id, userId);

        if (product == null)
        {
            return NotFound();
        }

        await _productService.DeleteAsync(id, userId);

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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var products = await _productService.GetDeletedProductsAsync(userId);

        if (!products.Any(p => p.Id == id))
        {
            return NotFound();
        }

        await _productService.RestoreAsync(id, userId);

        return RedirectToAction(nameof(Deleted));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PermanentDelete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var products = await _productService.GetDeletedProductsAsync(userId);
        var product = products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        await _productService.PermanentDeleteAsync(id, userId);

        return RedirectToAction(nameof(Deleted));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteImage(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var image = _productImageService.GetProductImageById(
            id,
            userId);

        if (image == null)
        {
            return NotFound();
        }

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
