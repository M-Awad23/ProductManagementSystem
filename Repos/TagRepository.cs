using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repos
{
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _context;
        public TagRepository(AppDbContext context) => _context = context;

        public IEnumerable<Tag> GetAllTags(string userId) => _context.Tags.Where(t => t.UserId == userId).ToList();
        public Tag? GetTagById(int id, string userId) => _context.Tags.FirstOrDefault(t => t.Id == id && t.UserId == userId);
        public void AddTag(Tag tag) { _context.Tags.Add(tag); _context.SaveChanges(); }
        public void UpdateTag(Tag tag, string userId)
        {
            var existing = GetTagById(tag.Id, userId);
            if (existing == null) return;
            existing.Name = tag.Name;
            _context.SaveChanges();
        }
        public void DeleteTag(int id, string userId)
        {
            var tag = GetTagById(id, userId);
            if (tag == null) return;
            _context.Tags.Remove(tag);
            _context.SaveChanges();
        }
    }
}