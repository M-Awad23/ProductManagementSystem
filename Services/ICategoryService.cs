using ProductManagementSystem.Models;

namespace ProductManagementSystem.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories(string userId);
        Category? GetCategoryById(int id, string userId);
        (bool Success, string? ErrorMessage) CreateCategory(string name, string? description, string userId);
        (bool Success, string? ErrorMessage) UpdateCategory(int id, string name, string? description, string userId);
        bool DeleteCategory(int id, string userId);
    }
}