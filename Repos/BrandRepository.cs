using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repos
{
    public class BrandRepository : IBrandRepository
    {
        private readonly AppDbContext _context;
        public BrandRepository(AppDbContext context) => _context = context;

        public IEnumerable<Brand> GetAllBrands(string userId) => _context.Brands.Where(b => b.UserId == userId).ToList();
        public Brand? GetBrandById(int id, string userId) => _context.Brands.FirstOrDefault(b => b.Id == id && b.UserId == userId);
        public void AddBrand(Brand brand) { _context.Brands.Add(brand); _context.SaveChanges(); }
        public void UpdateBrand(Brand brand, string userId)
        {
            var existing = GetBrandById(brand.Id, userId);
            if (existing == null) return;
            existing.Name = brand.Name;
            _context.SaveChanges();
        }
        public void DeleteBrand(int id, string userId)
        {
            var brand = GetBrandById(id, userId);
            if (brand == null) return;
            _context.Brands.Remove(brand);
            _context.SaveChanges();
        }
    }
}