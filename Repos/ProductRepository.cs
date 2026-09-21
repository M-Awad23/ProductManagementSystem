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
            var products = _context.Products
    .Include(p => p.ProductImages)
    .Where(p => p.UserId == userId && !p.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                products = products.Where(p =>
                    p.Name.Contains(search) ||
                    p.Description.Contains(search));
            }

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            if (brandId.HasValue)
            {
                products = products.Where(p => p.BrandId == brandId.Value);
            }

            if (supplierId.HasValue)
            {
                products = products.Where(p => p.SupplierId == supplierId.Value);
            }

            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }

            if (minQuantity.HasValue)
            {
                products = products.Where(p => p.Quantity >= minQuantity.Value);
            }

            if (maxQuantity.HasValue)
            {
                products = products.Where(p => p.Quantity <= maxQuantity.Value);
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

                case "quantity_asc":
                    products = products.OrderBy(p => p.Quantity);
                    break;

                case "quantity_desc":
                    products = products.OrderByDescending(p => p.Quantity);
                    break;

                case "oldest":
                    products = products.OrderBy(p => p.CreatedAt);
                    break;

                default:
                    products = products.OrderByDescending(p => p.CreatedAt);
                    break;
            }

            if (page < 1)
                page = 1;

            if (pageSize < 1)
                pageSize = 8;

            return await products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
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
            var products = _context.Products
                .Where(p => p.UserId == userId && !p.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                products = products.Where(p =>
                    p.Name.Contains(search) ||
                    p.Description.Contains(search));
            }

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            if (brandId.HasValue)
            {
                products = products.Where(p => p.BrandId == brandId.Value);
            }

            if (supplierId.HasValue)
            {
                products = products.Where(p => p.SupplierId == supplierId.Value);
            }

            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }

            if (minQuantity.HasValue)
            {
                products = products.Where(p => p.Quantity >= minQuantity.Value);
            }

            if (maxQuantity.HasValue)
            {
                products = products.Where(p => p.Quantity <= maxQuantity.Value);
            }

            return await products.CountAsync();
        }

        public async Task<List<Product>> GetUserProductsAsync(string userId)
        {
            return await _context.Products
                .Where(p => p.UserId == userId && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id, string userId)
        {
            return await _context.Products
     .Include(p => p.ProductImages)
     .Include(p => p.ProductTags)
         .ThenInclude(pt => pt.Tag)
     .FirstOrDefaultAsync(p =>
         p.Id == id &&
         p.UserId == userId &&
         !p.IsDeleted);



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

        public async Task<List<Product>> GetDeletedProductsAsync(string userId)
        {
            return await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductTags)
                    .ThenInclude(pt => pt.Tag)
                .Where(p =>
                    p.UserId == userId &&
                    p.IsDeleted)
                .OrderByDescending(p => p.DeletedAt)
                .ToListAsync();
        }

        public async Task RestoreAsync(int id, string userId)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    p.UserId == userId &&
                    p.IsDeleted);

            if (product != null)
            {
                product.IsDeleted = false;
                product.DeletedAt = null;
                await _context.SaveChangesAsync();
            }
        }

        public async Task PermanentDeleteAsync(int id, string userId)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductTags)
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    p.UserId == userId &&
                    p.IsDeleted);

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    p.UserId == userId &&
                    !p.IsDeleted);

            if (product != null)
            {
                product.IsDeleted = true;
                product.DeletedAt = DateTime.Now;

                await _context.SaveChangesAsync();
            }
        }
    }
}