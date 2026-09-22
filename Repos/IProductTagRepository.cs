using ProductManagementSystem.Models;
namespace ProductManagementSystem.Repos
{
    public interface IProductTagRepository
    {
        void AddProductTags(int productId, List<int> tagIds, string userId);
        void ReplaceProductTags(int productId, List<int> tagIds, string userId);
    }
}