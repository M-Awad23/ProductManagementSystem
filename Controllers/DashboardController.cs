using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagementSystem.Services;
using System.Security.Claims;

namespace ProductManagementSystem.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        ViewBag.TotalProducts =
            await _dashboardService.GetTotalProductsAsync(userId);

        ViewBag.ProductsByCategory =
            await _dashboardService.GetProductsByCategoryAsync(userId);

        ViewBag.ProductsByBrand =
            await _dashboardService.GetProductsByBrandAsync(userId);

        ViewBag.ProductsBySupplier =
            await _dashboardService.GetProductsBySupplierAsync(userId);

        ViewBag.RecentProducts =
            await _dashboardService.GetRecentlyAddedProductsAsync(
                userId,
                5);

        ViewBag.ProductImageCount =
            await _dashboardService.GetProductImageCountAsync(userId);

        ViewBag.CategoryCount =
            await _dashboardService.GetCategoryCountAsync();

        ViewBag.BrandCount =
            await _dashboardService.GetBrandCountAsync();

        return View();
    }
}   