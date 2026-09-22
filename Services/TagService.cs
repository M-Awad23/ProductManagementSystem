using ProductManagementSystem.Models;
using ProductManagementSystem.Repos;

namespace ProductManagementSystem.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public IEnumerable<Tag> GetAllTags(string userId) =>
            _tagRepository.GetAllTags(userId);

        public Tag? GetTagById(int id, string userId) =>
            _tagRepository.GetTagById(id, userId);

        public (bool Success, string? ErrorMessage) CreateTag(
            string name,
            string userId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Tag name is required.");

            _tagRepository.AddTag(new Tag
            {
                Name = name,
                UserId = userId
            });

            return (true, null);
        }

        public (bool Success, string? ErrorMessage) UpdateTag(
            int id,
            string name,
            string userId)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(name))
                return (false, "Invalid tag.");

            var tag = _tagRepository.GetTagById(id, userId);

            if (tag == null)
                return (false, "Tag not found.");

            tag.Name = name;
            _tagRepository.UpdateTag(tag, userId);

            return (true, null);
        }

        public bool DeleteTag(int id, string userId)
        {
            if (_tagRepository.GetTagById(id, userId) == null)
                return false;

            _tagRepository.DeleteTag(id, userId);
            return true;
        }
    }
}