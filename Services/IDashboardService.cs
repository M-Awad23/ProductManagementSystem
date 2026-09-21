using ProductManagementSystem.Models;

namespace ProductManagementSystem.Services
{
    public interface IDashboardService
    {
        Task<int> GetTotalProductsAsync(string userId);
        Task<IEnumerable<object>> GetProductsByCategoryAsync(string userId);
        Task<IEnumerable<object>> GetProductsByBrandAsync(string userId);
        Task<IEnumerable<object>> GetProductsBySupplierAsync(string userId);
        Task<List<Product>> GetRecentlyAddedProductsAsync(string userId, int count);
        Task<int> GetProductImageCountAsync(string userId);
        Task<int> GetCategoryCountAsync();
        Task<int> GetBrandCountAsync();
    }
}