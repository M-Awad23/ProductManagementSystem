using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repos
{
    public interface IProductTagRepository
    {
        void AddProductTags(int productId, List<int> tagIds);
        void ReplaceProductTags(int productId, List<int> tagIds);
    }
}