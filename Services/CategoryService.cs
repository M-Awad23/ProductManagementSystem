using ProductManagementSystem.Models;
using ProductManagementSystem.Repos;

namespace ProductManagementSystem.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository) => _categoryRepository = categoryRepository;
        public IEnumerable<Category> GetAllCategories(string userId) => _categoryRepository.GetAllCategories(userId);
        public Category? GetCategoryById(int id, string userId) => _categoryRepository.GetCategoryById(id, userId);
        public void AddCategory(Category category) => _categoryRepository.AddCategory(category);
        public void UpdateCategory(Category category, string userId) => _categoryRepository.UpdateCategory(category, userId);
        public void DeleteCategory(int id, string userId) => _categoryRepository.DeleteCategory(id, userId);
    }
}