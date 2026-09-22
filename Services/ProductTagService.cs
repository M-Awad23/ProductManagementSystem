using ProductManagementSystem.Repos;
namespace ProductManagementSystem.Services
{
    public class ProductTagService : IProductTagService
    {
        private readonly IProductTagRepository _productTagRepository;
        public ProductTagService(IProductTagRepository productTagRepository)
        {
            _productTagRepository = productTagRepository;
        }
        public void AddProductTags(int productId, List<int> tagIds, string userId)
            => _productTagRepository.AddProductTags(productId, tagIds, userId);
        public void ReplaceProductTags(int productId, List<int> tagIds, string userId)
            => _productTagRepository.ReplaceProductTags(productId, tagIds, userId);
    }
}