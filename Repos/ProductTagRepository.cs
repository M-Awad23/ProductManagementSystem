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

        public void AddProductTags(int productId, List<int> tagIds)
        {
            foreach (var tagId in tagIds.Distinct())
            {
                _context.ProductTags.Add(new ProductTag
                {
                    ProductId = productId,
                    TagId = tagId
                });
            }

            _context.SaveChanges();
        }

        public void ReplaceProductTags(int productId, List<int> tagIds)
        {
            var existingTags = _context.ProductTags
                .Where(pt => pt.ProductId == productId)
                .ToList();

            _context.ProductTags.RemoveRange(existingTags);

            foreach (var tagId in tagIds.Distinct())
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