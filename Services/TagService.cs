using ProductManagementSystem.Models;
using ProductManagementSystem.Repos;

namespace ProductManagementSystem.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        public TagService(ITagRepository tagRepository) => _tagRepository = tagRepository;
        public IEnumerable<Tag> GetAllTags(string userId) => _tagRepository.GetAllTags(userId);
        public Tag? GetTagById(int id, string userId) => _tagRepository.GetTagById(id, userId);
        public void AddTag(Tag tag) => _tagRepository.AddTag(tag);
        public void UpdateTag(Tag tag, string userId) => _tagRepository.UpdateTag(tag, userId);
        public void DeleteTag(int id, string userId) => _tagRepository.DeleteTag(id, userId);
    }
}