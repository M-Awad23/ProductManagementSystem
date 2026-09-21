using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagementSystem.Models;
using ProductManagementSystem.Services;
using System.Security.Claims;

namespace ProductManagementSystem.Controllers;

[Authorize]
public class ProductController : Controller
{

	private static readonly string[] AllowedImageExtensions =
{
	".jpg",
	".jpeg",
	".png",
	".gif"
};

	private const long MaxProductImageSize = 5 * 1024 * 1024;
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
		_pdfService = pdfService;
		_productService = productService;
		_categoryService = categoryService;
		_brandService = brandService;
		_supplierService = supplierService;
		_tagService = tagService;
		_productImageService = productImageService;
	}

	private void LoadProductFormData()
	{
		ViewBag.Categories = _categoryService.GetAllCategories();
		ViewBag.Brands = _brandService.GetAllBrands();
		ViewBag.Suppliers = _supplierService.GetAllSuppliers();
		ViewBag.Tags = _tagService.GetAllTags();
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

		ViewBag.Categories = _categoryService.GetAllCategories();
		ViewBag.Brands = _brandService.GetAllBrands();
		ViewBag.Suppliers = _supplierService.GetAllSuppliers();

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
	 List<IFormFile>? images)
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

		product.UserId = userId;
		product.CreatedAt = DateTime.Now;

		ModelState.Remove(nameof(Product.UserId));
		ModelState.Remove(nameof(Product.User));

		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		await _productService.AddAsync(product);

		if (images != null)
		{
			var uploadPath = Path.Combine(
				Directory.GetCurrentDirectory(),
				"wwwroot",
				"uploads",
				"products");

			Directory.CreateDirectory(uploadPath);

			foreach (var image in images)
			{
                if (image.Length == 0)
                {
                    continue;
                }

                var extension =
                    Path.GetExtension(image.FileName)
                        .ToLowerInvariant();

                if (!AllowedImageExtensions.Contains(extension))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Only JPG, JPEG, PNG, and GIF images are allowed."
                    });
                }

                if (image.Length > MaxProductImageSize)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Product images must be 5 MB or smaller."
                    });
                }

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadPath, fileName);

                using var stream = new FileStream(
                    filePath,
                    FileMode.Create);

                await image.CopyToAsync(stream);

                var productImage = new ProductImage
                {
                    ImageUrl = $"/uploads/products/{fileName}",
                    ProductId = product.Id
                };

                _productImageService.AddProductImage(productImage);
            }
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
	 List<IFormFile>? images)
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
		if (images != null)
		{
			var uploadPath = Path.Combine(
				Directory.GetCurrentDirectory(),
				"wwwroot",
				"uploads",
				"products");

			Directory.CreateDirectory(uploadPath);

			foreach (var image in images)
			{
                if (image.Length == 0)
                {
                    continue;
                }

                var extension =
                    Path.GetExtension(image.FileName)
                        .ToLowerInvariant();

                if (!AllowedImageExtensions.Contains(extension))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Only JPG, JPEG, PNG, and GIF images are allowed."
                    });
                }

                if (image.Length > MaxProductImageSize)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Product images must be 5 MB or smaller."
                    });
                }

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadPath, fileName);

                using var stream = new FileStream(
                    filePath,
                    FileMode.Create);

                await image.CopyToAsync(stream);

                var productImage = new ProductImage
                {
                    ImageUrl = $"/uploads/products/{fileName}",
                    ProductId = product.Id
                };

                _productImageService.AddProductImage(productImage);
            }
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteImage(int id)
    {
        var userId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        var image =
            _productImageService.GetProductImageById(
                id,
                userId);

        if (image == null)
        {
            return NotFound();
        }

        var filePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            image.ImageUrl.TrimStart('/').Replace(
                "/",
                Path.DirectorySeparatorChar.ToString()));

        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }

        _productImageService.DeleteProductImage(
            id,
            userId);

        return Json(new
        {
            success = true
        });
    }
}