using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repos
{
    public interface ISupplierRepository
    {
        IEnumerable<Supplier> GetAllSuppliers();

        Supplier? GetSupplierById(int id);

        void AddSupplier(Supplier supplier);

        void UpdateSupplier(Supplier supplier);

        void DeleteSupplier(int id);
    }
}