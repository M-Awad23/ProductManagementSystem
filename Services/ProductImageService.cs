using ProductManagementSystem.Models;
using ProductManagementSystem.Repos;

namespace ProductManagementSystem.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _productImageRepository;

        public ProductImageService(IProductImageRepository productImageRepository)
        {
            _productImageRepository = productImageRepository;
        }

        public IEnumerable<ProductImage> GetAllProductImages()
        {
            return _productImageRepository.GetAllProductImages();
        }

        public ProductImage? GetProductImageById(
    int id,
    string userId)
        {
            return _productImageRepository
                .GetProductImageById(id, userId);
        }

        public void AddProductImage(ProductImage productImage)
        {
            _productImageRepository.AddProductImage(productImage);
        }

        public void UpdateProductImage(ProductImage productImage)
        {
            _productImageRepository.UpdateProductImage(productImage);
        }

        public void DeleteProductImage(
    int id,
    string userId)
        {
            _productImageRepository
                .DeleteProductImage(id, userId);
        }

        public void SetPrimaryImage(int imageId, string userId)
        {
            _productImageRepository.SetPrimaryImage(
                imageId,
                userId);
        }
    }
}   