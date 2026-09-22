using ProductManagementSystem.Models;
namespace ProductManagementSystem.Services
{
    public interface IBrandService
    {
        IEnumerable<Brand> GetAllBrands(string userId);
        Brand? GetBrandById(int id, string userId);
        void AddBrand(Brand brand);
        void UpdateBrand(Brand brand, string userId);
        void DeleteBrand(int id, string userId);
    }
}