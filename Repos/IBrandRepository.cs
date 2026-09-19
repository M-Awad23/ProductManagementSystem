using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repos
{
    public interface IBrandRepository
    {
        IEnumerable<Brand> GetAllBrands();
        Brand GetBrandById(int id);
        void AddBrand(Brand brand);
        void UpdateBrand(Brand brand);
        void DeleteBrand(int id);

    }
}
    