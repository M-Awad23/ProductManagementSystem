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

        public IEnumerable<Brand> GetAllBrands(string userId) =>
            _brandRepository.GetAllBrands(userId);

        public Brand? GetBrandById(int id, string userId) =>
            _brandRepository.GetBrandById(id, userId);

        public (bool Success, string? ErrorMessage) CreateBrand(
            string name,
            string userId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Brand name is required.");

            _brandRepository.AddBrand(new Brand
            {
                Name = name,
                UserId = userId
            });

            return (true, null);
        }

        public (bool Success, string? ErrorMessage) UpdateBrand(
            int id,
            string name,
            string userId)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(name))
                return (false, "Invalid brand.");

            var brand = _brandRepository.GetBrandById(id, userId);

            if (brand == null)
                return (false, "Brand not found.");

            brand.Name = name;
            _brandRepository.UpdateBrand(brand, userId);

            return (true, null);
        }

        public bool DeleteBrand(int id, string userId)
        {
            if (_brandRepository.GetBrandById(id, userId) == null)
                return false;

            _brandRepository.DeleteBrand(id, userId);
            return true;
        }
    }
}