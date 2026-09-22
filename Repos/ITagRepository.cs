using ProductManagementSystem.Models;
namespace ProductManagementSystem.Repos
{
    public interface ITagRepository
    {
        IEnumerable<Tag> GetAllTags(string userId);
        Tag? GetTagById(int id, string userId);
        void AddTag(Tag tag);
        void UpdateTag(Tag tag, string userId);
        void DeleteTag(int id, string userId);
    }
}