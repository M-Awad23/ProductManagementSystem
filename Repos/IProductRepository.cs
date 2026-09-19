using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync(
            string userId,
            string? search,
            string? sortOrder);

        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id, string userId);
    }
}