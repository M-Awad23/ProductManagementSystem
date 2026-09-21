using ProductManagementSystem.Models;

namespace ProductManagementSystem.Services
{
    public interface IProductImageRepository
    {
        IEnumerable<ProductImage> GetAllProductImages();
        ProductImage? GetProductImageById(int id, string userId);
        void AddProductImage(ProductImage productImage);
        void UpdateProductImage(ProductImage productImage);
        void DeleteProductImage(int id, string userId);
    }
}