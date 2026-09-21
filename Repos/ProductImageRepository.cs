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
        public ProductImage? GetProductImageById(int id, string userId)
        {
            return _context.ProductImages
                .Include(i => i.Product)
                .FirstOrDefault(i =>
                    i.Id == id &&
                    i.Product != null &&
                    i.Product.UserId == userId);
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
        public void DeleteProductImage(int id, string userId)
        {
            var productImage = _context.ProductImages
                .Include(i => i.Product)
                .FirstOrDefault(i =>
                    i.Id == id &&
                    i.Product != null &&
                    i.Product.UserId == userId);

            if (productImage != null)
            {
                _context.ProductImages.Remove(productImage);
                _context.SaveChanges();
            }
        }
    }
}
