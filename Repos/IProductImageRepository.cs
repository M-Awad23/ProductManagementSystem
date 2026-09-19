using ProductManagementSystem.Models;
 
namespace ProductManagementSystem.Repos
{
    public interface IProductImageRepository
    {
        IEnumerable<ProductImage> GetAllProductImages();
        ProductImage? GetProductImageById(int id);
        void AddProductImage(ProductImage productImage);
        void UpdateProductImage(ProductImage productImage);
        void DeleteProductImage(int id);    
    }
}
