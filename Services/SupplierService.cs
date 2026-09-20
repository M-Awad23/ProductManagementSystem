using ProductManagementSystem.Models;
using ProductManagementSystem.Repos;

namespace ProductManagementSystem.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public IEnumerable<Supplier> GetAllSuppliers()
        {
            return _supplierRepository.GetAllSuppliers();
        }

        public Supplier? GetSupplierById(int id)
        {
            return _supplierRepository.GetSupplierById(id);
        }

        public void AddSupplier(Supplier supplier)
        {
            _supplierRepository.AddSupplier(supplier);
        }

        public void UpdateSupplier(Supplier supplier)
        {
            _supplierRepository.UpdateSupplier(supplier);
        }

        public void DeleteSupplier(int id)
        {
            _supplierRepository.DeleteSupplier(id);
        }
    }
}