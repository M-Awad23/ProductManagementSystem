using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repos
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly AppDbContext _context;
        public SupplierRepository(AppDbContext context) => _context = context;

        public IEnumerable<Supplier> GetAllSuppliers(string userId) => _context.Suppliers.Where(s => s.UserId == userId).ToList();
        public Supplier? GetSupplierById(int id, string userId) => _context.Suppliers.FirstOrDefault(s => s.Id == id && s.UserId == userId);
        public void AddSupplier(Supplier supplier) { _context.Suppliers.Add(supplier); _context.SaveChanges(); }
        public void UpdateSupplier(Supplier supplier, string userId)
        {
            var existing = GetSupplierById(supplier.Id, userId);
            if (existing == null) return;
            existing.Name = supplier.Name;
            existing.Country = supplier.Country;
            _context.SaveChanges();
        }
        public void DeleteSupplier(int id, string userId)
        {
            var supplier = GetSupplierById(id, userId);
            if (supplier == null) return;
            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();
        }
    }
}