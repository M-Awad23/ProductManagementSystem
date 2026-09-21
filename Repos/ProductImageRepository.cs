using Microsoft.EntityFrameworkCore;
using ProductManagementSystem.Models;
using ProductManagementSystem.Services;
namespace ProductManagementSystem.Repos
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly AppDbContext _context;
        public ProductImageRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<ProductImage> GetAllProductImages()
        {
            return _context.ProductImages.ToList();
        }
        public ProductImage? GetProductImageById(int id)
        {
            return _context.ProductImages.Find(id);
        }
        public void AddProductImage(ProductImage productImage)
        {
            _context.ProductImages.Add(productImage);
            _context.SaveChanges();
        }
        public void UpdateProductImage(ProductImage productImage)
        {
            _context.ProductImages.Update(productImage);
            _context.SaveChanges();
        }
        public void DeleteProductImage(int id)
        {
            var productImage = _context.ProductImages.Find(id);
            if (productImage != null)
            {
                _context.ProductImages.Remove(productImage);
                _context.SaveChanges();
            }
        }
    }
}
