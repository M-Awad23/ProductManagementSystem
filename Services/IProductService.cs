using ProductManagementSystem.Models;

namespace ProductManagementSystem.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync(
            string userId,
            string? search,
            string? sortOrder);

        Task<Product?> GetByIdAsync(int id, string userId);
        Task AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(int id, string userId);
    }
}