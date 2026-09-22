using ProductManagementSystem.Models;
using ProductManagementSystem.Repos;

namespace ProductManagementSystem.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        public SupplierService(ISupplierRepository supplierRepository) => _supplierRepository = supplierRepository;
        public IEnumerable<Supplier> GetAllSuppliers(string userId) => _supplierRepository.GetAllSuppliers(userId);
        public Supplier? GetSupplierById(int id, string userId) => _supplierRepository.GetSupplierById(id, userId);
        public void AddSupplier(Supplier supplier) => _supplierRepository.AddSupplier(supplier);
        public void UpdateSupplier(Supplier supplier, string userId) => _supplierRepository.UpdateSupplier(supplier, userId);
        public void DeleteSupplier(int id, string userId) => _supplierRepository.DeleteSupplier(id, userId);
    }
}