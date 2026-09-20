using ProductManagementSystem.Models;

namespace ProductManagementSystem.Services
{
    public interface ITagService
    {
        IEnumerable<Tag> GetAllTags();

        Tag? GetTagById(int id);

        void AddTag(Tag tag);

        void UpdateTag(Tag tag);

        void DeleteTag(int id);
    }
}