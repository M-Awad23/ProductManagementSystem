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

        public IEnumerable<Tag> GetAllTags()
        {
            return _tagRepository.GetAllTags();
        }

        public Tag? GetTagById(int id)
        {
            return _tagRepository.GetTagById(id);
        }

        public void AddTag(Tag tag)
        {
            _tagRepository.AddTag(tag);
        }

        public void UpdateTag(Tag tag)
        {
            _tagRepository.UpdateTag(tag);
        }

        public void DeleteTag(int id)
        {
            _tagRepository.DeleteTag(id);
        }
    }
}