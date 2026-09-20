using ProductManagementSystem.Models;

namespace ProductManagementSystem.Services
{
    public interface IProductImageService
    {
        IEnumerable<ProductImage> GetAllProductImages();
        ProductImage? GetProductImageById(int id);
        void AddProductImage(ProductImage productImage);
        void UpdateProductImage(ProductImage productImage);
        void DeleteProductImage(int id);
    }
}