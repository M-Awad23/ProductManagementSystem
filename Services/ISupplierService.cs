using ProductManagementSystem.Models;
namespace ProductManagementSystem.Services
{
    public interface ISupplierService
    {
        IEnumerable<Supplier> GetAllSuppliers(string userId);
        Supplier? GetSupplierById(int id, string userId);
        void AddSupplier(Supplier supplier);
        void UpdateSupplier(Supplier supplier, string userId);
        void DeleteSupplier(int id, string userId);
    }
}