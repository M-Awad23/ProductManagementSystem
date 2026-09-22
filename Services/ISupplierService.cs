using ProductManagementSystem.Models;

namespace ProductManagementSystem.Services
{
    public interface ISupplierService
    {
        IEnumerable<Supplier> GetAllSuppliers(string userId);
        Supplier? GetSupplierById(int id, string userId);
        (bool Success, string? ErrorMessage) CreateSupplier(string name, string? country, string userId);
        (bool Success, string? ErrorMessage) UpdateSupplier(int id, string name, string? country, string userId);
        bool DeleteSupplier(int id, string userId);
    }
}