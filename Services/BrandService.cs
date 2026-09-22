using ProductManagementSystem.Models;
using ProductManagementSystem.Repos;

namespace ProductManagementSystem.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        public BrandService(IBrandRepository brandRepository) => _brandRepository = brandRepository;
        public IEnumerable<Brand> GetAllBrands(string userId) => _brandRepository.GetAllBrands(userId);
        public Brand? GetBrandById(int id, string userId) => _brandRepository.GetBrandById(id, userId);
        public void AddBrand(Brand brand) => _brandRepository.AddBrand(brand);
        public void UpdateBrand(Brand brand, string userId) => _brandRepository.UpdateBrand(brand, userId);
        public void DeleteBrand(int id, string userId) => _brandRepository.DeleteBrand(id, userId);
    }
}