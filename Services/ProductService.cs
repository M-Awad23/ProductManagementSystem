using ProductManagementSystem.Models;
using ProductManagementSystem.Repositories;

namespace ProductManagementSystem.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> GetProductsAsync(
            string userId,
            string? search,
            string? sortOrder)
        {
            return await _productRepository
                .GetProductsAsync(userId, search, sortOrder);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Product product)
        {
            await _productRepository.AddAsync(product);
        }

        public async Task UpdateAsync(Product product) 
        {
            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id, string userId)
        {
            await _productRepository.DeleteAsync(id, userId);
        }
    }
}