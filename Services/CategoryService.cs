using ProductManagementSystem.Models;
using ProductManagementSystem.Repos;

namespace ProductManagementSystem.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<Category> GetAllCategories(string userId) =>
            _categoryRepository.GetAllCategories(userId);

        public Category? GetCategoryById(int id, string userId) =>
            _categoryRepository.GetCategoryById(id, userId);

        public (bool Success, string? ErrorMessage) CreateCategory(
            string name,
            string? description,
            string userId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Category name is required.");

            _categoryRepository.AddCategory(new Category
            {
                Name = name,
                Description = description ?? string.Empty,
                UserId = userId
            });

            return (true, null);
        }

        public (bool Success, string? ErrorMessage) UpdateCategory(
            int id,
            string name,
            string? description,
            string userId)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(name))
                return (false, "Invalid category.");

            var category = _categoryRepository.GetCategoryById(id, userId);

            if (category == null)
                return (false, "Category not found.");

            category.Name = name;
            category.Description = description ?? string.Empty;

            _categoryRepository.UpdateCategory(category, userId);

            return (true, null);
        }

        public bool DeleteCategory(int id, string userId)
        {
            if (_categoryRepository.GetCategoryById(id, userId) == null)
                return false;

            _categoryRepository.DeleteCategory(id, userId);
            return true;
        }
    }
}