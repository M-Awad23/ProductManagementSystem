using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagementSystem.Models;
using ProductManagementSystem.Services;
using System.Security.Claims;




namespace ProductManagementSystem.Controllers;
[Authorize]
    public class ProductController : Controller

    {
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index(string search, string sortOrder)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var products = await _productService
            .GetProductsAsync(userId, search, sortOrder);

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView("_ProductList", products);
        }

        return View(products);
    }

    public IActionResult Create()
        {
            return PartialView("_ProductForm", new Product());
        }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
    [Bind("Name,Description,Price,Quantity,CategoryId,BrandId,SupplierId")] Product product)
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

        var products = await _productService
    .GetProductsAsync(userId, null, null);
        return PartialView("_ProductList", products);
    }

    public async Task<IActionResult> Details(int id)
        {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            {
                return NotFound();
            }

            return PartialView("_Details", product);
        }

        public async Task<IActionResult> Edit(int id)
        {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            {
                return NotFound();
            }

            return PartialView("_ProductForm", product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
    int id,
    [Bind("Id,Name,Description,Price,Quantity,CategoryId,BrandId,SupplierId")] Product product)
        {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        product.UserId = userId;
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

        await _productService.UpdateAsync(product);
        var products = await _productService
    .GetProductsAsync(userId, null, null);

        return PartialView("_ProductList", products);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _productService.DeleteAsync(id, userId);

        var products = await _productService
            .GetProductsAsync(userId, null, null);

        return PartialView("_ProductList", products);
    }

        
    }

