using ProductManagementSystem.Models;
using ProductManagementSystem.Repos;

namespace ProductManagementSystem.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<int> GetTotalProductsAsync(string userId)
        {
            return await _dashboardRepository.GetTotalProductsAsync(userId);
        }

        public async Task<IEnumerable<object>> GetProductsByCategoryAsync(string userId)
        {
            return await _dashboardRepository.GetProductsByCategoryAsync(userId);
        }

        public async Task<IEnumerable<object>> GetProductsByBrandAsync(string userId)
        {
            return await _dashboardRepository.GetProductsByBrandAsync(userId);
        }

        public async Task<IEnumerable<object>> GetProductsBySupplierAsync(string userId)
        {
            return await _dashboardRepository.GetProductsBySupplierAsync(userId);
        }

        public async Task<List<Product>> GetRecentlyAddedProductsAsync(
            string userId,
            int count)
        {
            return await _dashboardRepository.GetRecentlyAddedProductsAsync(
                userId,
                count);
        }

        public async Task<int> GetProductImageCountAsync(string userId)
        {
            return await _dashboardRepository.GetProductImageCountAsync(userId);
        }

        public async Task<int> GetCategoryCountAsync()
        {
            return await _dashboardRepository.GetCategoryCountAsync();
        }

        public async Task<int> GetBrandCountAsync()
        {
            return await _dashboardRepository.GetBrandCountAsync();
        }
    }
}