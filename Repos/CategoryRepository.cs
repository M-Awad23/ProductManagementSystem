using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repos
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context) => _context = context;

        public IEnumerable<Category> GetAllCategories(string userId) => _context.Categories.Where(c => c.UserId == userId).ToList();
        public Category? GetCategoryById(int id, string userId) => _context.Categories.FirstOrDefault(c => c.Id == id && c.UserId == userId);
        public void AddCategory(Category category) { _context.Categories.Add(category); _context.SaveChanges(); }
        public void UpdateCategory(Category category, string userId)
        {
            var existing = GetCategoryById(category.Id, userId);
            if (existing == null) return;
            existing.Name = category.Name;
            existing.Description = category.Description;
            _context.SaveChanges();
        }
        public void DeleteCategory(int id, string userId)
        {
            var category = GetCategoryById(id, userId);
            if (category == null) return;
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}