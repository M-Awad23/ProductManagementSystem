using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repos
{
    public interface ITagRepository
    {
        IEnumerable<Tag> GetAllTag();
        Tag GetTagById(int id);
        void AddTag(Tag tag);
        void UpdateTag(Tag tag);
        void DeleteTag(Tag tag);

    }
}
