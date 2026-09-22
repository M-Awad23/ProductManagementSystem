using Microsoft.AspNetCore.Http;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync(
            string userId,
            string? search,
            string? sortOrder,
            int? categoryId,
            int? brandId,
            int? supplierId,
            decimal? minPrice,
            decimal? maxPrice,
            int? minQuantity,
            int? maxQuantity,
            int page,
            int pageSize);

        Task<int> GetProductCountAsync(
            string userId,
            string? search,
            int? categoryId,
            int? brandId,
            int? supplierId,
            decimal? minPrice,
            decimal? maxPrice,
            int? minQuantity,
            int? maxQuantity);

        Task<Product?> GetByIdAsync(int id, string userId);
        Task<List<Product>> GetUserProductsAsync(string userId);
        Task<List<Product>> GetDeletedProductsAsync(string userId);

        Task<(bool Success, string? ErrorMessage)> CreateProductAsync(
            Product product,
            string userId,
            IEnumerable<IFormFile>? images,
            List<int>? tagIds);

        Task<(bool Success, string? ErrorMessage)> UpdateProductAsync(
            Product product,
            string userId,
            IEnumerable<IFormFile>? images,
            List<int>? tagIds);

        Task UpdateAsync(Product product);
        Task DeleteAsync(int id, string userId);
        Task RestoreAsync(int id, string userId);
        Task PermanentDeleteAsync(int id, string userId);
    }
}