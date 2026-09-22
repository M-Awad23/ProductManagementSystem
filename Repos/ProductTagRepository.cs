using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repos
{
    public class ProductTagRepository : IProductTagRepository
    {
        private readonly AppDbContext _context;

        public ProductTagRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddProductTags(int productId, List<int> tagIds, string userId)
        {
            var validTagIds = _context.Tags
                .Where(t => t.UserId == userId && tagIds.Contains(t.Id))
                .Select(t => t.Id)
                .ToList();

            foreach (var tagId in validTagIds.Distinct())
            {
                _context.ProductTags.Add(new ProductTag
                {
                    ProductId = productId,
                    TagId = tagId
                });
            }

            _context.SaveChanges();
        }

        public void ReplaceProductTags(int productId, List<int> tagIds, string userId)
        {
            var existingTags = _context.ProductTags
                .Where(pt => pt.ProductId == productId)
                .ToList();

            _context.ProductTags.RemoveRange(existingTags);

            var validTagIds = _context.Tags
                .Where(t => t.UserId == userId && tagIds.Contains(t.Id))
                .Select(t => t.Id)
                .ToList();

            foreach (var tagId in validTagIds.Distinct())
            {
                _context.ProductTags.Add(new ProductTag
                {
                    ProductId = productId,
                    TagId = tagId
                });
            }

            _context.SaveChanges();
        }
    }
}