using Microsoft.EntityFrameworkCore;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetProductsAsync(
    string userId,
    string? search,
    string? sortOrder)
        {
            var products = _context.Products
                .Where(p => p.UserId == userId);

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p =>
                    p.Name.Contains(search) ||
                    p.Description.Contains(search));
            }

            switch (sortOrder)
            {
                case "name_asc":
                    products = products.OrderBy(p => p.Name);
                    break;

                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;

                case "price_asc":
                    products = products.OrderBy(p => p.Price);
                    break;

                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;

                default:
                    products = products.OrderByDescending(p => p.CreatedAt);
                    break;
            }

            return await products.ToListAsync();
        }

        public async Task<List<Product>> GetUserProductsAsync(string userId)
        {
            return await _context.Products
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id, string userId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    p.UserId == userId);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    p.UserId == userId);

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}