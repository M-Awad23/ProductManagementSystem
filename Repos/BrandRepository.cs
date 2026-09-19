using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repos
{
    public class BrandRepository : IBrandRepository
    {
        private readonly AppDbContext _context;
        public BrandRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Brand> GetAllBrands()
        {
            return _context.Brands.ToList();
        }
        public Brand GetBrandById(int id)
        {
            return _context.Brands.Find(id);
        }
        public void AddBrand(Brand brand)
        {
            _context.Brands.Add(brand);
            _context.SaveChanges();
        }
        public void UpdateBrand(Brand brand)
        {
            _context.Brands.Update(brand);
            _context.SaveChanges();
        }
        public void DeleteBrand(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
                _context.SaveChanges();
            }
        }


    }
}
