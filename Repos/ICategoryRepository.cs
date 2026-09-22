using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repos
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories(string userId);
        Category? GetCategoryById(int id, string userId);
        void AddCategory(Category category);
        void UpdateCategory(Category category, string userId);
        void DeleteCategory(int id, string userId);
    }
}