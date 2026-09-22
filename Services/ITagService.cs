using ProductManagementSystem.Models;

namespace ProductManagementSystem.Services
{
    public interface ITagService
    {
        IEnumerable<Tag> GetAllTags(string userId);
        Tag? GetTagById(int id, string userId);
        (bool Success, string? ErrorMessage) CreateTag(string name, string userId);
        (bool Success, string? ErrorMessage) UpdateTag(int id, string name, string userId);
        bool DeleteTag(int id, string userId);
    }
}