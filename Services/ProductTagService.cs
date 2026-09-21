using ProductManagementSystem.Repos;

namespace ProductManagementSystem.Services
{
    public class ProductTagService : IProductTagService
    {
        private readonly IProductTagRepository _productTagRepository;

        public ProductTagService(
            IProductTagRepository productTagRepository)
        {
            _productTagRepository = productTagRepository;
        }

        public void AddProductTags(
            int productId,
            List<int> tagIds)
        {
            _productTagRepository.AddProductTags(
                productId,
                tagIds);
        }

        public void ReplaceProductTags(
            int productId,
            List<int> tagIds)
        {
            _productTagRepository.ReplaceProductTags(
                productId,
                tagIds);
        }
    }
}