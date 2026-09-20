using ProductManagementSystem.Models;
using ProductManagementSystem.Repos;

namespace ProductManagementSystem.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public IEnumerable<Brand> GetAllBrands()
        {
            return _brandRepository.GetAllBrands();
        }

        public Brand? GetBrandById(int id)
        {
            return _brandRepository.GetBrandById(id);
        }

        public void AddBrand(Brand brand)
        {
            _brandRepository.AddBrand(brand);
        }

        public void UpdateBrand(Brand brand)
        {
            _brandRepository.UpdateBrand(brand);
        }

        public void DeleteBrand(int id)
        {
            _brandRepository.DeleteBrand(id);
        }
    }
}