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

        public IEnumerable<Tag> GetAllTags()
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

        public void DeleteTag(int id)
        {
            var tag = _context.Tags.Find(id);

            if (tag != null)
            {
                _context.Tags.Remove(tag);
                _context.SaveChanges();
            }
        }
    }
}