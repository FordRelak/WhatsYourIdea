using Domain;
using Domain.Entities;

namespace WhatsYourIdea.Applications.Services
{
    public class TagService : ITagService
    {
        private readonly IRepository<Tag> _tagRepository;

        public TagService(IRepository<Tag> tagRepository)
        {
            _tagRepository = tagRepository;
        }
    }
}