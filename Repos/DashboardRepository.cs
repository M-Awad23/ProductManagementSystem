using Microsoft.EntityFrameworkCore;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repos
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalProductsAsync(string userId)
        {
            return await _context.Products
                .CountAsync(p =>
                    p.UserId == userId &&
                    !p.IsDeleted);
        }

        public async Task<IEnumerable<object>> GetProductsByCategoryAsync(string userId)
        {
            return await _context.Products
                .Where(p =>
                    p.UserId == userId &&
                    !p.IsDeleted)
                .GroupBy(p => p.Category != null
                    ? p.Category.Name
                    : "Uncategorized")
                .Select(g => new
                {
                    Name = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetProductsByBrandAsync(string userId)
        {
            return await _context.Products
                .Where(p =>
                    p.UserId == userId &&
                    !p.IsDeleted)
                .GroupBy(p => p.Brand != null
                    ? p.Brand.Name
                    : "No Brand")
                .Select(g => new
                {
                    Name = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetProductsBySupplierAsync(string userId)
        {
            return await _context.Products
                .Where(p =>
                    p.UserId == userId &&
                    !p.IsDeleted)
                .GroupBy(p => p.Supplier != null
                    ? p.Supplier.Name
                    : "No Supplier")
                .Select(g => new
                {
                    Name = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();
        }

        public async Task<List<Product>> GetRecentlyAddedProductsAsync(
            string userId,
            int count)
        {
            return await _context.Products
                .Where(p =>
                    p.UserId == userId &&
                    !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetProductImageCountAsync(string userId)
        {
            return await _context.ProductImages
                .CountAsync(i =>
                    i.Product != null &&
                    i.Product.UserId == userId &&
                    !i.Product.IsDeleted);
        }

        public async Task<int> GetCategoryCountAsync()
        {
            return await _context.Categories.CountAsync();
        }

        public async Task<int> GetBrandCountAsync()
        {
            return await _context.Brands.CountAsync();
        }
    }
}