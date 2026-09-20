using ProductManagementSystem.Models;

namespace ProductManagementSystem.Services
{
    public interface IBrandService
    {
        IEnumerable<Brand> GetAllBrands();

        Brand? GetBrandById(int id);

        void AddBrand(Brand brand);

        void UpdateBrand(Brand brand);

        void DeleteBrand(int id);
    }
}