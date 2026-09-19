using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repos
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
        Category? GetCategoryById(int id);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int id);
    }
}