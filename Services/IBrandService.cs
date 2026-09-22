using ProductManagementSystem.Models;

namespace ProductManagementSystem.Services
{
    public interface IBrandService
    {
        IEnumerable<Brand> GetAllBrands(string userId);
        Brand? GetBrandById(int id, string userId);
        (bool Success, string? ErrorMessage) CreateBrand(string name, string userId);
        (bool Success, string? ErrorMessage) UpdateBrand(int id, string name, string userId);
        bool DeleteBrand(int id, string userId);
    }
}