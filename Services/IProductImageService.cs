using ProductManagementSystem.Models;

namespace ProductManagementSystem.Services
{
    public interface IProductImageService
    {
        IEnumerable<ProductImage> GetAllProductImages();

        void SetPrimaryImage(int imageId, string userId);
        ProductImage? GetProductImageById(int id, string userId);
        void AddProductImage(ProductImage productImage);
        void UpdateProductImage(ProductImage productImage);
        void DeleteProductImage(int id, string userId);
    }
}