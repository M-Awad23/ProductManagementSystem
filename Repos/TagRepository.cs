using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repos
{
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _context;

        public TagRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Tag> GetAllTag()
        {
            return _context.Tags.ToList();
        }

        public Tag? GetTagById(int id)
        {
            return _context.Tags.Find(id);
        }

        public void AddTag(Tag tag)
        {
            _context.Tags.Add(tag);
            _context.SaveChanges();
        }

        public void UpdateTag(Tag tag)
        {
            _context.Tags.Update(tag);
            _context.SaveChanges();
        }

        public void DeleteTag(Tag tag)
        {
            _context.Tags.Remove(tag);
            _context.SaveChanges();
        }
    }
}