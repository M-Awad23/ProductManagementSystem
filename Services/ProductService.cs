using Microsoft.AspNetCore.Http;
using ProductManagementSystem.Models;
using ProductManagementSystem.Repositories;

namespace ProductManagementSystem.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly ISupplierService _supplierService;
        private readonly IProductTagService _productTagService;
        private readonly IProductImageService _productImageService;

        public ProductService(
            IProductRepository productRepository,
            ICategoryService categoryService,
            IBrandService brandService,
            ISupplierService supplierService,
            IProductTagService productTagService,
            IProductImageService productImageService)
        {
            _productRepository = productRepository;
            _categoryService = categoryService;
            _brandService = brandService;
            _supplierService = supplierService;
            _productTagService = productTagService;
            _productImageService = productImageService;
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
                userId, search, sortOrder, categoryId, brandId, supplierId,
                minPrice, maxPrice, minQuantity, maxQuantity, page, pageSize);
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
                userId, search, categoryId, brandId, supplierId,
                minPrice, maxPrice, minQuantity, maxQuantity);
        }

        public async Task<Product?> GetByIdAsync(int id, string userId)
        {
            return await _productRepository.GetByIdAsync(id, userId);
        }

        public async Task<List<Product>> GetUserProductsAsync(string userId)
        {
            return await _productRepository.GetUserProductsAsync(userId);
        }

        public async Task<List<Product>> GetDeletedProductsAsync(string userId)
        {
            return await _productRepository.GetDeletedProductsAsync(userId);
        }

        public async Task<(bool Success, string? ErrorMessage)> CreateProductAsync(
            Product product,
            string userId,
            IEnumerable<IFormFile>? images,
            List<int>? tagIds)
        {
            product.UserId = userId;
            product.CreatedAt = DateTime.Now;

            var catalogError = ValidateCatalogOwnership(product, userId);
            if (catalogError != null)
            {
                return (false, catalogError);
            }

            await _productRepository.AddAsync(product);

            if (tagIds != null && tagIds.Any())
            {
                _productTagService.AddProductTags(product.Id, tagIds, userId);
            }

            var uploadResult = await _productImageService.AddProductImagesAsync(
                product.Id,
                userId,
                images);

            if (!uploadResult.Success)
            {
                return (false, uploadResult.ErrorMessage);
            }

            return (true, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdateProductAsync(
            Product product,
            string userId,
            IEnumerable<IFormFile>? images,
            List<int>? tagIds)
        {
            var existingProduct = await _productRepository.GetByIdAsync(
                product.Id,
                userId);

            if (existingProduct == null)
            {
                return (false, "Product not found.");
            }

            var catalogError = ValidateCatalogOwnership(product, userId);
            if (catalogError != null)
            {
                return (false, catalogError);
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Quantity = product.Quantity;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.BrandId = product.BrandId;
            existingProduct.SupplierId = product.SupplierId;

            await _productRepository.UpdateAsync(existingProduct);

            _productTagService.ReplaceProductTags(
                existingProduct.Id,
                tagIds ?? new List<int>(),
                userId);

            var uploadResult = await _productImageService.AddProductImagesAsync(
                existingProduct.Id,
                userId,
                images);

            if (!uploadResult.Success)
            {
                return (false, uploadResult.ErrorMessage);
            }

            return (true, null);
        }

        public async Task UpdateAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var product = await _productRepository.GetByIdAsync(id, userId);

            if (product == null)
                return false;

            await _productRepository.DeleteAsync(id, userId);
            return true;
        }

        public async Task<bool> RestoreAsync(int id, string userId)
        {
            var products = await _productRepository.GetDeletedProductsAsync(userId);

            if (!products.Any(p => p.Id == id))
                return false;

            await _productRepository.RestoreAsync(id, userId);
            return true;
        }

        public async Task<bool> PermanentDeleteAsync(int id, string userId)
        {
            var products = await _productRepository.GetDeletedProductsAsync(userId);

            if (!products.Any(p => p.Id == id))
                return false;

            await _productRepository.PermanentDeleteAsync(id, userId);
            return true;
        }

        private string? ValidateCatalogOwnership(Product product, string userId)
        {
            if (product.CategoryId.HasValue &&
                _categoryService.GetCategoryById(product.CategoryId.Value, userId) == null)
            {
                return "Invalid category.";
            }

            if (product.BrandId.HasValue &&
                _brandService.GetBrandById(product.BrandId.Value, userId) == null)
            {
                return "Invalid brand.";
            }

            if (product.SupplierId.HasValue &&
                _supplierService.GetSupplierById(product.SupplierId.Value, userId) == null)
            {
                return "Invalid supplier.";
            }

            return null;
        }
    }
}