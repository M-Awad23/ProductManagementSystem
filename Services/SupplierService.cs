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

        public IEnumerable<Supplier> GetAllSuppliers(string userId) =>
            _supplierRepository.GetAllSuppliers(userId);

        public Supplier? GetSupplierById(int id, string userId) =>
            _supplierRepository.GetSupplierById(id, userId);

        public (bool Success, string? ErrorMessage) CreateSupplier(
            string name,
            string? country,
            string userId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Supplier name is required.");

            _supplierRepository.AddSupplier(new Supplier
            {
                Name = name,
                Country = country ?? string.Empty,
                UserId = userId
            });

            return (true, null);
        }

        public (bool Success, string? ErrorMessage) UpdateSupplier(
            int id,
            string name,
            string? country,
            string userId)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(name))
                return (false, "Invalid supplier.");

            var supplier = _supplierRepository.GetSupplierById(id, userId);

            if (supplier == null)
                return (false, "Supplier not found.");

            supplier.Name = name;
            supplier.Country = country ?? string.Empty;

            _supplierRepository.UpdateSupplier(supplier, userId);

            return (true, null);
        }

        public bool DeleteSupplier(int id, string userId)
        {
            if (_supplierRepository.GetSupplierById(id, userId) == null)
                return false;

            _supplierRepository.DeleteSupplier(id, userId);
            return true;
        }
    }
}