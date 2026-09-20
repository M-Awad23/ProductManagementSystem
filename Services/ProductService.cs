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
    string? sortOrder,
    int? categoryId,
    int? brandId,
    int? supplierId,
    decimal? minPrice,
    decimal? maxPrice,
    int? minQuantity,
    int? maxQuantity,
    int page,
    int pageSize)
        {
            return await _productRepository.GetProductsAsync(
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

        }

        public async Task<int> GetProductCountAsync(
    string userId,
    string? search,
    int? categoryId,
    int? brandId,
    int? supplierId,
    decimal? minPrice,
    decimal? maxPrice,
    int? minQuantity,
    int? maxQuantity)
        {
            return await _productRepository.GetProductCountAsync(
                userId,
                search,
                categoryId,
                brandId,
                supplierId,
                minPrice,
                maxPrice,
                minQuantity,
                maxQuantity);
        }

        public async Task<Product?> GetByIdAsync(int id, string userId)
        {
            return await _productRepository.GetByIdAsync(id, userId);
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