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
        Task AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(int id, string userId);
    }
}