namespace ProductManagementSystem.Services
{
    public interface IProductTagService
    {
        void AddProductTags(int productId, List<int> tagIds);
        void ReplaceProductTags(int productId, List<int> tagIds);
    }
}